using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Barcode.Services;
using RetailTrade.Domain.Services;
using RetailTradeClient.State.Authenticators;
using RetailTradeClient.State.Messages;
using RetailTradeClient.State.Navigators;
using RetailTradeClient.State.ProductSale;
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
                services.AddSingleton(s => new MainWindow());

                services.AddTransient(CreateMainWindowViewModel);
                services.AddTransient(CreateHomeViewModel);
                services.AddTransient(CreateLoginViewModel);
                services.AddTransient(CreateGlobalMessageViewModel);
                services.AddTransient(CreateProductsWithoutBarcodeViewModel);
                services.AddTransient(CreatePaymentCashViewModel);
                services.AddTransient(CreatePaymentComplexViewModel);

                services.AddSingleton<CreateViewModel<HomeViewModel>>(servicesProvider => () => CreateHomeViewModel(servicesProvider));
                services.AddSingleton<CreateViewModel<LoginViewModel>>(servicesProvider => () => CreateLoginViewModel(servicesProvider));
                services.AddSingleton<CreateViewModel<ProductsWithoutBarcodeViewModel>>(servicesProvider => () => CreateProductsWithoutBarcodeViewModel(servicesProvider));

                services.AddSingleton<IViewModelFactory, ViewModelFactory>();

                services.AddSingleton<ViewModelDelegateRenavigator<LoginViewModel>>();
                services.AddSingleton<ViewModelDelegateRenavigator<HomeViewModel>>();
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
                services.GetRequiredService<IViewModelFactory>(),
                services.GetRequiredService<IUserStore>(),
                services.GetRequiredService<IOrganizationService>());
        }

        private static HomeViewModel CreateHomeViewModel(IServiceProvider services)
        {
            return new HomeViewModel(services.GetRequiredService<IUserStore>(),
                services.GetRequiredService<IReceiptService>(),
                services.GetRequiredService<IAuthenticator>(),
                services.GetRequiredService<IShiftStore>(),
                services.GetRequiredService<IProductSaleStore>(),
                services.GetRequiredService<IBarcodeService>(),
                services.GetRequiredService<ProductsWithoutBarcodeViewModel>(),
                services.GetRequiredService<PaymentCashViewModel>(),
                services.GetRequiredService<PaymentComplexViewModel>());
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
