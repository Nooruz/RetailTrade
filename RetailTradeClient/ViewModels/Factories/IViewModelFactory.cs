using RetailTradeClient.State.Navigators;
using RetailTradeClient.ViewModels.Base;

namespace RetailTradeClient.ViewModels.Factories
{
    public interface IViewModelFactory
    {
        BaseViewModel CreateViewModel(ViewType viewType);
    }
}
