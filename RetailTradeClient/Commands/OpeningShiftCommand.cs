using RetailTradeClient.State.Shifts;
using System.Threading.Tasks;

namespace RetailTradeClient.Commands
{
    public class OpeningShiftCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly IShiftStore _shiftStore;
        private int _userId;

        #endregion

        #region Constructor

        public OpeningShiftCommand(IShiftStore shiftStore, 
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
            await _shiftStore.OpeningShift(_userId);
        }
    }
}
