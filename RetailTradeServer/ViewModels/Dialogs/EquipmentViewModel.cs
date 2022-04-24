using DevExpress.Mvvm;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Properties;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class TypeEquipment
    {
        public static string BarcodeScanner => "Сканер штрихкода";
        public static string LablePrinter => "Принтер этикеток";
    }

    public class EquipmentViewModel : BaseDialogViewModel
    {
        #region Private Members

        private string _selectedTypeEquipment;
        private string _selectedComPort = Settings.Default.BarcodeCom;
        private int _selectedBarcodeSpeed = Settings.Default.BarcodeSpeed;
        private string _selectedPrinter = Settings.Default.DefaultLabelPrinter;

        #endregion

        #region Public Properties

        public IEnumerable<string> TypeEquipments => Settings.Default.TypeEquipment.Cast<string>().ToList();
        public IEnumerable<string> ComPorts => SerialPort.GetPortNames();
        public IEnumerable<int> BarcodeSpeed => new List<int>() { 2400, 4800, 7200, 9600, 14400, 19200, 38400 };
        public IEnumerable<string> LocalPrinters => PrinterSettings.InstalledPrinters.Cast<string>().ToList();
        public Visibility BarcodeSettings => SelectedTypeEquipment == TypeEquipment.BarcodeScanner ? Visibility.Visible : Visibility.Collapsed;
        public Visibility LabelPrinterSettings => SelectedTypeEquipment == TypeEquipment.LablePrinter ? Visibility.Visible : Visibility.Collapsed;
        public string SelectedTypeEquipment
        {
            get => _selectedTypeEquipment;
            set
            {
                _selectedTypeEquipment = value;
                OnPropertyChanged(nameof(SelectedTypeEquipment));
                OnPropertyChanged(nameof(BarcodeSettings));
                OnPropertyChanged(nameof(LabelPrinterSettings));
            }
        }
        public string SelectedComPort
        {
            get => _selectedComPort;
            set
            {
                _selectedComPort = value;
                OnPropertyChanged(nameof(SelectedComPort));
            }
        }
        public int SelectedBarcodeSpeed
        {
            get => _selectedBarcodeSpeed;
            set
            {
                _selectedBarcodeSpeed = value;
                OnPropertyChanged(nameof(SelectedBarcodeSpeed));
            }
        }
        public string SelectedPrinter
        {
            get => _selectedPrinter;
            set
            {
                _selectedPrinter = value;
                OnPropertyChanged(nameof(SelectedPrinter));
            }
        }

        #endregion

        #region Commands

        public ICommand CreateCommand => new RelayCommand(Create);

        #endregion

        #region Constructor

        public EquipmentViewModel()
        {

        }

        #endregion

        #region Private Voids

        private void Create()
        {
            if (SelectedTypeEquipment == TypeEquipment.BarcodeScanner)
            {
                if (!string.IsNullOrEmpty(SelectedComPort))
                {
                    Settings.Default.BarcodeCom = SelectedComPort;
                    Settings.Default.BarcodeSpeed = SelectedBarcodeSpeed;
                    Settings.Default.Save();
                    CurrentWindowService.Close();
                }
                else
                {
                    _ = MessageBoxService.ShowMessage("Выберите СОМ порт!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(SelectedPrinter))
                {
                    Settings.Default.DefaultLabelPrinter = SelectedPrinter;
                    Settings.Default.Save();
                    CurrentWindowService.Close();
                }
                else
                {
                    _ = MessageBoxService.ShowMessage("Выберите принтер!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
                }
            }
        }

        #endregion
    }
}
