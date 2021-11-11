using RetailTrade.Domain.Exceptions;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels;
using RetailTradeServer.ViewModels.Factories;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RetailTradeServer.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly IAuthenticator _authenticator;
        private readonly IMessageStore _messageStore;
        private LoginViewModel _viewModel;

        #endregion

        #region Commands

        public ICommand UpdateCurrentViewModelCommand { get; }

        #endregion

        #region Constructor

        public LoginCommand(LoginViewModel viewModel,
            IAuthenticator authenticator,
            INavigator navigator,
            IRetailTradeViewModelFactory viewModelFactory,
            IMessageStore messageStore)
        {
            _authenticator = authenticator;
            _viewModel = viewModel;
            _messageStore = messageStore;

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, viewModelFactory);

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
                await _authenticator.Login(_viewModel.Username, _viewModel.Password);

                UpdateCurrentViewModelCommand.Execute(ViewType.Home);
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
