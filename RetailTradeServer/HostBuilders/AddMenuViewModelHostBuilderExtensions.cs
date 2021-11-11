using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels;
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

                services.AddTransient(CreateProductCategoryViewModel);
                services.AddTransient(CreateArrivalProductViewModel);
                services.AddTransient(CreateWriteDownProductViewModel);
                services.AddTransient(CreateOrderProductViewModel);
                services.AddTransient(CreateProductBarcodeViewModel);
                services.AddTransient(CreateAnalyticalPanelViewModel);
                services.AddTransient(CreateBranchViewModel);
                services.AddTransient(CreateUserViewModel);
                services.AddTransient(CreateRefundToSupplierViewModel);

                services.AddSingleton<CreateMenuViewModel<ProductCategoryViewModel>>(servicesProvider => () => CreateProductCategoryViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<ArrivalProductViewModel>>(servicesProvider => () => CreateArrivalProductViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<WriteDownProductViewModel>>(servicesProvider => () => CreateWriteDownProductViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<OrderProductViewModel>>(servicesProvider => () => CreateOrderProductViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<ProductBarcodeViewModel>>(servicesProvider => () => CreateProductBarcodeViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<AnalyticalPanelViewModel>>(servicesProvider => () => CreateAnalyticalPanelViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<BranchViewModel>>(servicesProvider => () => CreateBranchViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<UserViewModel>>(servicesProvider => () => CreateUserViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<RefundToSupplierViewModel>>(servicesProvider => () => CreateRefundToSupplierViewModel(servicesProvider));

                services.AddSingleton<IMenuViewModelFactory, MenuViewModelFactory>();
            });
        }

        private static ProductCategoryViewModel CreateProductCategoryViewModel(IServiceProvider services)
        {
            return new ProductCategoryViewModel(services.GetRequiredService<IProductSubcategoryService>(),
                services.GetRequiredService<IProductCategoryService>(),
                services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IUIManager>(),
                services.GetRequiredService<IDataService<Unit>>(),
                services.GetRequiredService<ISupplierService>(),
                services.GetRequiredService<GlobalMessageViewModel>(),
                services.GetRequiredService<IMessageStore>());
        }

        private static ArrivalProductViewModel CreateArrivalProductViewModel(IServiceProvider services)
        {
            return new ArrivalProductViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IArrivalProductService>(),
                services.GetRequiredService<IUIManager>());
        }

        private static WriteDownProductViewModel CreateWriteDownProductViewModel(IServiceProvider services)
        {
            return new WriteDownProductViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IWriteDownProductService>(),
                services.GetRequiredService<IUIManager>());
        }

        private static OrderProductViewModel CreateOrderProductViewModel(IServiceProvider services)
        {
            return new OrderProductViewModel(services.GetRequiredService<IUIManager>());
        }

        private static ProductBarcodeViewModel CreateProductBarcodeViewModel(IServiceProvider services)
        {
            return new ProductBarcodeViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IUIManager>());
        }

        private static AnalyticalPanelViewModel CreateAnalyticalPanelViewModel(IServiceProvider services)
        {
            return new AnalyticalPanelViewModel(services.GetRequiredService<IReceiptService>());
        }

        private static BranchViewModel CreateBranchViewModel(IServiceProvider services)
        {
            return new BranchViewModel(services.GetRequiredService<IDataService<Branch>>(),
                services.GetRequiredService<IUIManager>(),
                services.GetRequiredService<IUserService>());
        }

        private static UserViewModel CreateUserViewModel(IServiceProvider services)
        {
            return new UserViewModel(services.GetRequiredService<IUserService>(),
                services.GetRequiredService<IUIManager>(),
                services.GetRequiredService<IAuthenticator>(),
                services.GetRequiredService<IRoleService>());
        }

        private static RefundToSupplierViewModel CreateRefundToSupplierViewModel(IServiceProvider services)
        {
            return new RefundToSupplierViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<ISupplierService>(),
                services.GetRequiredService<IUIManager>(),
                services.GetRequiredService<IProductRefundToSupplierService>());
        }
    }
}
