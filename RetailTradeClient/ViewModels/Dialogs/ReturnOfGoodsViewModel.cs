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
    public class ReturnOfGoodsViewModel : BaseDialogViewModel
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
            }
        }
        public Receipt SelectedReceipt { get; set; }

        #endregion

        #region Commands

        public ICommand LoadedCommand { get; }
        public ICommand ReturnCommand { get; }

        #endregion

        #region Constructor

        public ReturnOfGoodsViewModel(IReceiptService receiptService,
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

            }
            else
            {
                _manager.ShowMessage("Выберите квитанцию.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion
    }
}
