using DevExpress.XtraPrinting;
using RetailTrade.Barcode.Services;
using RetailTrade.CashRegisterMachine;
using RetailTrade.CashRegisterMachine.Services;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Properties;
using RetailTradeClient.Report;
using RetailTradeClient.State.Reports;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RetailTradeClient.State.ProductSales
{
    public class ProductSaleStore : IProductSaleStore, INotifyPropertyChanged
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly IReportService _reportService;
        private readonly IReceiptService _receiptService;
        private readonly IBarcodeService _barcodeService;
        private readonly ICashRegisterMachineService _cashRegisterMachineService;
        private readonly IShiftStore _shiftStore;
        private readonly ProductViewModel _productViewModel;
        private ObservableCollection<Sale> _sales = new();
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
        public ObservableCollection<Sale> Sales
        {
            get => _sales;
            set
            {
                _sales = value;
                OnPropertyChanged(nameof(Sales));
            }
        }
        public decimal ToBePaid => Sales.Sum(p => p.Sum);
        public decimal Entered
        {
            get => _entered;
            set
            {
                _entered = value;
                OnPropertyChanged(nameof(Entered));
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
            IReportService reportService,
            IReceiptService receiptService,
            IShiftStore shiftStore,
            IBarcodeService barcodeService,
            ICashRegisterMachineService cashRegisterMachineService,
            ProductViewModel productViewModel)
        {
            _productService = productService;
            _reportService = reportService;
            _receiptService = receiptService;
            _shiftStore = shiftStore;
            _barcodeService = barcodeService;
            _cashRegisterMachineService = cashRegisterMachineService;
            _productViewModel = productViewModel;

            _barcodeService.OnBarcodeEvent += BarcodeService_OnBarcodeEvent;
            _productViewModel.OnProductsSelected += ProductViewModel_OnProductsSelected;
        }

        #endregion

        #region Public Events

        public event Action OnProductSalesChanged;
        public event Action<decimal> OnProductSale;
        public event Action OnPostponeReceiptChanged;
        public event Action<Sale> OnCreated;

        #endregion

        #region Public Voids

        public async Task AddProduct(string barcode)
        {
            if (Sales.Any())
            {
                Sale sale = Sales.FirstOrDefault(s => s.Barcode == barcode);
                if (sale != null)
                {
                    if (Settings.Default.IsKeepRecords)
                    {
                        if (sale.QuantityInStock < sale.Quantity + 1)
                        {
                            //_ = MessageBox.Show("Количество превышает остаток.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            sale.Quantity++;
                        }
                    }
                    else
                    {
                        sale.Quantity++;
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
            if (Sales.Any())
            {
                Sale sale = Sales.FirstOrDefault(s => s.Id == id);
                if (sale != null)
                {
                    if (Settings.Default.IsKeepRecords)
                    {
                        if (sale.QuantityInStock < sale.Quantity + 1)
                        {
                            //_ = MessageBox.Show("Количество превышает остаток.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            sale.Quantity++;
                        }
                    }
                    else
                    {
                        sale.Quantity++;
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
            Sale sale = Sales.FirstOrDefault(s => s.Id == id);
            if (sale != null)
            {
                _ = Sales.Remove(sale);
            }
            OnProductSalesChanged?.Invoke();
        }

        public void CreatePostponeReceipt()
        {
            if (Sales.Any())
            {
                PostponeReceipts.Add(new PostponeReceipt
                {
                    Id = Guid.NewGuid(),
                    DateTime = DateTime.Now,
                    Sum = Sales.Sum(sp => sp.Sum),
                    Sales = Sales.ToList()
                });
                Sales.Clear();
                OnPostponeReceiptChanged?.Invoke();
            }
        }

        public void ResumeReceipt(Guid guid)
        {
            PostponeReceipt postponeReceipt = PostponeReceipts.FirstOrDefault(p => p.Id == guid);
            if (postponeReceipt != null)
            {
                postponeReceipt.Sales.ForEach(s => Sales.Add(s));
                _ = PostponeReceipts.Remove(postponeReceipt);
                OnPostponeReceiptChanged?.Invoke();
            }
        }

        #endregion

        #region Private Voids

        private void ProductViewModel_OnProductsSelected(IEnumerable<Product> products)
        {
            try
            {
                products.ToList().ForEach(p =>
                {
                    if (Sales.Any())
                    {
                        Sale sale = Sales.FirstOrDefault(s => s.Id == p.Id);
                        if (sale != null)
                        {
                            if (Settings.Default.IsKeepRecords)
                            {
                                if (sale.QuantityInStock < sale.Quantity + 1)
                                {
                                    //_ = MessageBox.Show("Количество превышает остаток.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    sale.Quantity++;
                                }
                            }
                            else
                            {
                                sale.Quantity++;
                            }
                        }
                        else
                        {
                            AddProductToCart(p);
                        }
                    }
                    else
                    {
                        AddProductToCart(p);
                    }                    
                });
                OnProductSalesChanged?.Invoke();
                OnPropertyChanged(nameof(ToBePaid));
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private async void BarcodeService_OnBarcodeEvent(string barcode)
        {
            await AddProduct(barcode);
        }

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
                    Sales.Add(new Sale
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
                    OnCreated?.Invoke(Sales.FirstOrDefault(s => s.Id == product.Id));
                }
                catch (Exception)
                {
                    //ignore
                }
            }
        }

        public ProductViewModel SearchProduct()
        {
            return _productViewModel;
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;        

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task CashPayment()
        {
            try
            {
                PrintCashRegisterMachine();

                Receipt receipt = await _receiptService.CreateAsync(new Receipt()
                {
                    DateOfPurchase = DateTime.Now,
                    Sum = ToBePaid,
                    Deposited = Entered,
                    PaidInCash = ToBePaid,
                    ShiftId = _shiftStore.CurrentShift.Id,
                    Change = Change,
                    ProductSales = Sales.Select(s =>
                        new ProductSale
                        {
                            ProductId = s.Id,
                            Quantity = s.Quantity,
                            Sum = s.Sum,
                            SalePrice = s.SalePrice,
                            ArrivalPrice = s.ArrivalPrice
                        }).ToList()
                });

                await PrintReceipt(receipt);

                await _receiptService.CreateAsync(receipt);

                OnProductSale?.Invoke(Change);
                Sales.Clear();
                OnPropertyChanged(nameof(ToBePaid));
            }
            catch (Exception)
            {
                //ignore
            }
        }

        public async Task CashlessPayment()
        {
            
        }

        #endregion

        #region Private Voids

        private async Task PrintReceipt(Receipt receipt)
        {
            ProductSaleReport report = await _reportService.CreateProductSaleReport(receipt, Sales);

            PrintToolBase tool = new(report.PrintingSystem);
            tool.PrinterSettings.PrinterName = Settings.Default.DefaultReceiptPrinter;
            tool.PrintingSystem.EndPrint += PrintingSystem_EndPrint;
            tool.Print();
        }

        private void PrintCashRegisterMachine()
        {
            try
            {
                if (_cashRegisterMachineService.ECRMode() == ECRModeEnum.Mode2 || _cashRegisterMachineService.ECRMode() == ECRModeEnum.Mode0)
                {
                    _cashRegisterMachineService.Connect();
                    _cashRegisterMachineService.CheckType = 0;

                    if (Sales.Any())
                    {
                        foreach (Sale item in Sales)
                        {
                            _cashRegisterMachineService.Quantity = Convert.ToDouble(item.Quantity);
                            _cashRegisterMachineService.Price = item.SalePrice;

                            var sum1NSP = Math.Round(item.SalePrice * 1 / 102, 2);
                            string sumNSP = Math.Round(sum1NSP * 100, 0).ToString();

                            _cashRegisterMachineService.StringForPrinting =
                                string.Join(";", new string[] { "", item.TNVED, "", "", "0", "0", "4", sumNSP + "\n" + item.Name });
                            _cashRegisterMachineService.Tax1 = 4;
                            _cashRegisterMachineService.Tax2 = 0;
                            _cashRegisterMachineService.Tax3 = 0;
                            _cashRegisterMachineService.Tax4 = 0;
                            string result = _cashRegisterMachineService.Sale();
                        }

                        _cashRegisterMachineService.Summ1 = Sales.Sum(s => s.SalePrice);
                        _cashRegisterMachineService.StringForPrinting = "";
                        _cashRegisterMachineService.CloseCheck();
                        _cashRegisterMachineService.CutCheck();
                        _cashRegisterMachineService.Disconnect();

                        //_cashRegisterMachineService.RegisterNumber = 148;
                        //_cashRegisterMachineService.GetOperationReg();
                        //string d = _cashRegisterMachineService.GetOperationReg();
                        //int f = _cashRegisterMachineService.ContentsOfOperationRegister;
                        //string df = _cashRegisterMachineService.NameOperationReg;

                    }

                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void PrintingSystem_EndPrint(object sender, EventArgs e)
        {
            Change = Entered - ToBePaid;
            Sales.Clear();
            OnPropertyChanged(nameof(ToBePaid));
        }

        #endregion
    }
}
