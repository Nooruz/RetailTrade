using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class WareHouseViewModel : BaseViewModel
    {
        #region Private Members

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

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        public ICommand CreateWareHouseCommand => new RelayCommand(() => WindowService.Show(nameof(WareHouseDialogForm), new WareHouseDialogFormModel(_wareHouseService) { Title = "Склад (создание)" }));

        #endregion

        #region Constructor

        public WareHouseViewModel(IWareHouseService wareHouseService,
            IDataService<TypeWareHouse> typeWareHouseService)
        {
            _wareHouseService = wareHouseService;
            _typeWareHouseService = typeWareHouseService;

            Header = "Склады и магазины";

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
