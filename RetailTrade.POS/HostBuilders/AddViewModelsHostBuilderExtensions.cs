using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Domain.Services;
using RetailTrade.POS.States.Authenticators;
using RetailTrade.POS.States.Navigators;
using RetailTrade.POS.States.Users;
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
                _ = services.AddSingleton(s => new MainWindow());

                _ = services.AddTransient(CreateMainWindowViewModel);
                _ = services.AddTransient(CreateHomeViewModel);
                _ = services.AddTransient(CreateLoginViewModel);

                _ = services.AddSingleton<CreateViewModel<HomeViewModel>>(servicesProvider => () => CreateHomeViewModel(servicesProvider));
                _ = services.AddSingleton<CreateViewModel<LoginViewModel>>(servicesProvider => () => CreateLoginViewModel(servicesProvider));

                _ = services.AddSingleton<IViewModelFactory, ViewModelFactory>();

                _ = services.AddSingleton<ViewModelDelegateRenavigator<HomeViewModel>>();
                _ = services.AddSingleton<ViewModelDelegateRenavigator<LoginViewModel>>();
            });
        }

        private static MainViewModel CreateMainWindowViewModel(IServiceProvider services)
        {
            return new MainViewModel(services.GetRequiredService<INavigator>(),
                services.GetRequiredService<IViewModelFactory>(),
                services.GetRequiredService<IAuthenticator>());
        }

        private static HomeViewModel CreateHomeViewModel(IServiceProvider services)
        {
            return new HomeViewModel(services.GetRequiredService<IMenuNavigator>(),
                services.GetRequiredService<IMenuViewModelFactory>());
        }

        private static LoginViewModel CreateLoginViewModel(IServiceProvider services)
        {
            return new LoginViewModel(services.GetRequiredService<IAuthenticator>(),
                services.GetRequiredService<ViewModelDelegateRenavigator<HomeViewModel>>(),
                services.GetRequiredService<IUserService>(),
                services.GetRequiredService<IUserStore>());
        }
    }
}
