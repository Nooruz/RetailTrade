using DevExpress.DashboardCommon;
using DevExpress.DashboardWpf;
using DevExpress.DataAccess;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using System;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class AnalyticalPanelViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IReceiptService _receiptService;
        private DateTime _selectedDate = DateTime.Now.Date;

        #endregion

        #region Public Properties

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                GetAllReceipts(SelectedDate);
            }
        }
        public RetailTradeDashboard RetailTradeDashboard { get; set; }
        public DashboardControl RetailTradeDashboardControl { get; set; }

        #endregion

        #region Commands

        public ICommand LoadedDashboardControlCommand { get; }

        #endregion

        #region Constructor

        public AnalyticalPanelViewModel(IReceiptService receiptService)
        {
            _receiptService = receiptService;
            RetailTradeDashboard = new RetailTradeDashboard();
            LoadedDashboardControlCommand = new ParameterCommand(parameter => LoadedDashboardControl(parameter));
            GetAllReceipts(DateTime.Now.Date);
        }

        #endregion

        #region Private Vodis

        private async void GetAllReceipts(DateTime dateTime)
        {
            RetailTradeDashboard.ReceiptDataSource.DataSource = await _receiptService.GetReceiptsByDateAsync(dateTime);
            RetailTradeDashboardControl?.ReloadData();
        }

        private void LoadedDashboardControl(object parameter)
        {
            if (parameter is RoutedEventArgs e)
            {
                if (e.Source is DashboardControl sender)
                {
                    RetailTradeDashboardControl = sender;                    
                }
            }
        }

        #endregion
    }
}