using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Views;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.ObjectModel;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class ProductBarcodesDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly IProductBarcodeService _productBarcodeService;
        private Product _selectedProduct;
        private ObservableCollection<ProductBarcodeView> _productBarcodes = new();
        private ProductBarcode _selectedProductBarcode;

        #endregion

        #region Public Properties

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }
        public ObservableCollection<ProductBarcodeView> ProductBarcodes
        {
            get => _productBarcodes;
            set
            {
                _productBarcodes = value;
                OnPropertyChanged(nameof(ProductBarcodes));
            }
        }
        public ProductBarcode SelectedProductBarcode
        {
            get => _selectedProductBarcode;
            set
            {
                _selectedProductBarcode = value;
                OnPropertyChanged(nameof(SelectedProductBarcode));
            }
        }

        #endregion

        #region Constructor

        public ProductBarcodesDialogFormModel(IProductService productService, 
            IProductBarcodeService productBarcodeService)
        {
            _productService = productService;
            _productBarcodeService = productBarcodeService;

            _productBarcodeService.OnCreated += ProductBarcodeService_OnCreated;
            _productBarcodeService.OnEdited += ProductBarcodeService_OnEdited;
        }

        #endregion

        #region Private Voids

        private void ProductBarcodeService_OnCreated(ProductBarcodeView productBarcodeView)
        {
            try
            {
                ProductBarcodes.Add(productBarcodeView);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void ProductBarcodeService_OnEdited(ProductBarcode productBarcode)
        {
            try
            {
                SelectedProductBarcode.Barcode = productBarcode.Barcode;
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion

        #region Public Voids

        [Command]
        public void CreateBarcode()
        {
            WindowService.Show(nameof(CreateBarcodeDialogForm), new CreateBarcodeDialogFormModel(_productService, _productBarcodeService) 
            { 
                Title = "Штрихкод товара (создание)",
                SelectedProduct = SelectedProduct
            });
        }

        [Command]
        public async void UserControlLoaded()
        {
            if (SelectedProduct != null)
            {
                ProductBarcodes = new(await _productBarcodeService.GetAllByProductIdAsync(SelectedProduct.Id));
            }
            else
            {
                ProductBarcodes = new(await _productBarcodeService.GetAllViewsAsync());
            }
        }

        #endregion
    }
}
