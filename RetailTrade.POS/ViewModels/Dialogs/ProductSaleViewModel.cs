using RetailTrade.Domain.Models;

namespace RetailTrade.POS.ViewModels.Dialogs
{
    public class ProductSaleViewModel : BaseViewModel
    {
        #region Private Members

        private Receipt _editedReceipt;

        #endregion

        #region Public Properties

        public Receipt EditedReceipt
        {
            get => _editedReceipt;
            set
            {
                _editedReceipt = value;
                OnPropertyChanged(nameof(EditedReceipt));
            }
        }

        #endregion
    }
}
