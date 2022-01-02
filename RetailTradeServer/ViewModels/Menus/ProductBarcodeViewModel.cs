using DevExpress.XtraPrinting;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.ViewModels.Base;
using SalePageServer.Properties;
using SalePageServer.State.Dialogs;
using SalePageServer.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ProductBarcodeViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly IDialogService _dialogService;
        private Product _selectedProduct;
        private ProductBarcodePrinting _selectedProductBarcodePrinting;
        private ObservableQueue<Product> _products;

        #endregion

        #region Public Properties

        public IEnumerable<Product> Products => _products;
        public ObservableCollection<ProductBarcodePrinting> ProductBarcodePrintings { get; set; }
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

        public ICommand AddProductToPrintCommand { get; }
        public ICommand PrintProductBarcodeCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand GenerateBarcodeCommand { get; }
        public ICommand UserControlLoadedCommand { get; }

        #endregion

        #region Constructor

        public ProductBarcodeViewModel(IProductService productService,
            IDialogService dialogService)
        {
            _productService = productService;
            _dialogService = dialogService;

            ProductBarcodePrintings = new();
            _products = new();

            AddProductToPrintCommand = new RelayCommand(AddProductToPrint);
            PrintProductBarcodeCommand = new RelayCommand(PrintProductBarcode);
            ClearCommand = new RelayCommand(Clear);
            GenerateBarcodeCommand = new RelayCommand(GenerateBarcode);
            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);

            ProductBarcodePrintings.CollectionChanged += ProductBarcodePrintings_CollectionChanged;
        }        

        #endregion

        #region Private Voids

        private void AddProductToPrint()
        {
            if (ProductBarcodePrintings.FirstOrDefault(p => p.Id == SelectedProduct.Id) == null)
            {
                ProductBarcodePrintings.Add(new ProductBarcodePrinting
                {
                    Id = SelectedProduct.Id,
                    Name = SelectedProduct.Name,
                    Barcode = SelectedProduct.Barcode,
                    Quantity = 1,
                    Price = SelectedProduct.SalePrice
                });
            }
        }

        private async void PrintProductBarcode()
        {
            if (string.IsNullOrEmpty(Settings.Default.DefaultLabelPrinter))
            {
                _dialogService.ShowMessage("Принтер для печати этикеток не настроен.", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
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
                            Barcode = item.Barcode,
                            Price = item.Price
                        });
                    }
                }

                report.DataSource = productBarcodePrintings;
                await report.CreateDocumentAsync();

                PrintToolBase tool = new(report.PrintingSystem);
                tool.PrinterSettings.PrinterName = Settings.Default.DefaultLabelPrinter;
                tool.PrintingSystem.EndPrint += PrintingSystem_EndPrint;
                tool.Print();
            }
        }

        private void ProductBarcodePrintings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    if (item != null)
                    {
                        item.PropertyChanged -= Item_PropertyChanged;
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    if (item != null)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ProductBarcodePrinting));
        }

        private void Clear()
        {
            ProductBarcodePrintings.Clear();
        }

        private void PrintingSystem_EndPrint(object sender, EventArgs e)
        {
            ProductBarcodePrintings.Clear();
        }

        private async void GenerateBarcode()
        {
            if (SelectedProductBarcodePrinting != null)
            {
                await _productService.GenerateBarcode(Products.FirstOrDefault(p => p.Id == SelectedProductBarcodePrinting.Id));
            }
        }

        private async void UserControlLoaded()
        {
            _products.AddRange(await _productService.GetAllAsync());
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            ProductBarcodePrintings.Clear();
            SelectedProduct = null;
            base.Dispose();
        }

        #endregion        
    }
}
