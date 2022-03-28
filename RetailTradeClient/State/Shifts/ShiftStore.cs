using DevExpress.Mvvm;
using RetailTrade.CashRegisterMachine;
using RetailTrade.CashRegisterMachine.Services;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
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
        private readonly ICashRegisterMachineService _cashRegisterMachineService;
        private bool _isShiftOpen;
        private Shift _currentShift;

        #endregion

        #region Public Properties

        #endregion

        #region Constructor

        public ShiftStore(IShiftService shiftService,
            IReceiptService receiptService,
            IUserStore userStore,
            ICashRegisterMachineService cashRegisterMachineService)
        {
            _shiftService = shiftService;
            _receiptService = receiptService;
            _userStore = userStore;
            _cashRegisterMachineService = cashRegisterMachineService;
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
                ECRModeEnum eCRModeEnum = _cashRegisterMachineService.ECRMode();
                if (eCRModeEnum == ECRModeEnum.Mode3)
                {
                    if (MessageBoxService.ShowMessage($"{eCRModeEnum.GetStringValue()} Закрыть смену ККМ?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                    {
                        if (!string.IsNullOrEmpty(_cashRegisterMachineService.CloseShift()))
                        {
                            //_ = MessageBoxService.ShowMessage(_cashRegisterMachineService.ErrorMessage, "Sale Page", MessageButton.YesNo, MessageIcon.Question);
                            CurrentShiftChanged?.Invoke(CheckingResult.UnknownErrorWhenClosing);
                            return;
                        }
                    }
                }
                if (eCRModeEnum == ECRModeEnum.Mode4)
                {
                    if (MessageBoxService.ShowMessage($"{eCRModeEnum.GetStringValue()} Открыть смену ККМ?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                    {
                        if (!string.IsNullOrEmpty(_cashRegisterMachineService.OpenShift()))
                        {
                            //_ = MessageBoxService.ShowMessage($"Устройство ККМ. {_cashRegisterMachineService.ErrorMessage}", "Sale Page", MessageButton.OK, MessageIcon.Error);
                            CurrentShiftChanged?.Invoke(CheckingResult.UnknownErrorWhenClosing);
                            return;
                        }
                    }
                }
                if (eCRModeEnum != ECRModeEnum.Mode3 && eCRModeEnum != ECRModeEnum.Mode4)
                {
                    _ = MessageBoxService.ShowMessage($"Устройство ККМ. {eCRModeEnum.GetStringValue()}", "Sale Page", MessageButton.OK, MessageIcon.Information);
                }
            }
            catch (Exception e)
            {
                _ = MessageBoxService.ShowMessage(e.Message, "Sale Page", MessageButton.OK, MessageIcon.Error);
            }
        }        

        public async Task ClosingShift(IMessageBoxService MessageBoxService)
        {
            var result = await _shiftService.GetOpenShiftAsync();
            if (result != null)
            {
                CurrentShiftChanged.Invoke(await Closing());
                ECRModeEnum eCRModeEnum = _cashRegisterMachineService.ECRMode();
                if (eCRModeEnum != ECRModeEnum.Mode4)
                {
                    if (!string.IsNullOrEmpty(_cashRegisterMachineService.CloseShift()))
                    {
                        _ = MessageBoxService.ShowMessage($"Устройство ККМ. {_cashRegisterMachineService.ErrorMessage}", "Sale Page", MessageButton.OK, MessageIcon.Error);
                    }
                }
                return;
            }
            CurrentShiftChanged.Invoke(CheckingResult.NoOpenShift);
            return;
        }

        public async Task OpeningShift(IMessageBoxService MessageBoxService)
        {
            var result = await _shiftService.GetOpenShiftAsync();
            if (result == null)
            {
                CurrentShiftChanged.Invoke(await Opening());
                ECRModeEnum eCRModeEnum = _cashRegisterMachineService.ECRMode();
                if (eCRModeEnum != ECRModeEnum.Mode2)
                {
                    if (!string.IsNullOrEmpty(_cashRegisterMachineService.OpenShift()))
                    {
                        _ = MessageBoxService.ShowMessage($"Устройство ККМ. {_cashRegisterMachineService.ErrorMessage}", "Sale Page", MessageButton.OK, MessageIcon.Error);
                    }
                }
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
