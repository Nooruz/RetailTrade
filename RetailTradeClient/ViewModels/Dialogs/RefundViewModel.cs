using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Shifts;
using System.Collections.Generic;
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
        private readonly IRefundService _refundService;
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
            }
        }
        public Receipt SelectedReceipt { get; set; }

        #endregion

        #region Commands

        public ICommand LoadedCommand { get; }
        public ICommand ReturnCommand { get; }

        #endregion

        #region Constructor

        public RefundViewModel(IReceiptService receiptService,
            IShiftStore shiftStore,
            IRefundService refundService,
            IUIManager manager)
        {
            _receiptService = receiptService;
            _shiftStore = shiftStore;
            _refundService = refundService;
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

        private async void Return()
        {
            if (SelectedReceipt != null)
            {
                await _receiptService.Refund(SelectedReceipt);
                _manager.Close();
            }
            else
            {
                _manager.ShowMessage("Выберите квитанцию.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion
    }
}
