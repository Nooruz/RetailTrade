using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.POS.State.Shifts;
using RetailTrade.POS.Views.Dialogs;
using System;
using System.Linq;

namespace RetailTrade.POS.ViewModels.Dialogs
{
    public class PaymentViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IReceiptService _receiptService;
        private readonly IShiftStore _shiftStore;
        private Receipt _receipt = new();
        private bool _isDiscountReceipt;

        #endregion

        #region Public Properties

        public Receipt EditReceipt
        {
            get => _receipt;
            set
            {
                _receipt = value;
                PayInCash = EditReceipt.AmountWithoutDiscount;
                OnPropertyChanged(nameof(EditReceipt));
            }
        }
        public bool IsDiscountReceipt
        {
            get => _isDiscountReceipt;
            set
            {
                _isDiscountReceipt = value;
                OnPropertyChanged(nameof(IsDiscountReceipt));
                OnPropertyChanged(nameof(ReceiptDiscount));
            }
        }
        public decimal PayInCash
        {
            get => EditReceipt.PaidInCash;
            set
            {
                EditReceipt.PaidInCash = value;
                OnPropertyChanged(nameof(PayInCash));
                OnPropertyChanged(nameof(Change));
            }
        }
        public string ReceiptDiscount => IsDiscountReceipt ? $"{Math.Round(EditReceipt.DiscountAmount / EditReceipt.Total * 100, 2)}% • {EditReceipt.DiscountAmount:N2}" : string.Empty;
        public bool IsChange => EditReceipt.AmountWithoutDiscount - PayInCash < 0;
        public string Change => IsChange ? $"Сдача: {PayInCash - EditReceipt.AmountWithoutDiscount}" : string.Empty;
        public Shift CurrentShift => _shiftStore.CurrentShift;

        #endregion

        #region Private Voids

        private void ViewModel_OnDiscountChanged(decimal discountSum)
        {
            if (discountSum > 0)
            {
                IsDiscountReceipt = true;
                EditReceipt.DiscountAmount = discountSum;
            }
            OnPropertyChanged(nameof(ReceiptDiscount));
            PayInCash = EditReceipt.AmountWithoutDiscount;
        }

        private void ViewModel_OnDeleteDiscountReceipt()
        {
            IsDiscountReceipt = false;
            EditReceipt.DiscountAmount = 0;
            OnPropertyChanged(nameof(ReceiptDiscount));
            PayInCash = EditReceipt.AmountWithoutDiscount;
        }

        #endregion

        #region Actions

        public event Action OnSale;

        #endregion

        #region Constructor

        public PaymentViewModel(IReceiptService receiptService,
            IShiftStore shiftStore)
        {
            _receiptService = receiptService;
            _shiftStore = shiftStore;

            _shiftStore.CurrentShiftChanged += ShiftStore_CurrentShiftChanged;
        }

        #endregion

        #region Private Voids

        private void ShiftStore_CurrentShiftChanged(CheckingResult checkingResult)
        {
            OnPropertyChanged(nameof(CurrentShift));
        }

        #endregion

        #region Public Voids

        [Command]
        public async void PaidInCash()
        {
            try
            {
                if (!IsChange)
                {
                    PayInCash = EditReceipt.AmountWithoutDiscount;
                }
                EditReceipt.DateOfPurchase = DateTime.Now;

                if (CurrentShift == null)
                {
                    await _shiftStore.OpeningShift();
                }

                EditReceipt.ShiftId = CurrentShift.Id;

                EditReceipt.ProductSales.ToList().ForEach(p =>
                {
                    p.Product = null;
                    p.Receipt = null;
                });

                _ = await _receiptService.CreateAsync(EditReceipt);

                OnSale?.Invoke();

                CurrentWindowService.Close();
            }
            catch (Exception)
            {
                //ignore
            }
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
                    Receipt = EditReceipt
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
