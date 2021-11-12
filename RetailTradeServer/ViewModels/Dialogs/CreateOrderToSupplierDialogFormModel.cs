using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateOrderToSupplierDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IUIManager _manager;
        private int? _selectedSupplierId;
        private Product _selectedProduct;

        #endregion

        #region Public Properties

        public IEnumerable<Supplier> Suppliers => _supplierService.GetOnlyNames();

        public int? SelectedSupplierId
        {
            get => _selectedSupplierId;
            set
            {
                _selectedSupplierId = value;
                OnPropertyChanged(nameof(SelectedSupplierId));
                OnPropertyChanged(nameof(Products));
            }
        }

        public IEnumerable<Product> Products => SelectedSupplierId != null ? _productService.GetForRefund(SelectedSupplierId.Value) : null;

        public ObservableCollection<OrderProduct> OrderProducts { get; set; }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        public bool CanOrderProduct => OrderProducts.Count == 0 ? false : OrderProducts.FirstOrDefault(p => p.Quantity == 0) == null;

        #endregion

        #region Commands

        public ICommand RowDoubleClickCommand { get; }
        public ICommand ValidateCellCommand { get; }
        public ICommand OrderProductCommand { get; }
        public ICommand ClearCommand { get; }

        #endregion

        #region Constructor

        public CreateOrderToSupplierDialogFormModel(IProductService productService,
            ISupplierService supplierService,
            IUIManager manager)
        {
            _productService = productService;
            _supplierService = supplierService;
            _manager = manager;

            OrderProducts = new();

            RowDoubleClickCommand = new RelayCommand(RowDoubleClick);
            ValidateCellCommand = new ParameterCommand(parameter => ValidateCell(parameter));
            OrderProductCommand = new RelayCommand(Order);
            ClearCommand = new RelayCommand(Cleare);

            OrderProducts.CollectionChanged += ProductRefunds_CollectionChanged;
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
            OnPropertyChanged(nameof(OrderProducts));
            OnPropertyChanged(nameof(CanOrderProduct));
        }
        
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(OrderProducts));
            OnPropertyChanged(nameof(CanOrderProduct));
        }

        private void RowDoubleClick()
        {
            if (SelectedProduct != null)
            {
                if (OrderProducts.FirstOrDefault(pr => pr.ProductId == SelectedProduct.Id) == null)
                {
                    OrderProducts.Add(new OrderProduct
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
                if (((OrderProduct)e.Row).Product != null)
                {
                    if (((OrderProduct)e.Row).Product.Quantity < (decimal)e.Value)
                    {
                        e.IsValid = false;
                        e.ErrorContent = "Количество возврата не должно превышать количество на складе.";
                        _manager.ShowMessage("Количество возврата не должно превышать количество на складе.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            OnPropertyChanged(nameof(CanOrderProduct));
        }

        private async void Order()
        {
            if (OrderProducts.Count > 0)
            {
                //if (await _productRefundToSupplierService.AddRangeAsync(ProductRefunds.ToList()))
                //{
                //    _manager.ShowMessage("Операция успешно выполнена.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                //    Cleare();
                //}
                //else
                //{
                //    _manager.ShowMessage("Ошибка при выполнении операции.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
            }
        }

        private void Cleare()
        {
            OrderProducts.Clear();
        }

        #endregion
    }
}
