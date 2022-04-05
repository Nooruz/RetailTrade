using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Report;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ProductBarcodeViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
        private readonly IDataService<Unit> _unitService;
        private readonly IDataService<LabelPriceTag> _labelPriceTagService;
        private Product _selectedProduct;
        private ProductBarcodePrinting _selectedProductBarcodePrinting;
        private IEnumerable<Product> _products;
        private IEnumerable<Unit> _units;
        private IEnumerable<LabelPriceTag> _labelPriceTags;
        private IEnumerable<LabelPriceTag> _labels;
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
        public IEnumerable<Unit> Units
        {
            get => _units;
            set
            {
                _units = value;
                OnPropertyChanged(nameof(Units));
            }
        }
        public IEnumerable<LabelPriceTag> LabelPriceTags
        {
            get => _labelPriceTags;
            set
            {
                _labelPriceTags = value;
                OnPropertyChanged(nameof(LabelPriceTags));
            }
        }
        public IEnumerable<LabelPriceTag> Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                OnPropertyChanged(nameof(Labels));
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

        #region Constructor

        public ProductBarcodeViewModel(IProductService productService,
            ITypeProductService typeProductService,
            IDataService<Unit> unitService,
            IDataService<LabelPriceTag> labelPriceTagService)
        {
            _productService = productService;
            _typeProductService = typeProductService;
            _unitService = unitService;
            _labelPriceTagService = labelPriceTagService;

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
                            UnitId = item.UnitId,
                            SalePrice = item.SalePrice,
                            QuantityInStock = item.Quantity,
                            LabelId = 1
                        });
                        OnPropertyChanged(nameof(ProductBarcodePrintings));
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
                            QuantityInStock = product.Quantity
                        });
                    }
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

        #endregion

        #region Public Voids

        [Command]
        public void ShowLabel()
        {
            try
            {
                WindowService.Show(nameof(LabelPriceTagTemplateView), new LabelPriceTagTemplateViewModel() { LabelPriceTags = Labels });
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public async void PrintProductBarcode()
        {
            ProductBarcodeReport report = new();

            List<ProductBarcodePrinting> productBarcodePrintings = new();

            foreach (var item in ProductBarcodePrintings)
            {
                //for (int i = 0; i < item.Quantity; i++)
                //{
                //    productBarcodePrintings.Add(new ProductBarcodePrinting
                //    {
                //        Name = item.Name,
                //        Barcode = item.Barcode,
                //        UnitId = item.UnitId,
                //        SalePrice = item.SalePrice
                //    });
                //}
            }

            report.DataSource = productBarcodePrintings;
            await report.CreateDocumentAsync();

            DocumentViewerService.Show(nameof(DocumentViewerView), new DocumentViewerViewModel() { PrintingDocument = report });

            //PrintToolBase tool = new(report.PrintingSystem);
            //tool.PrinterSettings.PrinterName = Settings.Default.DefaultLabelPrinter;
            //tool.PrintingSystem.EndPrint += PrintingSystem_EndPrint;
            //tool.Print();
        }

        [Command]
        public void Clear()
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

        [Command]
        public async void UserControlLoaded()
        {
            Products = await _productService.GetAllAsync();
            Units = await _unitService.GetAllAsync();
            try
            {
                LabelPriceTags = await _labelPriceTagService.GetAllAsync();
                Labels = LabelPriceTags.Where(l => l.TypeLabelPriceTagId == 1).ToList();
            }
            catch (Exception)
            {
                //ignore
            }
            _productBarcodePrintings = new();
            ShowLoadingPanel = false;
        }

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
