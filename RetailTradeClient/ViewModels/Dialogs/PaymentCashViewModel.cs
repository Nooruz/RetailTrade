using DevExpress.Xpf.Editors;
using RetailTradeClient.Commands;
using RetailTradeClient.State.ProductSales;
using System;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class PaymentCashViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductSaleStore _productSaleStore;

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
                _productSaleStore.Entered = value;
                Change = _productSaleStore.Entered - _productSaleStore.ToBePaid;
                OnPropertyChanged(nameof(Entered));
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
        /// Сдача
        /// </summary>
        public decimal Change
        {
            get => _productSaleStore.Change;
            set
            {
                _productSaleStore.Change = value < 0 ? value * -1 : value;
                OnPropertyChanged(nameof(Change));
                OnPropertyChanged(nameof(ChangeLabel));
                OnPropertyChanged(nameof(ChangeForeground));
                OnPropertyChanged(nameof(CanMakeCashPayment));
            }
        }
        /// <summary>
        /// Если кнопка запятая нажата
        /// </summary>
        public bool IsCommaButtonPressed { get; set; }
        public string ChangeLabel => _productSaleStore.Entered - _productSaleStore.ToBePaid < 0 ? "Осталось доплатить:" : "Сдача:";
        public Brush ChangeForeground => _productSaleStore.Entered - _productSaleStore.ToBePaid < 0 ? Brushes.Red : Brushes.Green;
        public bool CanMakeCashPayment => _productSaleStore.Entered - _productSaleStore.ToBePaid >= 0;

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);

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
        public ICommand MakeCashPaymentCommand => new RelayCommand(MakeCashPayment);

        /// <summary>
        /// Следить за нажатием кнопки клавиатуры
        /// </summary>
        public ICommand TextInputCommand => new ParameterCommand(parameter => TextInput(parameter));

        public ICommand EnteredLoadedCommand => new ParameterCommand(parameter => EnteredLoaded(parameter));


        #endregion

        #region Constructor

        public PaymentCashViewModel(IProductSaleStore productSaleStore)
        {
            _productSaleStore = productSaleStore;

            _productSaleStore.OnProductSalesChanged += () => OnPropertyChanged(nameof(Change));
        }

        #endregion

        #region Private Voids

        private void UserControlLoaded()
        {
            Entered = _productSaleStore.ToBePaid;
        }

        private async void MakeCashPayment()
        {
            if (CanMakeCashPayment)
            {
                CurrentWindowService.Close();
                await _productSaleStore.CashPayment();
            }
        }

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

        #endregion
    }
}
