using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using SalePageServer.State.Dialogs;
using SalePageServer.Utilities;
using System.Collections.Generic;
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
        private ObservableQueue<User> _users;
        private IEnumerable<Role> _roles;

        #endregion

        #region Public Members

        public IEnumerable<User> Users => _users;
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

            _users = new();

            CreateCommand = new RelayCommand(CreateUser);
            DeleteCommand = new RelayCommand(DeleteUser);
            EditCommand = new RelayCommand(EditUser);
            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);

            _userService.PropertiesChanged += UserService_PropertiesChanged;
            _userService.OnUserCreated += UserService_OnUserCreated;
        }

        #endregion

        #region Private Voids

        private async void CreateUser()
        {
            await _dialogService.ShowDialog(new CreateUserDialogFormModel(_authenticator, _roleService, _dialogService, _messageStore) { Title = "Пользователи (создание)" });
        }

        private async void DeleteUser()
        {
            if (SelectedUser != null)
            {
                if (_dialogService.ShowMessage(SelectedUser.DeleteMark ? $"Снять пометку \"{SelectedUser.Username}\"?" : $"Пометить \"{SelectedUser.Username}\" на удаление?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _userService.MarkingForDeletion(SelectedUser);
                }
            }
            else
            {
                _ = _dialogService.ShowMessage("Выберите пользователя!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void EditUser()
        {
            if (SelectedUser != null)
            {
                if (SelectedUser.DeleteMark)
                {
                    _ = _dialogService.ShowMessage("Помечено на удаление!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    await _dialogService.ShowDialog(new CreateUserDialogFormModel(_authenticator, _roleService, _dialogService, _messageStore)
                    {
                        EditableUser = SelectedUser,
                        Title = $"Пользователи ({SelectedUser.Username})"
                    });
                }
            }
            else
            {
                _ = _dialogService.ShowMessage("Выберите пользователя!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void UserControlLoaded()
        {
            //_users = new(await _userService.GetAllAsync());
            Roles = await _roleService.GetAllAsync();
        }

        private void UserService_PropertiesChanged()
        {
            OnPropertyChanged(nameof(Users));
        }

        private void UserService_OnUserCreated(User user)
        {
            _users.Enqueue(user);
        }

        #endregion
    }
}
