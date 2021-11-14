using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
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
        private readonly IUIManager _manager;
        private Product _selectedProduct;

        #endregion

        #region Public Properties

        public IEnumerable<Product> Products => _productService.GetAll();
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

        #endregion

        #region Commands

        public ICommand AddProductToPrintCommand { get; }
        public ICommand PrintProductBarcodeCommand { get; }
        public ICommand ClearCommand { get; set; }

        #endregion

        #region Constructor

        public ProductBarcodeViewModel(IProductService productService,
            IUIManager manager)
        {
            _productService = productService;
            _manager = manager;

            ProductBarcodePrintings = new();

            AddProductToPrintCommand = new RelayCommand(AddProductToPrint);
            PrintProductBarcodeCommand = new RelayCommand(PrintProductBarcode);
            ClearCommand = new RelayCommand(Clear);

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

        private void PrintProductBarcode()
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
            report.CreateDocument();

            _manager.ShowDialog(new DocumentViewerViewModel
            {
                Title = "Предварительный просмотр",
                PrintingDocument = report
            },
                new DocumentViewerView(),
                WindowState.Maximized,
                ResizeMode.CanResize,
                SizeToContent.Manual);
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

        #endregion

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
