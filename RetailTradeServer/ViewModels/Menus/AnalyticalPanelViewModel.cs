using RetailTrade.Domain.Services;
using RetailTradeServer.ViewModels.Base;

namespace RetailTradeServer.ViewModels.Menus
{
    public class AnalyticalPanelViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IReceiptService _receiptService;

        #endregion

        #region Public Properties

        public RetailTradeDashboard RetailTradeDashboard { get; set; }

        #endregion

        #region Constructor

        public AnalyticalPanelViewModel(IReceiptService receiptService)
        {
            _receiptService = receiptService;
            RetailTradeDashboard = new RetailTradeDashboard();
            GetAllReceipts();
        }

        #endregion

        #region Private Vodis

        private async void GetAllReceipts()
        {
            RetailTradeDashboard.Receipts.DataSource = await _receiptService.GetReceiptsAsync();
        }

        #endregion
    }
}