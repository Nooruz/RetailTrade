using RetailTrade.Domain.Models;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services.AuthenticationServices
{
    public enum RegistrationResult
    {
        //Каттоо ийгиликтүү өттү
        Success,

        //Сыр соз талаптарга жооп бербейт
        PasswordDoesNotRequirements,

        //Сыр сөз тастыктоочу сыр сөз менен дал келбейт
        PasswordsDoNotMatch,

        //Колдонуучунун аты мурунтан эле бар
        UsernameAlreadyExists,

        //Башка ката
        OtherError
    }

    public interface IAuthenticationService
    {
        Task<RegistrationResult> Register(User user, string password, string confirmPassword);

        Task<User> Login(string username, string password);

        Task<RegistrationResult> Update(User user, string password, string confirmPassword);
    }
}
