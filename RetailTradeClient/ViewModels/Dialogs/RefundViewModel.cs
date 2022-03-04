using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.Properties;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class RefundViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IReceiptService _receiptService;
        private readonly IShiftStore _shiftStore;
        private ObservableCollection<Receipt> _receipts; 

        #endregion

        #region Public Properties

        public ObservableCollection<Receipt> Receipts
        {
            get => _receipts ?? new();
            set
            {
                _receipts = value;
                OnPropertyChanged(nameof(Receipts));
                OnPropertyChanged(nameof(CanShowLoadingPanel));
            }
        }
        public Receipt SelectedReceipt { get; set; }
        public bool CanShowLoadingPanel => Receipts != null && Receipts.Any();

        #endregion

        #region Commands

        public ICommand LoadedCommand { get; }
        public ICommand ReturnCommand { get; }

        #endregion

        #region Constructor

        public RefundViewModel(IReceiptService receiptService,
            IShiftStore shiftStore)
        {
            _receiptService = receiptService;
            _shiftStore = shiftStore;

            LoadedCommand = new RelayCommand(Loaded);
            ReturnCommand = new RelayCommand(Return);

            Receipts.CollectionChanged += Receipts_CollectionChanged;
        }

        #endregion

        #region Private Voids

        private async void Loaded()
        {
            Receipts = new(await _receiptService.GetReceiptsFromCurrentShift(_shiftStore.CurrentShift.Id));
        }

        private async void Return()
        {            
            if (SelectedReceipt != null)
            {
                if (Settings.Default.ShtrihMConnected)
                {
                    WindowService.Show(nameof(ReceiptNumberView), new ReceiptNumberViewModel(_receiptService) { SelectedReceipt = SelectedReceipt });
                    CurrentWindowService.Close();
                }
                else if (MessageBoxService.ShowMessage("Устройство фискального регистратора (ФР) не обноружено. Продолжить?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                {
                    if (await _receiptService.Refund(SelectedReceipt))
                    {
                        Receipt editReceipt = Receipts.FirstOrDefault(r => r.Id == SelectedReceipt.Id);
                        editReceipt.IsRefund = true;
                    }
                    CurrentWindowService.Close();
                }
            }
            else
            {
                _ = MessageBoxService.ShowMessage("Выберите квитанцию.", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
            }
        }

        private void Receipts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= Item_PropertyChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Receipts));
        }

        #endregion
    }
}
