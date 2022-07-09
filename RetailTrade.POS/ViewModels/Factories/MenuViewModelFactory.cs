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

        #endregion

        #region Constructor

        public MenuViewModelFactory(CreateMenuViewModel<SalesViewModel> createSalesViewModel, 
            CreateMenuViewModel<RefundViewModel> createRefundViewModel,
            CreateMenuViewModel<WorkSaleViewModel> createWorkSaleViewModel)
        {
            _createSalesViewModel = createSalesViewModel;
            _createRefundViewModel = createRefundViewModel;
            _createWorkSaleViewModel = createWorkSaleViewModel;
        }

        #endregion

        public BaseViewModel CreateViewModel(MenuViewType viewType)
        {
            return viewType switch
            {
                MenuViewType.Sales => _createSalesViewModel(),
                MenuViewType.Refund => _createRefundViewModel(),
                MenuViewType.Shift => throw new NotImplementedException(),
                MenuViewType.History => throw new NotImplementedException(),
                MenuViewType.DeferredReceipt => throw new NotImplementedException(),
                MenuViewType.WorkSale => _createWorkSaleViewModel(),
                _ => throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType"),
            };
        }
    }
}
