using RetailTrade.CashRegisterMachine;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Shifts;
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
                if (_manager.ShowMessage("Вы уверены?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _ = await _receiptService.Refund(SelectedReceipt);

                    ShtrihM.Connect();
                    ShtrihM.CheckType = 2;
                    ShtrihM.OperationType = 2;

                    foreach (ProductSale productSale in SelectedReceipt.ProductSales)
                    {
                        var sum1NSP = Math.Round(productSale.SalePrice * 1 / 113, 2);
                        var sum1NDS = Math.Round(productSale.SalePrice * 12 / 113, 2);
                        string sumNSP = Math.Round(sum1NSP * 100, 0).ToString();
                        string sumNDS = Math.Round(sum1NDS * 100, 0).ToString();

                        ShtrihM.Quantity = productSale.Quantity;
                        ShtrihM.Price = productSale.SalePrice;
                        ShtrihM.StringForPrinting = string.Join(";", new string[] { "", productSale.Product.TNVED, "", "", "0", "", "0", "" + "\n" + productSale.Product.Name });

                        //ShtrihM.Tax1 = 2;
                        //ShtrihM.Tax2 = 3;
                        //ShtrihM.Tax3 = 1;
                        //ShtrihM.Tax4 = 4;

                        ShtrihM.ReturnSale();
                    }


                    ShtrihM.Summ1 = SelectedReceipt.ProductSales.Sum(ps => ps.Sum);
                    ShtrihM.StringForPrinting = "11";
                    ShtrihM.CloseCheck();

                    //var result = ShtrihM.CloseCheckWithResult();
                    ShtrihM.CutCheck();

                    _manager.Close();
                }
            }
            else
            {
                _manager.ShowMessage("Выберите квитанцию.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion
    }
}
