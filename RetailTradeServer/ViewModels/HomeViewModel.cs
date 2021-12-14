using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.ViewModels.Factories;
using RetailTradeServer.Views.Dialogs;
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

        #endregion

        #region Commands
        
        public ICommand UpdateCurrentMenuViewModelCommand { get; }

        #region Информационная панель

        public ICommand SaleDashboardCommand { get; }

        #endregion

        #region Товары

        public ICommand ProductCategoryCommand { get; }
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

        public ICommand ReportClosingShiftsCommand { get; }
        public ICommand RevenueForPeriodCommand { get; }

        #endregion

        #region Настройки

        public ICommand PrinterCommand { get; }

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

            ProductCategoryCommand = new RelayCommand(ProductCategory);
            SaleDashboardCommand = new RelayCommand(SaleDashboard);
            ArrivalProductCommand = new RelayCommand(ArrivalProduct);
            WriteDownProductCommand = new RelayCommand(WriteDownProduct);
            OrderProductCommand = new RelayCommand(OrderProduct);
            BarcodeCommand = new RelayCommand(Barcode);
            UserCommand = new RelayCommand(User);
            BranchCommand = new RelayCommand(Branch);
            ReportClosingShiftsCommand = new RelayCommand(ReportClosingShifts);
            RefundToSupplierCommand = new RelayCommand(RefundToSupplier);
            SupplierCommand = new RelayCommand(OpenSupplier);
            PrinterCommand = new RelayCommand(Printer);
            RevenueForPeriodCommand = new RelayCommand(RevenueForPeriod);

            _menuNavigator.StateChanged += MenuNavigator_StateChanged;

            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.SaleDashboard);
        }

        #endregion

        #region Private Voids

        private void SaleDashboard()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.SaleDashboard);
        }

        private void Printer()
        {
            _dialogService.ShowDialog(new PrinterDialogFormModel(_messageStore) { Title = "Настройки принтеров" }, new PrinterDialogForm());
        }

        private void OpenSupplier()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.Supplier);
        }

        private void ProductCategory()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.ProductCategory);
        }

        private void RefundToSupplier()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.RefundToSupplier);
        }

        private void ArrivalProduct()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.ArrivalProduct);
        }

        private void WriteDownProduct()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.WriteDownProduct);
        }

        private void OrderProduct()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.OrderProduct);
        }

        private void User()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.User);
        }

        private void Branch()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.Branch);
        }

        private void MenuNavigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentMenuViewModel));
        }

        /// <summary>
        /// Ценники и этикетки (Штрих-код)
        /// </summary>
        private void Barcode()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.ProductBarcode);
        }

        private void ReportClosingShifts()
        {
            _ = _dialogService.ShowDialog(new ReportClosingShiftsDialogFormModel(_dialogService, _shiftService) { Title = "Закрытие смены" },
                new ReportClosingShiftsDialogForm());
        }

        private void RevenueForPeriod()
        {
            _ = _dialogService.ShowDialog(new ReportRevenueForPeriodDialogFormModel(_dialogService, _receiptService, _userStore) { Title = "Выручка за период" },
                new ReportRevenueForPeriodDialogForm());
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
