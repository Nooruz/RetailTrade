using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System.Collections.Generic;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ConnectingAndConfiguringHardwareViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IDataService<TypeEquipment> _typeEquipmentService;
        private IEnumerable<TypeEquipment> _typeEquipments;
        private int _selectedTypeEquipmentId = 1;

        #endregion

        #region Public Properties

        public int SelectedTypeEquipmentId
        {
            get => _selectedTypeEquipmentId;
            set
            {
                _selectedTypeEquipmentId = value;
                OnPropertyChanged(nameof(SelectedTypeEquipmentId));
            }
        }
        public IEnumerable<TypeEquipment> TypeEquipments
        {
            get => _typeEquipments;
            set
            {
                _typeEquipments = value;
                OnPropertyChanged(nameof(TypeEquipments));
            }
        }

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        //public ICommand AddEquipmentCommand => new RelayCommand(AddEquipment);
        public ICommand CreateEquipmentCommand => new RelayCommand(CreateEquipment);

        #endregion

        #region Constructor

        public ConnectingAndConfiguringHardwareViewModel(IDataService<TypeEquipment> typeEquipmentService)
        {
            _typeEquipmentService = typeEquipmentService;

            Header = "Подключение и настройка оборудования";
        }

        #endregion

        #region Private Voids

        private void CreateEquipment()
        {
            if (SelectedTypeEquipmentId != 0)
            {
                WindowService.Show(nameof(BarcodeScannerView), new EquipmentViewModel() { TypeEquipments = TypeEquipments, SelectedTypeEquipmentId = SelectedTypeEquipmentId });
            }
        }

        private async void UserControlLoaded()
        {
            TypeEquipments = await _typeEquipmentService.GetAllAsync();
            ShowLoadingPanel = false;
        }

        #endregion
    }
}
