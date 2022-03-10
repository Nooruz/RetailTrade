using RetailTradeClient.Report;
using System.Threading.Tasks;

namespace RetailTradeClient.State.Reports
{
    public class ReportService : IReportService
    {
        #region Private Members

        private readonly XReport _xReport;

        #endregion

        #region Constructor

        public ReportService(XReport xReport)
        {
            _xReport = xReport;
        }

        #endregion

        public async Task<XReport> CreateXReport()
        {
            _xReport.SetValues();
            await _xReport.CreateDocumentAsync();
            return _xReport;
        }
    }
}
