using DevExpress.Xpf.Editors;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.State.ProductSale;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using System;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class PaymentCashViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IUserStore _userStore;
        private readonly IProductSaleStore _productSaleStore;
        private readonly IReceiptService _receiptService;
        private readonly IShiftStore _shiftStore;

        #endregion

        #region Public Properties

        public bool IsEnteredFocusable { get; set; }

        /// <summary>
        /// Внесено
        /// </summary>
        public decimal Entered
        {
            get => _productSaleStore.Entered;
            set
            {
                _productSaleStore.Entered = value - Math.Truncate(value) == 0 ? Math.Truncate(value) : value;
                OnPropertyChanged(nameof(Entered));
                OnPropertyChanged(nameof(Change));
            }
        }
        
        /// <summary>
        /// Сумма к оплате
        /// </summary>
        public decimal AmountToBePaid
        {
            get => _productSaleStore.ToBePaid;
            set
            {
                _productSaleStore.ToBePaid = value;
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
        public ICommand DigitalButtonPressCommand => new ParameterCommand(parameter => EnterNumber(parameter));

        /// <summary>
        /// Нажатия кнопки очистки
        /// </summary>
        public ICommand ClearCommand => new RelayCommand(Clear);

        /// <summary>
        /// Нажатия кнопки пробела назад
        /// </summary>
        public ICommand BackspaceCommand => new RelayCommand(Baclspace);

        /// <summary>
        /// Нажатия кнопки запятая
        /// </summary>
        public ICommand CommaButtonPressCommand => new RelayCommand(CommaButtonPress);

        /// <summary>
        /// Оплатить
        /// </summary>
        public ICommand MakeCashPaymentCommand => new MakeCashPaymentCommand(_receiptService, _shiftStore, _userStore, _productSaleStore);

        /// <summary>
        /// Следить за нажатием кнопки клавиатуры
        /// </summary>
        public ICommand TextInputCommand => new ParameterCommand(parameter => TextInput(parameter));

        public ICommand EnteredLoadedCommand => new ParameterCommand(parameter => EnteredLoaded(parameter));


        #endregion

        #region Constructor

        public PaymentCashViewModel(IReceiptService receiptService,
            IUserStore userStore,
            IShiftStore shiftStore,
            IProductSaleStore productSaleStore)
        {
            _userStore = userStore;
            _productSaleStore = productSaleStore;
            _receiptService = receiptService;
            _shiftStore = shiftStore;

            _userStore.CurrentUserChanged += UserStore_CurrentUserChanged;
            _userStore.CurrentOrganizationChanged += UserStore_CurrentOrganizationChanged;
        }

        #endregion

        #region Actions

        public event Action OnPayment;

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

        private void UserStore_CurrentUserChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
        }
        private void UserStore_CurrentOrganizationChanged()
        {
            OnPropertyChanged(nameof(CurrentOrganization));
        }

        #endregion
    }
}
