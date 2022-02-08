using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using SalePageServer.State.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class SettingProductPriceViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
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

        public SettingProductPriceViewModel(IProductService productService,
            ITypeProductService typeProductService,
            IDialogService dialogService, 
            IDataService<Unit> unitService)
        {
            _productService = productService;
            _typeProductService = typeProductService;
            _dialogService = dialogService;
            _unitService = unitService;
        }

        #endregion

        #region Private Voids

        private void Create()
        {
            _dialogService.ShowDialog(new ProductDialogFormModel(_productService, _typeProductService));
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
