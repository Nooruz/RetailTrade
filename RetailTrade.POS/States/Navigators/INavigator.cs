using RetailTrade.POS.ViewModels;
using System;

namespace RetailTrade.POS.States.Navigators
{
    public enum ViewType
    {
        Login,
        Home,
    }

    public interface INavigator
    {
        BaseViewModel CurrentViewModel { get; set; }
        event Action StateChanged;
    }
}
