using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class SettingProductPriceViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IDataService<Unit> _unitService;
        private ObservableCollection<RevaluationProduct> _revaluationProducts;
        private IEnumerable<Unit> _units;

        #endregion

        #region Public Properties

        public ObservableCollection<RevaluationProduct> RevaluationProducts
        {
            get => _revaluationProducts ?? new();
            set
            {
                _revaluationProducts = value;
                OnPropertyChanged(nameof(RevaluationProducts));
            }
        }
        public IEnumerable<Unit> Units
        {
            get => _units;
            set
            {
                _units = value;
                OnPropertyChanged(nameof(Units));
            }
        }

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        public ICommand AddProductCommand => new RelayCommand(() => RevaluationProducts.Add(new RevaluationProduct()));

        #endregion

        #region Constructor

        public SettingProductPriceViewModel(IDataService<Unit> unitService)
        {
            _unitService = unitService;
        }

        #endregion

        #region Private Voids

        private void AddProduct()
        {

        }

        private async void UserControlLoaded()
        {
            Units = await _unitService.GetAllAsync();
            ShowLoadingPanel = false;
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            base.Dispose();
        }

        #endregion
    }
}
