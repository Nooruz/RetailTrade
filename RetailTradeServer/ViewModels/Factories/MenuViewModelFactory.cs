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
        private readonly CreateMenuViewModel<LossViewModel> _createLossViewModel;
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
        private readonly CreateMenuViewModel<PriceListViewModel> _createPriceListViewModel;
        private readonly CreateMenuViewModel<CreateProductViewModel> _createCreateProductViewModel;
        private readonly CreateMenuViewModel<PointSaleViewModel> _createPointSaleViewModel;
        private readonly CreateMenuViewModel<CreatePointSaleViewModel> _createCreatePointSaleViewModel;
        private readonly CreateMenuViewModel<EnterViewModel> _createEnterViewModel;
        private readonly CreateMenuViewModel<EnterProductViewModel> _createEnterProductViewModel;
        private readonly CreateMenuViewModel<LossProductViewModel> _createLossProductViewModel;
        private readonly CreateMenuViewModel<MoveViewModel> _createMoveViewModel;
        private readonly CreateMenuViewModel<MoveProductViewModel> _createMoveProductViewModel;

        #endregion

        #region Constructor

        public MenuViewModelFactory(CreateMenuViewModel<ProductViewModel> createProductViewModel,
            CreateMenuViewModel<ArrivalProductViewModel> createArrivalProductViewModel,
            CreateMenuViewModel<LossViewModel> createLossViewModel,
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
            CreateMenuViewModel<ReturnProductFromCustomerViewModel> createReturnProductFromCustomerViewModel,
            CreateMenuViewModel<PriceListViewModel> createPriceListViewModel,
            CreateMenuViewModel<CreateProductViewModel> createCreateProductViewModel,
            CreateMenuViewModel<PointSaleViewModel> createPointSaleViewModel,
            CreateMenuViewModel<CreatePointSaleViewModel> createCreatePointSaleViewModel,
            CreateMenuViewModel<EnterViewModel> createEnterViewModel,
            CreateMenuViewModel<EnterProductViewModel> createEnterProductViewModel,
            CreateMenuViewModel<LossProductViewModel> createLossProductViewModel,
            CreateMenuViewModel<MoveViewModel> createMoveViewModel,
            CreateMenuViewModel<MoveProductViewModel> createMoveProductViewModel)
        {
            _createProductViewModel = createProductViewModel;
            _createArrivalProductViewModel = createArrivalProductViewModel;
            _createLossViewModel = createLossViewModel;
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
            _createPriceListViewModel = createPriceListViewModel;
            _createCreateProductViewModel = createCreateProductViewModel;
            _createCreatePointSaleViewModel = createCreatePointSaleViewModel;
            _createPointSaleViewModel = createPointSaleViewModel;
            _createEnterViewModel = createEnterViewModel;
            _createEnterProductViewModel = createEnterProductViewModel;
            _createLossProductViewModel = createLossProductViewModel;
            _createMoveViewModel = createMoveViewModel;
            _createMoveProductViewModel = createMoveProductViewModel;
        }

        #endregion

        public BaseViewModel CreateViewModel(MenuViewType viewType)
        {
            return viewType switch
            {
                MenuViewType.Products => _createProductViewModel(),
                MenuViewType.ArrivalProduct => _createArrivalProductViewModel(),
                MenuViewType.Loss => _createLossViewModel(),
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
                MenuViewType.PriceListView => _createPriceListViewModel(),
                MenuViewType.CreateProductView => _createCreateProductViewModel(),
                MenuViewType.PointSale => _createPointSaleViewModel(),
                MenuViewType.CreatePointSale => _createCreatePointSaleViewModel(),
                MenuViewType.Enter => _createEnterViewModel(),
                MenuViewType.EnterProduct => _createEnterProductViewModel(),
                MenuViewType.LossProduct => _createLossProductViewModel(),
                MenuViewType.Move => _createMoveViewModel(),
                MenuViewType.MoveProduct => _createMoveProductViewModel(),
                _ => throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType"),
            };
        }
    }
}
