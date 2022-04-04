using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ProductBarcodeViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
        private Product _selectedProduct;
        private ProductBarcodePrinting _selectedProductBarcodePrinting;
        private IEnumerable<Product> _products;
        private ObservableCollection<ProductBarcodePrinting> _productBarcodePrintings = new();
        private ProductDialogFormModel _productDialogFormModel;

        #endregion

        #region Public Properties

        public IEnumerable<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public ObservableCollection<ProductBarcodePrinting> ProductBarcodePrintings
        {
            get => _productBarcodePrintings;
            set
            {
                _productBarcodePrintings = value;
                OnPropertyChanged(nameof(ProductBarcodePrintings));
            }
        }
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }
        public ProductBarcodePrinting SelectedProductBarcodePrinting
        {
            get => _selectedProductBarcodePrinting;
            set
            {
                _selectedProductBarcodePrinting = value;
                OnPropertyChanged(nameof(SelectedProductBarcodePrinting));
            }
        }

        #endregion

        #region Commands

        public ICommand PrintProductBarcodeCommand => new RelayCommand(PrintProductBarcode);
        public ICommand ClearCommand => new RelayCommand(Clear);
        public ICommand GenerateBarcodeCommand => new RelayCommand(GenerateBarcode);
        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);

        #endregion

        #region Constructor

        public ProductBarcodeViewModel(IProductService productService,
            ITypeProductService typeProductService)
        {
            _productService = productService;
            _typeProductService = typeProductService;

            _productDialogFormModel = new(_typeProductService);

            Header = "Ценники и этикетки (Штрих-код)";

            _productDialogFormModel.OnProductsSelected += ProductDialogFormModel_OnProductsSelected;
        }

        #endregion

        #region Private Voids

        private void ProductDialogFormModel_OnProductsSelected(IEnumerable<Product> products)
        {
            products.ToList().ForEach(item =>
            {
                try
                {
                    if (!ProductBarcodePrintings.Any(p => p.Id == item.Id))
                    {
                        ProductBarcodePrintings.Add(new ProductBarcodePrinting
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Barcode = item.Barcode,
                            Quantity = 1
                        });
                    }
                }
                catch (Exception)
                {
                    //ignore
                }
            });
        }

        private void AddProductToPrint(string barcode)
        {
            try
            {
                if (Products.Any())
                {
                    Product product = Products.FirstOrDefault(p => p.Barcode == barcode);
                    if (product != null)
                    {
                        ProductBarcodePrintings.Add(new ProductBarcodePrinting
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Barcode = product.Barcode,
                            Quantity = 1
                        });
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private async void PrintProductBarcode()
        {
            ProductBarcodeReport report = new();

            List<ProductBarcodePrinting> productBarcodePrintings = new();

            foreach (var item in ProductBarcodePrintings)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    productBarcodePrintings.Add(new ProductBarcodePrinting
                    {
                        Name = item.Name,
                        Barcode = item.Barcode
                    });
                }
            }

            report.DataSource = productBarcodePrintings;
            await report.CreateDocumentAsync();

            DocumentViewerService.Show(nameof(DocumentViewerView), new DocumentViewerViewModel() { PrintingDocument = report });

            //PrintToolBase tool = new(report.PrintingSystem);
            //tool.PrinterSettings.PrinterName = Settings.Default.DefaultLabelPrinter;
            //tool.PrintingSystem.EndPrint += PrintingSystem_EndPrint;
            //tool.Print();
        }

        private void Clear()
        {
            try
            {
                if (MessageBoxService.ShowMessage("Очистить список?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                {
                    ProductBarcodePrintings.Clear();
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void PrintingSystem_EndPrint(object sender, EventArgs e)
        {
            _productBarcodePrintings.Clear();
        }

        private async void GenerateBarcode()
        {
            if (SelectedProductBarcodePrinting != null)
            {
                SelectedProductBarcodePrinting.Barcode = await _productService.GenerateBarcode(SelectedProductBarcodePrinting.Id);
            }
        }

        private async void UserControlLoaded()
        {
            Products = await _productService.GetAllAsync();
            _productBarcodePrintings = new();
            ShowLoadingPanel = false;
        }

        #endregion

        #region Public Voids

        [Command]
        public void AddProduct()
        {
            _productDialogFormModel.Products = new(Products);
            WindowService.Show(nameof(ProductDialogForm), _productDialogFormModel);
        }

        [Command]
        public void SearchBarcode()
        {
            try
            {
                BarcodeSearchDialogFormModel viewModel = new();
                UICommand result = DialogService.ShowDialog(dialogCommands: viewModel.Commands, "Введите штрихкод", nameof(BarcodeSearchDialogForm), viewModel);
                if (result != null && result.Id is MessageBoxResult messageResult && messageResult == MessageBoxResult.OK)
                {
                    AddProductToPrint(viewModel.Barcode);
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            //ProductBarcodePrintings.Clear();
            SelectedProduct = null;
            base.Dispose();
        }

        #endregion        
    }
}
