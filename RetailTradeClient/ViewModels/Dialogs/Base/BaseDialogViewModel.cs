using RetailTradeClient.ViewModels.Base;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class BaseDialogViewModel : BaseViewModel
    {
        #region Public Properties

        public string Title { get; set; }
        public string CreateCommandContent { get; set; } = "Добавить";
        public bool Result { get; set; }

        #endregion

        #region Constructor

        public BaseDialogViewModel()
        {
            
        }

        #endregion
    }
}
