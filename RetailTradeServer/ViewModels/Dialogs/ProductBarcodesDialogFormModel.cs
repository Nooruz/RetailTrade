using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Views;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class ProductBarcodesDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly IProductBarcodeService _productBarcodeService;
        private Product _selectedProduct;
        private ObservableCollection<ProductBarcodeView> _productBarcodes = new();
        private ProductBarcodeView _selectedProductBarcode;

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
        public ProductBarcodeView SelectedProductBarcode
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
        public void EditBarcode()
        {
            try
            {
                if (SelectedProductBarcode != null)
                {
                    WindowService.Show(nameof(CreateBarcodeDialogForm), new CreateBarcodeDialogFormModel(_productService, _productBarcodeService)
                    {
                        Title = $"Штрихкод товара ({SelectedProductBarcode.Barcode})",
                        SelectedProduct = SelectedProduct,
                        SelectedProductBarcode = SelectedProductBarcode
                    });
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public async void DeleteBarcode()
        {
            try
            {
                if (SelectedProductBarcode != null)
                {
                    if (MessageBoxService.ShowMessage($"Удалить выбранный элемент?\n\"{SelectedProductBarcode.Barcode}\"", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                    {
                        if (await _productBarcodeService.DeleteAsync(SelectedProductBarcode.Id))
                        {
                            _ = ProductBarcodes.Remove(SelectedProductBarcode);
                        }
                    }
                }
                else
                {
                    MessageBoxService.ShowMessage("Выберите штрихкод!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
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
            if (SelectedProduct != null)
            {
                ProductBarcodes = new(await _productBarcodeService.GetAllByProductIdAsync(SelectedProduct.Id));
            }
            else
            {
                ProductBarcodes = new(await _productBarcodeService.GetAllViewsAsync());
            }
            if (ProductBarcodes != null && ProductBarcodes.Any())
            {
                SelectedProductBarcode = ProductBarcodes.LastOrDefault();
            }
        }

        #endregion
    }
}
