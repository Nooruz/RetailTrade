﻿using Microsoft.AspNet.Identity;
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
                services.AddSingleton<IArrivalProductService, ArrivalProductService>();
                services.AddSingleton<IProductService, ProductService>();
                services.AddSingleton<IUnitService, UnitService>();
                services.AddSingleton<IDataService<Branch>, GenericService<Branch>>();
                services.AddSingleton<IDataService<TypeWareHouse>, GenericService<TypeWareHouse>>();
                services.AddSingleton<ISupplierService, SupplierService>();
                services.AddSingleton<IReceiptService, ReceiptService>();
                services.AddSingleton<IShiftService, ShiftService>();
                services.AddSingleton<IWriteDownProductService, WriteDownProductService>();
                services.AddSingleton<IRefundToSupplierServiceProduct, RefundToSupplierServiceProduct>();
                services.AddSingleton<IOrderStatusService, OrderStatusService>();
                services.AddSingleton<IOrderProductService, OrderProductService>();
                services.AddSingleton<IOrderToSupplierService, OrderToSupplierService>();
                services.AddSingleton<IArrivalService, ArrivalService>();
                services.AddSingleton<IWriteDownService, WriteDownService>();
                services.AddSingleton<IRefundToSupplierService, RefundToSupplierService>();
                services.AddSingleton<IProductSaleService, ProductSaleService>();
                services.AddSingleton<IRefundService, RefundService>();
                services.AddSingleton<IEmployeeService, EmployeeService>();
                services.AddSingleton<IGroupEmployeeService, GroupEmployeeService>();
                services.AddSingleton<IDataService<Gender>, GenericService<Gender>>();
                services.AddSingleton<ILabelPriceTagService, LabelPriceTagService>();
                services.AddSingleton<IDataService<TypeLabelPriceTag>, GenericService<TypeLabelPriceTag>>();
                services.AddSingleton<ILabelPriceTagSizeService, LabelPriceTagSizeService>();
                services.AddSingleton<ITypeProductService, TypeProductService>();
                services.AddSingleton<IWareHouseService, WareHouseService>();
                services.AddSingleton<IRevaluationService, RevaluationService>();
                services.AddSingleton<IRevaluationProductService, RevaluationProductService>();
                services.AddSingleton<IDataService<Gender>, GenericService<Gender>>();
                services.AddSingleton<IDataService<ContractorType>, GenericService<ContractorType>>();
                services.AddSingleton<IContractorService, ContractorService>();
                services.AddSingleton<ICustomerService, CustomerService>();
                services.AddSingleton<IProductBarcodeService, ProductBarcodeService>();
                services.AddSingleton<IPointSaleService, PointSaleService>();
            });
        }
    }
}
