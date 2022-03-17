using DevExpress.XtraPrinting;
using RetailTradeClient.Report;
using RetailTradeClient.State.Reports;
using System;
using System.Threading.Tasks;

namespace RetailTradeClient.Commands
{
    /// <summary>
    /// Распечатать х-отчет
    /// </summary>
    public class PrintXReportCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly IReportService _reportService;

        #endregion

        #region Constructor

        public PrintXReportCommand()
        {
            //_reportService = new ReportService();
        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                XReport xReport = await _reportService.CreateXReport();
                PrintToolBase tool = new(xReport.PrintingSystem);
                tool.PrinterSettings.PrinterName = Properties.Settings.Default.DefaultReceiptPrinter;
                tool.PrintingSystem.EndPrint += PrintingSystem_EndPrint;
                tool.Print();
            }
            catch (Exception)
            {
                //ignore
            }            
        }

        private void PrintingSystem_EndPrint(object sender, EventArgs e)
        {
            //_manager.Close();
            //_viewModel.Result = true;
        }
    }
}
