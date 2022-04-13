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
using System;
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
        

        #endregion

        #region Public Voids

        [Command]
        public void Renavigate(object parameter)
        {
            if (Enum.TryParse(parameter.ToString(), out MenuViewType menuViewType))
            {
                UpdateCurrentMenuViewModelCommand.Execute(menuViewType);
            }
        }

        [Command]
        public void RevenueForPeriod()
        {
            WindowService.Show(nameof(ReportRevenueForPeriodDialogForm), new ReportRevenueForPeriodDialogFormModel(_receiptService, _userStore) { Title = "Выручка за период" });
        }

        [Command]
        public void Printer()
        {
            WindowService.Show(nameof(PrinterDialogForm), new PrinterDialogFormModel(_messageStore) { Title = "Настройки принтеров" });
        }

        [Command]
        public void CashShifts()
        {
            WindowService.Show(nameof(ReportClosingShiftsDialogForm), new ReportClosingShiftsDialogFormModel(_shiftService) { Title = "Закрытие смены" });
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
