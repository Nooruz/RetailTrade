using DevExpress.Mvvm;
using RetailTrade.Barcode.Services;
using RetailTrade.CashRegisterMachine;
using RetailTrade.Domain.Models;
using RetailTradeClient.Commands;
using RetailTradeClient.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class ApplicationSettingsViewModel : BaseDialogViewModel
    {
        #region Private Members

        private int? _selectedLocalPrinterId;
        private LocalPrinter _selectedReceiptPrinter;
        private string _selectedKKM = Settings.Default.DefaultKKMName;
        private BarcodeDevice _selectedBarcodeDevice = Enum.TryParse(Settings.Default.BarcodeDefaultDevice, out BarcodeDevice barcodeDevice) ? barcodeDevice : BarcodeDevice.Com;

        #endregion

        #region Public Properties

        public int SelectedBarcodeSpeed
        {
            get => Settings.Default.BarcodeSpeed;
            set
            {
                Settings.Default.BarcodeSpeed = value;
                OnPropertyChanged(nameof(SelectedBarcodeSpeed));
            }
        }
        public string SelectedComPort
        {
            get => Settings.Default.BarcodeCom;
            set
            {
                Settings.Default.BarcodeCom = value;
                OnPropertyChanged(nameof(SelectedComPort));
            }
        }
        public IEnumerable<string> ComPorts => SerialPort.GetPortNames();
        public IEnumerable<BarcodeDevice> BarcodeDevices => Enum.GetValues(typeof(BarcodeDevice)).Cast<BarcodeDevice>();
        public IEnumerable<int> BarcodeSpeed => new List<int>() { 2400, 4800, 7200, 9600, 14400, 19200, 38400 };
        public List<string> KKMs { get; set; }
        public string SelectedKKM
        {
            get => _selectedKKM;
            set
            {
                _selectedKKM = value;
                OnPropertyChanged(nameof(SelectedKKM));
            }
        }
        public ObservableCollection<LocalPrinter> LocalPrinters { get; set; }
        public int? SelectedLocalPrinterId
        {
            get => _selectedLocalPrinterId;
            set
            {
                _selectedLocalPrinterId = value;
                OnPropertyChanged(nameof(SelectedLocalPrinterId));
            }
        }
        public LocalPrinter SelectedReceiptPrinter
        {
            get => _selectedReceiptPrinter;
            set
            {
                _selectedReceiptPrinter = value;
                OnPropertyChanged(nameof(SelectedReceiptPrinter));
            }
        }
        public bool IsKeepRecords
        {
            get => Settings.Default.IsKeepRecords;
            set
            {
                Settings.Default.IsKeepRecords = value;
                OnPropertyChanged(nameof(IsKeepRecords));
            }
        }
        public Visibility ComSettingsVisibility => SelectedBarcodeDevice == BarcodeDevice.Com ? Visibility.Visible : Visibility.Collapsed;
        public BarcodeDevice SelectedBarcodeDevice
        {
            get => _selectedBarcodeDevice;
            set
            {
                _selectedBarcodeDevice = value;
                OnPropertyChanged(nameof(SelectedBarcodeDevice));
                OnPropertyChanged(nameof(ComSettingsVisibility));
            }
        }

        #endregion

        #region Commands

        public ICommand SaveCommand { get; }
        public ICommand SettingKKMCommand => new RelayCommand(SettingKKM);

        #endregion

        #region Constructor

        public ApplicationSettingsViewModel()
        {
            LocalPrinters = new();
            UpdateLocalPrinterList();
            KKMs = new List<string>()
            {
                "Штрих-М",
                "ОКА МФ2"
            };
            SaveCommand = new RelayCommand(Save);

            if (LocalPrinters.Count > 0)
            {
                SelectedReceiptPrinter = LocalPrinters.FirstOrDefault(lp => lp.Name == Settings.Default.DefaultReceiptPrinter);
            }

        }

        #endregion

        #region Private Voids

        private void SettingKKM()
        {
            if (string.IsNullOrEmpty(SelectedKKM))
            {
                MessageBoxService.ShowMessage("Выберите ККМ", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
            }
            else
            {
                switch (SelectedKKM)
                {
                    case "Штрих-М":
                        ShtrihM.ShowProperties();
                        break;
                    case "ОКА МФ2":
                        MessageBoxService.ShowMessage("Ошибка. Обратитесь к разработчику.", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
                        break;
                    default:
                        break;
                }
            }
        }

        private void Save()
        {
            Settings.Default.DefaultReceiptPrinter = SelectedReceiptPrinter?.Name;
            Settings.Default.DefaultKKMName = SelectedKKM;
            Settings.Default.BarcodeDefaultDevice = SelectedBarcodeDevice.ToString();
            Settings.Default.Save();
            CurrentWindowService.Close();
        }

        private void UpdateLocalPrinterList()
        {
            LocalPrinters.Clear();
            var printers = PrinterSettings.InstalledPrinters;

            for (int i = 0; i < printers.Count; i++)
            {
                LocalPrinters.Add(new LocalPrinter
                {
                    Id = i,
                    Name = printers[i].ToString()
                });
            }
        }

        #endregion
    }
}
