using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Views;
using RetailTrade.POS.States.Users;
using RetailTrade.POS.ViewModels.Dialogs;
using RetailTrade.POS.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<ProductWareHouseView> _products;
        private ObservableCollection<ProductSale> _productSales = new();
        private ProductWareHouseView _selectedProduct;
        private ProductSale _selectedProductSale;
        public IEnumerable<ProductBarcode> _productBarcodes;

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
        public ObservableCollection<ProductSale> ProductSales
        {
            get => _productSales;
            set
            {
                _productSales = value;
                OnPropertyChanged(nameof(ProductSales));
            }
        }
        public User CurrentUser => _userStore.CurrentUser;
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
        public decimal TotalSum => ProductSales.Sum(p => p.Total);
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

        #endregion

        #region Constructor

        public SalesViewModel(IProductService productService,
            IUserStore userStore,
            IProductBarcodeService productBarcodeService)
        {
            _productService = productService;
            _userStore = userStore;
            _productBarcodeService = productBarcodeService;
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
                ProductSales.Remove(productSale);
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
                    IncreaseQuantityProductSale(ProductSales.FirstOrDefault(p => p.ProductId == SelectedProduct.Id));
                }
                ProductSales.Move(ProductSales.Count - 1, 0);
                OnPropertyChanged(nameof(TotalSum));
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
                ProductSale? product = ProductSales.FirstOrDefault(p => p.ProductId == productSale.ProductId);
                if (product != null)
                {
                    product.Quantity += productSale.Quantity;                    
                }
                else
                {
                    ProductSales.Add(productSale);                    
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
                ProductSales.Add(new ProductSale
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
                SelectedProduct.Quantity -= 1;
            }
        }

        #endregion

        #region Public Vodis

        [Command]
        public void ClearProductSale()
        {
            try
            {
                if (ProductSales.Any())
                {
                    ProductSales.ToList().ForEach(s =>
                    {
                        ProductWareHouseView product = Products.FirstOrDefault(p => p.Id == s.ProductId);
                        product.Quantity += s.Quantity;
                    });
                    ProductSales.Clear();
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
                        ProductTableView.MouseDown += TableView_MouseDown;
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
