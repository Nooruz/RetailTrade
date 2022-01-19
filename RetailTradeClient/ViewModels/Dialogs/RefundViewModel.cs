using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.Properties;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.Views.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class RefundViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IReceiptService _receiptService;
        private readonly IShiftStore _shiftStore;
        private readonly IUIManager _manager;
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
            IShiftStore shiftStore,
            IUIManager manager)
        {
            _receiptService = receiptService;
            _shiftStore = shiftStore;
            _manager = manager;

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
                IUIManager uIManager = new UIManager();
                if (Settings.Default.ShtrihMConnected)
                {
                    _ = uIManager.ShowDialog(new ReceiptNumberViewModel(_receiptService, uIManager) { SelectedReceipt = SelectedReceipt }, new ReceiptNumberView());
                    _manager.Close();
                }
                else if (_manager.ShowMessage("Устройство фискального регистратора (ФР) не обноружено. Продолжить?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (await _receiptService.Refund(SelectedReceipt))
                    {
                        Receipt editReceipt = Receipts.FirstOrDefault(r => r.Id == SelectedReceipt.Id);
                        editReceipt.IsRefund = true;
                    }
                    _manager.Close();
                }
            }
            else
            {
                _manager.ShowMessage("Выберите квитанцию.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
