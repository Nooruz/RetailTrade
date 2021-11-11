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
        private Regex _usernameRegex;
        private Regex _passwordRegex;

        #endregion

        #region Constructor

        public AuthenticationService(IUserService userService,
            IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;

            _usernameRegex = new Regex("^[a-zA-Z][a-zA-Z0-9]{2,9}$");
            _passwordRegex = new Regex("");
        }

        #endregion

        public async Task<User> Login(string username, string password)
        {
            var storedUser = await _userService.GetByUsername(username);

            if (storedUser == null)
            {
                throw new InvalidUsernameOrPasswordException(username, password);
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(storedUser.PasswordHash, password);

            if (passwordResult != PasswordVerificationResult.Success)
            {
                throw new InvalidUsernameOrPasswordException(username, password);
            }

            return storedUser;
        }

        public async Task<RegistrationResult> Register(User user, string password, string confirmPassword)
        {
            if (!_usernameRegex.IsMatch(user.Username))
                return RegistrationResult.UsernameDoesNotRequirements;

            if (!_passwordRegex.IsMatch(password))
                return RegistrationResult.PasswordDoesNotRequirements;


            if (password != confirmPassword)
                return RegistrationResult.PasswordsDoNotMatch;

            try
            {
                var editingUser = await _userService.GetByUsername(user.Username);
                if (editingUser != null)
                    return RegistrationResult.UsernameAlreadyExists;

                user.PasswordHash = _passwordHasher.HashPassword(password);

                await _userService.CreateAsync(user);
                return RegistrationResult.Success;
            }
            catch
            {
                return RegistrationResult.OtherError;
            }
        }

        public async Task<RegistrationResult> Update(User user, string password, string confirmPassword)
        {
            if (_usernameRegex.IsMatch(user.Username))
            {
                return RegistrationResult.UsernameDoesNotRequirements;
            }

            try
            {
                var editUser = await _userService.GetAsync(user.Id);

                if (_passwordRegex.IsMatch(password))
                    return RegistrationResult.PasswordDoesNotRequirements;

                if (password != confirmPassword)
                    return RegistrationResult.PasswordsDoNotMatch;

                editUser.PasswordHash = _passwordHasher.HashPassword(password);

                editUser.Username = user.Username;
                editUser.RoleId = user.RoleId;
                await _userService.UpdateAsync(editUser.Id, editUser);

                return RegistrationResult.Success;
            }
            catch
            {
                return RegistrationResult.OtherError;
            }
        }
    }
}
