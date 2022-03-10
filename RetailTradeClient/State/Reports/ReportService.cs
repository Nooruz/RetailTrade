using RetailTrade.Domain.Models;
using RetailTradeClient.Report;
using System.Threading.Tasks;

namespace RetailTradeClient.State.Reports
{
    public class ReportService : IReportService
    {
        #region Private Members

        private readonly XReport _xReport;
        private readonly ProductSaleReport _productSaleReport;

        #endregion

        #region Constructor

        public ReportService(XReport xReport,
            ProductSaleReport productSaleReport)
        {
            _xReport = xReport;
            _productSaleReport = productSaleReport;
        }

        #endregion

        #region Publis Task Voids

        public async Task<XReport> CreateXReport()
        {
            _xReport.SetValues();
            await _xReport.CreateDocumentAsync();
            return _xReport;
        }

        public async Task<ProductSaleReport> CreateProductSaleReport(Receipt receipt)
        {
            _productSaleReport.SetValues(receipt);
            await _productSaleReport.CreateDocumentAsync();
            return _productSaleReport;
        }

        #endregion
    }
}
