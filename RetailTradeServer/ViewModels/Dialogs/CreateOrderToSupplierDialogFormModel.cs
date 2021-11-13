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
    public class CreateOrderToSupplierDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IOrderToSupplierService _orderToSupplierService;
        private readonly IUIManager _manager;
        private int? _selectedSupplierId;
        private Product _selectedProduct;
        private string _comment;

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
                Cleare();
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
            IOrderToSupplierService orderToSupplierService,
            IUIManager manager)
        {
            _productService = productService;
            _supplierService = supplierService;
            _orderToSupplierService = orderToSupplierService;
            _manager = manager;

            OrderProducts = new();

            RowDoubleClickCommand = new RelayCommand(RowDoubleClick);
            ValidateCellCommand = new ParameterCommand(parameter => ValidateCell(parameter));
            OrderProductCommand = new RelayCommand(CreateOrder);
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
                    if (((OrderProduct)e.Row).Product.Quantity < 0)
                    {
                        e.IsValid = false;
                        e.ErrorContent = "Количество заказа не должно быть 0.";
                        _manager.ShowMessage("Количество заказа не должно быть 0.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            OnPropertyChanged(nameof(CanOrderProduct));
        }

        private async void CreateOrder()
        {
            if (CanOrderProduct)
            {
                List<OrderProduct> orders = new();
                foreach (var item in OrderProducts)
                {
                    orders.Add(new OrderProduct
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }
                OrderToSupplier order = await _orderToSupplierService.CreateAsync(new OrderToSupplier
                {
                    OrderDate = DateTime.Now,
                    SupplierId = SelectedSupplierId.Value,
                    OrderStatusId = 1,
                    Comment = Comment,
                    OrderProducts = orders
                });
            }
        }

        private void Cleare()
        {
            OrderProducts.Clear();
        }

        #endregion
    }
}
