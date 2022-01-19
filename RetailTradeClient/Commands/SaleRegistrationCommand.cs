using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Navigators;
using RetailTradeClient.State.Shifts;
using System.Threading.Tasks;
using System.Windows;

namespace RetailTradeClient.Commands
{
    public class SaleRegistrationCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly IShiftStore _shiftStore;
        private readonly IUIManager _manager;
        private int _userId;
        private IRenavigator _homeRenavigator;

        #endregion

        #region Constructor

        public SaleRegistrationCommand(IShiftStore shiftStore,
            int userId,
            IUIManager manager,
            IRenavigator homeRenavigator)
        {
            _shiftStore = shiftStore;
            _userId = userId;
            _manager = manager;
            _homeRenavigator = homeRenavigator;
        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            switch (await _shiftStore.CheckingShift(_userId))
            {
                case CheckingResult.Open:
                    _manager.Close();
                    _homeRenavigator.Renavigate();
                    break;
                case CheckingResult.Close:
                    _manager.ShowMessage("Откройте новую смену", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case CheckingResult.Exceeded:
                    _manager.ShowMessage("Смена превысила 24 часа. Закройте смену!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case CheckingResult.IsAlreadyOpen:
                    break;
                case CheckingResult.ErrorOpeningShiftKKM:
                    break;
                case CheckingResult.ErrorOpening:
                    break;
                case CheckingResult.ErrorClosing:
                    break;
                case CheckingResult.UnknownErrorWhenClosing:
                    break;
                case CheckingResult.Nothing:
                    break;
                default:
                    break;
            }
        }
    }
}
