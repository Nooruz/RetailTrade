using RetailTradeClient.Commands;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Navigators;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using RetailTradeClient.Views.Dialogs;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class MainMenuViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IShiftStore _shiftStore;
        private readonly IUserStore _userStore;
        private readonly IUIManager _manager;

        #endregion

        #region Public Properties

        public bool IsShiftOpen => _shiftStore.IsShiftOpen;
        public bool IsUserAdmin => _userStore.CurrentUser.RoleId == 1;
        public Visibility Visibility => IsUserAdmin ? Visibility.Collapsed : Visibility.Visible;

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
            IUIManager manager,
            IRenavigator homeRenavigator,
            IUserStore userStore)
        {
            _shiftStore = shiftStore;
            _manager = manager;
            _userStore = userStore;

            OpeningShiftCommand = new OpeningShiftCommand(shiftStore, userId, manager);
            ClosingShiftCommand = new ClosingShiftCommand(shiftStore, userId, manager);
            SaleRegistrationCommand = new SaleRegistrationCommand(shiftStore, userId, manager, homeRenavigator);
            SettingCommand = new RelayCommand(Setting);

            shiftStore.CurrentShiftChanged += ShiftStore_CurrentShiftChanged;
        }

        #endregion

        #region Private Voids

        private void Setting()
        {            
            _manager.ShowDialog(new ApplicationSettingsViewModel(_manager) { Title = "Настройки"}, new ApplicationSettingsView());
        }

        private void ShiftStore_CurrentShiftChanged()
        {
            OnPropertyChanged(nameof(IsShiftOpen));
        }

        #endregion
    }
}