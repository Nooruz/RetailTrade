using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using SalePageServer.Commands;
using SalePageServer.Properties;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Private Members

        private IUserService _userService;
        private User _selectedUser;
        private string _password;
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
                OnPropertyChanged(nameof(CanLogin));
                OnPropertyChanged(nameof(SelectedUser));
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
        public Visibility IsAdminCreated => Settings.Default.AdminCreated ? Visibility.Collapsed : Visibility.Visible;

        #endregion

        #region Commands

        public ICommand LoginCommand { get; }
        public ICommand RegistrationCommand { get; }

        #endregion

        #region Constructor

        public LoginViewModel(IAuthenticator authenticator,
            IRenavigator registrationRenavigator,
            IRenavigator homeRenavigator,
            IMessageStore messageStore,
            GlobalMessageViewModel globalMessageViewModel,
            IUserService userService) : base(authenticator)
        {
            _userService = userService;
            GlobalMessageViewModel = globalMessageViewModel;

            LoginCommand = new LoginCommand(this, authenticator, homeRenavigator, messageStore);
            RegistrationCommand = new RenavigateCommand(registrationRenavigator);
            GetAdmin();
        }

        #endregion

        #region Private Voids

        private async void GetAdmin()
        {
            Users = await _userService.GetAdminAsync();
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
