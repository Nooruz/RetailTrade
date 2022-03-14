using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Properties;
using RetailTradeClient.State.Reports;
using RetailTradeClient.State.Shifts;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RetailTradeClient.State.ProductSale
{
    public class ProductSaleStore : IProductSaleStore, INotifyPropertyChanged
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly IReportService _reportService;
        private readonly IReceiptService _receiptService;
        private readonly IShiftStore _shiftStore;
        private ObservableCollection<Sale> _productSales = new();
        private ObservableCollection<PostponeReceipt> _postponeReceipts = new();
        private ObservableCollection<PaymentType> _paymentTypes = new();
        private decimal _entered;
        private decimal _change;
        private bool _saleCompleted;

        #endregion

        #region Public Properties

        public ObservableCollection<PostponeReceipt> PostponeReceipts
        {
            get => _postponeReceipts;
            set
            {
                _postponeReceipts = value;
                OnPropertyChanged(nameof(PostponeReceipts));
            }
        }
        public ObservableCollection<Sale> ProductSales
        {
            get => _productSales;
            set
            {
                _productSales = value;
                OnPropertyChanged(nameof(ProductSales));
            }
        }
        public decimal ToBePaid => ProductSales.Sum(p => p.Sum);
        public decimal Entered
        {
            get => _entered;
            set
            {
                _entered = value;
                OnPropertyChanged(nameof(Entered));
                OnPropertyChanged(nameof(Change));
            }
        }
        public decimal Change
        {
            get => _change;
            set
            {
                _change = value;
                OnPropertyChanged(nameof(Change));
            }
        }
        public bool SaleCompleted
        {
            get => _saleCompleted;
            set
            {
                _saleCompleted = value;
                OnPropertyChanged(nameof(SaleCompleted));
            }
        }
        public bool IsKeepRecords => Settings.Default.IsKeepRecords;
        public ObservableCollection<PaymentType> PaymentTypes
        {
            get => _paymentTypes;
            set
            {
                _paymentTypes = value;
                OnPropertyChanged(nameof(PaymentTypes));
            }
        }

        #endregion

        #region Constructor

        public ProductSaleStore(IProductService productService,
            IReportService reportService)
        {
            _productService = productService;
            _reportService = reportService;
        }

        #endregion

        #region Public Events

        public event Action OnProductSalesChanged;
        public event Action<bool> OnProductSale;
        public event Action OnPostponeReceiptChanged;

        #endregion

        #region Public Voids

        public async Task AddProduct(string barcode)
        {
            if (ProductSales.Any())
            {
                Sale sale = ProductSales.FirstOrDefault(s => s.Barcode == barcode);
                if (sale != null)
                {
                    if (Settings.Default.IsKeepRecords)
                    {
                        if (sale.QuantityInStock < sale.Quantity + 1)
                        {
                            _ = MessageBox.Show("Количество превышает остаток.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            sale.Quantity++;
                        }
                    }
                }
                else
                {
                    AddProductToCart(await GetProduct(barcode));
                }
            }
            else
            {
                AddProductToCart(await GetProduct(barcode));
            }
            OnProductSalesChanged?.Invoke();
            OnPropertyChanged(nameof(ToBePaid));
        }

        public async Task AddProduct(int id)
        {            
            if (ProductSales.Any())
            {
                Sale sale = ProductSales.FirstOrDefault(s => s.Id == id);
                if (sale != null)
                {
                    if (Settings.Default.IsKeepRecords)
                    {
                        if (sale.QuantityInStock < sale.Quantity + 1)
                        {
                            _ = MessageBox.Show("Количество превышает остаток.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            sale.Quantity++;
                        }
                    }
                }
                else
                {
                    AddProductToCart(await GetProduct(id));
                }
            }
            else
            {
                AddProductToCart(await GetProduct(id));
            }
            OnProductSalesChanged?.Invoke();
            OnPropertyChanged(nameof(ToBePaid));
        }

        public void DeleteProduct(int id)
        {
            Sale sale = ProductSales.FirstOrDefault(s => s.Id == id);
            if (sale != null)
            {
                _ = ProductSales.Remove(sale);
            }
            OnProductSalesChanged?.Invoke();
        }

        public void CreatePostponeReceipt()
        {
            if (ProductSales.Any())
            {
                PostponeReceipts.Add(new PostponeReceipt
                {
                    Id = Guid.NewGuid(),
                    DateTime = DateTime.Now,
                    Sum = ProductSales.Sum(sp => sp.Sum),
                    Sales = ProductSales.ToList()
                });
                ProductSales.Clear();
                OnPostponeReceiptChanged?.Invoke();
            }
        }

        public void ProductSale(bool success)
        {
            Change = Entered - ToBePaid;
            ProductSales.Clear();
            OnPropertyChanged(nameof(ToBePaid));
            OnProductSale?.Invoke(success);
        }

        public void ProductSaleCashless(bool success)
        {
            ProductSales.Clear();
            PaymentTypes.Clear();
            OnPropertyChanged(nameof(ToBePaid));
            OnProductSale?.Invoke(success);
        }

        public void ResumeReceipt(Guid guid)
        {
            PostponeReceipt postponeReceipt = PostponeReceipts.FirstOrDefault(p => p.Id == guid);
            if (postponeReceipt != null)
            {
                postponeReceipt.Sales.ForEach(s => ProductSales.Add(s));
                _ = PostponeReceipts.Remove(postponeReceipt);
                OnPostponeReceiptChanged?.Invoke();
            }
        }

        #endregion

        #region Private Voids

        private async Task<Product> GetProduct(string barcode)
        {
            if (IsKeepRecords)
            {
                return await _productService.Predicate(p => p.Barcode == barcode && p.DeleteMark == false && p.Quantity > 0, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, SalePrice = p.SalePrice, ArrivalPrice = p.ArrivalPrice, TNVED = p.TNVED, Barcode = p.Barcode });
            }
            else
            {
                return await _productService.Predicate(p => p.Barcode == barcode && p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name, SalePrice = p.SalePrice, ArrivalPrice = p.ArrivalPrice, TNVED = p.TNVED, Barcode = p.Barcode });
            }
        }

        private async Task<Product> GetProduct(int id)
        {
            if (IsKeepRecords)
            {
                return await _productService.Predicate(p => p.Id == id && p.DeleteMark == false && p.Quantity > 0, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, SalePrice = p.SalePrice, ArrivalPrice = p.ArrivalPrice, TNVED = p.TNVED, Barcode = p.Barcode });
            }
            else
            {
                return await _productService.Predicate(p => p.Id == id && p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name, SalePrice = p.SalePrice, ArrivalPrice = p.ArrivalPrice, TNVED = p.TNVED, Barcode = p.Barcode });
            }
        }

        private void AddProductToCart(Product product)
        {
            if (product != null)
            {
                try
                {
                    ProductSales.Add(new Sale
                    {
                        Id = product.Id,
                        Name = product.Name,
                        SalePrice = product.SalePrice,
                        ArrivalPrice = product.ArrivalPrice,
                        QuantityInStock = IsKeepRecords ? product.Quantity : 0,
                        TNVED = product.TNVED,
                        Quantity = 1,
                        Barcode = product.Barcode
                    });
                }
                catch (Exception)
                {
                    //ignore
                }
            }
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;        

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
