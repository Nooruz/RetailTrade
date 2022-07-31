using DevExpress.Mvvm.DataAnnotations;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Factories;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ProductRegistrationViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IMenuNavigator _menuNavigator;
        private readonly IMenuViewModelFactory _menuViewModelFactory;

        #endregion

        #region Public Properties



        #endregion

        #region Commands

        public ICommand UpdateCurrentMenuViewModelCommand => new UpdateCurrentMenuViewModelCommand(_menuNavigator, _menuViewModelFactory);

        #endregion

        #region Constructor

        public ProductRegistrationViewModel(IMenuNavigator menuNavigator,
            IMenuViewModelFactory menuViewModelFactory)
        {
            _menuNavigator = menuNavigator;
            _menuViewModelFactory = menuViewModelFactory;

            CreateCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.CreateProductRegistration));
        }

        #endregion

        #region Public Voids

        [Command]
        public void UserControlLoaded()
        {
            Header = "Оприходования";
            ShowLoadingPanel = false;
        }

        #endregion
    }
}
