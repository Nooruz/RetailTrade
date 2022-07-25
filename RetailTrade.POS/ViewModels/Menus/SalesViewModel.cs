using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Views;
using RetailTrade.POS.State.Shifts;
using RetailTrade.POS.States.Users;
using RetailTrade.POS.ViewModels.Dialogs;
using RetailTrade.POS.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTrade.POS.ViewModels.Menus
{
    public class SalesViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductBarcodeService _productBarcodeService;
        private readonly IProductService _productService;
        private readonly IUserStore _userStore;
        private readonly IShiftStore _shiftStore;
        private readonly IReceiptService _receiptService;
        private ObservableCollection<ProductWareHouseView> _products;
        private ProductWareHouseView _selectedProduct;
        private ProductSale _selectedProductSale;
        public IEnumerable<ProductBarcode> _productBarcodes;
        private Receipt _receipt = new();
        private bool _isDiscountReceipt;

        #endregion

        #region Public Properties

        public ObservableCollection<ProductWareHouseView> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public User CurrentUser => _userStore.CurrentUser;
        public Shift CurrentShift => _shiftStore.CurrentShift;
        public GridControl ProductGridControl { get; set; }
        public GridControl ProductSaleGridControl { get; set; }
        public ProductWareHouseView SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }
        public ProductSale SelectedProductSale
        {
            get => _selectedProductSale;
            set
            {
                _selectedProductSale = value;
                OnPropertyChanged(nameof(SelectedProductSale));
            }
        }
        public string TotalSum => Receipt.AmountWithoutDiscount.ToString("N2");
        public TableView ProductTableView { get; set; }
        public IEnumerable<ProductBarcode> ProductBarcodes
        {
            get => _productBarcodes;
            set
            {
                _productBarcodes = value;
                OnPropertyChanged(nameof(ProductBarcodes));
            }
        }
        public Visibility DiscountReceiptButtonVisibility => Receipt.ProductSales.Any() ? Visibility.Visible : Visibility.Collapsed;
        public Receipt Receipt
        {
            get
            {
                try
                {
                    _receipt.DateOfPurchase = DateTime.Now;
                    _receipt.PointSaleId = Properties.Settings.Default.PointSaleId;
                }
                catch (Exception)
                {
                    //ignore
                }
                return _receipt;
            }
            set
            {
                _receipt = value;
                OnPropertyChanged(nameof(Receipt));
            }
        }
        public bool IsDiscountReceipt
        {
            get => _isDiscountReceipt;
            set
            {
                _isDiscountReceipt = value;
                OnPropertyChanged(nameof(IsDiscountReceipt));
                OnPropertyChanged(nameof(ReceiptDiscount));
            }
        }
        public string ReceiptDiscount
        {
            get
            {
                try
                {
                    return IsDiscountReceipt ? $"{Math.Round(Receipt.DiscountAmount / Receipt.Total * 100, 2)}% • {Receipt.DiscountAmount:N2}" : string.Empty;
                }
                catch (Exception)
                {
                    //ignore
                }
                return string.Empty;
            }
        }

        #endregion

        #region Constructor

        public SalesViewModel(IProductService productService,
            IUserStore userStore,
            IProductBarcodeService productBarcodeService,
            IShiftStore shiftStore,
            IReceiptService receiptService)
        {
            _productService = productService;
            _userStore = userStore;
            _productBarcodeService = productBarcodeService;
            _shiftStore = shiftStore;
            _receiptService = receiptService;

            Receipt.ProductSales.CollectionChanged += ProductSales_CollectionChanged;
            _userStore.StateChanged += () => OnPropertyChanged(nameof(CurrentUser));
            _shiftStore.CurrentShiftChanged += (value) => OnPropertyChanged(nameof(CurrentShift));
        }

        #endregion

        #region Private Vodis

        private void TableView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                GetRowType(ProductGridControl.View.GetRowHandleByMouseEventArgs(e));
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void ProductSaleTableView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                GetProductSaleRowType(ProductGridControl.View.GetRowHandleByMouseEventArgs(e));
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void GetProductSaleRowType(int rowHandle)
        {
            try
            {
                if (ProductGridControl.IsGroupRowHandle(rowHandle))
                    return;
                if (rowHandle == DataControlBase.AutoFilterRowHandle)
                    return;
                if (rowHandle == DataControlBase.NewItemRowHandle)
                    return;
                if (rowHandle == DataControlBase.InvalidRowHandle)
                    return;

                PositionEditorViewModel viewModel = new()
                {
                    Title = "Редактор позиции",
                    EditProductSale = SelectedProductSale,
                    Rest = SelectedProduct.Quantity + SelectedProductSale.Quantity,
                    Product = Products.FirstOrDefault(p => p.Id == SelectedProductSale.ProductId)
                };

                viewModel.OnDeleteProductSale += ViewModel_OnDeleteProductSale;

                WindowService.Show(nameof(PositionEditorView), viewModel);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void ViewModel_OnDeleteProductSale(ProductSale productSale)
        {
            try
            {
                ProductWareHouseView product = Products.FirstOrDefault(p => p.Id == productSale.ProductId);
                product.Quantity += productSale.Quantity;
                Receipt.ProductSales.Remove(productSale);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void GetRowType(int rowHandle)
        {
            try
            {
                if (ProductGridControl.IsGroupRowHandle(rowHandle))
                    return;
                if (rowHandle == DataControlBase.AutoFilterRowHandle)
                    return;
                if (rowHandle == DataControlBase.NewItemRowHandle)
                    return;
                if (rowHandle == DataControlBase.InvalidRowHandle)
                    return;

                if (Properties.Settings.Default.EnterQuantityWhenAdding)
                {
                    AddingProductViewModel viewModel = new()
                    {
                        Title = "Добавление товара",
                        EditProductSale = new ProductSale
                        {
                            ProductId = SelectedProduct.Id,
                            PurchasePrice = SelectedProduct.PurchasePrice,
                            RetailPrice = SelectedProduct.RetailPrice,
                            WareHouseId = SelectedProduct.WareHouseId,
                            PointSaleId = Properties.Settings.Default.PointSaleId,
                            Product = new Product
                            {
                                Name = SelectedProduct.Name
                            }
                        },
                        Rest = SelectedProduct.Quantity
                    };
                    viewModel.OnProductSaleAdding += ViewModel_OnProductSaleAdding;
                    WindowService.Show(nameof(AddingProductView), viewModel);
                }
                else
                {
                    IncreaseQuantityProductSale(Receipt.ProductSales.FirstOrDefault(p => p.ProductId == SelectedProduct.Id));
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void ViewModel_OnProductSaleAdding(ProductSale productSale)
        {
            try
            {
                ProductSale? product = Receipt.ProductSales.FirstOrDefault(p => p.ProductId == productSale.ProductId);
                if (product != null)
                {
                    product.Quantity += productSale.Quantity;                    
                }
                else
                {
                    Receipt.ProductSales.Add(productSale);
                    Receipt.ProductSales.Move(Receipt.ProductSales.Count - 1, 0);
                }
                SelectedProduct.Quantity -= productSale.Quantity;
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void ProductTableView_SearchStringToFilterCriteria(object sender, SearchStringToFilterCriteriaEventArgs e)
        {
            try
            {
                if (ProductBarcodes.Any(p => p.Barcode == e.SearchString))
                {
                    e.Filter = CriteriaOperator.Parse("[ProductBarcodes][[Barcode] == ?]", e.SearchString);
                }
                else
                {
                    e.Filter = CriteriaOperator.Parse($"Contains([Name], '{e.SearchString}')");
                }                
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void IncreaseQuantityProductSale(ProductSale? productSale)
        {
            if (productSale != null)
            {
                productSale.Quantity++;
                productSale.TotalWithDiscount = productSale.Total - productSale.DiscountAmount;
                SelectedProduct.Quantity--;
            }
            else
            {
                Receipt.ProductSales.Add(new ProductSale
                {
                    ProductId = SelectedProduct.Id,
                    Quantity = 1,
                    PurchasePrice = SelectedProduct.PurchasePrice,
                    RetailPrice = SelectedProduct.RetailPrice,
                    WareHouseId = SelectedProduct.WareHouseId,
                    PointSaleId = Properties.Settings.Default.PointSaleId,
                    Product = new Product
                    {
                        Name = SelectedProduct.Name
                    }
                });
                Receipt.ProductSales.Move(Receipt.ProductSales.Count - 1, 0);
                SelectedProduct.Quantity -= 1;
            }
        }

        private void ProductSales_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ProductSale item in e.NewItems)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (ProductSale item in e.OldItems)
                {
                    item.PropertyChanged -= Item_PropertyChanged;
                }
            }
            OnPropertyChanged(nameof(TotalSum));
            OnPropertyChanged(nameof(DiscountReceiptButtonVisibility));
        }

        private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TotalSum));
        }

        private void ViewModel_OnDiscountChanged(decimal discountSum)
        {
            if (discountSum > 0)
            {
                IsDiscountReceipt = true;
                Receipt.DiscountAmount = discountSum;
            }
            OnPropertyChanged(nameof(ReceiptDiscount));
            OnPropertyChanged(nameof(TotalSum));
        }

        private void ViewModel_OnDeleteDiscountReceipt()
        {
            IsDiscountReceipt = false;
            Receipt.DiscountAmount = 0;
            OnPropertyChanged(nameof(ReceiptDiscount));
            OnPropertyChanged(nameof(TotalSum));
        }

        private void ViewModel_OnSale()
        {
            Receipt = new();
        }

        #endregion

        #region Public Vodis

        [Command]
        public void Payment()
        {
            try
            {
                PaymentViewModel viewModel = new(_receiptService, _shiftStore)
                {
                    Title = "Оплата",
                    EditReceipt = Receipt
                };

                viewModel.OnSale += ViewModel_OnSale;

                WindowService.Show(nameof(PaymentView), viewModel);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void Customer()
        {
            try
            {
                WindowService.Show(nameof(CustomerView), new CustomerViewModel() { Title = "Покупатель" });
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void DiscountReceipt()
        {
            try
            {
                DiscountReceiptViewModel viewModel = new()
                {
                    Title = "Скидка",
                    Receipt = Receipt
                };

                viewModel.OnDiscountChanged += ViewModel_OnDiscountChanged;
                viewModel.OnDeleteDiscountReceipt += ViewModel_OnDeleteDiscountReceipt;

                WindowService.Show(nameof(DiscountReceiptView), viewModel);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void ClearProductSale()
        {
            try
            {
                if (Receipt.ProductSales.Any())
                {
                    Receipt.ProductSales.ToList().ForEach(s =>
                    {
                        ProductWareHouseView product = Products.FirstOrDefault(p => p.Id == s.ProductId);
                        product.Quantity += s.Quantity;
                    });
                    Receipt.ProductSales.Clear();
                    OnPropertyChanged(nameof(TotalSum));
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public async void UserControlLoaded()
        {
            ProductBarcodes = await _productBarcodeService.GetAllAsync();
            IEnumerable<ProductWareHouseView> products = await _productService.GetProducts(Properties.Settings.Default.WareHouseId);
            Products = new(products.Select(s => new ProductWareHouseView
            {
                Id = s.Id,
                Name = s.Name,
                Supplier = s.Supplier,
                Unit = s.Unit,
                TNVED = s.TNVED,
                PurchasePrice = s.PurchasePrice,
                RetailPrice = s.RetailPrice,
                Quantity = s.Quantity,
                WareHouseId = s.WareHouseId,
                ProductBarcodes = ProductBarcodes.Where(p => p.ProductId == s.Id).ToList(),
            }).ToList());
        }

        [Command]
        public void ProductGridControlLoaded(object sender)
        {
            try
            {
                if (sender is RoutedEventArgs e)
                {
                    if (e.Source is GridControl gridControl)
                    {
                        ProductGridControl = gridControl;
                        ProductTableView = ProductGridControl.View as TableView;
                        ProductTableView.SearchColumns = string.Join(";", ProductGridControl.Columns.Select(g => g.FieldName));
                        ProductTableView.MouseLeftButtonDown += TableView_MouseDown;
                        ProductTableView.SearchStringToFilterCriteria += ProductTableView_SearchStringToFilterCriteria;
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void ProductSaleGridControlLoaded(object sender)
        {
            try
            {
                if (sender is RoutedEventArgs e)
                {
                    if (e.Source is GridControl gridControl)
                    {
                        ProductSaleGridControl = gridControl;
                        TableView productSaleTableView = ProductSaleGridControl.View as TableView;
                        productSaleTableView.MouseDown += ProductSaleTableView_MouseDown;
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion
    }
}
