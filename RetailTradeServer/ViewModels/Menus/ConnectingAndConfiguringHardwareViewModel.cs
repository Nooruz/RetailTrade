using DevExpress.Mvvm;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using SalePageServer.Properties;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ConnectingAndConfiguringHardwareViewModel : BaseViewModel
    {
        #region Private Members

        private IEnumerable<TypeEquipment> _typeEquipments;
        private string _selectedTypeEquipment = TypeEquipment.BarcodeScanner;

        #endregion

        #region Public Properties

        public string SelectedTypeEquipment
        {
            get => _selectedTypeEquipment;
            set
            {
                _selectedTypeEquipment = value;
                OnPropertyChanged(nameof(SelectedTypeEquipment));
            }
        }
        public IEnumerable<string> TypeEquipments => Settings.Default.TypeEquipment.Cast<string>().ToList();

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        public ICommand CreateEquipmentCommand => new RelayCommand(CreateEquipment);

        #endregion

        #region Constructor

        public ConnectingAndConfiguringHardwareViewModel()
        {
            Header = "Подключение и настройка оборудования";
        }

        #endregion

        #region Private Voids

        private void CreateEquipment()
        {
            if (!string.IsNullOrEmpty(SelectedTypeEquipment))
            {
                WindowService.Show(nameof(EquipmentView), new EquipmentViewModel() { SelectedTypeEquipment = SelectedTypeEquipment });
            }
        }

        private async void UserControlLoaded()
        {
            ShowLoadingPanel = false;
        }

        #endregion
    }
}
