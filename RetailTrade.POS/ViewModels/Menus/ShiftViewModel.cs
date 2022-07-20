using RetailTrade.Domain.Models;
using RetailTrade.POS.State.Shifts;
using System.Windows;

namespace RetailTrade.POS.ViewModels.Menus
{
    public class ShiftViewModel : BaseViewModel
    {
        #region Private Voids

        private IShiftStore _shiftStore;

        #endregion

        #region Public Properties

        public bool IsShiftOpen => _shiftStore.IsShiftOpen;
        public string CurrentShiftText => IsShiftOpen ? $"Смена открыта {CurrentShift.OpeningDate:dd MMMM yyyy} г. в {CurrentShift.OpeningDate:HH:mm}" : $"Смена закрыта {CurrentShift.ClosingDate:dd MMMM yyyyy} г. в {CurrentShift.ClosingDate:HH:mm}";
        public Shift CurrentShift => _shiftStore.CurrentShift;
        public Visibility OpenShiftVisibility => IsShiftOpen ? Visibility.Collapsed : Visibility.Visible;

        #endregion

        #region Constructor

        public ShiftViewModel(IShiftStore shiftStore)
        {
            _shiftStore = shiftStore;

            _shiftStore.CurrentShiftChanged += ShiftStore_CurrentShiftChanged;
        }

        #endregion

        #region Private Voids

        private void ShiftStore_CurrentShiftChanged(CheckingResult checkingResult)
        {
            OnPropertyChanged(nameof(IsShiftOpen));
            OnPropertyChanged(nameof(OpenShiftVisibility));
        }

        #endregion
    }
}
