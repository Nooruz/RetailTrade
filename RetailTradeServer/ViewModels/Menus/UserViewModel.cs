using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using SalePageServer.State.Dialogs;
using System.Collections.Generic;

namespace RetailTradeServer.ViewModels.Menus
{
    public class UserViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;
        private readonly IAuthenticator _authenticator;
        private readonly IRoleService _roleService;

        #endregion

        #region Public Members

        public IEnumerable<User> Users => _userService.GetAll();

        #endregion

        #region Constructor

        public UserViewModel(IUserService userService,
            IDialogService dialogService,
            IAuthenticator authenticator,
            IRoleService roleService)
        {
            _userService = userService;
            _dialogService = dialogService;
            _authenticator = authenticator;
            _roleService = roleService;

            CreateCommand = new RelayCommand(CreateUser);

            _userService.PropertiesChanged += UserService_PropertiesChanged;
        }

        #endregion

        #region Private Voids

        private void CreateUser()
        {
            _dialogService.ShowDialog(new CreateUserDialogFormModel(_authenticator, _roleService, _dialogService), 
                new CreateUserDialogForm());
        }

        private void UserService_PropertiesChanged()
        {
            OnPropertyChanged(nameof(Users));
        }

        #endregion
    }
}
