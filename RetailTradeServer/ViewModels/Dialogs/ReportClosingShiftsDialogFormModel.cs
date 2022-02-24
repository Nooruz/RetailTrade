using DevExpress.Mvvm;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class ReportClosingShiftsDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IShiftService _shiftService;
        private DateTime _selectedStartDate = DateTime.Now.Date.AddDays(-30);
        private DateTime _selectedEndDate = DateTime.Now.Date;

        #endregion

        #region Public Properties

        public DateTime SelectedStartDate
        {
            get => _selectedStartDate;
            set
            {
                _selectedStartDate = value;
                OnPropertyChanged(nameof(SelectedStartDate));
            }
        }

        public DateTime SelectedEndDate
        {
            get => _selectedEndDate;
            set
            {
                _selectedEndDate = value;
                OnPropertyChanged(nameof(SelectedEndDate));
            }
        }

        #endregion

        #region Commands

        public ICommand PrintClosingShiftsCommand => new RelayCommand(PrintClosingShifts);

        #endregion

        #region Constructor

        public ReportClosingShiftsDialogFormModel(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        #endregion

        #region Private Voids

        private async void PrintClosingShifts()
        {
            ClosingShiftsReport closingShiftsReport = new(SelectedStartDate, SelectedEndDate)
            {
                DataSource = await _shiftService.GetClosingShifts(SelectedStartDate, SelectedEndDate)
            };

            await closingShiftsReport.CreateDocumentAsync();

            DocumentViewerService.Show(nameof(DocumentViewerView), new DocumentViewerViewModel { PrintingDocument = closingShiftsReport });
        }

        #endregion
    }
}
