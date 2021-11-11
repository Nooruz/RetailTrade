using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class ReportClosingShiftsDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IUIManager _manager;
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

        public ICommand PrintClosingShiftsCommand { get; }

        #endregion

        #region Constructor

        public ReportClosingShiftsDialogFormModel(IUIManager manager,
            IShiftService shiftService)
        {
            _manager = manager;
            _shiftService = shiftService;

            PrintClosingShiftsCommand = new RelayCommand(PrintClosingShifts);
        }

        #endregion

        #region Private Voids

        private async void PrintClosingShifts()
        {
            ClosingShiftsReport closingShiftsReport = new()
            {
                DataSource = await _shiftService.GetClosingShifts(SelectedStartDate, SelectedEndDate)
            };

            closingShiftsReport.StartDate.Value = SelectedStartDate;
            closingShiftsReport.EndDate.Value = SelectedEndDate;

            await closingShiftsReport.CreateDocumentAsync();

            await _manager.ShowDialog(new DocumentViewerViewModel() { PrintingDocument = closingShiftsReport }, 
                new DocumentViewerView(),
                WindowState.Maximized,
                ResizeMode.CanResize,
                SizeToContent.Manual);
        }

        #endregion
    }
}
