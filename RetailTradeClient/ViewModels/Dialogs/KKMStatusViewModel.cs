namespace RetailTradeClient.ViewModels.Dialogs
{
    public class KKMStatusViewModel : BaseDialogViewModel
    {
        #region Private Members

        private string _status;

        #endregion

        #region Public Properties

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        #endregion
    }
}
