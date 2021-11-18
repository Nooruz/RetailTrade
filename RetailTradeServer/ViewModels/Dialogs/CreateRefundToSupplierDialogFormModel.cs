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
    public class CreateRefundToSupplierDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IArrivalService _arrivalService;
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
        public ObservableCollection<ArrivalProduct> ArrivalProducts { get; set; }
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }
        public bool CanArrivalProduct => ArrivalProducts.Count == 0 ? false : ArrivalProducts.FirstOrDefault(p => p.Quantity == 0) == null;

        #endregion

        #region Commands

        public ICommand RowDoubleClickCommand { get; }
        public ICommand ValidateCellCommand { get; }
        public ICommand ArrivalProductCommand { get; }
        public ICommand ClearCommand { get; }

        #endregion

        #region Constructor

        public CreateRefundToSupplierDialogFormModel(IProductService productService,
            ISupplierService supplierService,
            IArrivalService arrivalService,
            IUIManager manager)
        {
            _productService = productService;
            _supplierService = supplierService;
            _arrivalService = arrivalService;
            _manager = manager;

            ArrivalProducts = new();

            GetSupplier();

            RowDoubleClickCommand = new RelayCommand(RowDoubleClick);
            ValidateCellCommand = new ParameterCommand(parameter => ValidateCell(parameter));
            ArrivalProductCommand = new RelayCommand(CreateArrival);
            ClearCommand = new RelayCommand(Cleare);

            ArrivalProducts.CollectionChanged += ProductRefunds_CollectionChanged;
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
            OnPropertyChanged(nameof(ArrivalProducts));
            OnPropertyChanged(nameof(CanArrivalProduct));
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ArrivalProducts));
            OnPropertyChanged(nameof(CanArrivalProduct));
        }

        private void RowDoubleClick()
        {
            if (SelectedProduct != null)
            {
                if (ArrivalProducts.FirstOrDefault(pr => pr.ProductId == SelectedProduct.Id) == null)
                {
                    ArrivalProducts.Add(new ArrivalProduct
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
                if (((ArrivalProduct)e.Row).Product != null)
                {
                    if (((ArrivalProduct)e.Row).Product.Quantity < 0)
                    {
                        e.IsValid = false;
                        e.ErrorContent = "Количество не должно быть 0.";
                        _manager.ShowMessage("Количество не должно быть 0.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            OnPropertyChanged(nameof(CanArrivalProduct));
        }

        private async void CreateArrival()
        {
            if (CanArrivalProduct)
            {
                List<ArrivalProduct> arrivals = new();
                foreach (var item in ArrivalProducts)
                {
                    arrivals.Add(new ArrivalProduct
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }
                try
                {
                    Arrival arrival = await _arrivalService.CreateAsync(new Arrival
                    {
                        ArrivalDate = DateTime.Now,
                        SupplierId = SelectedSupplier.Id,
                        Comment = Comment,
                        ArrivalProducts = arrivals
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
            ArrivalProducts.Clear();
        }

        private async void GetProducts()
        {
            if (SelectedSupplier != null)
            {
                Products = await _productService.PredicateSelect(p => p.SupplierId == SelectedSupplier.Id, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, Unit = p.Unit });
            }
        }

        private async void GetSupplier()
        {
            Suppliers = await _supplierService.GetAllAsync();
        }

        #endregion
    }
}
