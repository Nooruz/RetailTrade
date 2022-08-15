using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RetailTradeServer.ViewModels.Menus
{
    public class CreateProductRegistrationViewModel : BaseViewModel
    {
        #region Private Members

        private Registration _registration = new();
        private readonly IProductService _productService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IRegistrationService _registrationService;
        private IEnumerable<Product> _products;
        private IEnumerable<WareHouse> _wareHouses;
        private RegistrationProduct _selectedRegistrationProduct;

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
        public RegistrationProduct SelectedRegistrationProduct
        {
            get => _selectedRegistrationProduct;
            set
            {
                _selectedRegistrationProduct = value;
                OnPropertyChanged(nameof(SelectedRegistrationProduct));
            }
        }

        #endregion

        #region Constructor

        public CreateProductRegistrationViewModel(IProductService productService, 
            IWareHouseService wareHouseService,
            IRegistrationService registrationService)
        {
            _productService = productService;
            _wareHouseService = wareHouseService;
            _registrationService = registrationService;
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

        [Command]
        public async void Save()
        {
            try
            {
                if (CreatedRegistration.WareHouseId != 0 && CreatedRegistration.RegistrationProducts != null && CreatedRegistration.RegistrationProducts.Any())
                {
                    CreatedRegistration.RegistrationProducts.ToList().ForEach(r =>
                    {
                        r.Product = null;
                    });
                    CreatedRegistration.RegistrationDate = DateTime.Now;
                    CreatedRegistration.Sum = CreatedRegistration.RegistrationProducts.Sum(r => r.Amount);
                    await _registrationService.CreateAsync(CreatedRegistration);
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion

        #region Private Voids

        private void RegistrationProductTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Cell.Property == nameof(RegistrationProduct.Product))
                {
                    if (SelectedRegistrationProduct != null && SelectedRegistrationProduct.Product != null)
                    {
                        SelectedRegistrationProduct.ProductId = SelectedRegistrationProduct.Product.Id;
                        SelectedRegistrationProduct.Price = SelectedRegistrationProduct.Product.PurchasePrice;
                        SelectedRegistrationProduct.Quantity = 1;
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
