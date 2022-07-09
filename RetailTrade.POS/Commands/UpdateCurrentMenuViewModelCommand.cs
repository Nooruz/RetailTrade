﻿using RetailTrade.POS.States.Navigators;
using RetailTrade.POS.ViewModels.Factories;
using System;
using System.Windows.Input;

namespace RetailTrade.POS.Commands
{
    public class UpdateCurrentMenuViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly IMenuNavigator _navigator;
        private readonly IMenuViewModelFactory _viewModelFactory;

        public UpdateCurrentMenuViewModelCommand(IMenuNavigator navigator,
            IMenuViewModelFactory viewModelFactory)
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
            if (parameter is MenuViewType viewType)
            {
                _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
            }
        }
    }
}
