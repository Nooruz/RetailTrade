using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.ViewModels.Factories;
using RetailTradeServer.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IMenuNavigator _menuNavigator;
        private readonly IMessageStore _messageStore;
        private readonly IShiftService _shiftService;
        private readonly IProductSaleService _productSaleService;
        private readonly IReceiptService _receiptService;
        private readonly IUserStore _userStore;
        private readonly IMenuViewModelFactory _menuViewModelFactory;
        private ObservableCollection<BaseViewModel> _currentMenuViewModels = new();

        #endregion

        #region Public Properties

        public ObservableCollection<BaseViewModel> CurrentMenuViewModels => _menuNavigator.CurrentViewModels;
        public string OrganizationName => _userStore.CurrentOrganization != null ? _userStore.CurrentOrganization.Name : "";

        #endregion

        #region Commands

        public ICommand UpdateCurrentMenuViewModelCommand => new UpdateCurrentMenuViewModelCommand(_menuNavigator, _menuViewModelFactory);

        #region Моя организация

        public ICommand EmployeeCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.Employee));

        #endregion

        #region Информационная панель

        public ICommand SaleDashboardCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.SaleDashboard));

        #endregion

        #region Продажи

        public ICommand ProductsCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.Products));
        public ICommand RevaluationCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.RevaluationView));
        public ICommand ReturnProductFromCustomerCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.ReturnProductFromCustomerView));
        public ICommand ArrivalProductCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.ArrivalProduct));
        public ICommand WriteDownProductCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.WriteDownProduct));
        public ICommand OrderProductCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.OrderProduct));
        public ICommand RefundToSupplierCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.RefundToSupplier));

        #endregion

        #region Склады

        public ICommand WareHouseCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.WareHouseView));

        #endregion

        #region Справочники и инструменты

        public ICommand BarcodeCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.ProductBarcode));
        public ICommand UserCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.User));
        public ICommand BranchCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.Branch));
        public ICommand SupplierCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.Supplier));

        #endregion

        #region Отчеты

        public ICommand CashShiftsCommand => new RelayCommand(CashShifts);
        public ICommand RevenueForPeriodCommand => new RelayCommand(RevenueForPeriod);
        public ICommand CashiersViewCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.CashierView));

        #endregion

        #region Настройки

        public ICommand PrinterCommand => new RelayCommand(Printer);
        public ICommand ConnectingAndConfiguringEquipmentCommand => new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.ConnectingAndConfiguringEquipment));

        #endregion

        #endregion

        #region Constructor

        public HomeViewModel(IMenuNavigator menuNavigator,
            IMenuViewModelFactory menuViewModelFactory,
            IShiftService shiftService,
            IMessageStore messageStore,
            IProductSaleService productSaleService,
            IUserStore userStore,
            IReceiptService receiptService)
        {
            _menuNavigator = menuNavigator;
            _shiftService = shiftService;
            _messageStore = messageStore;
            _productSaleService = productSaleService;
            _userStore = userStore;
            _receiptService = receiptService;
            _menuViewModelFactory = menuViewModelFactory;

            _menuNavigator.StateChanged += MenuNavigator_StateChanged;

            CloseCommand = new ParameterCommand(Close);

            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.SaleDashboard);
        }

        #endregion

        #region Private Voids

        private void MenuNavigator_StateChanged(BaseViewModel obj)
        {
            OnPropertyChanged(nameof(CurrentMenuViewModels));
            //BaseViewModel viewModel = CurrentMenuViewModels.FirstOrDefault(v => v.ToString() == obj.ToString());
            //if (viewModel == null)
            //{
            //    obj.IsSelected = true;
            //    CurrentMenuViewModels.Add(obj);
            //}
            //else
            //{
            //    viewModel.IsSelected = true;
            //}
        }
        private void Close(object parameter)
        {
            BaseViewModel viewModel = CurrentMenuViewModels.FirstOrDefault(v => v.ToString() == parameter.ToString());
            if (viewModel != null)
            {
                viewModel.Dispose();
                _ = CurrentMenuViewModels.Remove(viewModel);
            }            
        }
        private void Printer()
        {
            WindowService.Show(nameof(PrinterDialogForm), new PrinterDialogFormModel(_messageStore) { Title = "Настройки принтеров" });
        }
        private void CashShifts()
        {
            WindowService.Show(nameof(ReportClosingShiftsDialogForm), new ReportClosingShiftsDialogFormModel(_shiftService) { Title = "Закрытие смены" });
        }
        private void RevenueForPeriod()
        {
            WindowService.Show(nameof(ReportRevenueForPeriodDialogForm), new ReportRevenueForPeriodDialogFormModel(_receiptService, _userStore) { Title = "Выручка за период" });
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            _menuNavigator.StateChanged -= MenuNavigator_StateChanged;
            base.Dispose();
        }

        #endregion
    }
}
