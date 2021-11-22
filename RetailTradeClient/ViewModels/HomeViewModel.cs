using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.State.Authenticators;
using RetailTradeClient.State.CashRegisterControlMachine;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Messages;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using RetailTradeClient.ViewModels.Base;
using RetailTradeClient.ViewModels.Dialogs;
using RetailTradeClient.Views;
using RetailTradeClient.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IUserStore _userStore;
        private readonly IProductService _productService;
        private readonly IProductSaleService _productSaleService;
        private readonly IReceiptService _receiptService;
        private readonly IUIManager _manager;
        private readonly IMessageStore _messageStore;
        private readonly IAuthenticator _authenticator;
        private readonly ICashRegisterControlMachine _cashRegisterControlMachine;
        private readonly IShiftStore _shiftStore;
        private readonly PaymentCashViewModel _paymentCashViewModel;
        private string _barcode;
        private Sale _selectedProductSale;
        private ObservableCollection<Product> _products;
        private decimal _change;

        #endregion

        #region Public Properties

        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public ObservableCollection<Sale> SaleProducts { get; set; }
        public decimal Sum
        {
            get
            {
                decimal sum = SaleProducts.Sum(sp => sp.Sum);
                if (sum != 0)
                    Change = 0;
                return sum;
            }
            set
            {

            }
        }
        public decimal Change
        {
            get => _change;
            set
            {
                _change = value;
                OnPropertyChanged(nameof(Change));
            }
        }
        public decimal ToBePaid => Sum;
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged(nameof(Barcode));
            }
        }
        public string Info => $"РМК: {(_userStore.CurrentUser != null ? _userStore.CurrentUser.FullName : string.Empty)}";
        public List<PostponeReceipt> PostponeReceipts { get; set; }
        public Sale SelectedProductSale
        {
            get => _selectedProductSale;
            set
            {
                _selectedProductSale = value;
                OnPropertyChanged(nameof(SelectedProductSale));
            }
        }
        public int FocusedRowHandle => SaleProducts.Count - 1;

        #endregion

        #region Commands

        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        public ICommand LogoutCommand { get; }
        public ICommand TextInputCommand { get; }
        public ICommand KeyDownCommand { get; }

        /// <summary>
        /// Настройки принтера
        /// </summary>
        public ICommand PrinterSettingsCommand { get; }

        /// <summary>
        /// Отложить чек
        /// </summary>
        public ICommand PostponeReceiptCommand { get; }

        /// <summary>
        /// Просмотр отложенных чеков
        /// </summary>
        public ICommand OpenPostponeReceiptCommand { get; }

        /// <summary>
        /// Добавить товар в корзину
        /// </summary>
        public ICommand AddProductToSaleCommand { get; }

        /// <summary>
        /// Оплата наличными
        /// </summary>
        public ICommand PaymentCashCommand { get; }

        /// <summary>
        /// Удалить выбранную из корзина товара
        /// </summary>
        public ICommand DeleteSelectedRowCommand { get; }

        /// <summary>
        /// Закрыть смену
        /// </summary>
        public ICommand ClosingShiftCommand { get; }

        /// <summary>
        /// Распечатать х-отчет
        /// </summary>
        public ICommand PrintXReportCommand { get; }

        /// <summary>
        /// Настройки ККМ
        /// </summary>
        public ICommand CRMSettingsCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand LoadedHomeViewCommand { get; }

        /// <summary>
        /// Проверка ввода количестов товаров для продажи
        /// </summary>
        public ICommand QuantityValidateCommand { get; }

        /// <summary>
        /// Отменить
        /// </summary>
        public ICommand CancelCommand { get; }

        public ICommand QuantityContentChangedCommand { get; }

        #endregion

        #region Constructor

        public HomeViewModel(IUserStore userStore,
            IProductService productService,
            IProductSaleService productSaleService,
            IReceiptService receiptService,
            IUIManager manager,
            IMessageStore messageStore,
            IAuthenticator authenticator,
            ICashRegisterControlMachine cashRegisterControlMachine,
            IShiftStore shiftStore)
        {
            _userStore = userStore;
            _productService = productService;
            _productSaleService = productSaleService;
            _receiptService = receiptService;
            _manager = manager;
            _messageStore = messageStore;
            _authenticator = authenticator;
            _cashRegisterControlMachine = cashRegisterControlMachine;
            _shiftStore = shiftStore;

            SaleProducts = new();
            PostponeReceipts = new List<PostponeReceipt>();

            _paymentCashViewModel = new(_receiptService, _productSaleService, _userStore, _manager, _cashRegisterControlMachine, _shiftStore) { Title = "Оплата наличными" };

            LogoutCommand = new RelayCommand(Logout);
            TextInputCommand = new ParameterCommand(parameter => TextInput(parameter));
            KeyDownCommand = new ParameterCommand(parameter => KeyDown(parameter));
            PrinterSettingsCommand = new RelayCommand(PrinterSettings);
            PostponeReceiptCommand = new RelayCommand(PostponeReceipt);
            OpenPostponeReceiptCommand = new RelayCommand(OpenPostponeReceipt);
            AddProductToSaleCommand = new ParameterCommand(pc => AddProductToSale(pc));
            PaymentCashCommand = new RelayCommand(PaymentCash);
            DeleteSelectedRowCommand = new RelayCommand(DeleteSelectedRow);
            PrintXReportCommand = new PrintXReportCommand();
            CRMSettingsCommand = new RelayCommand(CRMSettings);
            LoadedHomeViewCommand = new ParameterCommand(parameter => LoadedHomeView(parameter));
            QuantityValidateCommand = new ParameterCommand(parameter => QuantityValidate(parameter));
            CancelCommand = new RelayCommand(Cancel);
            QuantityContentChangedCommand = new ParameterCommand(parameter => QuantityContentChanged(parameter));

            SaleProducts.CollectionChanged += SaleProducts_CollectionChanged;
            _productService.PropertiesChanged += ProductService_PropertiesChanged;
            _paymentCashViewModel.PropertyChanged += PaymentCashViewModel_PropertyChanged;
            OpenMainMenu();
        }

        #endregion

        #region Private Voids

        private void SaleProducts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= Item_PropertyChanged;
                }
            }

            if (e.NewStartingIndex > 47)
            {
                _manager.ShowMessage("49", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }

            OnPropertyChanged(nameof(Sum));
            OnPropertyChanged(nameof(FocusedRowHandle));
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SaleProducts));
            OnPropertyChanged(nameof(Sum));
            OnPropertyChanged(nameof(FocusedRowHandle));
        }

        private void LoadedHomeView(object parameter)
        {
            ProductService_PropertiesChanged();
            if (parameter is RoutedEventArgs e)
            {
                if (e.Source is HomeView homeView)
                {
                    homeView.Focus();
                }
            }
        }

        private async void ProductService_PropertiesChanged()
        {
            Products = new(await _productService.PredicateSelect(p => p.Quantity > 0,
                p => new Product { Id = p.Id, Name = p.Name, SalePrice = p.SalePrice, Quantity = p.Quantity }));
        }

        private void QuantityValidate(object parameter)
        {
            if (parameter is GridCellValidationEventArgs e)
            {
                if (((Sale)e.Row).QuantityInStock < Convert.ToDecimal(e.Value))
                {
                    _manager.ShowMessage("Количество превышает остаток.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    e.ErrorContent = "Количество превышает остаток.";
                    e.IsValid = false;
                }
            }
        }

        /// <summary>
        /// Штрих-код сканер менен сканлерлеп жаткандагы цифраларды алуу
        /// </summary>
        /// <param name="parameter">Цифралар</param>
        private void TextInput(object parameter)
        {
            if (parameter is TextCompositionEventArgs e)
            {
                Barcode = Barcode + e.Text;
            }
        }

        /// <summary>
        /// Штрих-кодду сканерлеп буткондон кийинки басылуучу ентерди алуу
        /// </summary>
        /// <param name="parameter">Ентер</param>
        private async void KeyDown(object parameter)
        {
            if (parameter is KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    var newProduct = await _productService.Predicate(p => p.Barcode == Barcode && p.Quantity > 0,
                        p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, SalePrice = p.SalePrice });
                    if (newProduct != null)
                    {
                        var product = SaleProducts.FirstOrDefault(sp => sp.Id == newProduct.Id);
                        if (product == null)
                        {
                            SaleProducts.Add(new Sale
                            {
                                Id = newProduct.Id,
                                Name = newProduct.Name,
                                Quantity = 1,
                                QuantityInStock = newProduct.Quantity,
                                SalePrice = newProduct.SalePrice,
                                Sum = newProduct.SalePrice * 1
                            });
                        }
                        else
                        {
                            product.Quantity++;
                            product.Sum = product.SalePrice * product.Quantity;
                        }
                    }
                    OnPropertyChanged(nameof(Sum));
                    OnPropertyChanged(nameof(ToBePaid));
                    Barcode = string.Empty;
                }
            }
        }      

        /// <summary>
        /// Принтерлерди настройкалоо
        /// </summary>
        private void PrinterSettings()
        {
            _manager.ShowDialog(new PrinterViewModel(_messageStore) { Title = "Настройка принтеров" }, new PrinterView());
        }

        /// <summary>
        /// Отложить чек
        /// </summary>
        private void PostponeReceipt()
        {
            if (SaleProducts.Count != 0)
            {
                PostponeReceipts.Add(new PostponeReceipt
                {
                    Id = new Guid(),
                    DateTime = DateTime.Now,       
                    Sum = SaleProducts.Sum(sp => sp.Sum),
                    PostponeProducts = SaleProducts.Select(sp => new PostponeProduct { Id = sp.Id, Name = sp.Name, SalePrice = sp.SalePrice, Quantity = sp.Quantity, Sum = sp.Sum }).ToList()
                });
                SaleProducts.Clear();
                OnPropertyChanged(nameof(Sum));
            }            
        }

        /// <summary>
        /// Просмотр отложенных чеков
        /// </summary>
        private async void OpenPostponeReceipt()
        {
            if (PostponeReceipts.Count > 0)
            {
                if (await _manager.ShowDialog(new PostponeReceiptViewModel(this, _manager) { Title = "Выбор чека" }, new PostponeReceiptView()))
                {

                }
            }            
        }

        /// <summary>
        /// Штрих-коду жок товарларды басканда
        /// </summary>
        /// <param name="parameter">Товардын коду</param>
        private void AddProductToSale(object parameter)
        {
            if (parameter is int id)
            {
                var product = Products.FirstOrDefault(p => p.Id == id);
                var saleProduct = SaleProducts.FirstOrDefault(sp => sp.Id == id);
                if (saleProduct == null)
                {
                    SaleProducts.Add(new Sale
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Quantity = 1,
                        SalePrice = product.SalePrice,
                        Sum = product.SalePrice,
                        QuantityInStock = product.Quantity - 1
                    });
                }
                else
                {
                    saleProduct.Quantity++;
                }
            }
        }

        /// <summary>
        /// Оплата наличными
        /// </summary>
        private async void PaymentCash()
        {
            if (SaleProducts.Count > 0)
            {
                _paymentCashViewModel.SaleProducts = SaleProducts.ToList();

                if (await _manager.ShowDialog(_paymentCashViewModel,
                new PaymentCashView()))
                {
                    SaleProducts.Clear();
                }
            }
        }

        /// <summary>
        /// Выйти из аккаунта
        /// </summary>
        private void Logout()
        {
            if (_manager.ShowMessage("Выйти?", "Выход из аккаунта", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _authenticator.Logout();
            }
        }

        private async void OpenMainMenu()
        {
            var result = await _manager.ShowDialog(new MainMenuViewModel(_cashRegisterControlMachine, _shiftStore, _userStore.CurrentUser.Id, _manager)
            {
                Title = $"РМК: {(_userStore.CurrentUser != null ? _userStore.CurrentUser.FullName : null)}"
            },
                new MainMenuView());
        }

        /// <summary>
        /// Удалить выбранный товар из корзины
        /// </summary>
        private void DeleteSelectedRow()
        {
            if (SelectedProductSale != null)
            {
                SaleProducts.Remove(SelectedProductSale);
            }
        }

        /// <summary>
        /// Настройки ККМ
        /// </summary>
        private async void CRMSettings()
        {
            await _manager.ShowDialog(new CommunicationSettingsViewModel() { Title = "Настройка связи с ККМ" }, new CommunicationSettingsView());
        }

        private void PaymentCashViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PaymentCashViewModel.Change))
            {
                if (sender is PaymentCashViewModel viewModel)
                {
                    Change = viewModel.Change;
                }
            }
        }

        /// <summary>
        /// Отменить
        /// </summary>
        private void Cancel()
        {
            SaleProducts.Clear();
        }

        private void QuantityContentChanged(object parameter)
        {

        }

        #endregion

        #region Dispose

        public override void Dispose()
        {

            base.Dispose();
        }

        #endregion
    }
}
