using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Services.AuthenticationServices;
using RetailTrade.EntityFramework.Services;

namespace RetailTrade.POS.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                _ = services.AddSingleton<IPasswordHasher, PasswordHasher>();

                _ = services.AddSingleton<IAuthenticationService, AuthenticationService>();
                _ = services.AddSingleton<IUserService, UserService>();
                _ = services.AddSingleton<IRoleService, RoleService>();
                _ = services.AddSingleton<IShiftService, ShiftService>();
                _ = services.AddSingleton<IArrivalProductService, ArrivalProductService>();
                _ = services.AddSingleton<IProductService, ProductService>();
                _ = services.AddSingleton<IPointSaleService, PointSaleService>();
                _ = services.AddSingleton<IProductBarcodeService, ProductBarcodeService>();
            });
        }
    }
}
