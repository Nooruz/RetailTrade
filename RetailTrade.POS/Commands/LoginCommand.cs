using RetailTrade.Domain.Exceptions;
using RetailTrade.POS.States.Authenticators;
using RetailTrade.POS.States.Navigators;
using RetailTrade.POS.ViewModels;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RetailTrade.POS.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly LoginViewModel _loginViewModel;
        private readonly IAuthenticator _authenticator;
        private readonly IRenavigator _homeRavigator;

        #endregion

        #region Constructor

        public LoginCommand(LoginViewModel loginViewModel,
            IAuthenticator authenticator,
            IRenavigator homeRavigator)
        {
            _loginViewModel = loginViewModel;
            _authenticator = authenticator;
            _homeRavigator = homeRavigator;

            _loginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _authenticator.Login(_loginViewModel.SelectedUser.Username, _loginViewModel.SelectedUser.PasswordHash);
                _homeRavigator.Renavigate();
            }
            catch (InvalidUsernameOrPasswordException e)
            {
                //_messageStore.SetCurrentMessage(e.Message, MessageType.Error);
            }
            catch
            {
                //_messageStore.SetCurrentMessage("Ошибка входа.", MessageType.Error);
            }
        }

        private void LoginViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }
    }
}
