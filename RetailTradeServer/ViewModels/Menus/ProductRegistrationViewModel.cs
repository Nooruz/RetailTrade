using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
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
        private readonly IRegistrationService _registrationService;

        #endregion

        #region Public Properties



        #endregion

        #region Commands

        public ICommand UpdateCurrentMenuViewModelCommand => new UpdateCurrentMenuViewModelCommand(_menuNavigator, _menuViewModelFactory);

        #endregion

        #region Constructor

        public ProductRegistrationViewModel(IMenuNavigator menuNavigator,
            IMenuViewModelFactory menuViewModelFactory,
            IRegistrationService registrationService)
        {
            _menuNavigator = menuNavigator;
            _menuViewModelFactory = menuViewModelFactory;
            _registrationService = registrationService;

            CreateCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.CreateProductRegistration));
            _registrationService.OnCreated += RegistrationService_OnCreated;
        }

        #endregion

        #region Private Voids

        private void RegistrationService_OnCreated(Registration registration)
        {
            
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
