using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Properties;
using RetailTradeClient.State.ProductSales;
using RetailTradeClient.State.Reports;
using RetailTradeClient.State.Shifts;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RetailTradeClient.Commands
{
    public class MakeComplexPaymentCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly IReceiptService _receiptService;
        private readonly IShiftStore _shiftStore;
        private readonly IReportService _reportService;
        private readonly IProductSaleStore _productSaleStore;

        #endregion

        #region Constructor

        public MakeComplexPaymentCommand(IReceiptService receiptService,
            IShiftStore shiftStore,
            IReportService reportService,
            IProductSaleStore productSaleStore)
        {
            _receiptService = receiptService;
            _shiftStore = shiftStore;
            _reportService = reportService;
            _productSaleStore = productSaleStore;
        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if (_productSaleStore.Change == 0)
            {
                try
                {
                    Receipt newReceipt;
                    //Создания чека
                    newReceipt = await _receiptService.CreateAsync(new Receipt
                    {
                        DateOfPurchase = DateTime.Now,                        
                        PaidInCash = _productSaleStore.PaymentTypes.Where(pt => pt.Id == 1).Sum(pt => pt.Sum),
                        PaidInCashless = _productSaleStore.PaymentTypes.Where(pt => pt.Id == 2).Sum(pt => pt.Sum),
                        ShiftId = _shiftStore.CurrentShift.Id,
                        Change = 0,
                        ProductSales = _productSaleStore.Sales.Select(s =>
                            new ProductSale
                            {
                                ProductId = s.Id,
                                Quantity = s.Quantity,
                                Total = s.Total,
                                SalePrice = s.SalePrice,
                                ArrivalPrice = s.ArrivalPrice
                            }).ToList()
                    }, Settings.Default.IsKeepRecords);

                    //Подготовка документа для печати чека
                    //ProductSaleReport report = await _reportService.CreateProductSaleReport(newReceipt);

                    ////Подготовка принтера
                    //PrintToolBase tool = new(report.PrintingSystem);
                    //tool.PrinterSettings.PrinterName = Settings.Default.DefaultReceiptPrinter;
                    //tool.PrintingSystem.EndPrint += PrintingSystem_EndPrint;
                    //tool.Print();


                }
                catch (Exception)
                {
                    
                }
            }
            else
            {
                MessageBox.Show("Остаток должен быть 0", "Sale Page", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void PrintingSystem_EndPrint(object sender, EventArgs e)
        {
            
        }
    }
}
