using RetailTradeServer.ViewModels.Base;

namespace RetailTradeServer.ViewModels.Dialogs.Base
{
    public class BaseDialogViewModel : BaseViewModel
    {
        #region Private Members

        private string _title;

        #endregion

        #region Public Properties

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public string CreateCommandContent { get; set; } = "Добавить";
        public MessageViewModel ErrorMessageViewModel { get; }
        public string ErrorMessage
        {
            set => ErrorMessageViewModel.Message = value;
        }

        #endregion

        #region Constructor

        public BaseDialogViewModel()
        {
            ErrorMessageViewModel = new MessageViewModel();
        }

        #endregion
    }
}
