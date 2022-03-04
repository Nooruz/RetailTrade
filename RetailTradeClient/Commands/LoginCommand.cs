using RetailTrade.Domain.Exceptions;
using RetailTradeClient.State.Authenticators;
using RetailTradeClient.State.Messages;
using RetailTradeClient.ViewModels;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RetailTradeClient.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly LoginViewModel _loginViewModel;
        private readonly IAuthenticator _authenticator;
        private readonly IMessageStore _messageStore;

        #endregion

        #region Constructor

        public LoginCommand(LoginViewModel loginViewModel,
            IAuthenticator authenticator,
            IMessageStore messageStore)
        {
            _loginViewModel = loginViewModel;
            _authenticator = authenticator;
            _messageStore = messageStore;

            _loginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return _loginViewModel.CanLogin;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _authenticator.Login(_loginViewModel.SelectedUser.Username, _loginViewModel.Password);
            }
            catch (InvalidUsernameOrPasswordException e)
            {
                _messageStore.SetCurrentMessage(e.Message, MessageType.Error);
            }
            catch
            {
                _messageStore.SetCurrentMessage("Ошибка входа.", MessageType.Error);
            }
        }

        private void LoginViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }
    }
}
