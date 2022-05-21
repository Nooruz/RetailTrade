using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Report;
using RetailTradeServer.State.Printing;
using SalePageServer.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailTradeServer.State.Reports
{
    public class ReportService : IReportService
    {
        #region Private Members

        private readonly LabelReport _labelReport;
        private readonly ILabelPrintingService _labelPrintingService;
        private readonly BalancesAndAvailabilityProducts _balancesAndAvailabilityProducts;
        private readonly IProductService _productService;

        #endregion

        #region Constructor

        public ReportService(LabelReport labelReport,
            ILabelPrintingService labelPrintingService,
            BalancesAndAvailabilityProducts balancesAndAvailabilityProducts,
            IProductService productService)
        {
            _labelReport = labelReport;
            _labelPrintingService = labelPrintingService;
            _balancesAndAvailabilityProducts = balancesAndAvailabilityProducts;
            _productService = productService;
        }

        #endregion

        #region Publis Task Voids

        public async Task<LabelReport> CreateLabelReport()
        {
            try
            {
                List<LabelPrinting> labelPrintings = new();
                if (_labelPrintingService.LabelPrintings.Any())
                {
                    foreach (var item in _labelPrintingService.LabelPrintings)
                    {
                        for (int i = 0; i < item.Quantity; i++)
                        {
                            labelPrintings.Add(new LabelPrinting
                            {
                                Name = item.Name,
                                Barcode = item.Barcode
                            });
                        }
                    }
                }
                _labelReport.DataSource = labelPrintings;
                await _labelReport.CreateDocumentAsync();
                return _labelReport;
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<LabelReport> ForTemplate()
        {
            try
            {
                List<LabelPrinting> labelPrintings = new()
                {
                    new LabelPrinting
                    {
                        Name = "Наименование товара",
                        Barcode = "2000000000001"
                    }
                };
                _labelReport.DataSource = labelPrintings;
                await _labelReport.CreateDocumentAsync();
                return _labelReport;
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<LabelReport> ChangeSizeLabelReport(int width, int height)
        {
            try
            {
                _labelReport.ChangeSize(width, height);
                List<LabelPrinting> labelPrintings = new()
                {
                    new LabelPrinting
                    {
                        Name = "Наименование товара",
                        Barcode = "2000000000002"
                    }
                };
                _labelReport.DataSource = labelPrintings;
                await _labelReport.CreateDocumentAsync();
                return _labelReport;
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<BalancesAndAvailabilityProducts> CreateBalancesAndAvailabilityProducts()
        {
            try
            {
                _balancesAndAvailabilityProducts.DataSource = await _productService.Report();
                await _balancesAndAvailabilityProducts.CreateDocumentAsync();
                return _balancesAndAvailabilityProducts;
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        #endregion
    }
}
