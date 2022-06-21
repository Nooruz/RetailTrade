using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Views;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System;
using System.Collections.Generic;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateBarcodeDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly IProductBarcodeService _productBarcodeService;
        private Product _selectedProduct;
        private IEnumerable<Product> _products;
        private int? _selectedProductId;
        private string _barcode;
        private ProductBarcodeView _selectedProductBarcode;

        #endregion

        #region Public Properties

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                if (_selectedProduct != null)
                {
                    SelectedProductId = _selectedProduct.Id;
                }
                OnPropertyChanged(nameof(SelectedProduct));
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
        public int? SelectedProductId
        {
            get => _selectedProductId;
            set
            {
                _selectedProductId = value;
                OnPropertyChanged(nameof(SelectedProductId));
            }
        }
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged(nameof(Barcode));
            }
        }
        public ProductBarcodeView SelectedProductBarcode
        {
            get => _selectedProductBarcode;
            set
            {
                _selectedProductBarcode = value;
                if (_selectedProductBarcode != null)
                {
                    IsEditMode = true;
                    Barcode = _selectedProductBarcode.Barcode;
                }
                OnPropertyChanged(nameof(SelectedProductBarcode));
            }
        }
        public bool IsEditMode { get; set; }

        #endregion

        #region Constructor

        public CreateBarcodeDialogFormModel(IProductService productService,
            IProductBarcodeService productBarcodeService)
        {
            _productService = productService;
            _productBarcodeService = productBarcodeService;
            CloseCommand = new RelayCommand(() => CurrentWindowService.Close());
        }

        #endregion

        #region Private Voids



        #endregion

        #region Public Voids

        [Command]
        public async void UserControlLoaded()
        {
            Products = await _productService.GetAllAsync();
        }

        [Command]
        public async void CreateBarcode()
        {
            try
            {
                if (!string.IsNullOrEmpty(Barcode))
                {
                    if (await _productBarcodeService.CheckAsync(Barcode))
                    {
                        MessageBoxService.ShowMessage($"Штрихкод \"{Barcode}\" существует!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
                    }
                    else
                    {
                        if (IsEditMode)
                        {
                            ProductBarcode productBarcode = new()
                            {
                                Id = SelectedProductBarcode.Id,
                                Barcode = Barcode,
                                ProductId = SelectedProductBarcode.ProductId
                            };
                            _ = await _productBarcodeService.UpdateAsync(productBarcode.Id, productBarcode);
                        }
                        else
                        {
                            _ = await _productBarcodeService.CreateAsync(new ProductBarcode
                            {
                                Barcode = Barcode,
                                ProductId = SelectedProductId.Value
                            });
                        }
                        CurrentWindowService.Close();
                    }
                }
                else
                {
                    MessageBoxService.ShowMessage("Введите штрихкод!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void GenerateBarcode()
        {
            Barcode = $"2{new('0', 12 - SelectedProductId.ToString().Length)}{SelectedProductId}";
        }

        #endregion
    }
}
