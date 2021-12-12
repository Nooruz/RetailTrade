using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.State.Authenticators;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Messages;
using RetailTradeClient.State.Navigators;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using RetailTradeClient.ViewModels.Base;
using System.Collections.Generic;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IUserService _userService;
        private string _password;
        private User _selectedUser;
        private IEnumerable<User> _users;

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
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
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
        public bool CanLogin => SelectedUser != null && !string.IsNullOrEmpty(Password);

        #endregion

        #region Commands

        public ICommand LoginCommand { get; set; }
        public ICommand UserControlLoadedCommand { get; }

        #endregion

        #region Constructor

        public LoginViewModel(IAuthenticator authenticator,
            IRenavigator homeNavigato,
            IUIManager manager,
            GlobalMessageViewModel globalMessageViewModel,
            IMessageStore messageStore,
            IUserService userService,
            IShiftStore shiftStore,
            IUserStore userStore)
        {
            GlobalMessageViewModel = globalMessageViewModel;
            _userService = userService;

            LoginCommand = new LoginCommand(this, authenticator, homeNavigato, manager, messageStore, shiftStore, userStore);
            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);
        }

        #endregion

        #region Private Voids

        private async void UserControlLoaded()
        {
            Users = await _userService.GetAllAsync();
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            GlobalMessageViewModel.Dispose();

            base.Dispose();
        }

        #endregion        
    }
}
