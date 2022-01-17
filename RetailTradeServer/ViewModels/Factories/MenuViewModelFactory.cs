﻿using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Menus;
using System;

namespace RetailTradeServer.ViewModels.Factories
{
    public class MenuViewModelFactory : IMenuViewModelFactory
    {
        #region Private Members

        private readonly CreateMenuViewModel<ProductCategoryViewModel> _createProductCategoryViewModel;
        private readonly CreateMenuViewModel<ArrivalProductViewModel> _createArrivalProductViewModel;
        private readonly CreateMenuViewModel<WriteDownProductViewModel> _createWriteDownProductViewModel;
        private readonly CreateMenuViewModel<OrderProductViewModel> _createOrderProductViewModel;
        private readonly CreateMenuViewModel<ProductBarcodeViewModel> _createProductBarcodeViewModel;
        private readonly CreateMenuViewModel<BranchViewModel> _createBranchViewModel;
        private readonly CreateMenuViewModel<UserViewModel> _createUserViewModel;
        private readonly CreateMenuViewModel<RefundToSupplierViewModel> _createRefundToSupplierViewModel;
        private readonly CreateMenuViewModel<SupplierViewModel> _createSupplierViewModel;
        private readonly CreateMenuViewModel<SaleDashboardView> _createMenuViewModel;
        private readonly CreateMenuViewModel<EmployeesViewModel> _createEmployeesViewModel;
        private readonly CreateMenuViewModel<ConnectingAndConfiguringEquipmentViewModel> _createConnectingAndConfiguringEquipmentViewModel;

        #endregion

        #region Constructor

        public MenuViewModelFactory(CreateMenuViewModel<ProductCategoryViewModel> createProductCategoryViewModel,
            CreateMenuViewModel<ArrivalProductViewModel> createArrivalProductViewModel,
            CreateMenuViewModel<WriteDownProductViewModel> createWriteDownProductViewModel,
            CreateMenuViewModel<OrderProductViewModel> createOrderProductViewModel,
            CreateMenuViewModel<ProductBarcodeViewModel> createProductBarcodeViewModel,
            CreateMenuViewModel<BranchViewModel> createBranchViewModel,
            CreateMenuViewModel<UserViewModel> createUserViewModel,
            CreateMenuViewModel<RefundToSupplierViewModel> createRefundToSupplierViewModel,
            CreateMenuViewModel<SupplierViewModel> createSupplierViewModel,
            CreateMenuViewModel<SaleDashboardView> createMenuViewModel,
            CreateMenuViewModel<EmployeesViewModel> createEmployeesViewModel,
            CreateMenuViewModel<ConnectingAndConfiguringEquipmentViewModel> createConnectingAndConfiguringEquipmentViewModel)
        {
            _createProductCategoryViewModel = createProductCategoryViewModel;
            _createArrivalProductViewModel = createArrivalProductViewModel;
            _createWriteDownProductViewModel = createWriteDownProductViewModel;
            _createOrderProductViewModel = createOrderProductViewModel;
            _createProductBarcodeViewModel = createProductBarcodeViewModel;
            _createBranchViewModel = createBranchViewModel;
            _createUserViewModel = createUserViewModel;
            _createRefundToSupplierViewModel = createRefundToSupplierViewModel;
            _createSupplierViewModel = createSupplierViewModel;
            _createMenuViewModel = createMenuViewModel;
            _createEmployeesViewModel = createEmployeesViewModel;
            _createConnectingAndConfiguringEquipmentViewModel = createConnectingAndConfiguringEquipmentViewModel;
        }

        #endregion

        public BaseViewModel CreateViewModel(MenuViewType viewType)
        {
            return viewType switch
            {
                MenuViewType.ProductCategory => _createProductCategoryViewModel(),
                MenuViewType.ArrivalProduct => _createArrivalProductViewModel(),
                MenuViewType.WriteDownProduct => _createWriteDownProductViewModel(),
                MenuViewType.OrderProduct => _createOrderProductViewModel(),
                MenuViewType.ProductBarcode => _createProductBarcodeViewModel(),
                MenuViewType.SaleDashboard => _createMenuViewModel(),
                MenuViewType.Branch => _createBranchViewModel(),
                MenuViewType.User => _createUserViewModel(),
                MenuViewType.RefundToSupplier => _createRefundToSupplierViewModel(),
                MenuViewType.Supplier => _createSupplierViewModel(),
                MenuViewType.Employee => _createEmployeesViewModel(),
                MenuViewType.ConnectingAndConfiguringEquipment => _createConnectingAndConfiguringEquipmentViewModel(),
                _ => throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType"),
            };
        }
    }
}
