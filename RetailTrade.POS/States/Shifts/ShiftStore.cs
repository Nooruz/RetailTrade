using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.POS.States.Users;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RetailTrade.POS.State.Shifts
{
    public class ShiftStore : IShiftStore
    {
        #region Private Members

        private readonly IShiftService _shiftService;
        private readonly IReceiptService _receiptService;
        private readonly IUserStore _userStore;
        private bool _isShiftOpen;
        private Shift _currentShift;
        private int _pointSaleId => RetailTrade.POS.Properties.Settings.Default.PointSaleId;

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
            try
            {
                CurrentShiftChanged.Invoke(await Check());                
            }
            catch (Exception e)
            {
                _ = MessageBoxService.ShowMessage(e.Message, "Sale Page", MessageButton.OK, MessageIcon.Error);
            }
        }        

        public async Task ClosingShift(IMessageBoxService MessageBoxService)
        {
            var result = await _shiftService.GetOpenShiftAsync(_pointSaleId);
            if (result != null)
            {
                CurrentShiftChanged.Invoke(await Closing());                
                return;
            }
            CurrentShiftChanged.Invoke(CheckingResult.NoOpenShift);
            return;
        }

        public async Task OpeningShift(IMessageBoxService MessageBoxService)
        {
            var result = await _shiftService.GetOpenShiftAsync(_pointSaleId);
            if (result == null)
            {
                CurrentShiftChanged.Invoke(await Opening());
                return;
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

        private async Task<CheckingResult> Check()
        {
            var shift = await _shiftService.GetOpenShiftAsync(_pointSaleId);
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
                //XReport xReport = await _reportService.CreateXReport();
                //PrintToolBase tool = new(xReport.PrintingSystem);
                //tool.PrinterSettings.PrinterName = Settings.Default.DefaultReceiptPrinter;
                //tool.Print();
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
