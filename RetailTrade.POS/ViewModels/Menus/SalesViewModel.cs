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
using System.Collections;
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

        private readonly IProductService _productService;
        private readonly IUserStore _userStore;
        private ObservableCollection<ProductWareHouseView> _products;
        private ObservableCollection<ProductSale> _productSales = new();
        private ProductWareHouseView _selectedProduct;

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
        public decimal TotalSum => ProductSales.Sum(p => p.Total);

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
            if (ProductGridControl.IsGroupRowHandle(rowHandle))
                return;
            if (rowHandle == DataControlBase.AutoFilterRowHandle)
                return;
            if (rowHandle == DataControlBase.NewItemRowHandle)
                return;
            if (rowHandle == DataControlBase.InvalidRowHandle)
                return;

            WindowService.Show(nameof(PositionEditorView), new PositionEditorViewModel());
        }

        private void GetRowType(int rowHandle)
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
                    Total = SelectedProduct.RetailPrice,
                    WareHouseId = SelectedProduct.WareHouseId,
                    PointSaleId = Properties.Settings.Default.PointSaleId,
                    Product = new Product
                    {
                        Name = SelectedProduct.Name
                    }
                });
                ProductSales.Move(ProductSales.Count - 1, 0);
            }
            else
            {
                productSale.Quantity++;
                productSale.Total = (decimal)productSale.Quantity * productSale.RetailPrice;
            }
            OnPropertyChanged(nameof(TotalSum));
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
                        TableView tableView = ProductGridControl.View as TableView;
                        tableView.MouseDown += TableView_MouseDown;
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
