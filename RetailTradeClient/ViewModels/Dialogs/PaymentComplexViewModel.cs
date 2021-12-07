using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class PaymentComplexViewModel : BaseDialogViewModel
    {
        #region Private Members

        private decimal _amountToBePaid;
        private decimal _amountCash;
        private List<Sale> _saleProducts;
        private ObservableCollection<PaymentType> _paymentTypes = new();
        private PaymentType _selectedPaymentType;
        private bool _sumFocusable;

        #endregion

        #region Public Properties

        public GridControl ComplexPaymentGridControl { get; set; }

        public ObservableCollection<PaymentType> PaymentTypes
        {
            get => _paymentTypes;
            set
            {
                _paymentTypes = value;
                OnPropertyChanged(nameof(PaymentTypes));
            }
        }

        public PaymentType SelectedPaymentType
        {
            get => _selectedPaymentType;
            set
            {
                _selectedPaymentType = value;
                OnPropertyChanged(nameof(SelectedPaymentType));
            }
        }

        /// <summary>
        /// Сумма к оплате
        /// </summary>
        public decimal AmountToBePaid
        {
            get => _amountToBePaid;
            set
            {
                _amountToBePaid = value;
                OnPropertyChanged(nameof(AmountToBePaid));
                OnPropertyChanged(nameof(Balance));
            }
        }

        /// <summary>
        /// Остаток
        /// </summary>
        public decimal Balance
        {
            get => AmountToBePaid - PaymentTypes.Sum(pt => pt.Sum);
            set
            {

            }
        }

        public List<Sale> SaleProducts
        {
            get => _saleProducts;
            set
            {
                _saleProducts = value;
                AmountToBePaid = _saleProducts.Sum(sp => sp.Sum);
                OnPropertyChanged(nameof(SaleProducts));
            }
        }

        public decimal AmountCash
        {
            get => _amountCash;
            set
            {
                _amountCash = value;
                OnPropertyChanged(nameof(AmountCash));
            }
        }

        public bool SumFocusable
        {
            get => _sumFocusable;
            set
            {
                _sumFocusable = value;
                OnPropertyChanged(nameof(SumFocusable));
            }
        }

        /// <summary>
        /// Если кнопка запятая нажата
        /// </summary>
        public bool IsCommaButtonPressed { get; set; }

        #endregion

        #region Commands

        public ICommand PaymentCashCommand { get; }
        public ICommand PaymentCashlessCommand { get; }
        public ICommand DigitalButtonPressCommand { get; }
        public ICommand CommaButtonPressCommand { get; }
        public ICommand BackspaceCommand { get; }
        public ICommand GridControlLoadedCommand { get; }
        public ICommand MakeComplexPaymentCommand { get; }

        #endregion

        #region Constructor

        public PaymentComplexViewModel(IReceiptService receiptService,
            IUIManager manager,
            IShiftStore shiftStore,
            IUserStore userStore)
        {
            PaymentCashCommand = new RelayCommand(PaymentCash);
            PaymentCashlessCommand = new RelayCommand(PaymentCashless);
            CommaButtonPressCommand = new RelayCommand(CommaButtonPress);
            BackspaceCommand = new RelayCommand(Backspace);
            GridControlLoadedCommand = new ParameterCommand(sender => GridControlLoaded(sender));
            DigitalButtonPressCommand = new ParameterCommand(sender => DigitalButtonPress(sender));
            MakeComplexPaymentCommand = new MakeComplexPaymentCommand(this, receiptService, manager, shiftStore, userStore);

            PaymentTypes.CollectionChanged += PaymentTypes_CollectionChanged;
        }

        #endregion

        #region Private Voids

        private void GridControlLoaded(object sender)
        {
            if (sender is RoutedEventArgs e)
            {
                if (e.Source is GridControl gridControl)
                {
                    ComplexPaymentGridControl = gridControl;
                    ComplexPaymentGridControl.SelectedItemChanged += ComplexPaymentGridControl_SelectedItemChanged;
                }
            }
        }

        private void ComplexPaymentGridControl_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
        }

        private void Backspace()
        {
            if (SelectedPaymentType != null && SelectedPaymentType.Sum != 0)
            {
                PaymentType paymentType = PaymentTypes.FirstOrDefault(p => p.Id == SelectedPaymentType.Id);
                string number = paymentType.Sum.ToString();
                paymentType.Sum = number.Length == 1 ? 0 : Convert.ToDecimal(number.Remove(number.Length - 1, 1));
                if (BitConverter.GetBytes(decimal.GetBits(paymentType.Sum)[3])[2] == 0) IsCommaButtonPressed = false;
            }
        }

        private void CommaButtonPress()
        {
            IsCommaButtonPressed = true;
        }

        private void DigitalButtonPress(object sender)
        {
            int number;
            if (!SumFocusable)
            {
                if (int.TryParse(sender.ToString(), out number))
                {
                    if (IsCommaButtonPressed)
                    {
                        if (SelectedPaymentType != null)
                        {

                            PaymentType paymentType = PaymentTypes.FirstOrDefault(p => p.Id == SelectedPaymentType.Id);

                            if (paymentType.Sum == 0)
                            {
                                paymentType.Sum = Convert.ToDecimal($"0,{number}");
                            }
                            else
                            {
                                int count = BitConverter.GetBytes(decimal.GetBits(paymentType.Sum)[3])[2];
                                if (count == 0)
                                {
                                    paymentType.Sum = Convert.ToDecimal(paymentType.Sum.ToString() + $",{number}");
                                }
                                if (count > 0 && count < 2)
                                {
                                    paymentType.Sum = Convert.ToDecimal(paymentType.Sum.ToString() + number.ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        if (SelectedPaymentType != null)
                        {

                            PaymentType paymentType = PaymentTypes.FirstOrDefault(p => p.Id == SelectedPaymentType.Id);

                            if (paymentType.Sum == 0)
                            {
                                paymentType.Sum = number;
                            }
                            else
                            {
                                paymentType.Sum = Convert.ToDecimal(paymentType.Sum.ToString() + number.ToString());
                            }
                        }
                    }
                }
            }
        }

        private void PaymentCash()
        {
            if (PaymentTypes.FirstOrDefault(p => p.Id == 1) == null)
            {               
                PaymentTypes.Add(new()
                {
                    Id = 1,
                    Name = "Наличными",
                    Sum = 0
                });
                SelectedPaymentType = PaymentTypes.FirstOrDefault(p => p.Id == 1);
            }
        }

        private void PaymentCashless()
        {
            if (PaymentTypes.FirstOrDefault(p => p.Id == 2) == null)
            {
                PaymentTypes.Add(new()
                {
                    Id = 2,
                    Name = "Безналичными",
                    Sum = 0
                });
                SelectedPaymentType = PaymentTypes.FirstOrDefault(p => p.Id == 2);
            }
        }

        private void PaymentTypes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    if (item != null)
                    {
                        item.PropertyChanged -= Item_PropertyChanged;
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    if (item != null)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PaymentTypes));
            OnPropertyChanged(nameof(Balance));
        }

        #endregion
    }
}
