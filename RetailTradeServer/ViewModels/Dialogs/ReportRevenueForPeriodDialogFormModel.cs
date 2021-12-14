using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using SalePageServer.State.Dialogs;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class ReportRevenueForPeriodDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IDialogService _dialogService;
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

        public ICommand PrintRevenueForPeriodCommand { get; }

        #endregion

        #region Constructor

        public ReportRevenueForPeriodDialogFormModel(IDialogService dialogService,
            IReceiptService receiptService,
            IUserStore userStore)
        {
            _dialogService = dialogService;
            _receiptService = receiptService;
            _userStore = userStore;

            PrintRevenueForPeriodCommand = new RelayCommand(PrintRevenueForPeriod);
        }

        #endregion

        #region Private Voids

        private async void PrintRevenueForPeriod()
        {
            RevenueForPeriodReport revenueForPeriodReport = new(_userStore.CurrentOrganization, SelectedStartDate, SelectedEndDate)
            {
                DataSource = await _receiptService.Predicate(r => r.DateOfPurchase.Date >= SelectedStartDate && r.DateOfPurchase.Date <= SelectedEndDate,
                    r => new Receipt { Sum = r.Sum, ProductSales =
                    r.ProductSales.Select(p => new ProductSale { ArrivalPrice = p.ArrivalPrice, SalePrice = p.SalePrice, Quantity = p.Quantity }).ToList(),
                        Shift = r.Shift
                    })
            };

            await revenueForPeriodReport.CreateDocumentAsync();

            await _dialogService.ShowDialog(new DocumentViewerViewModel()
            {
                Title = "Закрытие смены",
                PrintingDocument = revenueForPeriodReport
            },
                new DocumentViewerView());
        }

        #endregion
    }
}
