﻿using RetailTrade.POS.ViewModels;
using System;

namespace RetailTrade.POS.States.Navigators
{
    public class MenuNavigator : IMenuNavigator
    {
        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel?.Dispose();

                _currentViewModel = value;
                StateChanged?.Invoke();
            }
        }

        public event Action? StateChanged;
    }
}