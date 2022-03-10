using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Barcode;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
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
        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
        private readonly ISupplierService _supplierService;
        private readonly IUserStore _userStore;
        private readonly IZebraBarcodeScanner _zebraBarcodeScanner;
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

        public ICommand CreateOrderCommand => new RelayCommand(CreateOrder);
        public ICommand DeleteOrderCommand => new RelayCommand(DeleteOrder);
        public ICommand ValidateCommand => new ParameterCommand(parameter => Validate(parameter));
        public ICommand DuplicateOrderCommand => new RelayCommand(DuplicateOrder);
        public ICommand PrintOrderCommand => new RelayCommand(PrintOrder);
        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);

        #endregion

        #region Constructor

        public OrderProductViewModel(IOrderToSupplierService orderToSupplierService,
            IProductService productService,
            ITypeProductService typeProductService,
            ISupplierService supplierService,
            IOrderStatusService orderStatusService,
            IUserStore userStore,
            IZebraBarcodeScanner zebraBarcodeScanner,
            IDataService<Unit> unitService)
        {
            _orderToSupplierService = orderToSupplierService;
            _productService = productService;
            _typeProductService = typeProductService;
            _supplierService = supplierService;
            _orderStatusService = orderStatusService;
            _userStore = userStore;
            _unitService = unitService;
            _zebraBarcodeScanner = zebraBarcodeScanner;

            Header = "Заказы поставщикам";

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
                _ = MessageBoxService.ShowMessage("Выберите заказ.", "", MessageButton.OK, MessageIcon.Exclamation);
            }
        }

        private async void DuplicateOrder()
        {
            if (SelectedOrderToSupplier != null)
            {

            }
            else
            {
                _ = MessageBoxService.ShowMessage("Выберите заказ.", "", MessageButton.OK, MessageIcon.Exclamation);
            }
        }

        private void Validate(object parameter)
        {

        }

        private void CreateOrder()
        {
            WindowService.Show(nameof(CreateOrderToSupplierDialogForm), new CreateOrderToSupplierDialogFormModel(_productService, _typeProductService, _supplierService, _orderToSupplierService, _zebraBarcodeScanner, _unitService, _userStore) { Title = "Заказ поставшику (новый)" });
        }

        private async void DeleteOrder()
        {
            if (SelectedOrderToSupplier != null)
            {
                if (MessageBoxService.ShowMessage("Вы точно хотите удалит выбранный заказ?", "", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                {
                    await _orderToSupplierService.DeleteAsync(SelectedOrderToSupplier.Id);
                }                
            }
            else
            {
                _ = MessageBoxService.ShowMessage("Выберите заказ.", "", MessageButton.OK, MessageIcon.Exclamation);
            }
        }

        #endregion

        public override void Dispose()
        {
            OrdersToSuppliers = null;
            OrderStatuses = null;
            SelectedOrderToSupplier = null;
            _orderToSupplierService.OnOrderToSupplierCreated -= OrderToSupplierService_OnOrderToSupplierCreated;
            base.Dispose();
        }
    }
}
