using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Shifts;
using System.Threading.Tasks;
using System.Windows;

namespace RetailTradeClient.Commands
{
    public class ClosingShiftCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly IShiftStore _shiftStore;
        private readonly IUIManager _manager;
        private int _userId;

        #endregion

        #region Constructor

        public ClosingShiftCommand(IShiftStore shiftStore, 
            int userId, 
            IUIManager manager)
        {
            _shiftStore = shiftStore;
            _userId = userId;
            _manager = manager;
        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            switch (await _shiftStore.ClosingShift(_userId))
            {
                case CheckingResult.Open:
                    break;
                case CheckingResult.Close:
                    _manager.ShowMessage("Смена успешно закрыта.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case CheckingResult.IsAlreadyOpen:
                    break;
                case CheckingResult.Exceeded:
                    break;
                case CheckingResult.ErrorOpeningShiftKKM:
                    _manager.ShowMessage("Не удалось закрыть смену фискального регистратора (ФР)", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case CheckingResult.UnknownErrorWhenClosing:
                    _manager.ShowMessage("Неизветсная ошибка при соединение с сервером.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                default:
                    break;
            }
        }
    }
}
