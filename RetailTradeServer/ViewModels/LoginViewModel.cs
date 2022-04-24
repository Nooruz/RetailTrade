﻿using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using SalePageServer.Commands;
using RetailTradeServer.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IUserService _userService;
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
                OnPropertyChanged(nameof(IsSelectedUser));
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
        public bool IsSelectedUser => SelectedUser != null;

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
        }

        #endregion

        #region Public Voids

        [Command]
        public async void UserControlLoaded()
        {
            try
            {
                Users = await _userService.GetAdminAsync();
                if (!string.IsNullOrEmpty(Settings.Default.DefaultUserName))
                {
                    SelectedUser = Users.FirstOrDefault(u => u.Username == Settings.Default.DefaultUserName);
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
