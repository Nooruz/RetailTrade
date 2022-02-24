using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private ObservableCollection<Receipt> _dailySalesChart;
        private ObservableCollection<Receipt> _monthlySalesChart;
        private ObservableCollection<ProductSale> _ratingTenProducts;

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
        public ObservableCollection<Receipt> DailySalesChart
        {
            get => _dailySalesChart ?? new();
            set
            {
                _dailySalesChart = value;
                OnPropertyChanged(nameof(DailySalesChart));
            }
        }
        public ObservableCollection<Receipt> MonthlySalesChart
        {
            get => _monthlySalesChart ?? new();
            set
            {
                _monthlySalesChart = value;
                OnPropertyChanged(nameof(MonthlySalesChart));
            }
        }
        public ObservableCollection<ProductSale> RatingTenProducts
        {
            get => _ratingTenProducts ?? new();
            set
            {
                _ratingTenProducts = value;
                OnPropertyChanged(nameof(RatingTenProducts));
            }
        }

        #endregion

        #region Commands

        public ICommand UserControlCommand => new RelayCommand(UserControl);
        public ICommand YesterdayCommand => new RelayCommand(Yesterday);
        public ICommand TodayCommand => new RelayCommand(Today);
        public ICommand LastMonthCommand => new RelayCommand(LastMonth);
        public ICommand CurrentMonthCommand => new RelayCommand(CurrentMonth);

        #endregion

        #region Constructor

        public SaleDashboardView(IReceiptService receiptService,
            IProductSaleService productSaleService)
        {
            _receiptService = receiptService;
            _productSaleService = productSaleService;

            AllowHide = false;
            Header = "Панель мониторинга продаж и доходов";
        }

        #endregion

        #region Private Vodis

        private async void UserControl()
        {
            DateTime mondayOfLastWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek - 6);

            DailySalesChart = new(await _receiptService.GetSaleAmoundToday());
            MonthlySalesChart = new(await _receiptService.GetSaleAmoundCurrentMonth());
            RatingTenProducts = new(await _productSaleService.GetRatingTenProducts());
            SaleAmountToday = DailySalesChart.Sum(s => s.Sum);
            SaleAmountYesterday = MonthlySalesChart.Where(s => s.DateOfPurchase.Date == DateTime.Now.Date.AddDays(-1)).Sum(s => s.Sum);
            SaleAmountLastWeek = MonthlySalesChart.Where(r => r.DateOfPurchase.Date >= mondayOfLastWeek && r.DateOfPurchase <= mondayOfLastWeek.AddDays(6)).Sum(s => s.Sum);
            SaleAmountCurrentMonth = MonthlySalesChart.Sum(s => s.Sum);
            SaleAmountLastMonth = await _receiptService.GetSaleAmoundLastMonth();
            SaleAmountBeginningYear = await _receiptService.GetSaleAmoundBeginningYear();
            ShowLoadingPanel = false;
        }

        private async void Yesterday()
        {

        }
        private async void Today()
        {
        }

        private async void LastMonth()
        {
        }

        private async void CurrentMonth()
        {
        }

        #endregion
    }
}