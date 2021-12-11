using RetailTradeServer.Commands;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Dialogs.Base;
using SalePageServer.Properties;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class PrinterDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IMessageStore _messageStore;

        #endregion

        #region Public Properties

        public ObservableCollection<LocalPrinter> LocalPrinters { get; set; }
        public GlobalMessageViewModel GlobalMessageViewModel { get; }
        public LocalPrinter SelectedLabelPrinter { get; set; }
        public LocalPrinter SelectedReportPrinter { get; set; }

        #endregion

        #region Commands

        public ICommand UpdateLocalPrinterListCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        #endregion

        #region Constructor

        public PrinterDialogFormModel(IMessageStore messageStore)
        {
            _messageStore = messageStore;

            GlobalMessageViewModel = new(messageStore);
            LocalPrinters = new();

            UpdateLocalPrinterListCommand = new RelayCommand(UpdateLocalPrinterList);
            SaveCommand = new RelayCommand(Save);

            UpdateLocalPrinterList();

            if (LocalPrinters.Count > 0)
            {
                SelectedLabelPrinter = LocalPrinters.FirstOrDefault(lp => lp.Name == Settings.Default.DefaultLabelPrinter);
                SelectedReportPrinter = LocalPrinters.FirstOrDefault(lp => lp.Name == Settings.Default.DefaultReportPrinter);
            }

            LocalPrinters.CollectionChanged += LocalPrinters_CollectionChanged;
        }

        #endregion

        #region Private Voids

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
            if (SelectedLabelPrinter != null)
            {
                Settings.Default.DefaultLabelPrinter = SelectedLabelPrinter.Name;
                Settings.Default.Save();
            }
            if (SelectedReportPrinter != null)
            {
                Settings.Default.DefaultReportPrinter = SelectedReportPrinter.Name;
                Settings.Default.Save();
            }
            if (SelectedLabelPrinter != null || SelectedReportPrinter != null)
            {
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

    public class LocalPrinter : INotifyPropertyChanged
    {
        #region Private Members

        private int _id;
        private string _name;

        #endregion

        #region Public Properties

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        #endregion

        #region NotifyPropertyChanged

        public virtual void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
