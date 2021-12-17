﻿using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using SalePageServer.State.Dialogs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class UserViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;
        private readonly IAuthenticator _authenticator;
        private readonly IRoleService _roleService;
        private readonly IMessageStore _messageStore;
        private ObservableCollection<User> _users;

        #endregion

        #region Public Members

        public ObservableCollection<User> Users
        {
            get => _users ?? (new());
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }
        public User SelectedUser { get; set; }

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand { get; }

        #endregion

        #region Constructor

        public UserViewModel(IUserService userService,
            IDialogService dialogService,
            IAuthenticator authenticator,
            IRoleService roleService,
            IMessageStore messageStore)
        {
            _userService = userService;
            _dialogService = dialogService;
            _authenticator = authenticator;
            _roleService = roleService;
            _messageStore = messageStore;

            CreateCommand = new RelayCommand(CreateUser);
            DeleteCommand = new RelayCommand(DeleteUser);
            EditCommand = new RelayCommand(EditUser);
            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);

            _userService.PropertiesChanged += UserService_PropertiesChanged;
        }

        #endregion

        #region Private Voids

        private void CreateUser()
        {
            _ = _dialogService.ShowDialog(new CreateUserDialogFormModel(_authenticator, _roleService, _dialogService, _messageStore) { Title = "Пользователи (создание)" },
                new CreateUserDialogForm());
        }

        private void DeleteUser()
        {
            
        }

        private void EditUser()
        {
            if (SelectedUser != null)
            {
                CreateUserDialogFormModel viewModel = new(_authenticator, _roleService, _dialogService, _messageStore)
                {
                    IsEditMode = true,
                    EditableUser = SelectedUser,
                    Title = "Пользователи (редактирование)"
                };
                _ = _dialogService.ShowDialog(viewModel,
                new CreateUserDialogForm());
            }
            else
            {
                _ = _dialogService.ShowMessage("Выберите пользователя!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void UserControlLoaded()
        {
            Users = new(await _userService.GetAllAsync());
        }

        private void UserService_PropertiesChanged()
        {
            OnPropertyChanged(nameof(Users));
        }

        #endregion
    }
}
