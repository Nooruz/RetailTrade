using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using SalePageServer.State.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class SettingProductPriceViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IDialogService _dialogService;
        private readonly IDataService<Unit> _unitService;
        private ObservableCollection<RevaluationProduct> _revaluationProducts = new();
        private IEnumerable<Unit> _units;

        #endregion

        #region Public Properties

        public ObservableCollection<RevaluationProduct> RevaluationProducts
        {
            get => _revaluationProducts;
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
        public ICommand AddProductCommand => new RelayCommand(AddProduct);
        public ICommand ProductCommand => new RelayCommand(Create);

        #endregion

        #region Constructor

        public SettingProductPriceViewModel(IDialogService dialogService, 
            IDataService<Unit> unitService)
        {
            _dialogService = dialogService;
            _unitService = unitService;
        }

        #endregion

        #region Private Voids

        private void Create()
        {
            //_dialogService.ShowDialog(new Produc);
        }

        private void AddProduct()
        {
            RevaluationProducts.Add(new RevaluationProduct());
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
