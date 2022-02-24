using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Menus;
using System;

namespace RetailTradeServer.ViewModels.Factories
{
    public class MenuViewModelFactory : IMenuViewModelFactory
    {
        #region Private Members

        private readonly CreateMenuViewModel<ProductViewModel> _createProductViewModel;
        private readonly CreateMenuViewModel<ArrivalProductViewModel> _createArrivalProductViewModel;
        private readonly CreateMenuViewModel<WriteDownProductViewModel> _createWriteDownProductViewModel;
        private readonly CreateMenuViewModel<OrderProductViewModel> _createOrderProductViewModel;
        private readonly CreateMenuViewModel<ProductBarcodeViewModel> _createProductBarcodeViewModel;
        private readonly CreateMenuViewModel<BranchViewModel> _createBranchViewModel;
        private readonly CreateMenuViewModel<UserViewModel> _createUserViewModel;
        private readonly CreateMenuViewModel<RefundToSupplierViewModel> _createRefundToSupplierViewModel;
        private readonly CreateMenuViewModel<SupplierViewModel> _createSupplierViewModel;
        private readonly CreateMenuViewModel<SaleDashboardView> _createMenuViewModel;
        private readonly CreateMenuViewModel<EmployeesViewModel> _createEmployeesViewModel;
        private readonly CreateMenuViewModel<ConnectingAndConfiguringHardwareViewModel> _createConnectingAndConfiguringEquipmentViewModel;
        private readonly CreateMenuViewModel<CashShiftsViewModel> _createCashShiftsViewModel;
        private readonly CreateMenuViewModel<WareHouseViewModel> _createWareHouseViewModel;
        private readonly CreateMenuViewModel<RevaluationViewModel> _createRevaluationViewModel;
        private readonly CreateMenuViewModel<ReturnProductFromCustomerViewModel> _createReturnProductFromCustomerViewModel;

        #endregion

        #region Constructor

        public MenuViewModelFactory(CreateMenuViewModel<ProductViewModel> createProductViewModel,
            CreateMenuViewModel<ArrivalProductViewModel> createArrivalProductViewModel,
            CreateMenuViewModel<WriteDownProductViewModel> createWriteDownProductViewModel,
            CreateMenuViewModel<OrderProductViewModel> createOrderProductViewModel,
            CreateMenuViewModel<ProductBarcodeViewModel> createProductBarcodeViewModel,
            CreateMenuViewModel<BranchViewModel> createBranchViewModel,
            CreateMenuViewModel<UserViewModel> createUserViewModel,
            CreateMenuViewModel<RefundToSupplierViewModel> createRefundToSupplierViewModel,
            CreateMenuViewModel<SupplierViewModel> createSupplierViewModel,
            CreateMenuViewModel<SaleDashboardView> createMenuViewModel,
            CreateMenuViewModel<EmployeesViewModel> createEmployeesViewModel,
            CreateMenuViewModel<ConnectingAndConfiguringHardwareViewModel> createConnectingAndConfiguringEquipmentViewModel,
            CreateMenuViewModel<CashShiftsViewModel> createCashShiftsViewModel,
            CreateMenuViewModel<WareHouseViewModel> createWareHouseViewModel,
            CreateMenuViewModel<RevaluationViewModel> createRevaluationViewModel,
            CreateMenuViewModel<ReturnProductFromCustomerViewModel> createReturnProductFromCustomerViewModel)
        {
            _createProductViewModel = createProductViewModel;
            _createArrivalProductViewModel = createArrivalProductViewModel;
            _createWriteDownProductViewModel = createWriteDownProductViewModel;
            _createOrderProductViewModel = createOrderProductViewModel;
            _createProductBarcodeViewModel = createProductBarcodeViewModel;
            _createBranchViewModel = createBranchViewModel;
            _createUserViewModel = createUserViewModel;
            _createRefundToSupplierViewModel = createRefundToSupplierViewModel;
            _createSupplierViewModel = createSupplierViewModel;
            _createMenuViewModel = createMenuViewModel;
            _createEmployeesViewModel = createEmployeesViewModel;
            _createConnectingAndConfiguringEquipmentViewModel = createConnectingAndConfiguringEquipmentViewModel;
            _createCashShiftsViewModel = createCashShiftsViewModel;
            _createWareHouseViewModel = createWareHouseViewModel;
            _createRevaluationViewModel = createRevaluationViewModel;
            _createReturnProductFromCustomerViewModel = createReturnProductFromCustomerViewModel;
        }

        #endregion

        public BaseViewModel CreateViewModel(MenuViewType viewType)
        {
            return viewType switch
            {
                MenuViewType.Products => _createProductViewModel(),
                MenuViewType.ArrivalProduct => _createArrivalProductViewModel(),
                MenuViewType.WriteDownProduct => _createWriteDownProductViewModel(),
                MenuViewType.OrderProduct => _createOrderProductViewModel(),
                MenuViewType.ProductBarcode => _createProductBarcodeViewModel(),
                MenuViewType.SaleDashboard => _createMenuViewModel(),
                MenuViewType.Branch => _createBranchViewModel(),
                MenuViewType.User => _createUserViewModel(),
                MenuViewType.RefundToSupplier => _createRefundToSupplierViewModel(),
                MenuViewType.Supplier => _createSupplierViewModel(),
                MenuViewType.Employee => _createEmployeesViewModel(),
                MenuViewType.ConnectingAndConfiguringEquipment => _createConnectingAndConfiguringEquipmentViewModel(),
                MenuViewType.CashierView => _createCashShiftsViewModel(),
                MenuViewType.WareHouseView => _createWareHouseViewModel(),
                MenuViewType.RevaluationView => _createRevaluationViewModel(),
                MenuViewType.ReturnProductFromCustomerView => _createReturnProductFromCustomerViewModel(),
                _ => throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType"),
            };
        }
    }
}
