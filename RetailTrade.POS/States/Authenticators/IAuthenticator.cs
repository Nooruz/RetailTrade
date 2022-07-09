using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services.AuthenticationServices;
using System;
using System.Threading.Tasks;

namespace RetailTrade.POS.States.Authenticators
{
    public interface IAuthenticator
    {
        User CurrentUser { get; }

        event Action StateChanged;

        Task<RegistrationResult> Register(User user, string password, string confirmPassword);
        Task Login(string username, string password);
        Task<RegistrationResult> Update(User user, string password, string confirmPassword);
        void Logout();
    }
}
