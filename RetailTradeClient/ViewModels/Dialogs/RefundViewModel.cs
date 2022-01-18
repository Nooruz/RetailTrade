using RetailTrade.CashRegisterMachine;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.Views.Dialogs;
using System;
using System.Collections.Generic;
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
        private IEnumerable<Receipt> _receipts; 

        #endregion

        #region Public Properties

        public IEnumerable<Receipt> Receipts
        {
            get => _receipts;
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
        }

        #endregion

        #region Private Voids

        private async void Loaded()
        {
            Receipts = await _receiptService.GetReceiptsFromCurrentShift(_shiftStore.CurrentShift.Id);
        }

        private void Return()
        {
            if (SelectedReceipt != null)
            {
                _ = _manager.ShowDialog(new ReceiptNumberViewModel(_receiptService, new UIManager()) { SelectedReceipt = SelectedReceipt }, new ReceiptNumberView());
            }
            else
            {
                _manager.ShowMessage("Выберите квитанцию.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion
    }
}
