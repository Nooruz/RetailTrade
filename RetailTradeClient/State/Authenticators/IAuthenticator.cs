using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services.AuthenticationServices;
using System.Threading.Tasks;

namespace RetailTradeClient.State.Authenticators
{
    public interface IAuthenticator
    {
        Task<RegistrationResult> Register(User user, string password, string confirmPassword);
        Task Login(string username, string password);
        Task<RegistrationResult> Update(User user, string password, string confirmPassword);
        void Logout();
    }
}
