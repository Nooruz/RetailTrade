using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
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
        private readonly IUserStore _userStore;
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
            IUserStore userStore,
            IUIManager manager)
        {
            _productService = productService;
            _supplierService = supplierService;
            _orderToSupplierService = orderToSupplierService;
            _userStore = userStore;
            _manager = manager;

            OrderProducts = new();

            GetSupplier();

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
                    SupplierId = SelectedSupplier.Id,
                    OrderStatusId = 1,
                    Comment = Comment,
                    OrderProducts = orders
                });

                OrderToSupplierReport report = new(order, SelectedSupplier, _userStore.CurrentOrganization)
                {
                    DataSource = OrderProducts
                };

                await report.CreateDocumentAsync();

                await _manager.ShowDialog(new DocumentViewerViewModel() { PrintingDocument = report }, 
                    new DocumentViewerView(), 
                    WindowState.Maximized, 
                    ResizeMode.NoResize, 
                    SizeToContent.Manual);

            }
        }

        private void Cleare()
        {
            OrderProducts.Clear();
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
