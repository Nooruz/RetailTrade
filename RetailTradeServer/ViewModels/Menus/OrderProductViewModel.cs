using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using SalePageServer.State.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class OrderProductViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IOrderToSupplierService _orderToSupplierService;
        private readonly IOrderStatusService _orderStatusService;
        private readonly IDialogService _dialogService;
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IUserStore _userStore;
        private readonly IDataService<Unit> _unitService;
        private IEnumerable<OrderStatus> _orderStatuses;
        private ObservableCollection<OrderToSupplier> _ordersToSuppliers;

        #endregion

        #region Public Properties

        public ObservableCollection<OrderToSupplier> OrdersToSuppliers
        {
            get => _ordersToSuppliers ?? new();
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
        public ICommand UserControlLoadedCommand { get; }

        #endregion

        #region Constructor

        public OrderProductViewModel(IOrderToSupplierService orderToSupplierService,
            IDialogService dialogService,
            IProductService productService,
            ISupplierService supplierService,
            IOrderStatusService orderStatusService,
            IUserStore userStore,
            IDataService<Unit> unitService)
        {
            _orderToSupplierService = orderToSupplierService;
            _dialogService = dialogService;
            _productService = productService;
            _supplierService = supplierService;
            _orderStatusService = orderStatusService;
            _userStore = userStore;
            _unitService = unitService;

            CreateOrderCommand = new RelayCommand(CreateOrder);
            DeleteOrderCommand = new RelayCommand(DeleteOrder);
            ValidateCommand = new ParameterCommand(parameter => Validate(parameter));
            DuplicateOrderCommand = new RelayCommand(DuplicateOrder);
            PrintOrderCommand = new RelayCommand(PrintOrder);
            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);

            _orderToSupplierService.OnOrderToSupplierCreated += OrderToSupplierService_OnOrderToSupplierCreated;
        }

        #endregion

        #region Private Voids

        private async void UserControlLoaded()
        {
            OrdersToSuppliers = new(await _orderToSupplierService.GetAllAsync());
            OrderStatuses = await _orderStatusService.GetAllAsync();
            ShowLoadingPanel = false;
        }

        private void OrderToSupplierService_OnOrderToSupplierCreated(OrderToSupplier orderToSupplier)
        {
            OrdersToSuppliers.Add(orderToSupplier);
        }

        private async void PrintOrder()
        {
            if (SelectedOrderToSupplier != null)
            {

            }
            else
            {
                _dialogService.ShowMessage("Выберите заказ.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void DuplicateOrder()
        {
            if (SelectedOrderToSupplier != null)
            {

            }
            else
            {
                _dialogService.ShowMessage("Выберите заказ.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Validate(object parameter)
        {

        }

        private void CreateOrder()
        {
            _dialogService.ShowDialog(new CreateOrderToSupplierDialogFormModel(_productService, _supplierService, _orderToSupplierService, _orderStatusService, _unitService, _userStore, _dialogService) { Title = "Заказ поставшику (новый)" }, 
                new CreateOrderToSupplierDialogForm());
        }

        private async void DeleteOrder()
        {
            if (SelectedOrderToSupplier != null)
            {
                if (_dialogService.ShowMessage("Вы точно хотите удалит выбранный заказ?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _orderToSupplierService.DeleteAsync(SelectedOrderToSupplier.Id);
                }                
            }
            else
            {
                _dialogService.ShowMessage("Выберите заказ.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion

        public override void Dispose()
        {
            OrdersToSuppliers = null;
            OrderStatuses = null;
            SelectedOrderToSupplier = null;
            base.Dispose();
        }
    }
}
