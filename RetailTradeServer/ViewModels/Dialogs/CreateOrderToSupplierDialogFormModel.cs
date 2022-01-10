using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using SalePageServer.State.Dialogs;
using SalePageServer.Utilities;
using System;
using System.Collections.Generic;
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
        private readonly IOrderStatusService _orderStatusService;
        private readonly IOrderToSupplierService _orderToSupplierService;
        private readonly IDataService<Unit> _unitService;
        private readonly IUserStore _userStore;
        private readonly IDialogService _dialogService;
        private Supplier _selectedSupplier;
        private OrderProduct _selectedOrderProduct;
        private int _selectedOrderStatusId = 1;
        private string _comment;
        private IEnumerable<Supplier> _suppliers;
        private IEnumerable<Product> _products;
        private IEnumerable<OrderStatus> _orderStatuses;
        private IEnumerable<Unit> _units;
        private ObservableQueue<OrderProduct> _orderProducts;

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
        public IEnumerable<OrderStatus> OrderStatuses
        {
            get => _orderStatuses;
            set
            {
                _orderStatuses = value;
                OnPropertyChanged(nameof(OrderStatuses));
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
        public int SelectedOrderStatusId
        {
            get => _selectedOrderStatusId;
            set
            {
                _selectedOrderStatusId = value;
                OnPropertyChanged(nameof(SelectedOrderStatusId));
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
        public IEnumerable<OrderProduct> OrderProducts => _orderProducts;
        public OrderProduct SelectedOrderProduct
        {
            get => _selectedOrderProduct;
            set
            {
                _selectedOrderProduct = value;
                OnPropertyChanged(nameof(SelectedOrderProduct));                
            }
        }
        public bool CanOrderProduct => OrderProducts.Any() && !OrderProducts.Any(p => p.Quantity == 0);

        #endregion

        #region Commands

        public ICommand ValidateCellCommand { get; }
        public ICommand OrderProductCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand AddProductToOrderCommand { get; }
        public ICommand UserControlLoadedCommand { get; }
        public ICommand CellValueChangedCommand { get; }

        #endregion

        #region Constructor

        public CreateOrderToSupplierDialogFormModel(IProductService productService,
            ISupplierService supplierService,
            IOrderToSupplierService orderToSupplierService,
            IOrderStatusService orderStatusService,
            IDataService<Unit> unitService,
            IUserStore userStore,
            IDialogService dialogService)
        {
            _productService = productService;
            _supplierService = supplierService;
            _orderToSupplierService = orderToSupplierService;
            _orderStatusService = orderStatusService;
            _unitService = unitService;
            _userStore = userStore;
            _dialogService = dialogService;

            _orderProducts = new();

            ValidateCellCommand = new ParameterCommand(parameter => ValidateCell(parameter));
            OrderProductCommand = new RelayCommand(CreateOrder);
            ClearCommand = new RelayCommand(Cleare);
            AddProductToOrderCommand = new RelayCommand(AddProductToOrder);
            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);
            CellValueChangedCommand = new ParameterCommand(p => CellValueChanged(p));
        }

        #endregion

        #region Private Voids

        private void CellValueChanged(object parameter)
        {
            if (parameter is CellValueChangedEventArgs e)
            {
                if (e.Cell.Property == "ProductId")
                {
                    Product product = Products.FirstOrDefault(p => p.Id == (int)e.Value);
                    SelectedOrderProduct.ArrivalPrice = product.ArrivalPrice;
                    SelectedOrderProduct.Product = new Product { UnitId = product.UnitId };
                }
                TableView tableView = e.Source as TableView;
                tableView.PostEditor();
                tableView.Grid.UpdateTotalSummary();
            }
            OnPropertyChanged(nameof(CanOrderProduct));
        }

        private async void UserControlLoaded()
        {
            Suppliers = await _supplierService.GetAllAsync();
            OrderStatuses = await _orderStatusService.GetAllAsync();
            Units = await _unitService.GetAllAsync();
        }

        private void AddProductToOrder()
        {
            if (SelectedSupplier != null)
            {
                _orderProducts.Enqueue(new OrderProduct());
            }
            else
            {
                _dialogService.ShowMessage("Выберите поставщика!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            OnPropertyChanged(nameof(CanOrderProduct));
        }

        private void ValidateCell(object parameter)
        {
            if (parameter is GridCellValidationEventArgs e)
            {
                if ((decimal)e.Value < 0)
                {
                    e.IsValid = false;
                    e.ErrorContent = "Количество заказа не должно быть 0.";
                    _dialogService.ShowMessage("Количество заказа не должно быть 0.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            OnPropertyChanged(nameof(CanOrderProduct));
        }

        private async void CreateOrder()
        {
            OrderToSupplier order = await _orderToSupplierService.CreateAsync(new OrderToSupplier
            {
                OrderDate = DateTime.Now,
                SupplierId = SelectedSupplier.Id,
                OrderStatusId = 1,
                Comment = Comment,
                OrderProducts = OrderProducts.Select(o => new OrderProduct { ProductId = o.ProductId, Quantity = o.Quantity }).ToList()
            });

            OrderToSupplierReport report = new(order, SelectedSupplier, _userStore.CurrentOrganization)
            {
                DataSource = OrderProducts
            };

            await report.CreateDocumentAsync();

            await _dialogService.ShowPrintDialog(report);
        }

        private void Cleare()
        {
            _orderProducts.Clear();
        }

        private async void GetProducts()
        {
            if (SelectedSupplier != null)
            {
                Products = await _productService.PredicateSelect(p => p.SupplierId == SelectedSupplier.Id, p => new Product { Id = p.Id, Name = p.Name, UnitId = p.UnitId, ArrivalPrice = p.ArrivalPrice });
            }
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            base.Dispose();
        }

        #endregion
    }
}
