using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Linq;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class ReportRevenueForPeriodDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IReceiptService _receiptService;
        private readonly IUserStore _userStore;
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

        public ICommand PrintRevenueForPeriodCommand => new RelayCommand(PrintRevenueForPeriod);

        #endregion

        #region Constructor

        public ReportRevenueForPeriodDialogFormModel(IReceiptService receiptService,
            IUserStore userStore)
        {
            _receiptService = receiptService;
            _userStore = userStore;

        }

        #endregion

        #region Private Voids

        private async void PrintRevenueForPeriod()
        {
            RevenueForPeriodReport revenueForPeriodReport = new(_userStore.CurrentOrganization, SelectedStartDate, SelectedEndDate)
            {
                
            };

            await revenueForPeriodReport.CreateDocumentAsync();

            DocumentViewerService.Show(nameof(DocumentViewerView), new DocumentViewerViewModel() { PrintingDocument = revenueForPeriodReport });

        }

        #endregion
    }
}
