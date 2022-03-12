using RetailTrade.Domain.Models;
using RetailTradeClient.Report;
using RetailTradeClient.State.ProductSale;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTradeClient.State.Reports
{
    public class ReportService : IReportService
    {
        #region Private Members

        private readonly XReport _xReport;
        private readonly ProductSaleReport _productSaleReport;
        private readonly IProductSaleStore _productSaleStore;

        #endregion

        #region Constructor

        public ReportService(XReport xReport,
            ProductSaleReport productSaleReport,
            IProductSaleStore productSaleStore)
        {
            _xReport = xReport;
            _productSaleReport = productSaleReport;
            _productSaleStore = productSaleStore;
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
            _productSaleReport.SetValues(receipt, _productSaleStore.ProductSales);
            await _productSaleReport.CreateDocumentAsync();
            return _productSaleReport;
        }

        #endregion
    }
}
