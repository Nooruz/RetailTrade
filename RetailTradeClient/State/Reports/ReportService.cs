using RetailTrade.Domain.Models;
using RetailTradeClient.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTradeClient.State.Reports
{
    public class ReportService : IReportService
    {
        #region Private Members

        private readonly XReport _xReport;
        private readonly ProductSaleReport _productSaleReport;
        private readonly DiscountReceiptReport _discountReceiptReport;

        #endregion

        #region Constructor

        public ReportService(XReport xReport,
            ProductSaleReport productSaleReport, 
            DiscountReceiptReport discountReceiptReport)
        {
            _xReport = xReport;
            _productSaleReport = productSaleReport;
            _discountReceiptReport = discountReceiptReport;
        }

        #endregion

        #region Publis Task Voids

        public async Task<XReport> CreateXReport()
        {
            _xReport.SetValues();
            await _xReport.CreateDocumentAsync();
            return _xReport;
        }

        public async Task<ProductSaleReport> CreateProductSaleReport(Receipt receipt, IEnumerable<Sale> sales)
        {
            _productSaleReport.SetValues(receipt, sales);
            await _productSaleReport.CreateDocumentAsync();
            return _productSaleReport;
        }

        public async Task<DiscountReceiptReport> CreateDiscountReceiptReport(Receipt receipt, IEnumerable<Sale> sales)
        {
            _discountReceiptReport.SetValues(receipt, sales);
            await _discountReceiptReport.CreateDocumentAsync();
            return _discountReceiptReport;
        }

        #endregion
    }
}
