using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using System;
using System.Threading.Tasks;

namespace RetailTradeClient.State.Shifts
{
    public class ShiftStore : IShiftStore
    {
        #region Private Members

        private readonly IShiftService _shiftService;
        private bool _isShiftOpen;
        private Shift _currentShift;

        #endregion

        #region Constructor

        public ShiftStore(IShiftService shiftService)
        {
            _shiftService = shiftService;
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

        public async Task ClosingShift(int userId)
        {
            if (await _shiftService.ClosingShiftAsync(userId))
            {
                IsShiftOpen = false;
                CurrentShift = null;
                CurrentShiftChanged?.Invoke();
            }            
        }

        public async Task<CheckingResult> OpeningShift(int userId)
        {
            var result = await _shiftService.GetOpenShiftByUserIdAsync(userId);
            if (result == null)
            {
                var openShift = await _shiftService.OpeningShiftAsync(userId);
                CurrentShift = openShift;
                IsShiftOpen = true;                
                CurrentShiftChanged?.Invoke();
                return CheckingResult.Open;
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
