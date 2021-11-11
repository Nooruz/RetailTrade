using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Menus;
using System;

namespace RetailTradeServer.ViewModels.Factories
{
    public class MenuViewModelFactory : IMenuViewModelFactory
    {
        #region Private Members

        private readonly CreateMenuViewModel<ProductCategoryViewModel> _createProductCategoryViewModel;
        private readonly CreateMenuViewModel<ArrivalProductViewModel> _createArrivalProductViewModel;
        private readonly CreateMenuViewModel<WriteDownProductViewModel> _createWriteDownProductViewModel;
        private readonly CreateMenuViewModel<OrderProductViewModel> _createOrderProductViewModel;
        private readonly CreateMenuViewModel<ProductBarcodeViewModel> _createProductBarcodeViewModel;
        private readonly CreateMenuViewModel<AnalyticalPanelViewModel> _createAnalyticalPanelViewModel;
        private readonly CreateMenuViewModel<BranchViewModel> _createBranchViewModel;
        private readonly CreateMenuViewModel<UserViewModel> _createUserViewModel;
        private readonly CreateMenuViewModel<RefundToSupplierViewModel> _createRefundToSupplierViewModel;

        #endregion

        #region Constructor

        public MenuViewModelFactory(CreateMenuViewModel<ProductCategoryViewModel> createProductCategoryViewModel,
            CreateMenuViewModel<ArrivalProductViewModel> createArrivalProductViewModel,
            CreateMenuViewModel<WriteDownProductViewModel> createWriteDownProductViewModel,
            CreateMenuViewModel<OrderProductViewModel> createOrderProductViewModel,
            CreateMenuViewModel<ProductBarcodeViewModel> createProductBarcodeViewModel,
            CreateMenuViewModel<AnalyticalPanelViewModel> createAnalyticalPanelViewModel,
            CreateMenuViewModel<BranchViewModel> createBranchViewModel,
            CreateMenuViewModel<UserViewModel> createUserViewModel,
            CreateMenuViewModel<RefundToSupplierViewModel> createRefundToSupplierViewModel)
        {
            _createProductCategoryViewModel = createProductCategoryViewModel;
            _createArrivalProductViewModel = createArrivalProductViewModel;
            _createWriteDownProductViewModel = createWriteDownProductViewModel;
            _createOrderProductViewModel = createOrderProductViewModel;
            _createProductBarcodeViewModel = createProductBarcodeViewModel;
            _createAnalyticalPanelViewModel = createAnalyticalPanelViewModel;
            _createBranchViewModel = createBranchViewModel;
            _createUserViewModel = createUserViewModel;
            _createRefundToSupplierViewModel = createRefundToSupplierViewModel;
        }

        #endregion

        public BaseViewModel CreateViewModel(MenuViewType viewType)
        {
            return viewType switch
            {
                MenuViewType.ProductCategory => _createProductCategoryViewModel(),
                MenuViewType.ArrivalProduct => _createArrivalProductViewModel(),
                MenuViewType.WriteDownProduct => _createWriteDownProductViewModel(),
                MenuViewType.OrderProduct => _createOrderProductViewModel(),
                MenuViewType.ProductBarcode => _createProductBarcodeViewModel(),
                MenuViewType.AnalyticalPanel => _createAnalyticalPanelViewModel(),
                MenuViewType.Branch => _createBranchViewModel(),
                MenuViewType.User => _createUserViewModel(),
                MenuViewType.RefundToSupplier => _createRefundToSupplierViewModel(),
                _ => throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType"),
            };
        }
    }
}
