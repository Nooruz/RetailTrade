using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Services.AuthenticationServices;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.ViewModels.Dialogs.Base;
using SalePageServer.State.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateUserDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IAuthenticator _authenticator;
        private readonly IRoleService _roleService;
        private readonly IDialogService _dialogService;
        private string _username;
        private string _fullName;
        private string _password;
        private string _confirmPassword;
        private int? _selectedRoleId;

        #endregion

        #region Public Properties

        public IEnumerable<Role> Roles => _roleService.GetAll();

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }
        public int? SelectedRoleId
        {
            get => _selectedRoleId;
            set
            {
                _selectedRoleId = value;
                OnPropertyChanged(nameof(SelectedRoleId));
            }
        }

        #endregion

        #region Commands

        public ICommand CreateUserCommand { get; }

        #endregion

        #region Constructor

        public CreateUserDialogFormModel(IAuthenticator authenticator,
            IRoleService roleService,
            IDialogService dialogService)
        {
            _authenticator = authenticator;
            _roleService = roleService;
            _dialogService = dialogService;

            CreateUserCommand = new RelayCommand(CreateUser);
        }

        #endregion

        #region Private Voids

        private async void CreateUser()
        {
            if (string.IsNullOrEmpty(Username))
            {
                MessageBox.Show("Username");
                return;
            }
            if (string.IsNullOrEmpty(FullName))
            {
                MessageBox.Show("FullName");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Password");
                return;
            }
            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                MessageBox.Show("ConfirmPassword");
                return;
            }
            if (SelectedRoleId == null)
            {
                MessageBox.Show("SelectedRoleId");
                return;
            }
            if (await _authenticator.Register(new User
                {
                    Username = Username,
                    FullName = FullName,
                    JoinedDate = DateTime.Now,
                    RoleId = SelectedRoleId.Value
                }, 
                Password, 
                ConfirmPassword) == RegistrationResult.Success)
            {
                _dialogService.Close();
            }
        }

        #endregion
    }
}
