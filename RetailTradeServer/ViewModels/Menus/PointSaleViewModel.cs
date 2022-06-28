using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Factories;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class PointSaleViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IMenuNavigator _menuNavigator;
        private readonly IMenuViewModelFactory _menuViewModelFactory;
        private readonly IPointSaleService _pointSaleService;
        private ObservableCollection<PointSale> _pointSales = new();
        private PointSale _selectedpointSale;

        #endregion

        #region Public Properties

        public ObservableCollection<PointSale> PointSales
        {
            get => _pointSales;
            set
            {
                _pointSales = value;
                OnPropertyChanged(nameof(PointSales));
            }
        }
        public PointSale SelectedPointSale
        {
            get => _selectedpointSale;
            set
            {
                _selectedpointSale = value;
                OnPropertyChanged(nameof(SelectedPointSale));
            }
        }

        #endregion

        #region Commands

        public ICommand UpdateCurrentMenuViewModelCommand => new UpdateCurrentMenuViewModelCommand(_menuNavigator, _menuViewModelFactory);

        #endregion

        #region Constructor

        public PointSaleViewModel(IMenuNavigator menuNavigator,
            IMenuViewModelFactory menuViewModelFactory,
            IPointSaleService pointSaleService)
        {
            _menuNavigator = menuNavigator;
            _menuViewModelFactory = menuViewModelFactory;
            _pointSaleService = pointSaleService;

            Header = "Точки продажи";

            CreateCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.CreatePointSale));

            _pointSaleService.OnCreated += PointSaleService_OnCreated;
        }

        #endregion

        #region Private Voids

        private void PointSaleService_OnCreated(PointSale pointSale)
        {
            PointSales.Add(pointSale);
        }

        #endregion

        #region Public Voids

        [Command]
        public async void UserControlLoaded()
        {
            PointSales = new(await _pointSaleService.GetAllAsync());
            ShowLoadingPanel = false;
        }

        #endregion
    }
}
