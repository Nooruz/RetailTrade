using RetailTradeServer.ViewModels.Base;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs.Base
{
    public class BaseDialogViewModel : BaseViewModel
    {
        #region Public Properties

        public string Title { get; set; }
        public string CreateCommandContent { get; set; } = "Добавить";
        public MessageViewModel ErrorMessageViewModel { get; }
        public string ErrorMessage
        {
            set => ErrorMessageViewModel.Message = value;
        }

        #endregion

        #region Commands

        public ICommand CloseCommand { get; set; }

        #endregion

        #region Constructor

        public BaseDialogViewModel()
        {
            ErrorMessageViewModel = new MessageViewModel();
        }

        #endregion
    }
}
