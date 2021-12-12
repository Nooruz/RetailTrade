using RetailTrade.Domain.Exceptions;
using RetailTradeClient.State.Authenticators;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Messages;
using RetailTradeClient.State.Navigators;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using RetailTradeClient.ViewModels;
using RetailTradeClient.ViewModels.Dialogs;
using RetailTradeClient.Views.Dialogs;
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
        private readonly IRenavigator _homeNavigato;
        private readonly IUIManager _manager;
        private readonly IShiftStore _shiftStore;
        private readonly IUserStore _userStore;

        #endregion

        #region Constructor

        public LoginCommand(LoginViewModel loginViewModel,
            IAuthenticator authenticator,
            IRenavigator homeNavigato,
            IUIManager manager,
            IMessageStore messageStore,
            IShiftStore shiftStore,
            IUserStore userStore)
        {
            _loginViewModel = loginViewModel;
            _authenticator = authenticator;
            _messageStore = messageStore;
            _homeNavigato = homeNavigato;
            _manager = manager;
            _shiftStore = shiftStore;
            _userStore = userStore;

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

                _ = await _manager.ShowDialog(new MainMenuViewModel(_shiftStore, _userStore.CurrentUser.Id, _manager, _homeNavigato, _userStore)
                {
                    Title = $"РМК: {(_userStore.CurrentUser?.FullName)}"
                },
                    new MainMenuView());
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
