using DevExpress.Mvvm;
using RetailTrade.CashRegisterMachine;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Properties;
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
        private bool _isShiftOpen;
        private Shift _currentShift;

        #endregion

        #region Public Properties

        #endregion

        #region Constructor

        public ShiftStore(IShiftService shiftService,
            IReceiptService receiptService)
        {
            _shiftService = shiftService;
            _receiptService = receiptService;
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

        public ObservableCollection<Receipt> Receipts => new(_receiptService.GetReceiptsFromCurrentShift(CurrentShift.Id));

        public event Action<CheckingResult> CurrentShiftChanged;

        public async Task CheckingShift(IMessageBoxService MessageBoxService,
            int userId)
        {
            if (ShtrihM.CheckConnection() == 0)
            {
                ShtrihMConnected(true);
                CurrentShiftChanged.Invoke(await Check(userId));
                return;
            }
            if (MessageBoxService.ShowMessage("Устройство фискального регистратора (ФР) не обноружено. Продолжить?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
            {
                ShtrihMConnected(false);
                CurrentShiftChanged.Invoke(await Check(userId));
                return;
            }
            CurrentShiftChanged.Invoke(CheckingResult.Nothing);
        }        

        public async Task ClosingShift(IMessageBoxService MessageBoxService,
            int userId)
        {
            var result = await _shiftService.GetOpenShiftByUserIdAsync(userId);
            if (result != null)
            {
                if (ShtrihM.CheckConnection() == 0)
                {
                    ShtrihM.PrintReportWithCleaning();
                    ShtrihMConnected(true);
                    CurrentShiftChanged.Invoke(await Closing(userId));
                    return;
                }
                if (MessageBoxService.ShowMessage("Устройство фискального регистратора (ФР) не обноружено. Продолжить?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                {
                    ShtrihMConnected(false);
                    CurrentShiftChanged.Invoke(await Closing(userId));
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

        public async Task OpeningShift(IMessageBoxService MessageBoxService,
            int userId)
        {
            var result = await _shiftService.GetOpenShiftByUserIdAsync(userId);
            if (result == null)
            {
                if (ShtrihM.CheckConnection() == 0)
                {
                    ShtrihM.OpenSession();
                    ShtrihMConnected(true);
                    CurrentShiftChanged.Invoke(await Opening(userId));
                    return;
                }
                if (MessageBoxService.ShowMessage("Устройство фискального регистратора (ФР) не обноружено. Продолжить?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                {
                    ShtrihMConnected(false);
                    CurrentShiftChanged.Invoke(await Opening(userId));
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

        private async Task<CheckingResult> Check(int userId)
        {
            var shift = await _shiftService.GetOpenShiftByUserIdAsync(userId);
            if (shift != null)
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
            return CheckingResult.Close;
        }

        private async Task<CheckingResult> Closing(int userId)
        {
            if (await _shiftService.ClosingShiftAsync(userId))
            {
                IsShiftOpen = false;
                CurrentShift = null;
                return CheckingResult.Close;
            }
            return CheckingResult.ErrorClosing;
        }

        private async Task<CheckingResult> Opening(int userId)
        {
            var openShift = await _shiftService.OpeningShiftAsync(userId);
            if (openShift != null)
            {
                CurrentShift = openShift;
                IsShiftOpen = true;
                return CheckingResult.Created;
            }
            return CheckingResult.ErrorOpening;
        }
    }
}
