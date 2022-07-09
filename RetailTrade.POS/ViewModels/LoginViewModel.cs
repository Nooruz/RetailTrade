using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.POS.Commands;
using RetailTrade.POS.States.Authenticators;
using RetailTrade.POS.States.Navigators;
using RetailTrade.POS.States.Users;
using System.Collections.Generic;
using System.Windows.Input;

namespace RetailTrade.POS.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IAuthenticator _authenticator;
        private readonly IRenavigator _homeNavigato;
        private readonly IUserService _userService;
        private readonly IUserStore _userStore;
        private User? _selectedUser;
        private IEnumerable<User>? _users;

        #endregion

        #region Public Properties

        public IEnumerable<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }
        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                if (_selectedUser != null)
                {
                    _selectedUser.PasswordHash = string.Empty;
                }
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        #endregion

        #region Commands

        public ICommand LoginCommand => new LoginCommand(this, _authenticator, _homeNavigato);

        #endregion

        #region Constructor

        public LoginViewModel(IAuthenticator authenticator,
            IRenavigator homeNavigato,
            IUserService userService,
            IUserStore userStore)
        {
            _userService = userService;
            _authenticator = authenticator;
            _userStore = userStore;
            _homeNavigato = homeNavigato;
        }

        #endregion

        #region Public Voids

        [Command]
        public async void UserControlLoaded()
        {
            Users = await _userService.GetCashiersAsync();
        }

        #endregion
    }
}
