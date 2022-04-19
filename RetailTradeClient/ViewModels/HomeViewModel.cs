using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using RetailTrade.Barcode.Services;
using RetailTrade.CashRegisterMachine;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.Customs;
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private readonly IBarcodeService _barcodeService;
        private readonly PaymentCashViewModel _paymentCashViewModel;
        private readonly PaymentComplexViewModel _paymentComplexViewModel;
        private readonly ProductViewModel _productViewModel;
        private readonly MainWindow _mainWindow;
        private ObservableCollection<Sale> _productSales = new();
        private ObservableCollection<PostponeReceipt> _postponeReceipts = new();
        private string _barcode;
        private Sale _selectedProductSale;
        private object _syncLock = new();
        private decimal _change;
        private decimal _cashPaySum;
        private decimal _cashlessPaySum;

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

        public ObservableCollection<Sale> ProductSales => _productSales;
        public ICollectionView SaleProductsCollectionView => CollectionViewSource.GetDefaultView(ProductSales);
        public bool IsKeepRecords => Settings.Default.IsKeepRecords;
        public decimal AmountWithoutDiscount => ProductSales.Sum(s => s.AmountWithoutDiscount);
        public decimal DiscountAmount => ProductSales.Sum(s => s.DiscountAmount);
        public decimal Total => ProductSales.Sum(s => s.Total);
        public decimal Change
        {
            get => _change;
            set
            {
                _change = value;
                OnPropertyChanged(nameof(Change));
            }
        }
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
        public ObservableCollection<PostponeReceipt> PostponeReceipts
        {
            get => _postponeReceipts;
            set
            {
                _postponeReceipts = value;
                OnPropertyChanged(nameof(PostponeReceipts));
            }
        }
        public Sale SelectedProductSale
        {
            get => _selectedProductSale;
            set
            {
                _selectedProductSale = value;
                OnPropertyChanged(nameof(SelectedProductSale));
                OnPropertyChanged(nameof(MaximumDiscount));
            }
        }
        public int FocusedRowHandle => ProductSales.Count - 1;
        public TableView SaleTableView { get; set; }
        public TextEdit CashPayTextEdit { get; set; }
        public TextEdit CashlessPayTextEdit { get; set; }
        public decimal CashPaySum
        {
            get => _cashPaySum;
            set
            {
                _cashPaySum = value;
                OnPropertyChanged(nameof(CashPaySum));
            }
        }
        public decimal CashlessPaySum
        {
            get => _cashlessPaySum;
            set
            {
                _cashlessPaySum = value;
                OnPropertyChanged(nameof(CashlessPaySum));
            }
        }
        public double MaximumPercentage => Settings.Default.MaximumPercentage;
        public decimal MaximumDiscount => GetMaximumDiscount();

        #endregion

        #region Commands

        /// <summary>
        /// Закрыть смену
        /// </summary>
        public ICommand ClosingShiftCommand { get; }

        /// <summary>
        /// Распечатать х-отчет
        /// </summary>
        public ICommand PrintXReportCommand => new PrintXReportCommand();

        #endregion        

        #region Constructor

        public HomeViewModel(IUserStore userStore,
            IReceiptService receiptService,
            IAuthenticator authenticator,
            IShiftStore shiftStore,
            IBarcodeService barcodeService,
            PaymentCashViewModel paymentCashViewModel,
            PaymentComplexViewModel paymentComplexViewModel,
            ProductViewModel productViewModel,
            MainWindow mainWindow)
        {
            _userStore = userStore;
            _receiptService = receiptService;
            _authenticator = authenticator;
            _shiftStore = shiftStore;
            _barcodeService = barcodeService;
            _paymentCashViewModel = paymentCashViewModel;
            _paymentComplexViewModel = paymentComplexViewModel;
            _productViewModel = productViewModel;
            _mainWindow = mainWindow;

            BindingOperations.EnableCollectionSynchronization(ProductSales, _syncLock);

            SaleProductsCollectionView.CollectionChanged += SaleProductsCollectionView_CollectionChanged;
            _productViewModel.OnProductsSelected += ProductViewModel_OnProductsSelected;
        }

        #endregion

        #region Private Voids

        private void ProductViewModel_OnProductsSelected(IEnumerable<Product> products)
        {
            try
            {
                foreach (Product product in products)
                {
                    Sale sale = ProductSales.FirstOrDefault(s => s.Id == product.Id);
                    if (sale != null)
                    {
                        if (Settings.Default.IsKeepRecords)
                        {
                            if (sale.QuantityInStock < sale.Quantity + 1)
                            {
                                //_ = MessageBox.Show("Количество превышает остаток.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                sale.Quantity++;
                            }
                        }
                    }
                    else
                    {
                        AddProductSale(new Sale
                        {
                            Id = product.Id,
                            Name = product.Name,
                            SalePrice = product.SalePrice,
                            ArrivalPrice = product.ArrivalPrice,
                            QuantityInStock = IsKeepRecords ? product.Quantity : 0,
                            TNVED = product.TNVED,
                            Quantity = 1,
                            Barcode = product.Barcode,
                            UnitName = string.Empty
                        });
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void AddProductSale(Sale sale)
        {
            try
            {
                ProductSales.Add(sale);
                SelectedProductSale = sale;
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void SaleProductsCollectionView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(AmountWithoutDiscount));
            OnPropertyChanged(nameof(DiscountAmount));
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(ProductSales));
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is Sale sale)
                    {
                        sale.PropertyChanged += Sale_PropertyChanged;
                    }
                }
            }
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is Sale sale)
                    {
                        sale.PropertyChanged -= Sale_PropertyChanged;
                    }
                }
            }
        }

        private void Sale_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(AmountWithoutDiscount));
            OnPropertyChanged(nameof(DiscountAmount));
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(ProductSales));
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
                                    //PaymentCash();
                                }
                                handled = true;
                                break;
                            case HOTKEY_F6:
                                if (vkey == VK_F6)
                                {
                                    //PaymentComplex();
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
                                    //_productSaleStore.CreatePostponeReceipt();
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

        private void CashPayTextEdit_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            Change = CashPaySum - Total;
        }

        private decimal GetMaximumDiscount()
        {
            try
            {
                return (decimal)MaximumPercentage * SelectedProductSale.SalePrice;
            }
            catch (Exception)
            {
                //ignore
            }
            return 0;
        }

        #endregion

        #region Public Voids

        [Command]
        public void Search()
        {
            WindowService.Show(nameof(ProductView), _productViewModel);
        }

        [Command]
        public void DiscountType()
        {
            try
            {
                SelectedProductSale.IsDiscountPercentage = !SelectedProductSale.IsDiscountPercentage;
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void CashPayLoaded(object sender)
        {
            try
            {
                if (sender is RoutedEventArgs e)
                {
                    if (e.Source is TextEdit textEdit)
                    {
                        CashPayTextEdit = textEdit;
                        CashPayTextEdit.EditValueChanged += CashPayTextEdit_EditValueChanged;
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void CashlessPayLoaded(object sender)
        {
            try
            {
                if (sender is RoutedEventArgs e)
                {
                    if (e.Source is TextEdit textEdit)
                    {
                        CashlessPayTextEdit = textEdit;
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void CashPay()
        {
            try
            {
                if (CashPayTextEdit != null)
                {
                    CashPayTextEdit.EditValueChanged -= CashPayTextEdit_EditValueChanged;
                    _ = CashPayTextEdit.Focus();
                    CashPayTextEdit.Text = Total.ToString();
                    CashPayTextEdit.SelectAll();
                    CashPayTextEdit.EditValueChanged += CashPayTextEdit_EditValueChanged;
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void CashlessPay()
        {
            try
            {
                if (CashlessPayTextEdit != null)
                {
                    _ = CashlessPayTextEdit.Focus();
                    CashlessPayTextEdit.SelectAll();
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void PunchReceipt()
        {

        }

        [Command]
        public void EditValueChanged(object value)
        {
            try
            {
                if (double.TryParse(value.ToString(), out double quantity))
                {
                    SelectedProductSale.Quantity = quantity;
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        /// <summary>
        /// Выйти из аккаунта
        /// </summary>
        [Command]
        public void Logout()
        {
            if (MessageBoxService.ShowMessage("Выйти?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
            {
                _authenticator.Logout();
            }
        }

        /// <summary>
        /// Принтерлерди настройкалоо
        /// </summary>
        [Command]
        public void PrinterSettings()
        {
            WindowService.Show(nameof(PrinterView), new PrinterViewModel() { Title = "Настройка принтеров" });
        }

        /// <summary>
        /// Просмотр отложенных чеков
        /// </summary>
        [Command]
        public void OpenPostponeReceipt()
        {
            if (PostponeReceipts.Any() && !ProductSales.Any())
            {
                //WindowService.Show(nameof(PostponeReceiptView), new PostponeReceiptViewModel(_productSaleStore) { Title = "Выбор чека" });
            }
        }

        /// <summary>
        /// Удалить выбранный товар из корзины
        /// </summary>
        [Command]
        public void DeleteSelectedRow()
        {
            try
            {
                if (SelectedProductSale != null)
                {
                    ProductSales.Remove(SelectedProductSale);
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        /// <summary>
        /// Настройки ККМ
        /// </summary>
        [Command]
        public void CRMSettings()
        {
            try
            {
                ShtrihM.ShowProperties();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void PrintReportWithoutCleaning()
        {
            try
            {
                _ = ShtrihM.PrintReportWithoutCleaning();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void PrintReportWithCleaning()
        {
            try
            {
                _ = ShtrihM.PrintReportWithCleaning();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void OpenSession()
        {
            try
            {
                _ = ShtrihM.OpenSession();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void CancelCheck()
        {
            try
            {
                _ = ShtrihM.SysAdminCancelCheck();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void SetTime()
        {
            try
            {
                ShtrihM.SetTime();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void CutCheck()
        {
            try
            {
                _ = ShtrihM.CutCheck();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void LoadedHomeView(object parameter)
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

        [Command]
        public void QuantityValidate(object parameter)
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

        [Command]
        public void ReturnGoods()
        {
            WindowService.Show(nameof(RefundView), new RefundViewModel(_receiptService, _shiftStore) { Title = "Возврат товаров" });
        }

        /// <summary>
        /// Отменить
        /// </summary>
        [Command]
        public void Cancel()
        {
            try
            {
                ProductSales.Clear();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void SaleTableViewLoaded(object parameter)
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

        [Command]
        public void DiscountEditValueChanged()
        {
            try
            {
                if (SelectedProductSale.IsDiscountPercentage)
                {
                    SelectedProductSale.DiscountAmount = decimal.Round(SelectedProductSale.SalePrice * (decimal)SelectedProductSale.DiscountPercent, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    SelectedProductSale.DiscountPercent = (double)SelectedProductSale.DiscountAmount / (double)SelectedProductSale.SalePrice;
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void DiscountValidate(object sender)
        {
            if (sender is ValidationEventArgs e)
            {
                if (!e.IsValid)
                {

                }
            }
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
