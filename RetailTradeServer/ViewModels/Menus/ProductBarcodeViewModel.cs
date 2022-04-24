using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Barcode.Services;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.State.Printing;
using RetailTradeServer.State.Reports;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using RetailTradeServer.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ProductBarcodeViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly IBarcodeService _barcodeService;
        private readonly ITypeProductService _typeProductService;
        private readonly IDataService<Unit> _unitService;
        private readonly ILabelPriceTagService _labelPriceTagService;
        private readonly IDataService<TypeLabelPriceTag> _typeLabelPriceTagService;
        private readonly ILabelPrintingService _labelPrintingService;
        private readonly IReportService _reportService;
        private readonly ILabelPriceTagSizeService _labelPriceTagSizeService;
        private object _syncLock = new();
        private Product _selectedProduct;
        private LabelPrinting _selectedLabelPrinting;
        private IEnumerable<Product> _products;
        private IEnumerable<Unit> _units;
        private IEnumerable<LabelPriceTag> _labelPriceTags;
        private ObservableCollection<LabelPriceTag> _labels = new();
        private ObservableCollection<LabelPrinting> _productBarcodePrintings = new();
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
        public ObservableCollection<LabelPriceTag> Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                OnPropertyChanged(nameof(Labels));
            }
        }
        public ObservableCollection<LabelPrinting> LabelPrintings => _labelPrintingService.LabelPrintings;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }
        public LabelPrinting SelectedLabelPrinting
        {
            get => _selectedLabelPrinting;
            set
            {
                _selectedLabelPrinting = value;
                OnPropertyChanged(nameof(SelectedLabelPrinting));
            }
        }
        public GridControl BarcodeGridControl { get; set; }
        public ICollectionView ProductBarcodeCollectionView => CollectionViewSource.GetDefaultView(LabelPrintings);

        #endregion

        #region Constructor

        public ProductBarcodeViewModel(IProductService productService,
            ITypeProductService typeProductService,
            IDataService<Unit> unitService,
            ILabelPriceTagService labelPriceTagService,
            IDataService<TypeLabelPriceTag> typeLabelPriceTagService,
            ILabelPrintingService labelPrintingService,
            IReportService reportService,
            ILabelPriceTagSizeService labelPriceTagSizeService,
            IBarcodeService barcodeService)
        {
            _productService = productService;
            _typeProductService = typeProductService;
            _unitService = unitService;
            _labelPriceTagService = labelPriceTagService;
            _typeLabelPriceTagService = typeLabelPriceTagService;
            _labelPrintingService = labelPrintingService;
            _reportService = reportService;
            _labelPriceTagSizeService = labelPriceTagSizeService;
            _barcodeService = barcodeService;

            _productDialogFormModel = new(_typeProductService);

            Header = "Ценники и этикетки (Штрих-код)";

            BindingOperations.EnableCollectionSynchronization(LabelPrintings, _syncLock);

            _productDialogFormModel.OnProductsSelected += ProductDialogFormModel_OnProductsSelected;
            _labelPriceTagService.OnCreated += LabelPriceTagService_OnCreated;
        }

        #endregion

        #region Private Voids

        private async void BarcodeService_OnBarcodeEvent(string barcode)
        {
            try
            {
                Product product = await _productService.GetByBarcodeAsync(barcode);
                if (product != null)
                {

                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void LabelPriceTagService_OnCreated(LabelPriceTag labelPriceTag)
        {
            try
            {
                Labels.Add(labelPriceTag);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void ProductDialogFormModel_OnProductsSelected(IEnumerable<Product> products)
        {
            products.ToList().ForEach(item =>
            {
                try
                {
                    _labelPrintingService.Add(new LabelPrinting
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Barcode = item.Barcode,
                        UnitId = item.UnitId,
                        SalePrice = item.SalePrice,
                        QuantityInStock = item.Quantity,
                        Quantity = 1,
                        LabelId = 1
                    });
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
                        _labelPrintingService.Add(new LabelPrinting
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Barcode = product.Barcode,
                            UnitId = product.UnitId,
                            SalePrice = product.SalePrice,
                            QuantityInStock = product.Quantity,
                            Quantity = 1,
                            LabelId = 1
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
            _labelPrintingService.Clear();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _barcodeService.OnBarcodeEvent -= AddProductToPrint;
            _barcodeService.Close(BarcodeDevice.Com);
        }

        #endregion

        #region Public Voids

        [Command]
        public void DeleteSelectedLabelPrinting()
        {
            if (SelectedLabelPrinting != null)
            {
                _labelPrintingService.Delete(SelectedLabelPrinting);
            }
        }

        [Command]
        public void ShowLabel(int labelId)
        {
            try
            {
                WindowService.Show(nameof(LabelPriceTagTemplateView), new LabelPriceTagTemplateViewModel(_reportService, _labelPriceTagService, _typeLabelPriceTagService, _labelPriceTagSizeService) { SelectedTypeLabelPriceTagId = labelId });
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public async void PrintProductBarcode()
        {
            try
            {
                DocumentViewerService.Show(nameof(DocumentViewerView), new DocumentViewerViewModel() { PrintingDocument = await _reportService.CreateLabelReport() });
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void Clear()
        {
            try
            {
                if (MessageBoxService.ShowMessage("Очистить список?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                {
                    _labelPrintingService.Clear();
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public async void UserControlLoaded(object sender)
        {
            try
            {
                if (sender is RoutedEventArgs e)
                {
                    if (e.Source is UserControl userControl)
                    {
                        userControl.Unloaded += UserControl_Unloaded;
                    }
                }
                _barcodeService.Open(BarcodeDevice.Com, Settings.Default.BarcodeCom, Settings.Default.BarcodeSpeed);
                _barcodeService.OnBarcodeEvent += AddProductToPrint;
                Products = await _productService.GetAllAsync();
                Units = await _unitService.GetAllAsync();
                LabelPriceTags = await _labelPriceTagService.GetAllAsync();
                Labels = new(LabelPriceTags.Where(l => l.TypeLabelPriceTagId == 1).ToList());
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

        [Command]
        public void GridControlLoaded(object sender)
        {
            try
            {
                if (sender is RoutedEventArgs e)
                {
                    if (e.Source is GridControl gridControl)
                    {
                        BarcodeGridControl = gridControl;
                    }
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
