using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Barcode;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Factories;
using RetailTradeServer.ViewModels.Menus;
using System;

namespace RetailTradeServer.HostBuilders
{
    public static class AddMenuViewModelHostBuilderExtensions
    {
        public static IHostBuilder AddMenuViewModels(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {

                services.AddTransient(CreateProductViewModel);
                services.AddTransient(CreateArrivalProductViewModel);
                services.AddTransient(CreateWriteDownProductViewModel);
                services.AddTransient(CreateOrderProductViewModel);
                services.AddTransient(CreateProductBarcodeViewModel);
                services.AddTransient(CreateSaleDashboardView);
                services.AddTransient(CreateBranchViewModel);
                services.AddTransient(CreateUserViewModel);
                services.AddTransient(CreateRefundToSupplierViewModel);
                services.AddTransient(CreateSupplierViewModel);
                services.AddTransient(CreateEmployeesViewModel);
                services.AddTransient(CreateConnectingAndConfiguringEquipmentViewModel);
                services.AddTransient(CreateCashShiftsViewModel);
                services.AddTransient(CreateWareHouseViewModel);
                services.AddTransient(CreateRevaluationViewModel);
                services.AddTransient(CreateReturnProductFromCustomerViewModel);

                services.AddSingleton<CreateMenuViewModel<ProductViewModel>>(servicesProvider => () => CreateProductViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<ArrivalProductViewModel>>(servicesProvider => () => CreateArrivalProductViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<WriteDownProductViewModel>>(servicesProvider => () => CreateWriteDownProductViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<OrderProductViewModel>>(servicesProvider => () => CreateOrderProductViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<ProductBarcodeViewModel>>(servicesProvider => () => CreateProductBarcodeViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<SaleDashboardView>>(servicesProvider => () => CreateSaleDashboardView(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<BranchViewModel>>(servicesProvider => () => CreateBranchViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<UserViewModel>>(servicesProvider => () => CreateUserViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<RefundToSupplierViewModel>>(servicesProvider => () => CreateRefundToSupplierViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<SupplierViewModel>>(servicesProvider => () => CreateSupplierViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<EmployeesViewModel>>(servicesProvider => () => CreateEmployeesViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<ConnectingAndConfiguringHardwareViewModel>>(servicesProvider => () => CreateConnectingAndConfiguringEquipmentViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<CashShiftsViewModel>>(servicesProvider => () => CreateCashShiftsViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<WareHouseViewModel>>(servicesProvider => () => CreateWareHouseViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<RevaluationViewModel>>(servicesProvider => () => CreateRevaluationViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<ReturnProductFromCustomerViewModel>>(servicesProvider => () => CreateReturnProductFromCustomerViewModel(servicesProvider));

                services.AddSingleton<IMenuViewModelFactory, MenuViewModelFactory>();
            });
        }

        private static ReturnProductFromCustomerViewModel CreateReturnProductFromCustomerViewModel(IServiceProvider services)
        {
            return new ReturnProductFromCustomerViewModel();
        }

        private static RevaluationViewModel CreateRevaluationViewModel(IServiceProvider services)
        {
            return new RevaluationViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<ITypeProductService>(),
                services.GetRequiredService<IRevaluationService>(),
                services.GetRequiredService<IMenuNavigator>(),
                services.GetRequiredService<IDataService<Unit>>());
        }

        private static SupplierViewModel CreateSupplierViewModel(IServiceProvider services)
        {
            return new SupplierViewModel(services.GetRequiredService<ISupplierService>());
        }

        private static ProductViewModel CreateProductViewModel(IServiceProvider services)
        {
            return new ProductViewModel(services.GetRequiredService<ITypeProductService>(),
                services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IDataService<Unit>>(),
                services.GetRequiredService<ISupplierService>(),
                services.GetRequiredService<IMessageStore>(),
                services.GetRequiredService<IZebraBarcodeScanner>(),
                services.GetRequiredService<IComBarcodeService>());
        }

        private static ArrivalProductViewModel CreateArrivalProductViewModel(IServiceProvider services)
        {
            return new ArrivalProductViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IArrivalService>(),
                services.GetRequiredService<ISupplierService>());
        }

        private static WriteDownProductViewModel CreateWriteDownProductViewModel(IServiceProvider services)
        {
            return new WriteDownProductViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IWriteDownService>(),
                services.GetRequiredService<ISupplierService>());
        }

        private static OrderProductViewModel CreateOrderProductViewModel(IServiceProvider services)
        {
            return new OrderProductViewModel(services.GetRequiredService<IOrderToSupplierService>(),
                services.GetRequiredService<IProductService>(),
                services.GetRequiredService<ITypeProductService>(),
                services.GetRequiredService<ISupplierService>(),
                services.GetRequiredService<IOrderStatusService>(),
                services.GetRequiredService<IUserStore>(),
                services.GetRequiredService<IZebraBarcodeScanner>(),
                services.GetRequiredService<IDataService<Unit>>());
        }

        private static ProductBarcodeViewModel CreateProductBarcodeViewModel(IServiceProvider services)
        {
            return new ProductBarcodeViewModel(services.GetRequiredService<IProductService>());
        }

        private static SaleDashboardView CreateSaleDashboardView(IServiceProvider services)
        {
            return new SaleDashboardView(services.GetRequiredService<IReceiptService>(),
                services.GetRequiredService<IProductSaleService>());
        }

        private static BranchViewModel CreateBranchViewModel(IServiceProvider services)
        {
            return new BranchViewModel(services.GetRequiredService<IDataService<Branch>>(),
                services.GetRequiredService<IUserService>());
        }

        private static UserViewModel CreateUserViewModel(IServiceProvider services)
        {
            return new UserViewModel(services.GetRequiredService<IUserService>(),
                services.GetRequiredService<IAuthenticator>(),
                services.GetRequiredService<IRoleService>(),
                services.GetRequiredService<IMessageStore>());
        }

        private static RefundToSupplierViewModel CreateRefundToSupplierViewModel(IServiceProvider services)
        {
            return new RefundToSupplierViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IRefundToSupplierService>(),
                services.GetRequiredService<IRefundToSupplierServiceProduct>(),
                services.GetRequiredService<ISupplierService>());
        }

        private static EmployeesViewModel CreateEmployeesViewModel(IServiceProvider services)
        {
            return new EmployeesViewModel(services.GetRequiredService<IEmployeeService>(),
                services.GetRequiredService<IGroupEmployeeService>(),
                services.GetRequiredService<IDataService<Gender>>());
        }

        private static ConnectingAndConfiguringHardwareViewModel CreateConnectingAndConfiguringEquipmentViewModel(IServiceProvider services)
        {
            return new ConnectingAndConfiguringHardwareViewModel(services.GetRequiredService<IDataService<TypeEquipment>>());
        }

        private static CashShiftsViewModel CreateCashShiftsViewModel(IServiceProvider services)
        {
            return new CashShiftsViewModel(services.GetRequiredService<IShiftService>(),
                services.GetRequiredService<IUserService>());
        }

        private static WareHouseViewModel CreateWareHouseViewModel(IServiceProvider services)
        {
            return new WareHouseViewModel(services.GetRequiredService<IWareHouseService>(),
                services.GetRequiredService<IDataService<TypeWareHouse>>());
        }
    }
}
