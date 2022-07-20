using RetailTrade.POS.States.Navigators;
using RetailTrade.POS.ViewModels.Menus;
using System;

namespace RetailTrade.POS.ViewModels.Factories
{
    public class MenuViewModelFactory : IMenuViewModelFactory
    {
        #region Private Members

        private readonly CreateMenuViewModel<SalesViewModel> _createSalesViewModel;
        private readonly CreateMenuViewModel<RefundViewModel> _createRefundViewModel;
        private readonly CreateMenuViewModel<WorkSaleViewModel> _createWorkSaleViewModel;
        private readonly CreateMenuViewModel<DeferredReceiptsViewModel> _createDeferredReceiptsViewModel;
        private readonly CreateMenuViewModel<HistoryViewModel> _createHistoryViewModel;
        private readonly CreateMenuViewModel<ShiftViewModel> _createShiftViewModel;

        #endregion

        #region Constructor

        public MenuViewModelFactory(CreateMenuViewModel<SalesViewModel> createSalesViewModel, 
            CreateMenuViewModel<RefundViewModel> createRefundViewModel,
            CreateMenuViewModel<WorkSaleViewModel> createWorkSaleViewModel,
            CreateMenuViewModel<DeferredReceiptsViewModel> createDeferredReceiptsViewModel,
            CreateMenuViewModel<HistoryViewModel> createHistoryViewModel,
            CreateMenuViewModel<ShiftViewModel> createShiftViewModel)
        {
            _createSalesViewModel = createSalesViewModel;
            _createRefundViewModel = createRefundViewModel;
            _createWorkSaleViewModel = createWorkSaleViewModel;
            _createDeferredReceiptsViewModel = createDeferredReceiptsViewModel;
            _createHistoryViewModel = createHistoryViewModel;
            _createShiftViewModel = createShiftViewModel;
        }

        #endregion

        public BaseViewModel CreateViewModel(MenuViewType viewType)
        {
            return viewType switch
            {
                MenuViewType.Sales => _createSalesViewModel(),
                MenuViewType.Refund => _createRefundViewModel(),
                MenuViewType.Shift => _createShiftViewModel(),
                MenuViewType.History => _createHistoryViewModel(),
                MenuViewType.DeferredReceipt => _createDeferredReceiptsViewModel(),
                MenuViewType.WorkSale => _createWorkSaleViewModel(),
                _ => throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType"),
            };
        }
    }
}
