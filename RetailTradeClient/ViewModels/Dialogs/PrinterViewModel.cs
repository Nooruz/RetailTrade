using RetailTrade.Domain.Models;
using RetailTradeClient.Commands;
using RetailTradeClient.State.Messages;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class PrinterViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IMessageStore _messageStore;
        private int? _selectedLocalPrinterId;
        private LocalPrinter _selectedLocalPrinter;

        #endregion

        #region Public Properties

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
        public GlobalMessageViewModel GlobalMessageViewModel { get; }

        #endregion

        #region Commands

        public ICommand UpdateLocalPrinterListCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        #endregion

        #region Constructor

        public PrinterViewModel()
        {
            _messageStore = new MessageStore();
            GlobalMessageViewModel = new(_messageStore);

            LocalPrinters = new();

            UpdateLocalPrinterListCommand = new RelayCommand(UpdateLocalPrinterList);
            SaveCommand = new RelayCommand(Save);

            UpdateLocalPrinterList();

            if (LocalPrinters.Count > 0)
            {
                SelectedLocalPrinter = LocalPrinters.FirstOrDefault(lp => lp.Name == Properties.Settings.Default.DefaultReceiptPrinter);
            }

            LocalPrinters.CollectionChanged += LocalPrinters_CollectionChanged;
        }

        #endregion

        #region Private Members

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

        private void Save()
        {
            if (SelectedLocalPrinter != null)
            {
                Properties.Settings.Default.DefaultReceiptPrinter = SelectedLocalPrinter.Name;
                Properties.Settings.Default.Save();
                _messageStore.SetCurrentMessage("Принтеры по умолчанию сохранены.", MessageType.Success);
            }
        }

        private void LocalPrinters_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= Item_PropertyChanged;
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += Item_PropertyChanged;
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(LocalPrinters));
        }

        #endregion
    }
}
