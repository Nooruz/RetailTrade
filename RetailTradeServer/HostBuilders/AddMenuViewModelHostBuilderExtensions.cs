﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailTrade.Barcode.Services;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.State.Printing;
using RetailTradeServer.State.Reports;
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
                services.AddTransient(CreateLossViewModel);
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
                services.AddTransient(CreatePriceListViewModel);
                services.AddTransient(CreateCreateProductViewModel);
                services.AddTransient(CreatePointSaleViewModel);
                services.AddTransient(CreateCreatePointSaleViewModel);
                services.AddTransient(CreateEnterViewModel);
                services.AddTransient(CreateEnterProductViewModel);
                services.AddTransient(CreateLossProductViewModel);
                services.AddTransient(CreateMoveViewModel);
                services.AddTransient(CreateMoveProductViewModel);

                services.AddSingleton<CreateMenuViewModel<ProductViewModel>>(servicesProvider => () => CreateProductViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<ArrivalProductViewModel>>(servicesProvider => () => CreateArrivalProductViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<LossViewModel>>(servicesProvider => () => CreateLossViewModel(servicesProvider));
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
                services.AddSingleton<CreateMenuViewModel<PriceListViewModel>>(servicesProvider => () => CreatePriceListViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<CreateProductViewModel>>(servicesProvider => () => CreateCreateProductViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<PointSaleViewModel>>(servicesProvider => () => CreatePointSaleViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<CreatePointSaleViewModel>>(servicesProvider => () => CreateCreatePointSaleViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<EnterViewModel>>(servicesProvider => () => CreateEnterViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<EnterProductViewModel>>(servicesProvider => () => CreateEnterProductViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<LossProductViewModel>>(servicesProvider => () => CreateLossProductViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<MoveViewModel>>(servicesProvider => () => CreateMoveViewModel(servicesProvider));
                services.AddSingleton<CreateMenuViewModel<MoveProductViewModel>>(servicesProvider => () => CreateMoveProductViewModel(servicesProvider));

                services.AddSingleton<IMenuViewModelFactory, MenuViewModelFactory>();
            });
        }

        private static MoveViewModel CreateMoveViewModel(IServiceProvider services)
        {
            return new MoveViewModel(services.GetRequiredService<IMenuNavigator>(),
                services.GetRequiredService<IMenuViewModelFactory>(),
                services.GetRequiredService<IDocumentService>(),
                services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IWareHouseService>(),
                services.GetRequiredService<IUserStore>(),
                services.GetRequiredService<IMessageStore>());
        }

        private static MoveProductViewModel CreateMoveProductViewModel(IServiceProvider services)
        {
            return new MoveProductViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IWareHouseService>(),
                services.GetRequiredService<IDocumentService>(),
                services.GetRequiredService<IUserStore>(),
                services.GetRequiredService<IMessageStore>());
        }

        private static LossProductViewModel CreateLossProductViewModel(IServiceProvider services)
        {
            return new LossProductViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IWareHouseService>(),
                services.GetRequiredService<IDocumentService>(),
                services.GetRequiredService<IUserStore>(),
                services.GetRequiredService<IMessageStore>());
        }

        private static EnterViewModel CreateEnterViewModel(IServiceProvider services)
        {
            return new EnterViewModel(services.GetRequiredService<IMenuNavigator>(),
                services.GetRequiredService<IMenuViewModelFactory>(),
                services.GetRequiredService<IDocumentService>(),
                services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IWareHouseService>(),
                services.GetRequiredService<IUserStore>(),
                services.GetRequiredService<IMessageStore>());
        }

        private static EnterProductViewModel CreateEnterProductViewModel(IServiceProvider services)
        {
            return new EnterProductViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IWareHouseService>(),
                services.GetRequiredService<IDocumentService>(),
                services.GetRequiredService<IUserStore>(),
                services.GetRequiredService<IMessageStore>());
        }

        private static CreatePointSaleViewModel CreateCreatePointSaleViewModel(IServiceProvider services)
        {
            return new CreatePointSaleViewModel(services.GetRequiredService<IPointSaleService>(),
                services.GetRequiredService<IMessageStore>(),
                services.GetRequiredService<IWareHouseService>(),
                services.GetRequiredService<IUserService>());
        }

        private static PointSaleViewModel CreatePointSaleViewModel(IServiceProvider services)
        {
            return new PointSaleViewModel(services.GetRequiredService<IMenuNavigator>(),
                services.GetRequiredService<IMenuViewModelFactory>(),
                services.GetRequiredService<IPointSaleService>(),
                services.GetRequiredService<IMessageStore>(),
                services.GetRequiredService<IWareHouseService>(),
                services.GetRequiredService<IUserService>());
        }

        private static CreateProductViewModel CreateCreateProductViewModel(IServiceProvider services)
        {
            return new CreateProductViewModel(services.GetRequiredService<ITypeProductService>(),
                services.GetRequiredService<IUnitService>(),
                services.GetRequiredService<IProductService>(),
                services.GetRequiredService<ISupplierService>(),
                services.GetRequiredService<IMessageStore>(),
                services.GetRequiredService<IBarcodeService>(),
                services.GetRequiredService<IProductBarcodeService>(),
                services.GetRequiredService<IWareHouseService>(),
                services.GetRequiredService<IProductSaleService>(),
                services.GetRequiredService<IArrivalProductService>());
        }

        private static PriceListViewModel CreatePriceListViewModel(IServiceProvider services)
        {
            return new PriceListViewModel();
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
                services.GetRequiredService<IUnitService>(),
                services.GetRequiredService<IRevaluationProductService>());
        }

        private static SupplierViewModel CreateSupplierViewModel(IServiceProvider services)
        {
            return new SupplierViewModel(services.GetRequiredService<ISupplierService>());
        }

        private static ProductViewModel CreateProductViewModel(IServiceProvider services)
        {
            return new ProductViewModel(services.GetRequiredService<ITypeProductService>(),
                services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IReportService>(),
                services.GetRequiredService<IMenuNavigator>(),
                services.GetRequiredService<IMenuViewModelFactory>(),
                services.GetRequiredService<IUnitService>(),
                services.GetRequiredService<ISupplierService>(),
                services.GetRequiredService<IMessageStore>(),
                services.GetRequiredService<IBarcodeService>(),
                services.GetRequiredService<IProductBarcodeService>(),
                services.GetRequiredService<IWareHouseService>(),
                services.GetRequiredService<IArrivalProductService>(),
                services.GetRequiredService<IProductSaleService>());
        }

        private static ArrivalProductViewModel CreateArrivalProductViewModel(IServiceProvider services)
        {
            return new ArrivalProductViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IArrivalService>(),
                services.GetRequiredService<ISupplierService>(),
                services.GetRequiredService<ITypeProductService>(),
                services.GetRequiredService<IBarcodeService>(),
                services.GetRequiredService<IArrivalProductService>(),
                services.GetRequiredService<IDataService<Unit>>(),
                services.GetRequiredService<IWareHouseService>());
        }

        private static LossViewModel CreateLossViewModel(IServiceProvider services)
        {
            return new LossViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<IDocumentService>(),
                services.GetRequiredService<IMenuNavigator>(),
                services.GetRequiredService<IMenuViewModelFactory>(),
                services.GetRequiredService<IWareHouseService>(),
                services.GetRequiredService<IUserStore>(),
                services.GetRequiredService<IMessageStore>());
        }

        private static OrderProductViewModel CreateOrderProductViewModel(IServiceProvider services)
        {
            return new OrderProductViewModel(services.GetRequiredService<IOrderToSupplierService>(),
                services.GetRequiredService<IProductService>(),
                services.GetRequiredService<ITypeProductService>(),
                services.GetRequiredService<ISupplierService>(),
                services.GetRequiredService<IOrderStatusService>(),
                services.GetRequiredService<IUserStore>(),
                services.GetRequiredService<IDataService<Unit>>());
        }

        private static ProductBarcodeViewModel CreateProductBarcodeViewModel(IServiceProvider services)
        {
            return new ProductBarcodeViewModel(services.GetRequiredService<IProductService>(),
                services.GetRequiredService<ITypeProductService>(),
                services.GetRequiredService<IDataService<Unit>>(),
                services.GetRequiredService<ILabelPriceTagService>(),
                services.GetRequiredService<IDataService<TypeLabelPriceTag>>(),
                services.GetRequiredService<ILabelPrintingService>(),
                services.GetRequiredService<IReportService>(),
                services.GetRequiredService<ILabelPriceTagSizeService>(),
                services.GetRequiredService<IBarcodeService>());
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
            return new ConnectingAndConfiguringHardwareViewModel();
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
