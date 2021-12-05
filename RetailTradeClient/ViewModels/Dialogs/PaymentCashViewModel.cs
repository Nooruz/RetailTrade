using DevExpress.Xpf.Editors;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class PaymentCashViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IUserStore _userStore;
        private decimal _entered;
        private decimal _amountToBePaid;
        private List<Sale> _saleProducts;

        #endregion

        #region Public Properties

        public bool IsEnteredFocusable { get; set; }

        /// <summary>
        /// Внесено
        /// </summary>
        public decimal Entered
        {
            get => _entered;
            set
            {
                _entered = value - Math.Truncate(value) == 0 ? Math.Truncate(value) : value;
                OnPropertyChanged(nameof(Entered));
                OnPropertyChanged(nameof(Change));
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
                OnPropertyChanged(nameof(Change));
            }
        }

        /// <summary>
        /// Сдача
        /// </summary>
        public decimal Change
        {
            get => Entered - AmountToBePaid;
            set
            {

            }
        }

        /// <summary>
        /// Покупаемы товары
        /// </summary>
        public List<Sale> SaleProducts
        {
            get => _saleProducts;
            set
            {
                _saleProducts = value;
                Entered = _saleProducts.Sum(sp => sp.Sum);
                AmountToBePaid = Entered;
                OnPropertyChanged(nameof(SaleProducts));
            }
        }

        /// <summary>
        /// Если кнопка запятая нажата
        /// </summary>
        public bool IsCommaButtonPressed { get; set; }

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public User CurrentUser => _userStore.CurrentUser;

        /// <summary>
        /// Организация
        /// </summary>
        public Organization CurrentOrganization => _userStore.Organization;

        #endregion

        #region Commands

        /// <summary>
        /// Нажатия кнопки с цифрами
        /// </summary>
        public ICommand DigitalButtonPressCommand { get; }

        /// <summary>
        /// Нажатия кнопки очистки
        /// </summary>
        public ICommand ClearCommand { get; }

        /// <summary>
        /// Нажатия кнопки пробела назад
        /// </summary>
        public ICommand BackspaceCommand { get; }

        /// <summary>
        /// Нажатия кнопки запятая
        /// </summary>
        public ICommand CommaButtonPressCommand { get; }

        /// <summary>
        /// Оплатить
        /// </summary>
        public ICommand MakeCashPaymentCommand { get; }

        /// <summary>
        /// Следить за нажатием кнопки клавиатуры
        /// </summary>
        public ICommand TextInputCommand { get; }

        public ICommand EnteredLoadedCommand { get; }


        #endregion

        #region Constructor

        public PaymentCashViewModel(IReceiptService receiptService,
            IProductSaleService productSaleService,
            IUserStore userStore,
            IUIManager manager,
            IShiftStore shiftStore)
        {
            _userStore = userStore;

            DigitalButtonPressCommand = new ParameterCommand(parameter => EnterNumber(parameter));
            TextInputCommand = new ParameterCommand(parameter => TextInput(parameter));
            ClearCommand = new RelayCommand(Clear);
            BackspaceCommand = new RelayCommand(Baclspace);
            CommaButtonPressCommand = new RelayCommand(CommaButtonPress);
            MakeCashPaymentCommand = new MakeCashPaymentCommand(this, receiptService, manager, shiftStore, userStore);
            EnteredLoadedCommand = new ParameterCommand(parameter => EnteredLoaded(parameter));

            _userStore.StateChanged += UserStore_StateChanged;
        }

        #endregion

        #region Private Voids

        private void EnteredLoaded(object parameter)
        {
            if (parameter is RoutedEventArgs e)
            {
                if (e.Source is TextEdit sender)
                {
                    sender.SelectAll();
                    sender.Focus();
                    IsEnteredFocusable = true;
                }
            }
        }

        private void TextInput(object parameter)
        {
            if (parameter is TextCompositionEventArgs textCompositionEventArgs)
            {
                EnterNumber(textCompositionEventArgs.Text);
            }
        }

        private void EnterNumber(object parameter)
        {
            int number;
            if (int.TryParse(parameter.ToString(), out number))
            {
                if (IsCommaButtonPressed)
                {
                    if (Entered == 0)
                        Entered = Convert.ToDecimal($"0,{number}");
                    else if (Entered <= Convert.ToDecimal("999999999999,99"))
                    {
                        int count = BitConverter.GetBytes(decimal.GetBits(Entered)[3])[2];
                        if (count == 0)
                        {
                            Entered = Convert.ToDecimal(Entered.ToString() + $",{number}");
                        }
                        if (count > 0 && count < 2)
                        {
                            Entered = IsEnteredFocusable ? Convert.ToDecimal(number.ToString()) : Convert.ToDecimal(Entered.ToString() + number.ToString());
                        }
                    }
                }
                else
                {
                    if (Entered == 0)
                        Entered = number;
                    else if (Entered <= Convert.ToDecimal("999999999999,99"))
                    {
                        Entered = IsEnteredFocusable ? Convert.ToDecimal(number.ToString()) : Convert.ToDecimal(Entered.ToString() + number.ToString());
                    }
                }      
            }
            if (IsEnteredFocusable) IsEnteredFocusable = false;
        }

        private void Clear()
        {
            Entered = 0;
            IsCommaButtonPressed = false;
        }

        private void Baclspace()
        {
            if (Entered != 0)
            {
                string number = Entered.ToString();
                Entered = number.Length == 1 ? 0 : Convert.ToDecimal(number.Remove(number.Length - 1, 1));
                if (BitConverter.GetBytes(decimal.GetBits(Entered)[3])[2] == 0) IsCommaButtonPressed = false;
            }
        }

        private void CommaButtonPress()
        {
            IsCommaButtonPressed = true;
        }

        private void UserStore_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(CurrentOrganization));
        }

        #endregion
    }
}
