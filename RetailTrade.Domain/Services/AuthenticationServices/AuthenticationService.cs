using Microsoft.AspNet.Identity;
using RetailTrade.Domain.Exceptions;
using RetailTrade.Domain.Models;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Private Members

        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IShiftService _shiftService;
        private Regex _passwordRegex;

        #endregion

        #region Constructor

        public AuthenticationService(IUserService userService,
            IPasswordHasher passwordHasher,
            IShiftService shiftService)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _shiftService = shiftService;

            _passwordRegex = new Regex("^[0-9]+$");
        }

        #endregion

        public async Task<User> Login(string username, string password)
        {
            User storedUser = await _userService.GetByUsername(username);

            if (storedUser == null)
            {
                throw new InvalidUsernameOrPasswordException("Неверное имя или пароль.", username, password);
            }

            PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(storedUser.PasswordHash, password);

            return passwordResult != PasswordVerificationResult.Success
                ? throw new InvalidUsernameOrPasswordException("Неверное имя или пароль.", username, password)
                : storedUser;
        }

        public async Task<RegistrationResult> Register(User user, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                return RegistrationResult.PasswordsDoNotMatch;
            }

            try
            {
                User editingUser = await _userService.GetByUsername(user.Username);
                if (editingUser != null)
                {
                    return RegistrationResult.UsernameAlreadyExists;
                }

                user.PasswordHash = _passwordHasher.HashPassword(password);

                _ = await _userService.CreateAsync(user);
                return RegistrationResult.Success;
            }
            catch
            {
                return RegistrationResult.OtherError;
            }
        }

        public async Task<RegistrationResult> Update(User user, string password, string confirmPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    _ = await _userService.UpdateAsync(user.Id, user);
                }
                else
                {
                    if (_passwordRegex.IsMatch(password))
                    {
                        return RegistrationResult.PasswordDoesNotRequirements;
                    }

                    if (password != confirmPassword)
                    {
                        return RegistrationResult.PasswordsDoNotMatch;
                    }
                    user.PasswordHash = _passwordHasher.HashPassword(password);

                    _ = await _userService.UpdateAsync(user.Id, user);
                }
                return RegistrationResult.Success;
            }
            catch
            {
                return RegistrationResult.OtherError;
            }
        }
    }
}
