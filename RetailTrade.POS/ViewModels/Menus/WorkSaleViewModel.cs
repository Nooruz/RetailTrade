using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using System.Collections.Generic;
using System.Linq;

namespace RetailTrade.POS.ViewModels.Menus
{
    public class WorkSaleViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IPointSaleService _pointSaleService;
        private IEnumerable<PointSale> _pointSales;

        #endregion

        #region Public Properties

        public IEnumerable<PointSale> PointSales
        {
            get => _pointSales;
            set
            {
                _pointSales = value;
                OnPropertyChanged(nameof(PointSales));
            }
        }
        public int SelectedPointSaleId
        {
            get => Properties.Settings.Default.PointSaleId;
            set
            {
                Properties.Settings.Default.PointSaleId = value;
                Properties.Settings.Default.WareHouseId = PointSales.FirstOrDefault(p => p.Id == value).Id;
                Properties.Settings.Default.Save();
                OnPropertyChanged(nameof(SelectedPointSaleId));
            }
        }

        #endregion

        #region Constructor

        public WorkSaleViewModel(IPointSaleService pointSaleService)
        {
            _pointSaleService = pointSaleService;
        }

        #endregion

        #region Public Voids

        [Command]
        public async void UserControlLoaded()
        {
            PointSales = await _pointSaleService.GetAllAsync();
        }

        #endregion
    }
}
