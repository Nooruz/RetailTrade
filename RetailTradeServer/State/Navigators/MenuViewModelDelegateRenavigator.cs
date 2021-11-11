using RetailTradeServer.ViewModels.Base;

namespace RetailTradeServer.State.Navigators
{
    public class MenuViewModelDelegateRenavigator<TMenuViewType> : IMenuRenavigator where TMenuViewType : BaseViewModel
    {
        private readonly IMenuNavigator _menuNavigator;
        private readonly CreateViewModel<TMenuViewType> _createViewModel;

        public MenuViewModelDelegateRenavigator(IMenuNavigator menuNavigator, CreateViewModel<TMenuViewType> createViewModel)
        {
            _menuNavigator = menuNavigator;
            _createViewModel = createViewModel;
        }

        public void Renavigate()
        {
            _menuNavigator.CurrentViewModel = _createViewModel();
        }
    }
}
