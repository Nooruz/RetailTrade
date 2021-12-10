using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Shifts;
using System.Threading.Tasks;
using System.Windows;

namespace RetailTradeClient.Commands
{
    public class OpeningShiftCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly IShiftStore _shiftStore;
        private readonly IUIManager _manager;
        private int _userId;

        #endregion

        #region Constructor

        public OpeningShiftCommand(IShiftStore shiftStore, 
            int userId,
            IUIManager manager)
        {
            _shiftStore = shiftStore;
            _manager = manager;
            _userId = userId;
        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            var result = await _shiftStore.OpeningShift(_userId);
            switch (result)
            {
                case CheckingResult.Open:
                    break;
                case CheckingResult.Close:
                    break;
                case CheckingResult.IsAlreadyOpen:
                    _manager.ShowMessage("Смена уже отркыта.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case CheckingResult.Exceeded:
                    _manager.ShowMessage("Смена превысила 24 часа. Закройте смену!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case CheckingResult.ErrorOpeningShiftKKM:
                    _manager.ShowMessage("Не удалось открыт смену фискального регистратора (ФР)", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                default:
                    break;
            }
        }
    }
}
