using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Services.AuthenticationServices;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Dialogs.Base;
using SalePageServer.State.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateUserDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IAuthenticator _authenticator;
        private readonly IRoleService _roleService;
        private readonly IDialogService _dialogService;
        private readonly IMessageStore _messageStore;
        private string _username;
        private string _fullName;
        private string _password;
        private string _confirmPassword;
        private int? _selectedRoleId;
        private User _editableUser;

        #endregion

        #region Public Properties

        public IEnumerable<Role> Roles => _roleService.GetAll();
        public GlobalMessageViewModel GlobalMessageViewModel { get; set; }
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
        public User EditableUser
        {
            get => _editableUser;
            set
            {
                _editableUser = value;
                if (_editableUser != null)
                {
                    Username = _editableUser.Username;
                    FullName = _editableUser.FullName;
                    SelectedRoleId = _editableUser.RoleId;
                }
                OnPropertyChanged(nameof(EditableUser));
            }
        }

        #endregion

        #region Commands

        public ICommand CreateUserCommand { get; }
        public ICommand SaveUserCommand { get; }

        #endregion

        #region Constructor

        public CreateUserDialogFormModel(IAuthenticator authenticator,
            IRoleService roleService,
            IDialogService dialogService,
            IMessageStore messageStore)
        {
            _authenticator = authenticator;
            _roleService = roleService;
            _dialogService = dialogService;
            _messageStore = messageStore;
            GlobalMessageViewModel = new(_messageStore);

            CreateUserCommand = new RelayCommand(CreateUser);
            SaveUserCommand = new RelayCommand(SaveUser);
        }

        #endregion

        #region Private Voids

        private async void CreateUser()
        {
            if (string.IsNullOrEmpty(Username))
            {
                _messageStore.SetCurrentMessage("Введит логин.", MessageType.Error);
                return;
            }
            if (string.IsNullOrEmpty(FullName))
            {
                _messageStore.SetCurrentMessage("Введит ФИО.", MessageType.Error);
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                _messageStore.SetCurrentMessage("Введит пароль.", MessageType.Error);
                return;
            }
            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                _messageStore.SetCurrentMessage("Введит подтверждение пароля.", MessageType.Error);
                return;
            }
            if (SelectedRoleId == null)
            {
                _messageStore.SetCurrentMessage("Выберите роль.", MessageType.Error);
                return;
            }

            switch (await _authenticator.Register(new User { Username = Username, FullName = FullName, JoinedDate = DateTime.Now, RoleId = SelectedRoleId.Value },
                Password,
                ConfirmPassword))
            {
                case RegistrationResult.Success:
                    _dialogService.Close();
                    break;
                case RegistrationResult.PasswordDoesNotRequirements:
                    break;
                case RegistrationResult.PasswordsDoNotMatch:
                    _messageStore.SetCurrentMessage("Пароли не совподают.", MessageType.Error);
                    break;
                case RegistrationResult.UsernameAlreadyExists:
                    _messageStore.SetCurrentMessage("Пользователь с таким логином существует.", MessageType.Error);
                    break;
                case RegistrationResult.OtherError:
                    _messageStore.SetCurrentMessage("Ошибка при создании пользователя.", MessageType.Error);
                    break;
                default:
                    break;
            }
        }

        private async void SaveUser()
        {
            if (string.IsNullOrEmpty(Username))
            {
                _messageStore.SetCurrentMessage("Введит логин.", MessageType.Error);
                return;
            }
            if (string.IsNullOrEmpty(FullName))
            {
                _messageStore.SetCurrentMessage("Введит ФИО.", MessageType.Error);
                return;
            }
            if (SelectedRoleId == null)
            {
                _messageStore.SetCurrentMessage("Выберите роль.", MessageType.Error);
                return;
            }

            EditableUser.Username = Username;
            EditableUser.FullName = FullName;
            EditableUser.Role = null;
            EditableUser.RoleId = SelectedRoleId.Value;

            RegistrationResult result = string.IsNullOrEmpty(Password)
                ? await _authenticator.Update(EditableUser, string.Empty, string.Empty)
                : await _authenticator.Update(EditableUser, Password, ConfirmPassword);

            switch (result)
            {
                case RegistrationResult.Success:
                    _dialogService.Close();
                    break;
                case RegistrationResult.PasswordDoesNotRequirements:
                    break;
                case RegistrationResult.PasswordsDoNotMatch:
                    _messageStore.SetCurrentMessage("Пароли не совподают.", MessageType.Error);
                    break;
                case RegistrationResult.UsernameAlreadyExists:
                    _messageStore.SetCurrentMessage("Пользователь с таким логином существует.", MessageType.Error);
                    break;
                case RegistrationResult.OtherError:
                    _messageStore.SetCurrentMessage("Ошибка при создании пользователя.", MessageType.Error);
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
