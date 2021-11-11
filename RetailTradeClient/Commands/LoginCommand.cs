using RetailTrade.Domain.Exceptions;
using RetailTradeClient.State.Authenticators;
using RetailTradeClient.State.Messages;
using RetailTradeClient.State.Navigators;
using RetailTradeClient.ViewModels;
using RetailTradeClient.ViewModels.Factories;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RetailTradeClient.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly LoginViewModel _loginViewModel;
        private readonly IAuthenticator _authenticator;
        private readonly IMessageStore _messageStore;

        #endregion

        #region Commands

        public ICommand UpdateCurrentViewModelCommand { get; }

        #endregion

        #region Constructor

        public LoginCommand(LoginViewModel loginViewModel,
            IAuthenticator authenticator,
            INavigator navigator,
            IViewModelFactory viewModelFactory,
            IMessageStore messageStore)
        {
            _loginViewModel = loginViewModel;
            _authenticator = authenticator;
            _messageStore = messageStore;

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, viewModelFactory);

            _loginViewModel.PropertyChanged += _loginViewModel_PropertyChanged;
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

                UpdateCurrentViewModelCommand.Execute(ViewType.Home);
            }
            catch (InvalidUsernameOrPasswordException)
            {
                _messageStore.SetCurrentMessage("Неверное имя или пароль.", MessageType.Error);
            }
            catch (Exception e)
            {
                _messageStore.SetCurrentMessage("Ошибка входа.", MessageType.Error);
            }
        }

        private void _loginViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }
    }
}
