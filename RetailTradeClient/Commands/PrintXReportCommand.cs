using DevExpress.XtraPrinting;
using RetailTradeClient.Report;
using RetailTradeClient.State.Shifts;
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

        private readonly IShiftStore _shiftStore;
        private int _userId;

        #endregion

        #region Constructor

        public PrintXReportCommand()
        {

        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            XReport report = new();
            await report.CreateDocumentAsync();

            PrintToolBase tool = new(report.PrintingSystem);
            tool.PrinterSettings.PrinterName = Properties.Settings.Default.DefaultReceiptPrinter;
            tool.PrintingSystem.EndPrint += PrintingSystem_EndPrint;
            tool.Print();
        }

        private void PrintingSystem_EndPrint(object sender, EventArgs e)
        {
            //_manager.Close();
            //_viewModel.Result = true;
        }
    }
}
