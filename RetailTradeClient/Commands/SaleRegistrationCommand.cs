using RetailTrade.Domain.Services;
using RetailTradeClient.State.Dialogs;
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

        #endregion

        #region Constructor

        public SaleRegistrationCommand(IShiftStore shiftStore,
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
            var checkinResult = await _shiftStore.CheckingShift(_userId);
            switch (checkinResult)
            {
                case CheckingResult.Open:
                    _manager.Close();
                    break;
                case CheckingResult.Close:
                    MessageBox.Show("Откройте новую смену");
                    break;
                case CheckingResult.Exceeded:
                    MessageBox.Show("Смена превысила 24 часа. Закройте смену!");
                    break;
            }
        }
    }
}
