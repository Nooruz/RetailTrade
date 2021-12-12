using RetailTradeServer.Commands;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using SalePageServer.Commands;
using SalePageServer.Properties;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Private Members

        private string _username = "admin";
        private string _password;

        #endregion

        #region Public Properties

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(CanLogin));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(CanLogin));
            }
        }
        public GlobalMessageViewModel GlobalMessageViewModel { get; }
        public bool CanLogin => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        public Visibility IsAdminCreated => Settings.Default.AdminCreated ? Visibility.Collapsed : Visibility.Visible;

        #endregion

        #region Commands

        public ICommand LoginCommand { get; }
        public ICommand RegistrationCommand { get; }

        #endregion

        #region Constructor

        public LoginViewModel(IAuthenticator authenticator,
            IRenavigator registrationRenavigator,
            IRenavigator homeRenavigator,
            IMessageStore messageStore,
            GlobalMessageViewModel globalMessageViewModel) : base(authenticator)
        {
            GlobalMessageViewModel = globalMessageViewModel;

            LoginCommand = new LoginCommand(this, authenticator, homeRenavigator, messageStore);
            RegistrationCommand = new RenavigateCommand(registrationRenavigator);
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
