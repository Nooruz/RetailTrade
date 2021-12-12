using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateWriteDownProductDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IWriteDownService _writeDownService;
        private readonly IUIManager _manager;
        private Supplier _selectedSupplier;
        private Product _selectedProduct;
        private string _comment;
        private IEnumerable<Supplier> _suppliers;
        private IEnumerable<Product> _products;

        #endregion

        #region Public Properties

        public IEnumerable<Supplier> Suppliers
        {
            get => _suppliers;
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
            }
        }
        public Supplier SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));
                OnPropertyChanged(nameof(Products));
                Cleare();
                GetProducts();
            }
        }
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
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
        public ObservableCollection<WriteDownProduct> WriteDownProducts { get; set; }
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }
        public bool CanWriteDownProduct => WriteDownProducts.Count == 0 ? false : WriteDownProducts.FirstOrDefault(p => p.Quantity == 0) == null;

        #endregion

        #region Commands

        public ICommand RowDoubleClickCommand { get; }
        public ICommand ValidateCellCommand { get; }
        public ICommand WriteDownProductCommand { get; }
        public ICommand ClearCommand { get; }

        #endregion

        #region Constructor

        public CreateWriteDownProductDialogFormModel(IProductService productService,
            ISupplierService supplierService,
            IWriteDownService writeDownService,
            IUIManager manager)
        {
            _productService = productService;
            _supplierService = supplierService;
            _writeDownService = writeDownService;
            _manager = manager;

            WriteDownProducts = new();

            GetSupplier();

            RowDoubleClickCommand = new RelayCommand(RowDoubleClick);
            ValidateCellCommand = new ParameterCommand(parameter => ValidateCell(parameter));
            WriteDownProductCommand = new RelayCommand(CreateArrival);
            ClearCommand = new RelayCommand(Cleare);

            WriteDownProducts.CollectionChanged += ProductRefunds_CollectionChanged;
        }

        #endregion

        #region Private Voids

        private void ProductRefunds_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
            OnPropertyChanged(nameof(WriteDownProducts));
            OnPropertyChanged(nameof(CanWriteDownProduct));
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(WriteDownProducts));
            OnPropertyChanged(nameof(CanWriteDownProduct));
        }

        private void RowDoubleClick()
        {
            if (SelectedProduct != null)
            {
                if (WriteDownProducts.FirstOrDefault(pr => pr.ProductId == SelectedProduct.Id) == null)
                {
                    WriteDownProducts.Add(new WriteDownProduct
                    {
                        Product = SelectedProduct,
                        ProductId = SelectedProduct.Id
                    });
                }
            }
        }

        private void ValidateCell(object parameter)
        {
            if (parameter is GridCellValidationEventArgs e)
            {
                if (((WriteDownProduct)e.Row).Product != null)
                {
                    if ((double)e.Value > ((WriteDownProduct)e.Row).Product.Quantity)
                    {
                        e.IsValid = false;
                        e.ErrorContent = "Количество списание товаров не должно превышать количество на складе.";
                        _manager.ShowMessage("Количество списание товаров не должно превышать количество на складе.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            OnPropertyChanged(nameof(CanWriteDownProduct));
        }

        private async void CreateArrival()
        {
            if (CanWriteDownProduct)
            {
                List<WriteDownProduct> writeDownProducts = new();
                foreach (WriteDownProduct item in WriteDownProducts)
                {
                    writeDownProducts.Add(new WriteDownProduct
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }
                try
                {
                    _ = await _writeDownService.CreateAsync(new WriteDown
                    {
                        WriteDownDate = DateTime.Now,
                        SupplierId = SelectedSupplier.Id,
                        Comment = Comment,
                        WriteDownProducts = writeDownProducts
                    });
                }
                catch (Exception e)
                {
                    //ignore
                }

                _manager.Close();
            }
        }

        private void Cleare()
        {
            WriteDownProducts.Clear();
        }

        private async void GetProducts()
        {
            if (SelectedSupplier != null)
            {
                Products = await _productService.PredicateSelect(p => p.SupplierId == SelectedSupplier.Id && p.Quantity > 0, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, Unit = p.Unit });
            }
        }

        private async void GetSupplier()
        {
            Suppliers = await _supplierService.GetAllAsync();
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            WriteDownProducts.CollectionChanged -= ProductRefunds_CollectionChanged;
            base.Dispose();
        }

        #endregion
    }
}
