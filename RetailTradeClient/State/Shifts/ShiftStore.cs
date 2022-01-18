using RetailTrade.CashRegisterMachine;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.State.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace RetailTradeClient.State.Shifts
{
    public class ShiftStore : IShiftStore
    {
        #region Private Members

        private readonly IShiftService _shiftService;
        private readonly IUIManager _manager;
        private bool _isShiftOpen;
        private Shift _currentShift;

        #endregion

        #region Constructor

        public ShiftStore(IShiftService shiftService)
        {
            _shiftService = shiftService;
            _manager = new UIManager();
        }        

        #endregion

        public bool IsShiftOpen
        {
            get => _isShiftOpen;
            private set
            {
                _isShiftOpen = value;
                CurrentShiftChanged?.Invoke();
            }
        }

        public Shift CurrentShift 
        {
            get => _currentShift;
            private set
            {
                _currentShift = value;
                CurrentShiftChanged?.Invoke();
            }
        }

        public event Action CurrentShiftChanged;

        public async Task<CheckingResult> CheckingShift(int userId)
        {
            if (ShtrihM.CheckConnection() == 0)
            {
                return await Check(userId);
            }
            else
            {
                if (_manager.ShowMessage("Устройство фискального регистратора (ФР) не обноружено. Продолжить?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    return await Check(userId);
                }
                else
                {
                    return CheckingResult.IsAlreadyOpen;
                }
            }            
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
                    CurrentShiftChanged?.Invoke();
                    return CheckingResult.Open;
                }
                if (DateTime.Now.Subtract(shift.OpeningDate).Days > 0)
                {
                    return CheckingResult.Exceeded;
                }
            }
            return CheckingResult.Close;
        }

        public async Task<CheckingResult> ClosingShift(int userId)
        {
            if (ShtrihM.CheckConnection() == 0)
            {
                ShtrihM.PrintReportWithCleaning();
                if (await _shiftService.ClosingShiftAsync(userId))
                {
                    IsShiftOpen = false;
                    CurrentShift = null;
                    CurrentShiftChanged?.Invoke();
                    return CheckingResult.Close;
                }
                else
                {
                    return CheckingResult.UnknownErrorWhenClosing;
                }
            }
            else
            {
                return CheckingResult.ErrorOpeningShiftKKM;
            }           
        }

        public async Task<CheckingResult> OpeningShift(int userId)
        {
            var result = await _shiftService.GetOpenShiftByUserIdAsync(userId);
            if (result == null)
            {
                if (ShtrihM.CheckConnection() == 0)
                {
                    ShtrihM.OpenSession();
                    var openShift = await _shiftService.OpeningShiftAsync(userId);
                    CurrentShift = openShift;
                    IsShiftOpen = true;
                    CurrentShiftChanged?.Invoke();
                    return CheckingResult.Open;
                }
                else
                {
                    return CheckingResult.ErrorOpeningShiftKKM;
                }                
            }
            if (DateTime.Now.Subtract(result.OpeningDate).Days > 0)
            {
                return CheckingResult.Exceeded;
            }
            CurrentShift = result;
            IsShiftOpen = true;
            CurrentShiftChanged?.Invoke();
            return CheckingResult.IsAlreadyOpen;
        }
    }
}
