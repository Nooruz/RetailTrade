using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using SalePageServer.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class SaleDashboardView : BaseViewModel
    {
        #region Private Members

        private readonly IReceiptService _receiptService;
        private readonly IProductSaleService _productSaleService;
        private decimal _saleAmountToday;
        private decimal _saleAmountYesterday;
        private decimal _saleAmountLastWeek;
        private decimal _saleAmountCurrentMonth;
        private decimal _saleAmountLastMonth;
        private decimal _saleAmountBeginningYear;
        private ObservableQueue<Receipt> _dailySalesChart;
        private ObservableQueue<Receipt> _monthlySalesChart;
        private ObservableQueue<ProductSale> _ratingTenProducts;

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
        public IEnumerable<Receipt> DailySalesChart => _dailySalesChart;
        public IEnumerable<Receipt> MonthlySalesChart => _monthlySalesChart;
        public IEnumerable<ProductSale> RatingTenProducts => _ratingTenProducts;

        #endregion

        #region Commands

        public ICommand UserControlCommand { get; }
        public ICommand YesterdayCommand { get; }
        public ICommand TodayCommand { get; }
        public ICommand LastMonthCommand { get; }
        public ICommand CurrentMonthCommand { get; }

        #endregion

        #region Constructor

        public SaleDashboardView(IReceiptService receiptService,
            IProductSaleService productSaleService)
        {
            _receiptService = receiptService;
            _productSaleService = productSaleService;

            UserControlCommand = new RelayCommand(UserControl);
            YesterdayCommand = new RelayCommand(Yesterday);
            TodayCommand = new RelayCommand(Today);
            LastMonthCommand = new RelayCommand(LastMonth);
            CurrentMonthCommand = new RelayCommand(CurrentMonth);
        }

        #endregion

        #region Private Vodis

        private async void UserControl()
        {
            //SaleAmountToday = await _receiptService.GetSaleAmoundToday();
            //SaleAmountYesterday = await _receiptService.GetSaleAmoundYesterday();
            //SaleAmountLastWeek = await _receiptService.GetSaleAmoundLastWeek();
            //SaleAmountCurrentMonth = await _receiptService.GetSaleAmoundCurrentMonth();
            //SaleAmountLastMonth = await _receiptService.GetSaleAmoundLastMonth();
            //SaleAmountBeginningYear = await _receiptService.GetSaleAmoundBeginningYear();
            //_dailySalesChart = new(await _receiptService.GetSaleAmoundToday());
            //_monthlySalesChart = new(await _receiptService.GetSaleAmoundCurrentMonth());
            //_ratingTenProducts = new(await _productSaleService.GetRatingTenProducts());
        }

        private async void Yesterday()
        {
            _dailySalesChart.Dequeue();
            //_dailySalesChart = new(await _receiptService.GetSaleAmoundYesterday());
        }
        private async void Today()
        {
            _dailySalesChart.Dequeue();
            //_dailySalesChart = new(await _receiptService.GetSaleAmoundToday());
        }

        private async void LastMonth()
        {
            _monthlySalesChart.Dequeue();
            //_monthlySalesChart = new(await _receiptService.GetSaleAmoundLastMonth());
        }

        private async void CurrentMonth()
        {
            _monthlySalesChart.Dequeue();
            //_monthlySalesChart = new(await _receiptService.GetSaleAmoundCurrentMonth());
        }

        #endregion
    }

    public class DataSeries
    {
        public string Name { get; set; }
        public ObservableCollection<Receipt> Values { get; set; }
    }

    public class DataRating
    {
        public string Name { get; set; }
        public ObservableCollection<ProductSale> Value { get; set; }
    }
}