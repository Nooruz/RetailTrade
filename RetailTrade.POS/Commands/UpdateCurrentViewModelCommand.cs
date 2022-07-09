using RetailTrade.POS.States.Navigators;
using RetailTrade.POS.ViewModels.Factories;
using System;
using System.Windows.Input;

namespace RetailTrade.POS.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly INavigator _navigator;
        private readonly IViewModelFactory _viewModelFactory;

        public UpdateCurrentViewModelCommand(INavigator navigator,
            IViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
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
                    _viewModelFactory.CreateViewModel(viewType)?.ToString())
                    return;
                _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
            }
        }
    }
}
