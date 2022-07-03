using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Factories;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class PointSaleViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IMenuNavigator _menuNavigator;
        private readonly IMenuViewModelFactory _menuViewModelFactory;
        private readonly IPointSaleService _pointSaleService;
        private readonly IMessageStore _messageStore;
        private readonly IWareHouseService _wareHouseService;
        private readonly IUserService _userService;
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
        public GridControl PointSaleGridControl { get; set; }

        #endregion

        #region Commands

        public ICommand UpdateCurrentMenuViewModelCommand => new UpdateCurrentMenuViewModelCommand(_menuNavigator, _menuViewModelFactory);

        #endregion

        #region Constructor

        public PointSaleViewModel(IMenuNavigator menuNavigator,
            IMenuViewModelFactory menuViewModelFactory,
            IPointSaleService pointSaleService,
            IMessageStore messageStore,
            IWareHouseService wareHouseService,
            IUserService userService)
        {
            _menuNavigator = menuNavigator;
            _menuViewModelFactory = menuViewModelFactory;
            _pointSaleService = pointSaleService;
            _messageStore = messageStore;
            _wareHouseService = wareHouseService;
            _userService = userService;

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

        private async void TableView_RowDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            try
            {
                _menuNavigator.CurrentViewModel = new CreatePointSaleViewModel(_pointSaleService, _messageStore, _wareHouseService, _userService)
                {
                    CreatedPoitnSale = await _pointSaleService.GetPointSaleUserAsync(SelectedPointSale.Id)
                };
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion

        #region Public Voids

        [Command]
        public async void UserControlLoaded()
        {
            PointSales = new(await _pointSaleService.GetAllAsync());
            ShowLoadingPanel = false;
        }

        [Command]
        public void PointSaleGridControlLoaded(object sender)
        {
            if (sender is RoutedEventArgs e)
            {
                if (e.Source is GridControl gridControl)
                {
                    PointSaleGridControl = gridControl;
                    TableView tableView = PointSaleGridControl.View as TableView;
                    tableView.RowDoubleClick += TableView_RowDoubleClick;
                }
            }
        }

        #endregion
    }
}
