using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RetailTradeServer.ViewModels.Menus
{
    public class CreateProductRegistrationViewModel : BaseViewModel
    {
        #region Private Members

        private Registration _registration = new();
        private readonly IProductService _productService;
        private readonly IWareHouseService _wareHouseService;
        private IEnumerable<Product> _products;
        private IEnumerable<WareHouse> _wareHouses;

        #endregion

        #region Public Properties

        public Registration CreatedRegistration
        {
            get => _registration;
            set
            {
                _registration = value;
                OnPropertyChanged(nameof(CreatedRegistration));
            }
        }
        public IEnumerable<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public IEnumerable<WareHouse> WareHouses
        {
            get => _wareHouses;
            set
            {
                _wareHouses = value;
                OnPropertyChanged(nameof(WareHouses));
            }
        }
        public TableView RegistrationProductTableView { get; set; }

        #endregion

        #region Constructor

        public CreateProductRegistrationViewModel(IProductService productService, 
            IWareHouseService wareHouseService)
        {
            _productService = productService;
            _wareHouseService = wareHouseService;
        }

        #endregion

        #region Public Voids

        [Command]
        public async void UserControlLoaded()
        {
            Header = "Оприходование (создание)";
            ShowLoadingPanel = false;
            WareHouses = await _wareHouseService.GetAllAsync();
            Products = await _productService.GetAllAsync();
        }

        [Command]
        public void RegistrationProductTableViewLoaded(object sender)
        {
            if (sender is RoutedEventArgs e)
            {
                if (e.Source is TableView tableView)
                {
                    RegistrationProductTableView = tableView;
                    RegistrationProductTableView.CellValueChanged += RegistrationProductTableView_CellValueChanged;
                }
            }
        }

        #endregion

        #region Private Voids

        private void RegistrationProductTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Cell.Property == nameof(RegistrationProduct.ProductId))
                {

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
