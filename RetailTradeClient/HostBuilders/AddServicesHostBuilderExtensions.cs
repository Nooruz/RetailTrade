using DrvFRLib;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Services.AuthenticationServices;
using RetailTrade.EntityFramework.Services;

namespace RetailTradeClient.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                services.AddSingleton<IPasswordHasher, PasswordHasher>();

                services.AddSingleton(s => new DrvFR());

                services.AddSingleton<IAuthenticationService, AuthenticationService>();
                services.AddSingleton<IUserService, UserService>();
                services.AddSingleton<IProductService, ProductService>();
                services.AddSingleton<IOrganizationService, OrganizationService>();
                services.AddSingleton<IProductSaleService, ProductSaleService>();
                services.AddSingleton<IShiftService, ShiftService>();
                services.AddSingleton<IRefundService, RefundService>();
            });
        }
    }
}
