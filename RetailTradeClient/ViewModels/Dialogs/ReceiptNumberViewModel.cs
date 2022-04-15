using DevExpress.Mvvm;
using RetailTrade.CashRegisterMachine;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using System;
using System.Linq;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class ReceiptNumberViewModel : BaseDialogViewModel
    {
        #region Private Members

        private int? _receiptNumber;
        private readonly IReceiptService _receiptService;

        #endregion

        #region Public Properties

        public int? ReceiptNumber
        {
            get => _receiptNumber;
            set
            {
                _receiptNumber = value;
                OnPropertyChanged(nameof(ReceiptNumber));
            }
        }
        public Receipt SelectedReceipt { get; set; }

        #endregion

        #region Commands

        public ICommand RefundCommand { get; }

        #endregion

        #region Constructor

        public ReceiptNumberViewModel(IReceiptService receiptService)
        {
            _receiptService = receiptService;

            RefundCommand = new RelayCommand(Refund);
        }

        #endregion

        #region Private Voids

        private async void Refund()
        {
            if (ReceiptNumber != null && ReceiptNumber > 0)
            {
                ShtrihM.Connect();
                ShtrihM.CheckType = 2;
                ShtrihM.OperationType = 2;

                foreach (ProductSale productSale in SelectedReceipt.ProductSales)
                {
                    var sum1NSP = Math.Round(productSale.SalePrice * 1 / 102, 2);
                    //var sum1NDS = Math.Round(productSale.SalePrice * 12 / 113, 2);
                    string sumNSP = Math.Round(sum1NSP * 100, 0).ToString();
                    //string sumNDS = Math.Round(sum1NDS * 100, 0).ToString();

                    ShtrihM.Quantity = productSale.Quantity;
                    ShtrihM.Price = productSale.SalePrice;
                    ShtrihM.StringForPrinting = string.Join(";", new string[] { "", productSale.Product.TNVED, "", "", "0", "", "2", sumNSP + "\n" + productSale.Product.Name });

                    ShtrihM.Tax1 = 0;
                    ShtrihM.Tax2 = 2;
                    ShtrihM.Tax3 = 0;
                    ShtrihM.Tax4 = 0;

                    ShtrihM.ReturnSale();
                }
                ShtrihM.Summ1 = SelectedReceipt.ProductSales.Sum(ps => ps.Total);
                ShtrihM.StringForPrinting = ReceiptNumber.ToString();
                ShtrihM.CloseCheck();
                ShtrihM.CutCheck();
                _ = await _receiptService.Refund(SelectedReceipt);
                CurrentWindowService.Close();
            }
            else
            {
                _ = MessageBoxService.ShowMessage("Номер чека должен быть больше 0.", "Sale Page", MessageButton.OK, MessageIcon.Error);
            }
        }

        #endregion
    }
}
