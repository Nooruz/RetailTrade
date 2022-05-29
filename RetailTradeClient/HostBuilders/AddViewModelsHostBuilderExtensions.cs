using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Barcode.Services;
using RetailTrade.CashRegisterMachine.Services;
using RetailTrade.Domain.Services;
using RetailTradeClient.State.Authenticators;
using RetailTradeClient.State.Messages;
using RetailTradeClient.State.Navigators;
using RetailTradeClient.State.ProductSales;
using RetailTradeClient.State.Reports;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using RetailTradeClient.ViewModels;
using RetailTradeClient.ViewModels.Base;
using RetailTradeClient.ViewModels.Components;
using RetailTradeClient.ViewModels.Dialogs;
using RetailTradeClient.ViewModels.Factories;
using System;

namespace RetailTradeClient.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                _ = services.AddSingleton(s => new MainWindow());

                _ = services.AddTransient(CreateMainWindowViewModel);
                _ = services.AddTransient(CreateHomeViewModel);
                _ = services.AddTransient(CreateLoginViewModel);
                _ = services.AddTransient(CreateGlobalMessageViewModel);
                _ = services.AddTransient(CreateProductsWithoutBarcodeViewModel);
                _ = services.AddTransient(CreatePaymentCashViewModel);
                _ = services.AddTransient(CreatePaymentComplexViewModel);

                _ = services.AddSingleton<CreateViewModel<HomeViewModel>>(servicesProvider => () => CreateHomeViewModel(servicesProvider));
                _ = services.AddSingleton<CreateViewModel<LoginViewModel>>(servicesProvider => () => CreateLoginViewModel(servicesProvider));
                _ = services.AddSingleton<CreateViewModel<ProductsWithoutBarcodeViewModel>>(servicesProvider => () => CreateProductsWithoutBarcodeViewModel(servicesProvider));

                _ = services.AddSingleton<IViewModelFactory, ViewModelFactory>();

                _ = services.AddSingleton<ViewModelDelegateRenavigator<LoginViewModel>>();
                _ = services.AddSingleton<ViewModelDelegateRenavigator<HomeViewModel>>();
            });
        }

        private static PaymentCashViewModel CreatePaymentCashViewModel(IServiceProvider services)
        {
            return new PaymentCashViewModel(services.GetRequiredService<IProductSaleStore>()) { Title = "Оплата наличными" };
        }

        private static PaymentComplexViewModel CreatePaymentComplexViewModel(IServiceProvider services)
        {
            return new PaymentComplexViewModel(services.GetRequiredService<IProductSaleStore>()) { Title = "Оплата наличными" };
        }

        private static ProductsWithoutBarcodeViewModel CreateProductsWithoutBarcodeViewModel(IServiceProvider services)
        {
            return new ProductsWithoutBarcodeViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IProductSaleStore>());
        }

        private static MainViewModel CreateMainWindowViewModel(IServiceProvider services)
        {
            return new MainViewModel(services.GetRequiredService<INavigator>(),
                services.GetRequiredService<IViewModelFactory>());
        }

        private static HomeViewModel CreateHomeViewModel(IServiceProvider services)
        {
            return new HomeViewModel(services.GetRequiredService<IUserStore>(),
                services.GetRequiredService<IReceiptService>(),
                services.GetRequiredService<IAuthenticator>(),
                services.GetRequiredService<IShiftStore>(),
                services.GetRequiredService<IBarcodeService>(),
                services.GetRequiredService<IProductService>(),
                services.GetRequiredService<ICashRegisterMachineService>(),
                services.GetRequiredService<PaymentCashViewModel>(),
                services.GetRequiredService<PaymentComplexViewModel>(),
                services.GetRequiredService<ProductViewModel>(),
                services.GetRequiredService<MainWindow>(),
                services.GetRequiredService<IReportService>(),
                services.GetRequiredService<IProductWareHouseService>());
        }

        private static LoginViewModel CreateLoginViewModel(IServiceProvider services)
        {
            return new LoginViewModel(services.GetRequiredService<IAuthenticator>(),
                services.GetRequiredService<ViewModelDelegateRenavigator<HomeViewModel>>(),
                services.GetRequiredService<GlobalMessageViewModel>(),
                services.GetRequiredService<IMessageStore>(),
                services.GetRequiredService<IUserService>(),
                services.GetRequiredService<IShiftStore>(),
                services.GetRequiredService<IUserStore>());
        }

        private static GlobalMessageViewModel CreateGlobalMessageViewModel(IServiceProvider services)
        {
            return new GlobalMessageViewModel(services.GetRequiredService<IMessageStore>());
        }
    }
}
