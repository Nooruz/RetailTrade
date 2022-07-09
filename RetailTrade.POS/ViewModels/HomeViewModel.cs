using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.POS.Commands;
using RetailTrade.POS.States.Navigators;
using RetailTrade.POS.ViewModels.Factories;
using System;
using System.Windows.Input;

namespace RetailTrade.POS.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IMenuNavigator _menuNavigator;
        private readonly IMenuViewModelFactory _menuViewModelFactory;

        #endregion

        #region Public Properties

        public ICommand UpdateCurrentMenuViewModelCommand => new UpdateCurrentMenuViewModelCommand(_menuNavigator, _menuViewModelFactory);

        #endregion

        #region Constructor

        public HomeViewModel(IMenuNavigator menuNavigator,
            IMenuViewModelFactory menuViewModelFactory)
        {
            _menuNavigator = menuNavigator;
            _menuViewModelFactory = menuViewModelFactory;

        }

        #endregion

        #region Private Voids



        #endregion

        #region Public Voids

        [Command]
        public void Renavigate(object parameter)
        {
            if (Enum.TryParse(parameter.ToString(), out MenuViewType menuViewType))
            {
                UpdateCurrentMenuViewModelCommand.Execute(menuViewType);
            }
        }

        #endregion
    }
}
