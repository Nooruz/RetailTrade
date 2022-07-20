using RetailTrade.Domain.Exceptions;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
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
        private readonly IOrganizationService _organizationService;
        private readonly IShiftService _shiftService;

        #endregion

        #region Constructor

        public Authenticator(IAuthenticationService authenticationService,
            IUserStore userStore,
            INavigator navigator,
            IViewModelFactory viewModelFactory,
            IOrganizationService organizationService,
            IShiftService shiftService)
        {
            _authenticationService = authenticationService;
            _userStore = userStore;
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
            _organizationService = organizationService;
            _shiftService = shiftService;
        }

        #endregion

        public async Task Login(string username, string password)
        {
            _userStore.CurrentUser = await _authenticationService.Login(username, password);
            _userStore.Organization = await _organizationService.GetCurrentOrganization();

            if (_userStore.CurrentUser != null && _userStore.CurrentUser.RoleId == 1)
            {
                return;
            }
            else
            {
                Shift shift = new();//await _shiftService.GetOpenShift();

                if (shift != null && shift.UserId != _userStore.CurrentUser.Id)
                {
                    throw new InvalidUsernameOrPasswordException(shift, $"Смена открыта пользователем \"{shift.User.FullName}\".");
                }
            }            
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
