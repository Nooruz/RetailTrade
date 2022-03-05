using DevExpress.Mvvm;
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
        private IMessageBoxService _messageBoxService;

        #endregion

        #region Constructor

        public ClosingShiftCommand(IMessageBoxService messageBoxService,
            IShiftStore shiftStore, 
            int userId)
        {
            _shiftStore = shiftStore;
            _userId = userId;
            _messageBoxService = messageBoxService;
        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await _shiftStore.ClosingShift(_messageBoxService, _userId);
        }
    }
}
