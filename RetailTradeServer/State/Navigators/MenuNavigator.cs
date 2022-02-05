using RetailTradeServer.ViewModels.Base;
using System;

namespace RetailTradeServer.State.Navigators
{
    public class MenuNavigator : IMenuNavigator
    {
        private BaseViewModel _addViewModel;

        public BaseViewModel AddViewModel
        {
            get => _addViewModel;
            set
            {
                _addViewModel?.Dispose();
                _addViewModel = value;
                StateChanged?.Invoke(_addViewModel);
            }
        }

        public event Action<BaseViewModel> StateChanged;
    }
}
