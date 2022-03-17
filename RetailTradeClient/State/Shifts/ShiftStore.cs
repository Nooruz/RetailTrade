using DevExpress.Mvvm;
using RetailTrade.CashRegisterMachine;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Properties;
using RetailTradeClient.State.Users;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RetailTradeClient.State.Shifts
{
    public class ShiftStore : IShiftStore
    {
        #region Private Members

        private readonly IShiftService _shiftService;
        private readonly IReceiptService _receiptService;
        private readonly IUserStore _userStore;
        private bool _isShiftOpen;
        private Shift _currentShift;

        #endregion

        #region Public Properties

        #endregion

        #region Constructor

        public ShiftStore(IShiftService shiftService,
            IReceiptService receiptService,
            IUserStore userStore)
        {
            _shiftService = shiftService;
            _receiptService = receiptService;
            _userStore = userStore;
        }

        #endregion

        public bool IsShiftOpen
        {
            get => _isShiftOpen;
            private set => _isShiftOpen = value;
        }

        public Shift CurrentShift
        {
            get => _currentShift;
            private set => _currentShift = value;
        }

        public event Action<CheckingResult> CurrentShiftChanged;

        public async Task CheckingShift(IMessageBoxService MessageBoxService)
        {
            if (ShtrihM.CheckConnection() == 0)
            {
                ShtrihMConnected(true);
                CurrentShiftChanged.Invoke(await Check());
                return;
            }
            if (MessageBoxService.ShowMessage("Устройство фискального регистратора (ФР) не обноружено. Продолжить?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
            {
                ShtrihMConnected(false);
                CurrentShiftChanged.Invoke(await Check());
                return;
            }
            CurrentShiftChanged.Invoke(CheckingResult.Nothing);
        }        

        public async Task ClosingShift(IMessageBoxService MessageBoxService)
        {
            var result = await _shiftService.GetOpenShiftAsync();
            if (result != null)
            {
                if (ShtrihM.CheckConnection() == 0)
                {
                    ShtrihM.PrintReportWithCleaning();
                    ShtrihMConnected(true);
                    CurrentShiftChanged.Invoke(await Closing());
                    return;
                }
                if (MessageBoxService.ShowMessage("Устройство фискального регистратора (ФР) не обноружено. Продолжить?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                {
                    ShtrihMConnected(false);
                    CurrentShiftChanged.Invoke(await Closing());
                    return;
                }
                else
                {
                    CurrentShiftChanged.Invoke(CheckingResult.Nothing);
                    return;
                }
            }
            CurrentShiftChanged.Invoke(CheckingResult.NoOpenShift);
            return;
        }

        public async Task OpeningShift(IMessageBoxService MessageBoxService)
        {
            var result = await _shiftService.GetOpenShiftAsync();
            if (result == null)
            {
                if (ShtrihM.CheckConnection() == 0)
                {
                    ShtrihM.OpenSession();
                    ShtrihMConnected(true);
                    CurrentShiftChanged.Invoke(await Opening());
                    return;
                }
                if (MessageBoxService.ShowMessage("Устройство фискального регистратора (ФР) не обноружено. Продолжить?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                {
                    ShtrihMConnected(false);
                    CurrentShiftChanged.Invoke(await Opening());
                    return;
                }
                else
                {
                    CurrentShiftChanged.Invoke(CheckingResult.Nothing);
                    return;
                }
            }
            if (DateTime.Now.Subtract(result.OpeningDate).Days > 0)
            {
                CurrentShiftChanged.Invoke(CheckingResult.Exceeded);
                return;
            }
            CurrentShift = result;
            IsShiftOpen = true;
            CurrentShiftChanged.Invoke(CheckingResult.IsAlreadyOpen);
        }

        private static void ShtrihMConnected(bool isConnected)
        {
            Settings.Default.ShtrihMConnected = isConnected;
            Settings.Default.Save();
        }

        private async Task<CheckingResult> Check()
        {
            var shift = await _shiftService.GetOpenShiftAsync();
            if (shift != null)
            {
                if (shift.UserId == _userStore.CurrentUser.Id)
                {
                    if (DateTime.Now.Subtract(shift.OpeningDate).Days == 0)
                    {
                        IsShiftOpen = true;
                        CurrentShift = shift;
                        return CheckingResult.Open;
                    }
                    if (DateTime.Now.Subtract(shift.OpeningDate).Days > 0)
                    {
                        return CheckingResult.Exceeded;
                    }
                }
                else
                {
                    return CheckingResult.ShiftOpenedByAnotherUser;
                }
            }
            return CheckingResult.NoOpenShift;
        }

        private async Task<CheckingResult> Closing()
        {
            if (await _shiftService.ClosingShiftAsync(_userStore.CurrentUser.Id))
            {
                IsShiftOpen = false;
                CurrentShift = null;
                return CheckingResult.Close;
            }
            return CheckingResult.ErrorClosing;
        }

        private async Task<CheckingResult> Opening()
        {
            var openShift = await _shiftService.OpeningShiftAsync(_userStore.CurrentUser.Id);
            if (openShift != null)
            {
                CurrentShift = openShift;
                IsShiftOpen = true;
                return CheckingResult.Created;
            }
            return CheckingResult.ErrorOpening;
        }

        public ObservableCollection<Receipt> GetReceipts()
        {
            return new(_receiptService.GetReceiptsFromCurrentShift(CurrentShift.Id));
        }
    }
}
