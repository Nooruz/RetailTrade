using DevExpress.XtraReports.UI;
using RetailTradeClient.ViewModels.Base;

namespace RetailTradeClient.ViewModels
{
    public class DocumentViewModel : BaseViewModel
    {
        #region Private Members

        private XtraReport _printingDocument;

        #endregion

        #region Public Members

        public XtraReport PrintingDocument
        {
            get => _printingDocument;
            set
            {
                _printingDocument = value;
                OnPropertyChanged(nameof(PrintingDocument));
            }
        }

        #endregion
    }
}
