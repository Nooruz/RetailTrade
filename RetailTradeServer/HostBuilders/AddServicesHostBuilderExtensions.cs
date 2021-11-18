using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Services.AuthenticationServices;
using RetailTrade.EntityFramework.Services;

namespace RetailTradeServer.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                services.AddSingleton<IPasswordHasher, PasswordHasher>();

                services.AddSingleton<IAuthenticationService, AuthenticationService>();
                services.AddSingleton<IUserService, UserService>();
                services.AddSingleton<IRoleService, RoleService>();
                services.AddSingleton<IOrganizationService, OrganizationService>();
                services.AddSingleton<IProductService, ProductService>();
                services.AddSingleton<IProductCategoryService, ProductCategoryService>();
                services.AddSingleton<IDataService<Unit>, GenericService<Unit>>();
                services.AddSingleton<IDataService<Branch>, GenericService<Branch>>();
                services.AddSingleton<ISupplierService, SupplierService>();
                services.AddSingleton<IProductSubcategoryService, ProductSubcategoryService>();
                services.AddSingleton<IReceiptService, ReceiptService>();
                services.AddSingleton<IShiftService, ShiftService>();
                services.AddSingleton<IArrivalProductService, ArrivalProductService>();
                services.AddSingleton<IWriteDownProductService, WriteDownProductService>();
                services.AddSingleton<IRefundToSupplierServiceProduct, RefundToSupplierServiceProduct>();
                services.AddSingleton<IOrderStatusService, OrderStatusService>();
                services.AddSingleton<IOrderProductService, OrderProductService>();
                services.AddSingleton<IOrderToSupplierService, OrderToSupplierService>();
                services.AddSingleton<IArrivalService, ArrivalService>();
                services.AddSingleton<IWriteDownService, WriteDownService>();
                services.AddSingleton<IRefundToSupplierService, RefundToSupplierService>();
            });
        }
    }
}
