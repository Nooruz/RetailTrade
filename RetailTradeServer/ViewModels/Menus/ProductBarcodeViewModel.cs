using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using SalePageServer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ProductBarcodeViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private Product _selectedProduct;
        private ProductBarcodePrinting _selectedProductBarcodePrinting;
        private IEnumerable<Product> _products;
        private ObservableQueue<ProductBarcodePrinting> _productBarcodePrintings;

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
        public IEnumerable<ProductBarcodePrinting> ProductBarcodePrintings => _productBarcodePrintings;
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

        public ICommand AddProductToPrintCommand => new RelayCommand(AddProductToPrint);
        public ICommand PrintProductBarcodeCommand => new RelayCommand(PrintProductBarcode);
        public ICommand ClearCommand => new RelayCommand(Clear);
        public ICommand GenerateBarcodeCommand => new RelayCommand(GenerateBarcode);
        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);

        #endregion

        #region Constructor

        public ProductBarcodeViewModel(IProductService productService)
        {
            _productService = productService;

            Header = "Ценники и этикетки (Штрих-код)";
        }        

        #endregion

        #region Private Voids

        private void AddProductToPrint()
        {
            if (ProductBarcodePrintings.FirstOrDefault(p => p.Id == SelectedProduct.Id) == null)
            {
                _productBarcodePrintings.Enqueue(new ProductBarcodePrinting
                {
                    Id = SelectedProduct.Id,
                    Name = SelectedProduct.Name,
                    Barcode = SelectedProduct.Barcode,
                    Quantity = 1
                });
                OnPropertyChanged(nameof(ProductBarcodePrintings));
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
            _productBarcodePrintings.Clear();
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
