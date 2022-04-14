using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Barcode.Services;
using RetailTrade.CashRegisterMachine;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.Properties;
using RetailTradeClient.State.Authenticators;
using RetailTradeClient.State.ProductSales;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using RetailTradeClient.ViewModels.Base;
using RetailTradeClient.ViewModels.Components;
using RetailTradeClient.ViewModels.Dialogs;
using RetailTradeClient.Views;
using RetailTradeClient.Views.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;

namespace RetailTradeClient.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IUserStore _userStore;
        private readonly IReceiptService _receiptService;
        private readonly IAuthenticator _authenticator;
        private readonly IShiftStore _shiftStore;
        private readonly IProductSaleStore _productSaleStore;
        private readonly IBarcodeService _barcodeService;
        private readonly PaymentCashViewModel _paymentCashViewModel;
        private readonly PaymentComplexViewModel _paymentComplexViewModel;
        private string _barcode;
        private Sale _selectedProductSale;
        private object _syncLock = new();
        private MainWindow _mainWindow;
        private decimal _change;
        private bool _isDiscountPercent = true;

        private const int HOTKEY_F5 = 1;
        private const int HOTKEY_F6 = 2;
        private const int HOTKEY_F7 = 3;
        private const int HOTKEY_ALT_F5 = 4;
        private const int HOTKEY_CTRL_F5 = 5;
        private const int HOTKEY_CTRL_F = 6;
        private const int HOTKEY_ESC = 7;
        private const int HOTKEY_CTRL_Z = 8;
        private const int HOTKEY_DEL = 9;
        //Modifiers:
        private const uint MOD_NONE = 0x0000; //(none)
        private const uint MOD_ALT = 0x0001; //ALT
        private const uint MOD_CONTROL = 0x0002; //CTRL
        private const uint MOD_SHIFT = 0x0004; //SHIFT
        private const uint MOD_WIN = 0x0008; //WINDOWS
        //CAPS LOCK:
        private const uint VK_F5 = 0x74;
        private const uint VK_F6 = 0x75;
        private const uint VK_F7 = 0x76;
        private const uint VK_CTRL_F = 0x46;
        private const uint VK_ESCAPE = 0x1B;
        private const uint VK_CTRL_Z = 0x5A;
        private const uint VK_DEL = 0x2E;
        private IntPtr _windowHandle;
        private HwndSource _source;

        #endregion

        #region Static

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        #endregion

        #region Public Properties

        public ObservableCollection<Sale> ProductSales => _productSaleStore.Sales;
        public ICollectionView SaleProductsCollectionView { get; set; }
        public bool IsKeepRecords => Settings.Default.IsKeepRecords;
        public decimal Sum
        {
            get => _productSaleStore.ToBePaid;
            set { }
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
        public ObservableCollection<PostponeReceipt> PostponeReceipts => _productSaleStore.PostponeReceipts;
        public Sale SelectedProductSale
        {
            get => _selectedProductSale;
            set
            {
                _selectedProductSale = value;
                OnPropertyChanged(nameof(SelectedProductSale));
            }
        }
        public int FocusedRowHandle => ProductSales.Count - 1;
        public TableView SaleTableView { get; set; }
        public ProductsWithoutBarcodeViewModel ProductsWithoutBarcodeViewModel { get; }
        public bool IsDiscountPercent
        {
            get => _isDiscountPercent;
            set
            {
                _isDiscountPercent = value;
                OnPropertyChanged(nameof(IsDiscountPercent));
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        public ICommand LogoutCommand => new RelayCommand(Logout);
        /// <summary>
        /// Настройки принтера
        /// </summary>
        public ICommand PrinterSettingsCommand => new RelayCommand(PrinterSettings);
        /// <summary>
        /// Отложить чек
        /// </summary>
        public ICommand PostponeReceiptCommand => new RelayCommand(() => _productSaleStore.CreatePostponeReceipt());
        /// <summary>
        /// Просмотр отложенных чеков
        /// </summary>
        public ICommand OpenPostponeReceiptCommand => new RelayCommand(OpenPostponeReceipt);
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
        public ICommand PrintXReportCommand => new PrintXReportCommand();
        /// <summary>
        /// Настройки ККМ
        /// </summary>
        public ICommand CRMSettingsCommand => new RelayCommand(CRMSettings);
        /// <summary>
        /// Снять отчет без гашения
        /// </summary>
        public static ICommand PrintReportWithoutCleaningCommand => new RelayCommand(() => ShtrihM.PrintReportWithoutCleaning());
        /// <summary>
        /// Снять отчет с гашнием
        /// </summary>
        public static ICommand PrintReportWithCleaningCommand => new RelayCommand(() => ShtrihM.PrintReportWithCleaning());
        /// <summary>
        /// Открыть смену ККМ
        /// </summary>
        public static ICommand OpenSessionCommand => new RelayCommand(() => ShtrihM.OpenSession());
        /// <summary>
        /// Краткий запрос
        /// </summary>
        public ICommand GetShortECRStatusCommand => new RelayCommand(GetShortECRStatus);
        /// <summary>
        /// Анулировать чек
        /// </summary>
        public static ICommand CancelCheckCommand => new RelayCommand(() => ShtrihM.SysAdminCancelCheck());
        /// <summary>
        /// Установить текущее время ККМ
        /// </summary>
        public ICommand SetTimeCommand = new RelayCommand(() => ShtrihM.SetTime());
        /// <summary>
        /// Отрезать чек
        /// </summary>
        public static ICommand CutCheckCommand => new RelayCommand(() => ShtrihM.CutCheck());
        /// <summary>
        /// 
        /// </summary>
        public ICommand LoadedHomeViewCommand => new ParameterCommand(parameter => LoadedHomeView(parameter));
        /// <summary>
        /// Проверка ввода количестов товаров для продажи
        /// </summary>
        public static ICommand QuantityValidateCommand => new ParameterCommand(parameter => QuantityValidate(parameter));
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
            IReceiptService receiptService,
            IAuthenticator authenticator,
            IShiftStore shiftStore,
            IProductSaleStore productSaleStore,
            IBarcodeService barcodeService,
            ProductsWithoutBarcodeViewModel productsWithoutBarcodeViewModel,
            PaymentCashViewModel paymentCashViewModel,
            PaymentComplexViewModel paymentComplexViewModel, 
            MainWindow mainWindow)
        {
            _userStore = userStore;
            _receiptService = receiptService;
            _authenticator = authenticator;
            _shiftStore = shiftStore;
            _productSaleStore = productSaleStore;
            _barcodeService = barcodeService;
            _paymentCashViewModel = paymentCashViewModel;
            _paymentComplexViewModel = paymentComplexViewModel;
            _mainWindow = mainWindow;

            ProductsWithoutBarcodeViewModel = productsWithoutBarcodeViewModel;

            SaleProductsCollectionView = CollectionViewSource.GetDefaultView(ProductSales);
            BindingOperations.EnableCollectionSynchronization(ProductSales, _syncLock);

            _productSaleStore.OnProductSalesChanged += () => OnPropertyChanged(nameof(Sum));
            _productSaleStore.OnProductSale += ProductSaleStore_OnProductSale;
            _productSaleStore.OnPostponeReceiptChanged += ProductSaleStore_OnPostponeReceiptChanged;
            _productSaleStore.OnCreated += ProductSaleStore_OnCreated;
        }

        #endregion

        #region Private Voids

        private void ProductSaleStore_OnCreated(Sale sale)
        {
            try
            {
                SelectedProductSale = sale;
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void ProductSaleStore_OnPostponeReceiptChanged()
        {
            OnPropertyChanged(nameof(ProductSales));
            OnPropertyChanged(nameof(PostponeReceipts));
            OnPropertyChanged(nameof(Sum));            
        }

        private void ProductSaleStore_OnProductSale(decimal change)
        {
            Change = change;
            OnPropertyChanged(nameof(ToBePaid));
            OnPropertyChanged(nameof(Sum));
        }

        private void BarcodeOpen()
        {
            if (Enum.IsDefined(typeof(BarcodeDevice), Settings.Default.BarcodeDefaultDevice))
            {
                BarcodeDevice barcodeDevice = Enum.Parse<BarcodeDevice>(Settings.Default.BarcodeDefaultDevice);
                if (barcodeDevice == BarcodeDevice.Com)
                {
                    _barcodeService.Open(barcodeDevice, Settings.Default.BarcodeCom, Settings.Default.BarcodeSpeed);
                }
            }
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

            

            WindowService.Show(nameof(KKMStatusView), viewModel);
        }

        private void ReturnGoods()
        {
            WindowService.Show(nameof(RefundView), new RefundViewModel(_receiptService, _shiftStore) { Title = "Возврат товаров" });
        }

        private void Multiply()
        {
            if (SaleTableView != null)
            {
                if (SaleTableView.Grid.CurrentItem == null)
                {
                    SaleTableView.Grid.CurrentItem = ProductSales.LastOrDefault();
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

        private void LoadedHomeView(object parameter)
        {
            if (parameter is RoutedEventArgs e)
            {
                if (e.Source is HomeView homeView)
                {
                    homeView.Unloaded += HomeView_Unloaded;
                    _windowHandle = new WindowInteropHelper(_mainWindow).Handle;
                    _source = HwndSource.FromHwnd(_windowHandle);
                    _source.AddHook(HwndHook);

                    RegisterHotKey(_windowHandle, HOTKEY_F5, MOD_NONE, VK_F5); //+
                    RegisterHotKey(_windowHandle, HOTKEY_F6, MOD_NONE, VK_F6); //+
                    RegisterHotKey(_windowHandle, HOTKEY_F7, MOD_NONE, VK_F7); //+
                    RegisterHotKey(_windowHandle, HOTKEY_ALT_F5, MOD_ALT, VK_F5); //+
                    RegisterHotKey(_windowHandle, HOTKEY_CTRL_F5, MOD_CONTROL, VK_F5); //+
                    RegisterHotKey(_windowHandle, HOTKEY_CTRL_F, MOD_CONTROL, VK_CTRL_F); //+
                    RegisterHotKey(_windowHandle, HOTKEY_ESC, MOD_NONE, VK_ESCAPE); //+
                    RegisterHotKey(_windowHandle, HOTKEY_CTRL_Z, MOD_CONTROL, VK_CTRL_Z); //+
                    RegisterHotKey(_windowHandle, HOTKEY_DEL, MOD_NONE, VK_DEL); //+
                }
            }

            BarcodeOpen();
        }

        private void HomeView_Unloaded(object sender, RoutedEventArgs e)
        {
            _source.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HOTKEY_F5);
            UnregisterHotKey(_windowHandle, HOTKEY_F6);
            UnregisterHotKey(_windowHandle, HOTKEY_F7);
            UnregisterHotKey(_windowHandle, HOTKEY_ALT_F5);
            UnregisterHotKey(_windowHandle, HOTKEY_CTRL_F5);
            UnregisterHotKey(_windowHandle, HOTKEY_CTRL_F);
            UnregisterHotKey(_windowHandle, HOTKEY_ESC);
            UnregisterHotKey(_windowHandle, HOTKEY_CTRL_Z);
            UnregisterHotKey(_windowHandle, HOTKEY_DEL);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            try
            {
                const int WM_HOTKEY = 0x0312;
                switch (msg)
                {
                    case WM_HOTKEY:
                        int vkey = ((int)lParam >> 16) & 0xFFFF;
                        switch (wParam.ToInt32())
                        {
                            case HOTKEY_F5:                                
                                if (vkey == VK_F5)
                                {
                                    PaymentCash();
                                }
                                handled = true;
                                break;
                            case HOTKEY_F6:
                                if (vkey == VK_F6)
                                {
                                    PaymentComplex();
                                }
                                handled = true;
                                break;
                            case HOTKEY_F7:
                                if (vkey == VK_F7)
                                {
                                    ReturnGoods();
                                }
                                handled = true;
                                break;
                            case HOTKEY_ALT_F5:
                                if (vkey == VK_F5)
                                {
                                    _productSaleStore.CreatePostponeReceipt();
                                }
                                handled = true;
                                break;
                            case HOTKEY_CTRL_F5:
                                if (vkey == VK_F5)
                                {
                                    OpenPostponeReceipt();
                                }
                                handled = true;
                                break;
                            case HOTKEY_CTRL_F:
                                if (vkey == VK_CTRL_F)
                                {
                                    Search();
                                }
                                handled = true;
                                break;
                            case HOTKEY_ESC:
                                if (vkey == VK_ESCAPE)
                                {
                                    Logout();
                                }
                                handled = true;
                                break;
                            case HOTKEY_CTRL_Z:
                                if (vkey == VK_CTRL_Z)
                                {
                                    Cancel();
                                }
                                handled = true;
                                break;
                            case HOTKEY_DEL:
                                if (vkey == VK_DEL)
                                {
                                    DeleteSelectedRow();
                                }
                                handled = true;
                                break;
                        }
                        break;
                }
            }
            catch (Exception)
            {
                //ignore
            }
            return IntPtr.Zero;
        }

        private static void QuantityValidate(object parameter)
        {
            if (parameter is GridCellValidationEventArgs e)
            {
                if (((Sale)e.Row).QuantityInStock < Convert.ToDouble(e.Value))
                {
                    _ = MessageBox.Show("Количество превышает остаток.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    e.ErrorContent = "Количество превышает остаток.";
                    e.IsValid = false;
                }
            }
        }

        /// <summary>
        /// Принтерлерди настройкалоо
        /// </summary>
        private void PrinterSettings()
        {
            WindowService.Show(nameof(PrinterView), new PrinterViewModel() { Title = "Настройка принтеров" });
        }

        /// <summary>
        /// Просмотр отложенных чеков
        /// </summary>
        private void OpenPostponeReceipt()
        {
            if (PostponeReceipts.Any() && !ProductSales.Any())
            {
                WindowService.Show(nameof(PostponeReceiptView), new PostponeReceiptViewModel(_productSaleStore) { Title = "Выбор чека" });
            }            
        }

        /// <summary>
        /// Оплата наличными
        /// </summary>
        private void PaymentCash()
        {
            if (ProductSales.Count > 0)
            {
                WindowService.Show(nameof(PaymentCashView), _paymentCashViewModel);
            }
        }

        private void PaymentComplex()
        {
            if (ProductSales.Count > 0)
            {
                WindowService.Show(nameof(PaymentComplexView), _paymentComplexViewModel);
            }
        }

        /// <summary>
        /// Выйти из аккаунта
        /// </summary>
        private void Logout()
        {
            if (MessageBoxService.ShowMessage("Выйти?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
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
                _productSaleStore.DeleteProduct(SelectedProductSale.Id);                
            }
        }

        /// <summary>
        /// Настройки ККМ
        /// </summary>
        private void CRMSettings()
        {
            ShtrihM.ShowProperties();
        }

        /// <summary>
        /// Отменить
        /// </summary>
        private void Cancel()
        {
            _productSaleStore.Sales.Clear();            
        }

        #endregion

        #region Public Voids

        [Command]
        public void Search()
        {
            WindowService.Show(nameof(ProductView), _productSaleStore.SearchProduct());
        }

        [Command]
        public void DiscountType()
        {
            IsDiscountPercent = !IsDiscountPercent;
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
