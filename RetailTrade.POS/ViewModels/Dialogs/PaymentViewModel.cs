using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.POS.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailTrade.POS.ViewModels.Dialogs
{
    public class PaymentViewModel : BaseViewModel
    {
        #region Private Members

        private Receipt _receipt = new();

        #endregion

        #region Public Properties

        public Receipt Receipt
        {
            get => _receipt;
            set
            {
                _receipt = value;
                Receipt.PaidInCash = Receipt.AmountWithoutDiscount;
                OnPropertyChanged(nameof(Receipt));
            }
        }

        #endregion

        #region Private Voids

        private void ViewModel_OnDiscountChanged(decimal discountSum)
        {
            if (discountSum > 0)
            {
                IsDiscountReceipt = true;
                Receipt.DiscountAmount = discountSum;
            }
            OnPropertyChanged(nameof(ReceiptDiscount));
            OnPropertyChanged(nameof(TotalSum));
        }

        private void ViewModel_OnDeleteDiscountReceipt()
        {
            IsDiscountReceipt = false;
            Receipt.DiscountAmount = 0;
            OnPropertyChanged(nameof(ReceiptDiscount));
            OnPropertyChanged(nameof(TotalSum));
        }

        #endregion

        #region Public Voids

        [Command]
        public void PaidInCash()
        {

        }

        [Command]
        public void PaidInCashless()
        {

        }

        [Command]
        public void OffsetPayment()
        {

        }

        [Command]
        public void DiscountReceipt()
        {
            try
            {
                DiscountReceiptViewModel viewModel = new()
                {
                    Title = "Скидка",
                    Receipt = Receipt
                };

                viewModel.OnDiscountChanged += ViewModel_OnDiscountChanged;
                viewModel.OnDeleteDiscountReceipt += ViewModel_OnDeleteDiscountReceipt;

                WindowService.Show(nameof(DiscountReceiptView), viewModel);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void Customer()
        {
            try
            {
                WindowService.Show(nameof(CustomerView), new CustomerViewModel() { Title = "Покупатель" });
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion
    }
}
