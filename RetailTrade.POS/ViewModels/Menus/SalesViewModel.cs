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
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTrade.POS.ViewModels.Menus
{
    public class SalesViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly IUserStore _userStore;
        private ObservableCollection<ProductWareHouseView> _products;
        private ObservableCollection<ProductSale> _productSales = new();
        private ProductWareHouseView _selectedProduct;
        private ProductSale _selectedProductSale;
        private string _searchText;

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
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                if (ProductTableView != null)
                {                    
                    ProductTableView.SearchString = SearchText;
                }
                OnPropertyChanged(nameof(SearchText));
            }
        }
        public TableView ProductTableView { get; set; }

        #endregion

        #region Constructor

        public SalesViewModel(IProductService productService,
            IUserStore userStore)
        {
            _productService = productService;
            _userStore = userStore;
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
                    Rest = SelectedProduct.Quantity + SelectedProductSale.Quantity
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

                ProductSale? productSale = ProductSales.FirstOrDefault(p => p.ProductId == SelectedProduct.Id);

                if (productSale == null)
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
                    ProductSales.Move(ProductSales.Count - 1, 0);
                    SelectedProductSale = ProductSales.FirstOrDefault(p => p.ProductId == SelectedProduct.Id);
                }
                else
                {
                    productSale.Quantity++;
                    SelectedProduct.Quantity--;
                    SelectedProductSale = productSale;
                }
                OnPropertyChanged(nameof(TotalSum));
            }
            catch (Exception)
            {
                //ignore
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
            Products = new(await _productService.GetProducts(Properties.Settings.Default.WareHouseId));
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
                        ProductTableView.ShowSearchPanelMode = ShowSearchPanelMode.Never;
                        ProductTableView.MouseDown += TableView_MouseDown;
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

        [Command]
        public void ClearSearchText()
        {
            SearchText = string.Empty;
        }

        #endregion
    }
}
