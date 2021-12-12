using RetailTrade.Domain.Models;
using RetailTradeClient.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class ApplicationSettingsViewModel : BaseDialogViewModel
    {
        #region Private Members

        private int? _selectedLocalPrinterId;
        private LocalPrinter _selectedLocalPrinter;

        #endregion

        #region Public Properties

        public List<string> KKMs { get; set; }

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
        public LocalPrinter SelectedLocalPrinter
        {
            get => _selectedLocalPrinter;
            set
            {
                _selectedLocalPrinter = value;
                OnPropertyChanged(nameof(SelectedLocalPrinter));
            }
        }

        #endregion

        #region Commands

        public ICommand SaveCommand { get; }

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
        }

        #endregion

        #region Private Voids

        private void Save()
        {

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
