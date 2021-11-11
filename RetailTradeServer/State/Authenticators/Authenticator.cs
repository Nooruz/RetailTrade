using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services.AuthenticationServices;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Factories;
using System;
using System.Threading.Tasks;

namespace RetailTradeServer.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        #region Private Members

        private readonly IAuthenticationService _authenticationService;
        private readonly IUserStore _userStore;
        private readonly INavigator _navigator;
        private readonly IRetailTradeViewModelFactory _viewModelFactory;

        #endregion

        #region Constructor

        public Authenticator(IAuthenticationService authenticationService,
            IUserStore userStore,
            INavigator navigator,
            IRetailTradeViewModelFactory viewModelFactory)
        {
            _authenticationService = authenticationService;
            _userStore = userStore;
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        #endregion

        #region Public Properties

        public User CurrentUser
        {
            get => _userStore.CurrentUser;
            private set
            {
                _userStore.CurrentUser = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;

        #endregion

        public async Task Login(string username, string password)
        {
            CurrentUser = await _authenticationService.Login(username, password);
        }

        public void Logout()
        {
            CurrentUser = null;
            _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(ViewType.Login);
        }

        public async Task<RegistrationResult> Register(User user, string password, string confirmPassword)
        {
            return await _authenticationService.Register(user, password, confirmPassword);
        }

        public async Task<RegistrationResult> Update(User user, string password, string confirmPassword)
        {
            return await _authenticationService.Update(user, password, confirmPassword);
        }
    }
}
