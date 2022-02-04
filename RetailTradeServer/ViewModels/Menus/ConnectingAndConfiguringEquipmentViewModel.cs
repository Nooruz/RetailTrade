using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using SalePageServer.State.Dialogs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ConnectingAndConfiguringEquipmentViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IDialogService _dialogService;
        private readonly IDataService<TypeEquipment> _typeEquipmentService;
        private IEnumerable<TypeEquipment> _typeEquipments;
        private TypeEquipment _selectedTypeEquipment;

        #endregion

        #region Public Properties

        public TypeEquipment SelectedTypeEquipment
        {
            get => _selectedTypeEquipment;
            set
            {
                _selectedTypeEquipment = value;
                OnPropertyChanged(nameof(SelectedTypeEquipment));
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
        public ICommand AddEquipmentCommand => new RelayCommand(AddEquipment);

        #endregion

        #region Constructor

        public ConnectingAndConfiguringEquipmentViewModel(IDataService<TypeEquipment> typeEquipmentService,
            IDialogService dialogService)
        {
            _typeEquipmentService = typeEquipmentService;
            _dialogService = dialogService;
        }

        #endregion

        #region Private Voids

        private async void UserControlLoaded()
        {
            TypeEquipments = await _typeEquipmentService.GetAllAsync();
            ShowLoadingPanel = false;
        }

        private void AddEquipment()
        {
            if (SelectedTypeEquipment != null)
            {
                switch (SelectedTypeEquipment.Id)
                {
                    //Сканеры штрихкода
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                _ = _dialogService.ShowMessage("Выберите тип оборудования!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion
    }
}
