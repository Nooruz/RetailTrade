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
                OnPropertyChanged(nameof(SelectedUser));
                OnPropertyChanged(nameof(CanLogin));
            }
        }
        public bool CanLogin => SelectedUser != null && !string.IsNullOrEmpty(SelectedUser.PasswordHash);

        #endregion

        #region Commands

        public ICommand LoginCommand => new LoginCommand(this, _authenticator);

        #endregion

        #region Constructor

        public LoginViewModel(IAuthenticator authenticator,
            IRenavigator homeNavigato,
            IUserService userService)
        {

        }

        #endregion
    }
}
