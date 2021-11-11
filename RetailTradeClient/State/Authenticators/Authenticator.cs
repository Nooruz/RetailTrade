using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services.AuthenticationServices;
using RetailTradeClient.State.Navigators;
using RetailTradeClient.State.Users;
using RetailTradeClient.ViewModels.Factories;
using System.Threading.Tasks;

namespace RetailTradeClient.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        #region Private Members

        private readonly IAuthenticationService _authenticationService;
        private readonly IUserStore _userStore;
        private readonly INavigator _navigator;
        private readonly IViewModelFactory _viewModelFactory;

        #endregion

        #region Constructor

        public Authenticator(IAuthenticationService authenticationService,
            IUserStore userStore,
            INavigator navigator,
            IViewModelFactory viewModelFactory)
        {
            _authenticationService = authenticationService;
            _userStore = userStore;
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        #endregion

        public async Task Login(string username, string password)
        {
            _userStore.CurrentUser = await _authenticationService.Login(username, password);
        }

        public void Logout()
        {
            _userStore.CurrentUser = null;
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
