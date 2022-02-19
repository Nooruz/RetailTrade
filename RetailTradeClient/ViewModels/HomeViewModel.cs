using CoreScanner;
using DevExpress.Xpf.Grid;
using RetailTrade.CashRegisterMachine;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.Properties;
using RetailTradeClient.State.Authenticators;
using RetailTradeClient.State.Barcode;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Messages;
using RetailTradeClient.State.ProductSale;
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
using System.Windows.Data;
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
        private readonly IShiftStore _shiftStore;
        private readonly IRefundService _refundService;
        private readonly IProductSaleStore _productSaleStore;
        private readonly IZebraBarcodeScanner _zebraBarcodeScanner;
        private readonly IComBarcodeService _comBarcodeService;
        private string _barcode;
        private Sale _selectedProductSale;
        private ObservableCollection<Product> _products;
        private decimal _change;
        private CCoreScannerClass _barcodeScanner;
        private object _syncLock = new();

        #endregion

        #region Public Properties

        public ObservableCollection<Product> ProductsWithoutBarcode
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(ProductsWithoutBarcode));
            }
        }
        public bool IsKeepRecorsd => Settings.Default.IsKeepRecords;
        public ObservableCollection<Sale> SaleProducts { get; set; }
        public ICollectionView SaleProductsCollectionView { get; set; }
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
        public TableView SaleTableView { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        public ICommand LogoutCommand => new RelayCommand(Logout);
        public ICommand TextInputCommand => new ParameterCommand(parameter => TextInput(parameter));
        public ICommand KeyDownCommand => new ParameterCommand(parameter => KeyDown(parameter));
        /// <summary>
        /// Настройки принтера
        /// </summary>
        public ICommand PrinterSettingsCommand => new RelayCommand(PrinterSettings);
        /// <summary>
        /// Отложить чек
        /// </summary>
        public ICommand PostponeReceiptCommand => new RelayCommand(PostponeReceipt);
        /// <summary>
        /// Просмотр отложенных чеков
        /// </summary>
        public ICommand OpenPostponeReceiptCommand => new RelayCommand(OpenPostponeReceipt);
        /// <summary>
        /// Добавить товар в корзину
        /// </summary>
        public ICommand AddProductToSaleCommand => new ParameterCommand(pc => AddProductToSale(pc));
        /// <summary>
        /// Оплата наличными
        /// </summary>
        public ICommand PaymentCashCommand => new RelayCommand(PaymentCash);
        /// <summary>
        /// Сложаня оплата
        /// </summary>
        public ICommand PaymentComplexCommand => new RelayCommand(PaymentComplex);
        /// <summary>
        /// Удалить выбранную из корзина товара
        /// </summary>
        public ICommand DeleteSelectedRowCommand => new RelayCommand(DeleteSelectedRow);
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
        public ICommand CRMSettingsCommand => new RelayCommand(CRMSettings);
        /// <summary>
        /// Снять отчет без гашения
        /// </summary>
        public ICommand PrintReportWithoutCleaningCommand => new RelayCommand(() => ShtrihM.PrintReportWithoutCleaning());
        /// <summary>
        /// Снять отчет с гашнием
        /// </summary>
        public ICommand PrintReportWithCleaningCommand => new RelayCommand(() => ShtrihM.PrintReportWithCleaning());
        /// <summary>
        /// Открыть смену ККМ
        /// </summary>
        public ICommand OpenSessionCommand => new RelayCommand(() => ShtrihM.OpenSession());
        /// <summary>
        /// Краткий запрос
        /// </summary>
        public ICommand GetShortECRStatusCommand => new RelayCommand(GetShortECRStatus);
        /// <summary>
        /// Анулировать чек
        /// </summary>
        public ICommand CancelCheckCommand => new RelayCommand(() => ShtrihM.SysAdminCancelCheck());
        /// <summary>
        /// Установить текущее время ККМ
        /// </summary>
        public ICommand SetTimeCommand = new RelayCommand(() => ShtrihM.SetTime());
        /// <summary>
        /// Отрезать чек
        /// </summary>
        public ICommand CutCheckCommand => new RelayCommand(() => ShtrihM.CutCheck());
        /// <summary>
        /// 
        /// </summary>
        public ICommand LoadedHomeViewCommand => new ParameterCommand(parameter => LoadedHomeView(parameter));
        /// <summary>
        /// Проверка ввода количестов товаров для продажи
        /// </summary>
        public ICommand QuantityValidateCommand => new ParameterCommand(parameter => QuantityValidate(parameter));
        /// <summary>
        /// Возврат товаров
        /// </summary>
        public ICommand ReturnGoodsCommand => new RelayCommand(ReturnGoods);
        /// <summary>
        /// Отменить
        /// </summary>
        public ICommand CancelCommand => new RelayCommand(Cancel);
        public ICommand SaleTableViewLoadedCommand => new ParameterCommand(parameter => SaleTableViewLoaded(parameter));
        public ICommand MultiplyCommand => new RelayCommand(Multiply);

        #endregion

        #region Constructor

        public HomeViewModel(IUserStore userStore,
            IProductService productService,
            IProductSaleService productSaleService,
            IReceiptService receiptService,
            IUIManager manager,
            IMessageStore messageStore,
            IAuthenticator authenticator,
            IShiftStore shiftStore,
            IRefundService refundService,
            IProductSaleStore productSaleStore,
            IZebraBarcodeScanner zebraBarcodeScanner,
            IComBarcodeService comBarcodeService)
        {
            _userStore = userStore;
            _productService = productService;
            _productSaleService = productSaleService;
            _receiptService = receiptService;
            _manager = manager;
            _messageStore = messageStore;
            _authenticator = authenticator;
            _shiftStore = shiftStore;
            _refundService = refundService;
            _productSaleStore = productSaleStore;
            _zebraBarcodeScanner = zebraBarcodeScanner;
            _comBarcodeService = comBarcodeService;

            SaleProducts = new();
            PostponeReceipts = new List<PostponeReceipt>();

            SaleProductsCollectionView = CollectionViewSource.GetDefaultView(SaleProducts);
            BindingOperations.EnableCollectionSynchronization(SaleProducts, _syncLock);

            PrintXReportCommand = new PrintXReportCommand();

            SaleProducts.CollectionChanged += SaleProducts_CollectionChanged;
            _productService.OnProductSaleOrRefund += ProductService_OnProductSaleOrRefund;
        }

        #endregion

        #region Private Voids

        private async void AddProductToSale(string barcode)
        {
            var newProduct = await _productService.Predicate(p => p.Barcode == barcode && p.Quantity > 0, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, SalePrice = p.SalePrice, TNVED = p.TNVED });
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
                        TNVED = newProduct.TNVED,
                        //Sum = newProduct.SalePrice * 1
                    });
                }
                else if (product.Quantity < product.QuantityInStock)
                {
                    product.Quantity++;
                    //product.Sum = product.SalePrice * (decimal)product.Quantity;
                }
                else
                {
                    _ = _manager.ShowMessage("Количество превышает остаток.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                OnPropertyChanged(nameof(Sum));
                OnPropertyChanged(nameof(ToBePaid));
            }            
        }

        private byte[] FromHex(string hex)
        {
            byte[] raw = new byte[hex.Length / 4];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 4, 4), 16);
            }
            return raw;
        }

        private void GetShortECRStatus()
        {
            ShtrihM.GetShortECRStatus();

            KKMStatusViewModel viewModel = new();

            viewModel.Title = "Краткий запрос состояния";

            viewModel.Status += "----------------------------------------\r";
            viewModel.Status += "Режим:\r";
            viewModel.Status += ShtrihM.GetECRMode() + "\r";
            viewModel.Status += "----------------------------------------\r";

            

            _ = _manager.ShowDialog(viewModel, new KKMStatusView());
        }

        private void ReturnGoods()
        {
            _ = _manager.ShowDialog(new RefundViewModel(_receiptService, _shiftStore, _manager) { Title = "Возврат товаров" },
                new RefundView());
        }

        private void Multiply()
        {
            if (SaleTableView != null)
            {
                if (SaleTableView.Grid.CurrentItem == null)
                {
                    SaleTableView.Grid.CurrentItem = SaleProducts.LastOrDefault();
                }
                SaleTableView.Grid.CurrentColumn = SaleTableView.Grid.Columns[2];                
                SaleTableView.Grid.View.ShowEditor();
            }
        }

        private void SaleTableView_ShownEditor(object sender, EditorEventArgs e)
        {
            SaleTableView.Grid.View.ActiveEditor.SelectAll();            
        }

        private void SaleTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            SaleTableView.Grid.CurrentColumn = null;
            SaleTableView.Grid.CurrentItem = null;
            SelectedProductSale = null;
        }

        private void SaleTableViewLoaded(object parameter)
        {
            if (parameter is RoutedEventArgs e)
            {
                if (e.Source is TableView tableView)
                {
                    SaleTableView = tableView;
                    SaleTableView.ShownEditor += SaleTableView_ShownEditor;
                    SaleTableView.CellValueChanged += SaleTableView_CellValueChanged;
                }
            }
        }        

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

        private async void LoadedHomeView(object parameter)
        {
            ProductsWithoutBarcode = IsKeepRecorsd ? new(await _productService.PredicateSelect(p => p.Quantity > 0 && p.WithoutBarcode == true && p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name, SalePrice = p.SalePrice, ArrivalPrice = p.ArrivalPrice, Quantity = p.Quantity })) : 
                new(await _productService.PredicateSelect(p => p.WithoutBarcode == true && p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name, SalePrice = p.SalePrice, ArrivalPrice = p.ArrivalPrice, Quantity = p.Quantity }));
            if (parameter is RoutedEventArgs e)
            {
                if (e.Source is HomeView homeView)
                {
                    homeView.Focus();
                }
            }
            _zebraBarcodeScanner.Open();
            _zebraBarcodeScanner.OnBarcodeEvent += ZebraBarcodeScanner_OnBarcodeEvent;

            _comBarcodeService.Open();
            _comBarcodeService.OnBarcodeEvent += ComBarcodeService_OnBarcodeEvent;
        }

        private async void ComBarcodeService_OnBarcodeEvent(string barcode)
        {
            BarcodeProductAddToSale(IsKeepRecorsd ? await _productService.Predicate(p => p.Barcode == barcode && p.DeleteMark == false && p.Quantity > 0, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, SalePrice = p.SalePrice, TNVED = p.TNVED }) :
                await _productService.Predicate(p => p.Barcode == barcode && p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, SalePrice = p.SalePrice, TNVED = p.TNVED }));
        }

        private async void ZebraBarcodeScanner_OnBarcodeEvent(string barcode)
        {
            BarcodeProductAddToSale(IsKeepRecorsd ? await _productService.Predicate(p => p.Barcode == barcode && p.DeleteMark == false && p.Quantity > 0, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, SalePrice = p.SalePrice, TNVED = p.TNVED }) :
                await _productService.Predicate(p => p.Barcode == barcode && p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, SalePrice = p.SalePrice, TNVED = p.TNVED }));
        }

        private void BarcodeProductAddToSale(Product product)
        {
            if (product != null)
            {
                var sale = SaleProducts.FirstOrDefault(sp => sp.Id == product.Id);
                if (sale == null)
                {
                    lock (_syncLock)
                    {
                        SaleProducts.Add(new Sale
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Quantity = 1,
                            QuantityInStock = product.Quantity,
                            SalePrice = product.SalePrice,
                            TNVED = product.TNVED,
                            Sum = product.SalePrice * 1
                        });
                    }
                }
                else if (IsKeepRecorsd && sale.Quantity < sale.QuantityInStock)
                {
                    sale.Quantity++;
                }
                else
                {
                    _ = _manager.ShowMessage("Количество превышает остаток.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                OnPropertyChanged(nameof(Sum));
                OnPropertyChanged(nameof(ToBePaid));
            }
        }

        private void ProductService_OnProductSaleOrRefund(int id, double quantity)
        {
            Product editProduct = ProductsWithoutBarcode.FirstOrDefault(p => p.Id == id);
            if (editProduct != null)
            {
                editProduct.Quantity = quantity;
            }            
        }

        private void ProductService_OnProductRefund(int id, double quantity)
        {
            Product editProduct = ProductsWithoutBarcode.FirstOrDefault(p => p.Id == id);
            editProduct.Quantity = quantity;
        }

        private void QuantityValidate(object parameter)
        {
            if (parameter is GridCellValidationEventArgs e)
            {
                if (((Sale)e.Row).QuantityInStock < Convert.ToDouble(e.Value))
                {
                    _ = _manager.ShowMessage("Количество превышает остаток.", "", MessageBoxButton.OK, MessageBoxImage.Information);
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
                Barcode += e.Text;
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
                        p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, SalePrice = p.SalePrice, TNVED = p.TNVED });
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
                                TNVED = newProduct.TNVED,
                                Sum = newProduct.SalePrice,
                            });
                        }
                        else if (product.Quantity < product.QuantityInStock)
                        {
                            product.Quantity++;
                            //product.Sum = newProduct.SalePrice,
                        }
                        else
                        {
                            _ = _manager.ShowMessage("Количество превышает остаток.", "", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    PostponeProducts = SaleProducts.Select(sp => new PostponeProduct { Id = sp.Id, Name = sp.Name, ArrivalPrice = sp.ArrivalPrice, SalePrice = sp.SalePrice, Quantity = sp.Quantity, Sum = sp.Sum }).ToList()
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
                if (await _manager.ShowDialog(new PostponeReceiptViewModel(this, _manager, _productService) { Title = "Выбор чека" }, new PostponeReceiptView()))
                {

                }
            }            
        }

        /// <summary>
        /// Штрих-коду жок товарларды басканда
        /// </summary>
        /// <param name="parameter">Товардын коду</param>
        private async void AddProductToSale(object parameter)
        {
            if (parameter is int id)
            {
                var saleProduct = SaleProducts.FirstOrDefault(sp => sp.Id == id);
                if (saleProduct == null)
                {
                    Product getProduct = IsKeepRecorsd ? await _productService.Predicate(p => p.Id == id && p.DeleteMark == false && p.Quantity > 0, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, SalePrice = p.SalePrice, TNVED = p.TNVED }) :
                            await _productService.Predicate(p => p.Id == id && p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, SalePrice = p.SalePrice, TNVED = p.TNVED });
                    SaleProducts.Add(new Sale
                    {
                        Id = getProduct.Id,
                        Name = getProduct.Name,
                        Quantity = 1,
                        SalePrice = getProduct.SalePrice,
                        ArrivalPrice = getProduct.ArrivalPrice,
                        Sum = getProduct.SalePrice,
                        QuantityInStock = getProduct.Quantity,
                        TNVED = getProduct.TNVED
                    });
                    _productSaleStore.AddProduct(new Sale
                    {
                        Id = getProduct.Id,
                        Name = getProduct.Name,
                        Quantity = 1,
                        SalePrice = getProduct.SalePrice,
                        ArrivalPrice = getProduct.ArrivalPrice,
                        Sum = getProduct.SalePrice,
                        QuantityInStock = getProduct.Quantity,
                        TNVED = getProduct.TNVED
                    });
                }
                else
                {
                    if (IsKeepRecorsd)
                    {
                        if (saleProduct.Quantity < saleProduct.QuantityInStock)
                        {
                            saleProduct.Quantity++;
                        }
                        else
                        {
                            _ = _manager.ShowMessage("Количество превышает остаток.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        saleProduct.Quantity++;
                    }
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
                PaymentCashViewModel _paymentCashViewModel = new(_receiptService, _productSaleService, _userStore, _manager, _shiftStore, _productSaleStore ) 
                { 
                    Title = "Оплата наличными",
                    SaleProducts = SaleProducts.ToList()
                };

                if (await _manager.ShowDialog(_paymentCashViewModel,
                new PaymentCashView()))
                {
                    SaleProducts.Clear();
                }
            }
        }

        private async void PaymentComplex()
        {
            if (SaleProducts.Count > 0)
            {
                PaymentComplexViewModel _paymentComplexViewModel = new(_receiptService, _manager, _shiftStore, _userStore) 
                { 
                    Title = "Оплата чека",
                    SaleProducts = SaleProducts.ToList()
                };
                
                if (await _manager.ShowDialog(_paymentComplexViewModel,
                new PaymentComplexView()))
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

        /// <summary>
        /// Удалить выбранный товар из корзины
        /// </summary>
        private void DeleteSelectedRow()
        {
            if (SelectedProductSale != null)
            {
                SaleProducts.Remove(SelectedProductSale);
            }
            else if (SaleProducts.Count > 0)
            {
                SaleProducts.Remove(SaleProducts.LastOrDefault());
            }
        }

        /// <summary>
        /// Настройки ККМ
        /// </summary>
        private void CRMSettings()
        {
            //await _manager.ShowDialog(new CommunicationSettingsViewModel() { Title = "Настройка связи с ККМ" }, new CommunicationSettingsView());            
            ShtrihM.ShowProperties();
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

        #endregion

        #region Dispose

        public override void Dispose()
        {
            try
            {
                //_barcodeScanner.Close(0, out int status);
            }
            catch (Exception)
            {
                //ignore
            }            
            base.Dispose();
        }

        #endregion
    }
}
