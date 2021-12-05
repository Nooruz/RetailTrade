using RetailTradeClient.Commands;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Shifts;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public static class OkaPF
    {
        [DllImport("drvasOkaMF_KZ", EntryPoint = "Demo")]
        public static extern void Demo();
    }

    

    public class MainMenuViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IShiftStore _shiftStore;
        private readonly IUIManager _manager;

        #endregion

        #region Public Properties

        public bool IsShiftOpen => _shiftStore.IsShiftOpen;

        #endregion

        #region Commands

        public ICommand OpeningShiftCommand { get; }
        public ICommand ClosingShiftCommand { get; }
        public ICommand SaleRegistrationCommand { get; }
        public ICommand SettingCommand { get; }

        #endregion

        #region Constructor

        public MainMenuViewModel(IShiftStore shiftStore,
            int userId,
            IUIManager manager)
        {
            _shiftStore = shiftStore;
            _manager = manager;

            OpeningShiftCommand = new OpeningShiftCommand(shiftStore, userId, manager);
            ClosingShiftCommand = new ClosingShiftCommand(shiftStore, userId);
            SaleRegistrationCommand = new SaleRegistrationCommand(shiftStore, userId, manager);
            SettingCommand = new RelayCommand(Setting);

            shiftStore.CurrentShiftChanged += ShiftStore_CurrentShiftChanged;
        }

        #endregion

        #region Private Voids

        private void Setting()
        {
            
        }

        private void ShiftStore_CurrentShiftChanged()
        {
            OnPropertyChanged(nameof(IsShiftOpen));
        }

        #endregion
    }
}