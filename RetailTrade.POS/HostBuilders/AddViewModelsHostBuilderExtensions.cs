using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.POS.States.Authenticators;
using RetailTrade.POS.States.Navigators;
using RetailTrade.POS.ViewModels;
using RetailTrade.POS.ViewModels.Factories;
using System;

namespace RetailTrade.POS.HostBuilders
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

                services.AddSingleton<CreateViewModel<LoginViewModel>>(servicesProvider => () => CreateLoginViewModel(servicesProvider));
                services.AddSingleton<CreateViewModel<HomeViewModel>>(servicesProvider => () => CreateHomeViewModel(servicesProvider));

                services.AddSingleton<IViewModelFactory, ViewModelFactory>();

                services.AddSingleton<ViewModelDelegateRenavigator<LoginViewModel>>();
                services.AddSingleton<ViewModelDelegateRenavigator<HomeViewModel>>();
            });
        }

        private static MainViewModel CreateMainWindowViewModel(IServiceProvider services)
        {
            return new MainViewModel(services.GetRequiredService<INavigator>(),
                services.GetRequiredService<IViewModelFactory>(),
                services.GetRequiredService<IAuthenticator>());
        }

        private static LoginViewModel CreateLoginViewModel(IServiceProvider services)
        {
            return new LoginViewModel();
        }

        private static HomeViewModel CreateHomeViewModel(IServiceProvider services)
        {
            return new HomeViewModel(services.GetRequiredService<IMenuNavigator>(),
                services.GetRequiredService<IMenuViewModelFactory>());
        }

    }
}
