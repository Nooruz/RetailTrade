using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class OrderProductViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IOrderToSupplierService _orderToSupplierService;
        private readonly IOrderStatusService _orderStatusService;
        private readonly IUIManager _manager;
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IUserStore _userStore;
        private IEnumerable<OrderStatus> _orderStatuses;
        private IEnumerable<OrderToSupplier> _ordersToSuppliers;

        #endregion

        #region Public Properties

        public IEnumerable<OrderToSupplier> OrdersToSuppliers
        {
            get => _ordersToSuppliers;
            set
            {
                _ordersToSuppliers = value;
                OnPropertyChanged(nameof(OrdersToSuppliers));
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
        public OrderToSupplier SelectedOrderToSupplier { get; set; }

        #endregion

        #region Commands

        public ICommand CreateOrderCommand { get; }
        public ICommand DeleteOrderCommand { get; }
        public ICommand ValidateCommand { get; }
        public ICommand DuplicateOrderCommand { get; }
        public ICommand PrintOrderCommand { get; }

        #endregion

        #region Constructor

        public OrderProductViewModel(IOrderToSupplierService orderToSupplierService,
            IUIManager manager,
            IProductService productService,
            ISupplierService supplierService,
            IOrderStatusService orderStatusService,
            IUserStore userStore)
        {
            _orderToSupplierService = orderToSupplierService;
            _manager = manager;
            _productService = productService;
            _supplierService = supplierService;
            _orderStatusService = orderStatusService;
            _userStore = userStore;

            CreateOrderCommand = new RelayCommand(CreateOrder);
            DeleteOrderCommand = new RelayCommand(DeleteOrder);
            ValidateCommand = new ParameterCommand(parameter => Validate(parameter));
            DuplicateOrderCommand = new RelayCommand(DuplicateOrder);
            PrintOrderCommand = new RelayCommand(PrintOrder);

            GetOrdersToSuppliers();
            GetOrderStatuses();

            _orderToSupplierService.PropertiesChanged += GetOrdersToSuppliers;
        }

        #endregion

        #region Private Voids

        private async void PrintOrder()
        {
            if (SelectedOrderToSupplier != null)
            {

            }
            else
            {
                _manager.ShowMessage("Выберите заказ.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void DuplicateOrder()
        {
            if (SelectedOrderToSupplier != null)
            {

            }
            else
            {
                _manager.ShowMessage("Выберите заказ.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Validate(object parameter)
        {

        }

        private async void GetOrdersToSuppliers()
        {
            OrdersToSuppliers = await _orderToSupplierService.GetAllAsync();
        }

        private async void GetOrderStatuses()
        {
            OrderStatuses = await _orderStatusService.GetAllAsync();
        }

        private void CreateOrder()
        {
            _manager.ShowDialog(new CreateOrderToSupplierDialogFormModel(_productService, _supplierService, _orderToSupplierService, _userStore, _manager) { Title = "Заказ поставшику (новый)" }, 
                new CreateOrderToSupplierDialogForm());
        }

        private async void DeleteOrder()
        {
            if (SelectedOrderToSupplier != null)
            {
                if (_manager.ShowMessage("Вы точно хотите удалит выбранный заказ?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _orderToSupplierService.DeleteAsync(SelectedOrderToSupplier.Id);
                }                
            }
            else
            {
                _manager.ShowMessage("Выберите заказ.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion
    }
}
