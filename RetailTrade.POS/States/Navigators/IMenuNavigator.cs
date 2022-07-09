using RetailTrade.POS.ViewModels;
using System;

namespace RetailTrade.POS.States.Navigators
{
    public enum MenuViewType
    {

    }

    public interface IMenuNavigator
    {
        BaseViewModel CurrentViewModel { get; set; }
        event Action StateChanged;
    }
}
