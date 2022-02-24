using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class UserViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IUserService _userService;
        private readonly IAuthenticator _authenticator;
        private readonly IRoleService _roleService;
        private readonly IMessageStore _messageStore;
        private ObservableCollection<User> _users;
        private IEnumerable<Role> _roles;

        #endregion

        #region Public Members

        public ObservableCollection<User> Users
        {
            get => _users ?? new();
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }
        public IEnumerable<Role> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }
        public User SelectedUser { get; set; }

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);

        #endregion

        #region Constructor

        public UserViewModel(IUserService userService,
            IAuthenticator authenticator,
            IRoleService roleService,
            IMessageStore messageStore)
        {
            _userService = userService;
            _authenticator = authenticator;
            _roleService = roleService;
            _messageStore = messageStore;

            _users = new();

            Header = "Пользователи";

            CreateCommand = new RelayCommand(CreateUser);
            DeleteCommand = new RelayCommand(DeleteUser);
            EditCommand = new RelayCommand(EditUser);

            _userService.PropertiesChanged += UserService_PropertiesChanged;
            _userService.OnUserCreated += UserService_OnUserCreated;
        }

        #endregion

        #region Private Voids

        private void CreateUser()
        {
            WindowService.Show(nameof(CreateUserDialogForm), new CreateUserDialogFormModel(_authenticator, _roleService, _messageStore) { Title = "Пользователи (создание)" });
        }

        private async void DeleteUser()
        {
            if (SelectedUser != null)
            {
                if (MessageBox.Show(SelectedUser.DeleteMark ? $"Снять пометку \"{SelectedUser.Username}\"?" : $"Пометить \"{SelectedUser.Username}\" на удаление?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _userService.MarkingForDeletion(SelectedUser);
                }
            }
            else
            {
                _ = MessageBox.Show("Выберите пользователя!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void EditUser()
        {
            if (SelectedUser != null)
            {
                if (SelectedUser.DeleteMark)
                {
                    _ = MessageBox.Show("Помечено на удаление!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    WindowService.Show(nameof(CreateUserDialogForm), new CreateUserDialogFormModel(_authenticator, _roleService, _messageStore)
                    {
                        EditableUser = SelectedUser,
                        Title = $"Пользователи ({SelectedUser.Username})"
                    });
                }
            }
            else
            {
                _ = MessageBox.Show("Выберите пользователя!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void UserControlLoaded()
        {
            Users = new(await _userService.GetAllAsync());
            Roles = await _roleService.GetAllAsync();
            ShowLoadingPanel = false;
        }

        private void UserService_PropertiesChanged()
        {
            OnPropertyChanged(nameof(Users));
        }

        private void UserService_OnUserCreated(User user)
        {
            Users.Add(user);
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            _userService.PropertiesChanged -= UserService_PropertiesChanged;
            _userService.OnUserCreated -= UserService_OnUserCreated;
            base.Dispose();
        }

        #endregion
    }
}
