using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using SalePageServer.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class SaleDashboardView : BaseViewModel
    {
        #region Private Members

        private readonly IReceiptService _receiptService;
        private decimal _saleAmountToday;
        private decimal _saleAmountYesterday;
        private decimal _saleAmountLastWeek;
        private decimal _saleAmountCurrentMonth;
        private decimal _saleAmountLastMonth;
        private decimal _saleAmountBeginningYear;
        private ObservableQueue<DataSeries> _dailySalesChart;

        #endregion

        #region Public Properties

        public decimal SaleAmountToday
        {
            get => _saleAmountToday;
            set
            {
                _saleAmountToday = value;
                OnPropertyChanged(nameof(SaleAmountToday));
            }
        }
        public decimal SaleAmountYesterday
        {
            get => _saleAmountYesterday;
            set
            {
                _saleAmountYesterday = value;
                OnPropertyChanged(nameof(SaleAmountYesterday));
            }
        }
        public decimal SaleAmountLastWeek
        {
            get => _saleAmountLastWeek;
            set
            {
                _saleAmountLastWeek = value;
                OnPropertyChanged(nameof(SaleAmountLastWeek));
            }
        }
        public decimal SaleAmountCurrentMonth
        {
            get => _saleAmountCurrentMonth;
            set
            {
                _saleAmountCurrentMonth = value;
                OnPropertyChanged(nameof(SaleAmountCurrentMonth));
            }
        }
        public decimal SaleAmountLastMonth
        {
            get => _saleAmountLastMonth;
            set
            {
                _saleAmountLastMonth = value;
                OnPropertyChanged(nameof(SaleAmountLastMonth));
            }
        }
        public decimal SaleAmountBeginningYear
        {
            get => _saleAmountBeginningYear;
            set
            {
                _saleAmountBeginningYear = value;
                OnPropertyChanged(nameof(SaleAmountBeginningYear));
            }
        }
        public IEnumerable<DataSeries> DailySalesChart => _dailySalesChart;

        #endregion

        #region Commands

        public ICommand UserControlCommand { get; }

        #endregion

        #region Constructor

        public SaleDashboardView(IReceiptService receiptService)
        {
            _receiptService = receiptService;
            UserControlCommand = new RelayCommand(UserControl);

            _dailySalesChart = new();
        }

        #endregion

        #region Private Vodis

        private async void UserControl()
        {
            SaleAmountToday = await _receiptService.GetSaleAmoundToday();
            SaleAmountYesterday = await _receiptService.GetSaleAmoundYesterday();
            SaleAmountLastWeek = await _receiptService.GetSaleAmoundLastWeek();
            SaleAmountCurrentMonth = await _receiptService.GetSaleAmoundCurrentMonth();
            SaleAmountLastMonth = await _receiptService.GetSaleAmoundLastMonth();
            SaleAmountBeginningYear = await _receiptService.GetSaleAmoundBeginningYear();
            _dailySalesChart.Enqueue(new DataSeries
            {
                Name = "New",
                Values = new(await _receiptService.GetAllAsync())
            });
        }

        #endregion
    }

    public class DataSeries
    {
        public string Name { get; set; }
        public ObservableCollection<Receipt> Values { get; set; }
    }
}