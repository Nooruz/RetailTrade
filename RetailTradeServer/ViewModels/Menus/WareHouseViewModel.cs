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
    public class WareHouseViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IDialogService _dialogService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IDataService<TypeWareHouse> _typeWareHouseService;
        private IEnumerable<TypeWareHouse> _typeWareHouses;
        private ObservableCollection<WareHouse> _wareHouses;

        #endregion

        #region Public Properties

        public IEnumerable<TypeWareHouse> TypeWareHouses
        {
            get => _typeWareHouses;
            set
            {
                _typeWareHouses = value;
                OnPropertyChanged(nameof(TypeWareHouses));
            }
        }
        public ObservableCollection<WareHouse> WareHouses
        {
            get => _wareHouses ?? new();
            set
            {
                _wareHouses = value;
                OnPropertyChanged(nameof(WareHouses));
            }
        }

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand { get; }
        public ICommand CreateWareHouseCommand { get; }

        #endregion

        #region Constructor

        public WareHouseViewModel(IDialogService dialogService,
            IWareHouseService wareHouseService,
            IDataService<TypeWareHouse> typeWareHouseService)
        {
            _dialogService = dialogService;
            _wareHouseService = wareHouseService;
            _typeWareHouseService = typeWareHouseService;

            Header = "Склады и магазины";

            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);
            CreateWareHouseCommand = new RelayCommand(() => _dialogService.ShowDialog(new WareHouseDialogFormModel(wareHouseService) { Title = "Склад (создание)" }));

            _wareHouseService.OnWareHouseCreated += WareHouseService_OnWareHouseCreated;
        }

        #endregion

        #region Private Voids

        private async void UserControlLoaded()
        {
            TypeWareHouses = await _typeWareHouseService.GetAllAsync();
            WareHouses = new(await _wareHouseService.GetAllAsync());
            ShowLoadingPanel = false;
        }

        private void WareHouseService_OnWareHouseCreated(WareHouse obj)
        {
            WareHouses.Add(obj);
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            _wareHouseService.OnWareHouseCreated -= WareHouseService_OnWareHouseCreated;
            base.Dispose();
        }

        #endregion
    }
}
