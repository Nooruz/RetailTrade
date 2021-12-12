using RetailTrade.Domain.Exceptions;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels;
using SalePageServer.Properties;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RetailTradeServer.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly IAuthenticator _authenticator;
        private readonly IMessageStore _messageStore;
        private readonly IRenavigator _homeRavigator;
        private LoginViewModel _viewModel;

        #endregion

        #region Constructor

        public LoginCommand(LoginViewModel viewModel,
            IAuthenticator authenticator,
            IRenavigator homeRavigator,
            IMessageStore messageStore)
        {
            _authenticator = authenticator;
            _viewModel = viewModel;
            _messageStore = messageStore;
            _homeRavigator = homeRavigator;

            _viewModel.PropertyChanged += LoginViewModel_PropertyChanged;
        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return _viewModel.CanLogin;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _authenticator.Login(_viewModel.SelectedUser.Username, _viewModel.Password);                
                _homeRavigator.Renavigate();
                if (!Settings.Default.AdminCreated)
                {
                    Settings.Default.AdminCreated = true;
                    Settings.Default.Save();
                }
            }
            catch (InvalidUsernameOrPasswordException)
            {
                _messageStore.SetCurrentMessage("Неверное имя или пароль.", MessageType.Error);
            }
            catch
            {
                _messageStore.SetCurrentMessage("Ошибка входа.", MessageType.Error);
            }
        }

        private void LoginViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.CanLogin))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
