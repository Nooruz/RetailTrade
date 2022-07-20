using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Domain.Services;
using RetailTrade.POS.State.Shifts;
using RetailTrade.POS.States.Users;
using RetailTrade.POS.ViewModels;
using RetailTrade.POS.ViewModels.Factories;
using RetailTrade.POS.ViewModels.Menus;
using System;

namespace RetailTrade.POS.HostBuilders
{
    public static class AddMenuViewModelHostBuilderExtensions
    {
        public static IHostBuilder AddMenuViewModels(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                _ = services.AddTransient(CreateSalesViewModel);
                _ = services.AddTransient(CreateRefundViewModel);
                _ = services.AddTransient(CreateWorkSaleViewModel);
                _ = services.AddTransient(CreateDeferredReceiptsViewModel);
                _ = services.AddTransient(CreateHistoryViewModel);
                _ = services.AddTransient(CreateShiftViewModel);

                _ = services.AddSingleton<CreateMenuViewModel<SalesViewModel>>(servicesProvider => () => CreateSalesViewModel(servicesProvider));
                _ = services.AddSingleton<CreateMenuViewModel<RefundViewModel>>(servicesProvider => () => CreateRefundViewModel(servicesProvider));
                _ = services.AddSingleton<CreateMenuViewModel<WorkSaleViewModel>>(servicesProvider => () => CreateWorkSaleViewModel(servicesProvider));
                _ = services.AddSingleton<CreateMenuViewModel<DeferredReceiptsViewModel>>(servicesProvider => () => CreateDeferredReceiptsViewModel(servicesProvider));
                _ = services.AddSingleton<CreateMenuViewModel<HistoryViewModel>>(servicesProvider => () => CreateHistoryViewModel(servicesProvider));
                _ = services.AddSingleton<CreateMenuViewModel<ShiftViewModel>>(servicesProvider => () => CreateShiftViewModel(servicesProvider));


                _ = services.AddSingleton<IMenuViewModelFactory, MenuViewModelFactory>();
            });
        }

        private static WorkSaleViewModel CreateWorkSaleViewModel(IServiceProvider services)
        {
            return new WorkSaleViewModel(services.GetRequiredService<IPointSaleService>());
        }

        private static DeferredReceiptsViewModel CreateDeferredReceiptsViewModel(IServiceProvider services)
        {
            return new DeferredReceiptsViewModel();
        }

        private static HistoryViewModel CreateHistoryViewModel(IServiceProvider services)
        {
            return new HistoryViewModel();
        }

        private static ShiftViewModel CreateShiftViewModel(IServiceProvider services)
        {
            return new ShiftViewModel(services.GetRequiredService<IShiftStore>());
        }

        private static RefundViewModel CreateRefundViewModel(IServiceProvider services)
        {
            return new RefundViewModel();
        }

        private static SalesViewModel CreateSalesViewModel(IServiceProvider services)
        {
            return new SalesViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IUserStore>(),
                services.GetRequiredService<IProductBarcodeService>(),
                services.GetRequiredService<IShiftStore>(),
                services.GetRequiredService<IReceiptService>());
        }
    }
}
