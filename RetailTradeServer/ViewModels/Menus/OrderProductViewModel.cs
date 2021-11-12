using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System.Collections.Generic;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class OrderProductViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IOrderToSupplierService _orderToSupplierService;
        private readonly IUIManager _manager;
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
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

        #endregion

        #region Commands

        public ICommand CreateOrderCommand { get; }

        #endregion

        #region Constructor

        public OrderProductViewModel(IOrderToSupplierService orderToSupplierService,
            IUIManager manager,
            IProductService productService,
            ISupplierService supplierService)
        {
            _orderToSupplierService = orderToSupplierService;
            _manager = manager;
            _productService = productService;
            _supplierService = supplierService;

            CreateOrderCommand = new RelayCommand(CreateOrder);

            GetOrdersToSuppliers();
        }

        #endregion

        #region Private Voids

        private async void GetOrdersToSuppliers()
        {
            OrdersToSuppliers = await _orderToSupplierService.GetAllAsync();
        }

        private void CreateOrder()
        {
            _manager.ShowDialog(new CreateOrderToSupplierDialogFormModel(_productService, _supplierService, _manager) { Title = "Заказ поставшику (новый)" }, 
                new CreateOrderToSupplierDialogForm());
        }

        #endregion
    }
}
