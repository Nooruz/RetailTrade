using RetailTradeClient.State.Shifts;
using System.Threading.Tasks;
using System.Windows;

namespace RetailTradeClient.Commands
{
    public class ClosingShiftCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly IShiftStore _shiftStore;
        private int _userId;

        #endregion

        #region Constructor

        public ClosingShiftCommand(IShiftStore shiftStore, 
            int userId)
        {
            _shiftStore = shiftStore;
            _userId = userId;
        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await _shiftStore.ClosingShift(_userId);
        }
    }
}
