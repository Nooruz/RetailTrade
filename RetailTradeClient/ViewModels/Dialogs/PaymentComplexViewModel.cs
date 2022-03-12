using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.State.ProductSale;
using RetailTradeClient.State.Reports;
using RetailTradeClient.State.Shifts;
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

        private readonly IReceiptService _receiptService;
        private readonly IShiftStore _shiftStore;
        private readonly IProductSaleStore _productSaleStore;
        private readonly IReportService _reportService;
        private decimal _amountCash;
        private PaymentType _selectedPaymentType;
        private bool _sumFocusable;

        #endregion

        #region Public Properties

        public GridControl ComplexPaymentGridControl { get; set; }
        public TableView ComplexPaymentTableView => ComplexPaymentGridControl != null ? ComplexPaymentGridControl.View as TableView : null;
        public ObservableCollection<PaymentType> PaymentTypes
        {
            get => _productSaleStore.PaymentTypes;
            set
            {
                _productSaleStore.PaymentTypes = value;
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
            get => _productSaleStore.ToBePaid;
            set { }
        }
        /// <summary>
        /// Остаток
        /// </summary>
        public decimal Balance
        {
            get
            {
                _productSaleStore.Change = AmountToBePaid - PaymentTypes.Sum(pt => pt.Sum);
                return _productSaleStore.Change;
            }
            set
            {

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

        #region Actions

        public event Action OnPayment;

        #endregion

        #region Commands

        public ICommand PaymentCashCommand => new RelayCommand(PaymentCash);
        public ICommand PaymentCashlessCommand => new RelayCommand(PaymentCashless);
        public ICommand DigitalButtonPressCommand => new ParameterCommand(sender => DigitalButtonPress(sender));
        public ICommand CommaButtonPressCommand => new RelayCommand(CommaButtonPress);
        public ICommand BackspaceCommand => new RelayCommand(Backspace);
        public ICommand GridControlLoadedCommand => new ParameterCommand(sender => GridControlLoaded(sender));
        public ICommand ClearCommand => new RelayCommand(Cleare);
        public ICommand MakeComplexPaymentCommand => new MakeComplexPaymentCommand(_receiptService, _shiftStore, _reportService, _productSaleStore);

        #endregion

        #region Constructor

        public PaymentComplexViewModel(IReceiptService receiptService,
            IShiftStore shiftStore,
            IReportService reportService,
            IProductSaleStore productSaleStore)
        {
            _receiptService = receiptService;
            _shiftStore = shiftStore;
            _reportService = reportService;
            _productSaleStore = productSaleStore;

            PaymentTypes.CollectionChanged += PaymentTypes_CollectionChanged;
            _productSaleStore.OnProductSale += ProductSaleStore_OnProductSale;
        }

        #endregion

        #region Private Voids

        private void ProductSaleStore_OnProductSale(bool obj)
        {
            CurrentWindowService.Close();
            _productSaleStore.Change = 0;
        }

        private void Cleare()
        {
            PaymentTypes.Clear();
            OnPropertyChanged(nameof(Balance));
        }

        private void ShowEditor(int column)
        {
            ComplexPaymentTableView.FocusedRowHandle = PaymentTypes.IndexOf(SelectedPaymentType);
            ComplexPaymentTableView.Grid.CurrentColumn = ComplexPaymentTableView.Grid.Columns[column];
            _ = ComplexPaymentTableView.Dispatcher.BeginInvoke(new Action(() =>
            {
                ComplexPaymentTableView.ShowEditor();
            }), DispatcherPriority.Render);
        }

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
            ShowEditor(1);
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
            if (!SumFocusable)
            {
                ComplexPaymentTableView.CloseEditor();
                if (int.TryParse(sender.ToString(), out int number))
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
