using RetailTrade.POS.States.Navigators;

namespace RetailTrade.POS.ViewModels.Factories
{
    public interface IViewModelFactory
    {
        BaseViewModel CreateViewModel(ViewType viewType);
    }
}
