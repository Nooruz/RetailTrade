using RetailTradeClient.ViewModels.Base;
using System;

namespace RetailTradeClient.State.Navigators
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
