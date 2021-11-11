using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;

namespace RetailTradeServer.ViewModels.Factories
{
    public interface IRetailTradeViewModelFactory
    {
        BaseViewModel CreateViewModel(ViewType viewType);
    }
}
