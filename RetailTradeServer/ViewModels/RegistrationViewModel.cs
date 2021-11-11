using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Factories;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        #region Private Members

        private string _username = "admin";
        private string _password;
        private string _confirmPassword;

        #endregion

        #region Public Properties

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(CanCreate));
            }
        }
        public bool CanCreate => !string.IsNullOrEmpty(Username);
        public MessageViewModel ErrorMessageViewModel { get; }
        public string ErrorMessage
        {
            set => ErrorMessageViewModel.Message = value;
        }

        #endregion

        #region Commands

        public ICommand AdminRegistrationCommand { get; }

        #endregion

        #region Constructor

        public RegistrationViewModel(IRoleService roleService,
            INavigator navigator,
            IAuthenticator authenticator,
            IRetailTradeViewModelFactory viewModelFactory)
        {
            AdminRegistrationCommand = new AdminRegistrationCommand(roleService, navigator, authenticator, this, viewModelFactory);
            ErrorMessageViewModel = new MessageViewModel();
        }

        #endregion
    }
}
