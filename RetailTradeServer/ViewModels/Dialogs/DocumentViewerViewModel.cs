using DevExpress.XtraReports.UI;
using RetailTradeServer.ViewModels.Dialogs.Base;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class DocumentViewerViewModel : BaseDialogViewModel
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
