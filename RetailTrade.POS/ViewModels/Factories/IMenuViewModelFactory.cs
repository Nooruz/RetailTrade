using RetailTrade.POS.States.Navigators;

namespace RetailTrade.POS.ViewModels.Factories
{
    public interface IMenuViewModelFactory
    {
        BaseViewModel CreateViewModel(MenuViewType viewType);
    }
}
