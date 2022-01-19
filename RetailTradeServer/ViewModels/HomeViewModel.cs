using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.ViewModels.Factories;
using SalePageServer.State.Dialogs;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IMenuNavigator _menuNavigator;
        private readonly IMessageStore _messageStore;
        private readonly IDialogService _dialogService;
        private readonly IShiftService _shiftService;
        private readonly IProductSaleService _productSaleService;
        private readonly IReceiptService _receiptService;
        private readonly IUserStore _userStore;

        #endregion

        #region Public Properties

        public BaseViewModel CurrentMenuViewModel => _menuNavigator.CurrentViewModel;
        public string OrganizationName => _userStore.CurrentOrganization != null ? _userStore.CurrentOrganization.Name : "";

        #endregion

        #region Commands

        public ICommand UpdateCurrentMenuViewModelCommand { get; }

        #region Моя организация

        public ICommand EmployeeCommand { get; set; }

        #endregion

        #region Информационная панель

        public ICommand SaleDashboardCommand { get; }

        #endregion

        #region Товары

        public ICommand ProductsCommand { get; }
        public ICommand ArrivalProductCommand { get; }
        public ICommand WriteDownProductCommand { get; }
        public ICommand OrderProductCommand { get; }
        public ICommand RefundToSupplierCommand { get; }

        #endregion

        #region Справочники и инструменты

        public ICommand BarcodeCommand { get; }
        public ICommand UserCommand { get; }
        public ICommand BranchCommand { get; }
        public ICommand SupplierCommand { get; }

        #endregion

        #region Отчеты

        public ICommand CashShiftsCommand { get; }
        public ICommand RevenueForPeriodCommand { get; }
        public ICommand CashiersViewCommand { get; }

        #endregion

        #region Настройки

        public ICommand PrinterCommand { get; }
        public ICommand ConnectingAndConfiguringEquipmentCommand { get; }

        #endregion

        #endregion

        #region Constructor

        public HomeViewModel(IMenuNavigator menuNavigator,
            IMenuViewModelFactory menuViewModelFactory,
            IDialogService dialogService,
            IShiftService shiftService,
            IMessageStore messageStore,
            IProductSaleService productSaleService,
            IUserStore userStore,
            IReceiptService receiptService)
        {
            _menuNavigator = menuNavigator;
            _dialogService = dialogService;
            _shiftService = shiftService;
            _messageStore = messageStore;
            _productSaleService = productSaleService;
            _userStore = userStore;
            _receiptService = receiptService;

            UpdateCurrentMenuViewModelCommand = new UpdateCurrentMenuViewModelCommand(_menuNavigator, menuViewModelFactory);

            ProductsCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.Products));
            SaleDashboardCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.SaleDashboard));
            ArrivalProductCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.ArrivalProduct));
            WriteDownProductCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.WriteDownProduct));
            OrderProductCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.OrderProduct));
            BarcodeCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.ProductBarcode));
            UserCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.User));
            BranchCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.Branch));            
            RefundToSupplierCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.RefundToSupplier));
            SupplierCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.Supplier));            
            EmployeeCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.Employee));
            ConnectingAndConfiguringEquipmentCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.ConnectingAndConfiguringEquipment));
            CashiersViewCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.CashierView));
            PrinterCommand = new RelayCommand(Printer);
            RevenueForPeriodCommand = new RelayCommand(RevenueForPeriod);
            CashShiftsCommand = new RelayCommand(CashShifts);

            _menuNavigator.StateChanged += MenuNavigator_StateChanged;

            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.SaleDashboard);
        }

        #endregion

        #region Private Voids
        private async void Printer()
        {
            await _dialogService.ShowDialog(new PrinterDialogFormModel(_messageStore) { Title = "Настройки принтеров" });
        }
        private void MenuNavigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentMenuViewModel));
        }

        private async void CashShifts()
        {
            await _dialogService.ShowDialog(new ReportClosingShiftsDialogFormModel(_dialogService, _shiftService) { Title = "Закрытие смены" });
        }

        private async void RevenueForPeriod()
        {
            await _dialogService.ShowDialog(new ReportRevenueForPeriodDialogFormModel(_dialogService, _receiptService, _userStore) { Title = "Выручка за период" });
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
