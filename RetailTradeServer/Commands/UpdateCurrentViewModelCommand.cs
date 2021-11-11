using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Factories;
using System;
using System.Windows.Input;

namespace RetailTradeServer.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly INavigator _navigator;
        private readonly IRetailTradeViewModelFactory _retailTradeViewModelFactory;

        public UpdateCurrentViewModelCommand(INavigator navigator,
            IRetailTradeViewModelFactory retailTradeViewModelFactory)
        {
            _navigator = navigator;
            _retailTradeViewModelFactory = retailTradeViewModelFactory;
        }

        public bool CanExecute(object parameter)
        {
            return false;
        }

        public void Execute(object parameter)
        {
            if (parameter is ViewType viewType)
            {
                if (_navigator.CurrentViewModel?.ToString() ==
                    _retailTradeViewModelFactory.CreateViewModel(viewType)?.ToString())
                    return;
                _navigator.CurrentViewModel = _retailTradeViewModelFactory.CreateViewModel(viewType);
            }
        }
    }
}
