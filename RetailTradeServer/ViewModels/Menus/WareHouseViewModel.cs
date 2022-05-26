using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace RetailTradeServer.ViewModels.Menus
{
    public class WareHouseViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IWareHouseService _wareHouseService;
        private readonly IDataService<TypeWareHouse> _typeWareHouseService;
        private IEnumerable<TypeWareHouse> _typeWareHouses;
        private ObservableCollection<WareHouse> _wareHouses;
        private WareHouse _selectedWareHouse;

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
        public WareHouse SelectedWareHouse
        {
            get => _selectedWareHouse;
            set
            {
                _selectedWareHouse = value;
                OnPropertyChanged(nameof(SelectedWareHouse));
            }
        }

        #endregion

        #region Constructor

        public WareHouseViewModel(IWareHouseService wareHouseService,
            IDataService<TypeWareHouse> typeWareHouseService)
        {
            _wareHouseService = wareHouseService;
            _typeWareHouseService = typeWareHouseService;

            Header = "Склады и магазины";

            EditCommand = new RelayCommand(Edit);

            _wareHouseService.OnWareHouseCreated += WareHouseService_OnWareHouseCreated;
            _wareHouseService.OnWareHouseEdited += WareHouseService_OnWareHouseEdited;
        }

        #endregion

        #region Private Voids

        private void WareHouseService_OnWareHouseCreated(WareHouse wareHouse)
        {
            WareHouses.Add(wareHouse);
        }
        private void WareHouseService_OnWareHouseEdited(WareHouse wareHouse)
        {
            if (SelectedWareHouse != null)
            {
                SelectedWareHouse.Name = wareHouse.Name;
                SelectedWareHouse.Address = wareHouse.Address;
                SelectedWareHouse.TypeWareHouseId = wareHouse.TypeWareHouseId;
                SelectedWareHouse.DeleteMark = wareHouse.DeleteMark;
            }
        }
        private void Edit()
        {
            if (SelectedWareHouse != null)
            {
                WindowService.Show(nameof(WareHouseDialogForm), new WareHouseDialogFormModel(_wareHouseService) { Title = $"Склад ({SelectedWareHouse.Name})", EditableWareHouse = SelectedWareHouse, IsEditMode = true });
            }
        }

        #endregion

        #region Public Properties

        [Command]
        public async void UserControlLoaded()
        {
            TypeWareHouses = await _typeWareHouseService.GetAllAsync();
            WareHouses = new(await _wareHouseService.GetAllAsync());
            ShowLoadingPanel = false;
        }

        [Command]
        public void CreateWareHouse()
        {
            WindowService.Show(nameof(WareHouseDialogForm), new WareHouseDialogFormModel(_wareHouseService) { TypeWareHouseId = 1 });
        }

        [Command]
        public void CreateRetailTrade()
        {
            WindowService.Show(nameof(WareHouseDialogForm), new WareHouseDialogFormModel(_wareHouseService) { TypeWareHouseId = 2 });
        }

        [Command]
        public async void DeleteMarkingProduct()
        {
            if (SelectedWareHouse != null)
            {
                if (MessageBoxService.Show(SelectedWareHouse.DeleteMark ? $"Снять пометку \"{SelectedWareHouse.Name}\"?" : $"Пометить \"{SelectedWareHouse.Name}\" на удаление?", "Sale Page", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _wareHouseService.MarkingForDeletion(SelectedWareHouse);
                }
            }
            else
            {
                _ = MessageBoxService.Show("Выберите склад или розничный магазин", "Sale Page", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
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
