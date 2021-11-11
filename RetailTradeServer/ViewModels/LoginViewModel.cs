using RetailTradeServer.Commands;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Factories;
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

        #endregion

        #region Commands

        public ICommand LoginCommand { get; set; }

        #endregion

        #region Constructor

        public LoginViewModel(IAuthenticator authenticator,
            INavigator navigato,
            IRetailTradeViewModelFactory viewModelFactory,
            IMessageStore messageStore,
            GlobalMessageViewModel globalMessageViewModel) : base(authenticator)
        {
            GlobalMessageViewModel = globalMessageViewModel;

            LoginCommand = new LoginCommand(this, authenticator, navigato, viewModelFactory, messageStore);
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
