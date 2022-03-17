using DevExpress.Mvvm;
using RetailTradeClient.Commands;
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
        private readonly IRenavigator _homeRenavigator;
        private readonly IUserStore _userStore;
        private int _userId;

        #endregion

        #region Public Properties

        public bool IsShiftOpen => _shiftStore.IsShiftOpen;
        public bool IsUserAdmin => _userStore.CurrentUser.RoleId == 1;
        public Visibility Visibility => IsUserAdmin ? Visibility.Collapsed : Visibility.Visible;

        #endregion

        #region Commands

        public ICommand OpeningShiftCommand => new RelayCommand(async () => await _shiftStore.OpeningShift(MessageBoxService));
        public ICommand ClosingShiftCommand => new RelayCommand(async () => await _shiftStore.ClosingShift(MessageBoxService));
        public ICommand SaleRegistrationCommand => new RelayCommand(async () => await _shiftStore.CheckingShift(MessageBoxService));
        public ICommand SettingCommand => new RelayCommand(() => WindowService.Show(nameof(ApplicationSettingsView), new ApplicationSettingsViewModel() { Title = "Настройки" }));

        #endregion

        #region Constructor

        public MainMenuViewModel(IShiftStore shiftStore,
            IRenavigator homeRenavigator,
            IUserStore userStore)
        {
            _shiftStore = shiftStore;
            _homeRenavigator = homeRenavigator;
            _userStore = userStore;

            _shiftStore.CurrentShiftChanged += ShiftStore_CurrentShiftChanged;
        }

        #endregion

        #region Private Voids

        private void ShiftStore_CurrentShiftChanged(CheckingResult result)
        {
            switch (result)
            {
                case CheckingResult.Open:
                    CurrentWindowService.Close();
                    _homeRenavigator.Renavigate();
                    break;
                case CheckingResult.Close:
                    _ = MessageBoxService.ShowMessage("Смена успешно закрыта.", "Sale Page", MessageButton.OK, MessageIcon.Information);
                    break;
                case CheckingResult.IsAlreadyOpen:
                    _ = MessageBoxService.ShowMessage("Смена уже отркыта.", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
                    break;
                case CheckingResult.Exceeded:
                    _ = MessageBoxService.ShowMessage("Смена превысила 24 часа. Закройте смену!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
                    break;
                case CheckingResult.ErrorOpeningShiftKKM:
                    _ = MessageBoxService.ShowMessage("Не удалось открыть смену фискального регистратора (ФР)", "Sale Page", MessageButton.OK, MessageIcon.Error);
                    break;
                case CheckingResult.ErrorOpening:
                    _ = MessageBoxService.ShowMessage("Ошибка при открытии смены.", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
                    break;
                case CheckingResult.ErrorClosing:
                    _ = MessageBoxService.ShowMessage("Ошибка при закрытии смены.", "Sale Page", MessageButton.OK, MessageIcon.Error);
                    break;
                case CheckingResult.UnknownErrorWhenClosing:
                    _ = MessageBoxService.ShowMessage("Неизветсная ошибка при соединение с сервером.", "Sale Page", MessageButton.OK, MessageIcon.Error);
                    break;
                case CheckingResult.Nothing:
                    break;
                case CheckingResult.NoOpenShift:
                    _ = MessageBoxService.ShowMessage("Смена не открыта.", "Sale Page", MessageButton.OK, MessageIcon.Error);
                    break;
                case CheckingResult.Created:
                    _ = MessageBoxService.ShowMessage("Смена успешно отркыта.", "Sale Page", MessageButton.OK, MessageIcon.Information);
                    break;
                case CheckingResult.ShiftOpenedByAnotherUser:
                    _ = MessageBoxService.ShowMessage("Смена открыта другим пользователем.", "Sale Page", MessageButton.OK, MessageIcon.Information);
                    break;
                default:
                    break;
            }
        }

        private void ShiftStore_CurrentShiftChanged()
        {
            OnPropertyChanged(nameof(IsShiftOpen));
        }

        #endregion
    }
}