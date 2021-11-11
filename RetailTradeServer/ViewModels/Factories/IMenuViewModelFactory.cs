using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;

namespace RetailTradeServer.ViewModels.Factories
{
    public interface IMenuViewModelFactory
    {
        BaseViewModel CreateViewModel(MenuViewType viewType);
    }
}
