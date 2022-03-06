using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.State.Authenticators;
using RetailTradeClient.State.Messages;
using RetailTradeClient.State.Navigators;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using RetailTradeClient.ViewModels.Base;
using RetailTradeClient.ViewModels.Dialogs;
using RetailTradeClient.Views.Dialogs;
using System.Collections.Generic;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IAuthenticator _authenticator;
        private readonly IRenavigator _homeNavigato;
        private readonly IMessageStore _messageStore;
        private readonly IShiftStore _shiftStore;
        private readonly IUserService _userService;
        private readonly IUserStore _userStore;
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

        public ICommand LoginCommand => new LoginCommand(this, _authenticator, _messageStore);
        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);

        #endregion

        #region Constructor

        public LoginViewModel(IAuthenticator authenticator,
            IRenavigator homeNavigato,
            GlobalMessageViewModel globalMessageViewModel,
            IMessageStore messageStore,
            IUserService userService,
            IShiftStore shiftStore,
            IUserStore userStore)
        {
            _authenticator = authenticator;
            _userService = userService;
            _homeNavigato = homeNavigato;
            _messageStore = messageStore;
            _shiftStore = shiftStore;
            _userStore = userStore;

            GlobalMessageViewModel = globalMessageViewModel;

            _userStore.CurrentUserChanged += UserStore_CurrentUserChanged;
        }

        #endregion

        #region Private Voids

        private void UserStore_CurrentUserChanged()
        {
            if (_userStore.CurrentUser != null)
            {
                WindowService?.Show(nameof(MainMenuView),
                new MainMenuViewModel(_shiftStore, _userStore.CurrentUser.Id, _homeNavigato, _userStore)
                {
                    Title = $"РМК: {_userStore.CurrentUser.FullName}"
                });
            }
        }

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
