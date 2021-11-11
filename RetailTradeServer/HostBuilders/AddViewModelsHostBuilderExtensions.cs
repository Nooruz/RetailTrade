﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Domain.Services;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Factories;
using System;

namespace RetailTradeServer.HostBuilders
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
                services.AddTransient(CreateRegistrationViewModel);
                services.AddTransient(CreateOrganizationViewModel);
                services.AddTransient(CreateSaleViewModel);
                services.AddTransient(CreateGlobalMessageViewModel);

                services.AddSingleton<CreateViewModel<LoginViewModel>>(servicesProvider => () => CreateLoginViewModel(servicesProvider));
                services.AddSingleton<CreateViewModel<RegistrationViewModel>>(servicesProvider => () => CreateRegistrationViewModel(servicesProvider));
                services.AddSingleton<CreateViewModel<HomeViewModel>>(servicesProvider => () => CreateHomeViewModel(servicesProvider));
                services.AddSingleton<CreateViewModel<OrganizationViewModel>>(servicesProvider => () => CreateOrganizationViewModel(servicesProvider));
                services.AddSingleton<CreateViewModel<SaleViewModel>>(servicesProvider => () => CreateSaleViewModel(servicesProvider));                

                services.AddSingleton<IRetailTradeViewModelFactory, RetailTradeViewModelFactory>();

                services.AddSingleton<ViewModelDelegateRenavigator<LoginViewModel>>();
                services.AddSingleton<ViewModelDelegateRenavigator<HomeViewModel>>();
                services.AddSingleton<ViewModelDelegateRenavigator<RegistrationViewModel>>();
                services.AddSingleton<ViewModelDelegateRenavigator<OrganizationViewModel>>();
                services.AddSingleton<ViewModelDelegateRenavigator<SaleViewModel>>();
            });
        }

        private static MainViewModel CreateMainWindowViewModel(IServiceProvider services)
        {
            return new MainViewModel(services.GetRequiredService<INavigator>(),
                services.GetRequiredService<IRetailTradeViewModelFactory>(),
                services.GetRequiredService<IAuthenticator>(),
                services.GetRequiredService<IMenuNavigator>(),
                services.GetRequiredService<IMenuViewModelFactory>());
        }

        private static HomeViewModel CreateHomeViewModel(IServiceProvider services)
        {
            return new HomeViewModel(services.GetRequiredService<IMenuNavigator>(),
                services.GetRequiredService<IMenuViewModelFactory>(),
                services.GetRequiredService<IUIManager>(),
                services.GetRequiredService<IShiftService>());
        }

        private static LoginViewModel CreateLoginViewModel(IServiceProvider services)
        {
            return new LoginViewModel(services.GetRequiredService<IAuthenticator>(),
                services.GetRequiredService<INavigator>(),
                services.GetRequiredService<IRetailTradeViewModelFactory>(),
                services.GetRequiredService<IMessageStore>(),
                services.GetRequiredService<GlobalMessageViewModel>());
        }

        private static RegistrationViewModel CreateRegistrationViewModel(IServiceProvider services)
        {
            return new RegistrationViewModel(services.GetRequiredService<IRoleService>(),
                services.GetRequiredService<INavigator>(),
                services.GetRequiredService<IAuthenticator>(),
                services.GetRequiredService<IRetailTradeViewModelFactory>());
        }

        private static OrganizationViewModel CreateOrganizationViewModel(IServiceProvider services)
        {
            return new OrganizationViewModel(services.GetRequiredService<IOrganizationService>(),
                services.GetRequiredService<IUserStore>());
        }

        private static SaleViewModel CreateSaleViewModel(IServiceProvider services)
        {
            return new SaleViewModel();
        }

        private static GlobalMessageViewModel CreateGlobalMessageViewModel(IServiceProvider services)
        {
            return new GlobalMessageViewModel(services.GetRequiredService<IMessageStore>());
        }        
    }
}
