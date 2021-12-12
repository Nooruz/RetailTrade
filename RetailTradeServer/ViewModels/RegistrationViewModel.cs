using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using SalePageServer.Commands;
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
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(CanCreate));
            }
        }
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                OnPropertyChanged(nameof(CanCreate));
            }
        }
        public bool CanCreate => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword);
        public GlobalMessageViewModel GlobalMessageViewModel { get; }

        #endregion

        #region Commands

        public ICommand AdminRegistrationCommand { get; }
        public ICommand LoginCommand { get; }

        #endregion

        #region Constructor

        public RegistrationViewModel(IRoleService roleService,
            IRenavigator organizationRenavigator,
            IRenavigator loginRenavigator,
            IAuthenticator authenticator,
            GlobalMessageViewModel globalMessageViewModel,
            IMessageStore messageStore)
        {
            GlobalMessageViewModel = globalMessageViewModel;

            AdminRegistrationCommand = new AdminRegistrationCommand(roleService, organizationRenavigator, authenticator, this, messageStore);
            LoginCommand = new RenavigateCommand(loginRenavigator);
        }

        #endregion
    }
}
