using RetailTradeServer.ViewModels.Base;
using System;

namespace RetailTradeServer.State.Navigators
{
    public enum ViewType
    {
        Null,
        Login,
        Registration,
        Home,
        Organization,
        Sale
    }

    public interface INavigator
    {
        BaseViewModel CurrentViewModel { get; set; }
        event Action StateChanged;
    }
}
