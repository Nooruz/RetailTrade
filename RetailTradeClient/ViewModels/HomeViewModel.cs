using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting;
using RetailTrade.Barcode.Services;
using RetailTrade.CashRegisterMachine;
using RetailTrade.CashRegisterMachine.Services;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Customs;
using RetailTradeClient.Properties;
using RetailTradeClient.Report;
using RetailTradeClient.State.Authenticators;
using RetailTradeClient.State.Reports;
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
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;

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
        private readonly IReportService _reportService;
        private readonly IProductService _productService;
        private readonly ICashRegisterMachineService _cashRegisterMachineService;
        private readonly PaymentCashViewModel _paymentCashViewModel;
        private readonly PaymentComplexViewModel _paymentComplexViewModel;
        private readonly PostponeReceiptViewModel _postponeReceiptViewModel;
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
        private const int HOTKEY_F8 = 9;
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
        private const uint VK_F8 = 0x77;
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

        public Visibility ManualDiscountVisibility => Settings.Default.IsUseManualDiscounts ? Visibility.Visible : Visibility.Collapsed;
        public ObservableCollection<Sale> ProductSales => _productSales;
        public ICollectionView SaleProductsCollectionView => CollectionViewSource.GetDefaultView(ProductSales);
        public bool IsKeepRecords => Settings.Default.IsKeepRecords;
        public decimal AmountWithoutDiscount => ProductSales.Sum(s => s.AmountWithoutDiscount);
        public decimal DiscountAmount => ProductSales.Sum(s => s.DiscountAmount);
        public decimal Total => ProductSales.Sum(s => s.Total);
        public decimal Change => (Total - CashPaySum - CashlessPaySum) < 0 ? (Total - CashPaySum - CashlessPaySum) * -1 : (Total - CashPaySum - CashlessPaySum);
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
                OnPropertyChanged(nameof(IsSelectedProductSale));
                QuantitySpin();
            }
        }
        public int FocusedRowHandle => ProductSales.Count - 1;
        public TableView SaleTableView { get; set; }
        public TextEdit CashPayTextEdit { get; set; }
        public TextEdit CashlessPayTextEdit { get; set; }
        public UnitSpinEdit QuantitySpinEdit { get; set; }
        public string ChangeLabel => CashPaySum + CashlessPaySum - Total < 0 ? "Осталось доплатить:" : "Сдача:";
        public Brush ChangeForeground => CashPaySum + CashlessPaySum - Total < 0 ? Brushes.Red : Brushes.Green;
        public decimal CashPaySum
        {
            get => _cashPaySum;
            set
            {
                _cashPaySum = value;
                OnPropertyChanged(nameof(CashPaySum));
                OnPropertyChanged(nameof(Change));
                OnPropertyChanged(nameof(ChangeLabel));
                OnPropertyChanged(nameof(ChangeForeground));
                OnPropertyChanged(nameof(CanPunchReceipt));
            }
        }
        public decimal CashlessPaySum
        {
            get => _cashlessPaySum;
            set
            {
                _cashlessPaySum = value;
                OnPropertyChanged(nameof(CashlessPaySum));
                OnPropertyChanged(nameof(Change));
                OnPropertyChanged(nameof(ChangeLabel));
                OnPropertyChanged(nameof(ChangeForeground));
                OnPropertyChanged(nameof(CanPunchReceipt));
            }
        }
        public double MaximumPercentage => Settings.Default.MaximumPercentage;
        public decimal MaximumDiscount => GetMaximumDiscount();
        public bool IsSelectedProductSale => SelectedProductSale != null;
        public string UserName => GetUserName();
        public bool CanPunchReceipt => (CashPaySum + CashlessPaySum) > 0 && CashPaySum + CashlessPaySum - Total >= 0;

        #endregion

        #region Commands

        /// <summary>
        /// Закрыть смену
        /// </summary>
        public ICommand ClosingShiftCommand { get; }

        #endregion        

        #region Constructor

        public HomeViewModel(IUserStore userStore,
            IReceiptService receiptService,
            IAuthenticator authenticator,
            IShiftStore shiftStore,
            IBarcodeService barcodeService,
            IProductService productService,
            ICashRegisterMachineService cashRegisterMachineService,
            PaymentCashViewModel paymentCashViewModel,
            PaymentComplexViewModel paymentComplexViewModel,
            ProductViewModel productViewModel,
            MainWindow mainWindow,
            IReportService reportService)
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
            _productService = productService;
            _reportService = reportService;
            _cashRegisterMachineService = cashRegisterMachineService;
            _postponeReceiptViewModel = new() { Title = "Выбор чека" };

            BindingOperations.EnableCollectionSynchronization(ProductSales, _syncLock);

            SaleProductsCollectionView.CollectionChanged += SaleProductsCollectionView_CollectionChanged;
            _productViewModel.OnProductsSelected += ProductViewModel_OnProductsSelected;
            _postponeReceiptViewModel.OnResume += PostponeReceiptViewModel_OnResume;
        }

        #endregion

        #region Private Voids

        private void PostponeReceiptViewModel_OnResume(PostponeReceipt postponeReceipt)
        {
            try
            {
                ProductSales.Clear();
                postponeReceipt.Sales.ForEach(sale =>
                {
                    AddProductSale(sale);
                });
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private string GetUserName()
        {
            try
            {
                return _userStore.CurrentUser.FullName;
            }
            catch (Exception)
            {
                //ignore
            }
            return string.Empty;
        }

        private void ProductViewModel_OnProductsSelected(IEnumerable<Sale> sales)
        {
            try
            {
                foreach (Sale item in sales)
                {
                    Sale sale = ProductSales.FirstOrDefault(s => s.Id == item.Id);
                    if (sale != null)
                    {
                        sale.Quantity = item.Quantity;
                    }
                    else
                    {
                        AddProductSale(new Sale
                        {
                            Id = item.Id,
                            Name = item.Name,
                            SalePrice = item.SalePrice,
                            ArrivalPrice = item.ArrivalPrice,
                            QuantityInStock = IsKeepRecords ? item.QuantityInStock : 0,
                            TNVED = item.TNVED,
                            Quantity = item.Quantity,
                            Barcode = item.Barcode,
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
                QuantitySpin();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void SaleProductsCollectionView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
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
            OnPropertyChanged(nameof(AmountWithoutDiscount));
            OnPropertyChanged(nameof(DiscountAmount));
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(ProductSales));
        }

        private void Sale_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {            
            OnPropertyChanged(nameof(AmountWithoutDiscount));
            OnPropertyChanged(nameof(DiscountAmount));
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(ProductSales));
            if (e.PropertyName == nameof(Sale.Quantity))
            {

            }
        }

        private void BarcodeOpen()
        {
            try
            {
                if (Enum.IsDefined(typeof(BarcodeDevice), Settings.Default.BarcodeDefaultDevice))
                {
                    _barcodeService.Open(Enum.Parse<BarcodeDevice>(Settings.Default.BarcodeDefaultDevice));
                }
            }
            catch (Exception)
            {
                //ignore
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
            try
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
                UnregisterHotKey(_windowHandle, HOTKEY_F8);
                _barcodeService.OnBarcodeEvent -= BarcodeService_OnBarcodeEvent;
                if (Enum.IsDefined(typeof(BarcodeDevice), Settings.Default.BarcodeDefaultDevice))
                {
                    _barcodeService.Close(Enum.Parse<BarcodeDevice>(Settings.Default.BarcodeDefaultDevice));
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private async void BarcodeService_OnBarcodeEvent(string barcode)
        {
            try
            {
                if (Settings.Default.IsKeepRecords)
                {
                    Sale sale = ProductSales.FirstOrDefault(s => s.Barcode == barcode);
                    if (sale != null)
                    {
                        SelectedProductSale = sale;
                        if (SelectedProductSale.QuantityInStock > SelectedProductSale.Quantity + 1)
                        {
                            SelectedProductSale.Quantity++;
                            ShowEditor(2);
                        }
                    }
                    else
                    {
                        Product product = await _productService.GetByBarcodeAsync(barcode);
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
                else
                {
                    Sale sale = ProductSales.FirstOrDefault(s => s.Barcode == barcode);
                    if (sale != null)
                    {
                        SelectedProductSale = sale;
                        SelectedProductSale.Quantity++;
                        ShowEditor(2);
                    }
                    else
                    {
                        Product product = await _productService.GetByBarcodeAsync(barcode);
                        AddProductSale(new Sale
                        {
                            Id = product.Id,
                            Name = product.Name,
                            SalePrice = product.SalePrice,
                            ArrivalPrice = product.ArrivalPrice,
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
                                    CashPay();
                                }
                                handled = true;
                                break;
                            case HOTKEY_F7:
                                if (vkey == VK_F7)
                                {
                                    CashlessPay();
                                }
                                handled = true;
                                break;
                            case HOTKEY_ALT_F5:
                                if (vkey == VK_F5)
                                {
                                    CreatePostponeReceipt();
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
                            case HOTKEY_F8:
                                if (vkey == VK_F8)
                                {
                                    PunchReceipt();
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

        private decimal GetMaximumDiscount()
        {
            if (SelectedProductSale != null)
            {
                return (decimal)MaximumPercentage * SelectedProductSale.AmountWithoutDiscount;
            }
            else
            {
                return 0;
            }
        }

        private void PrintCashRegisterMachine()
        {
            try
            {
                _cashRegisterMachineService.Connect();
                _cashRegisterMachineService.CheckType = 0;

                if (ProductSales.Any())
                {
                    foreach (Sale item in ProductSales)
                    {
                        _cashRegisterMachineService.Quantity = Convert.ToDouble(item.Quantity);
                        _cashRegisterMachineService.Price = item.SalePrice;

                        var sum1NSP = Math.Round(item.AmountWithoutDiscount * 1 / 102, 2);
                        string sumNSP = Math.Round(sum1NSP * 100, 0).ToString();

                        _cashRegisterMachineService.StringForPrinting =
                            string.Join(";", new string[] { "", item.TNVED, "", "", "0", "0", "4", sumNSP + "\n" + item.Name });
                        _cashRegisterMachineService.Tax1 = 4;
                        _cashRegisterMachineService.Tax2 = 0;
                        _cashRegisterMachineService.Tax3 = 0;
                        _cashRegisterMachineService.Tax4 = 0;
                        string result = _cashRegisterMachineService.Sale();
                    }

                    _cashRegisterMachineService.Summ1 = ProductSales.Sum(s => s.AmountWithoutDiscount);
                    _cashRegisterMachineService.StringForPrinting = "";
                    _cashRegisterMachineService.CloseCheck();
                    _cashRegisterMachineService.CutCheck();
                    _cashRegisterMachineService.Disconnect();

                    //_cashRegisterMachineService.RegisterNumber = 148;
                    //_cashRegisterMachineService.GetOperationReg();
                    //string d = _cashRegisterMachineService.GetOperationReg();
                    //int f = _cashRegisterMachineService.ContentsOfOperationRegister;
                    //string df = _cashRegisterMachineService.NameOperationReg;

                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void ShowEditor(int column)
        {
            _ = SaleTableView.Dispatcher.BeginInvoke(new Action(() =>
            {
                SaleTableView.FocusedRowHandle = ProductSales.IndexOf(SelectedProductSale);
                SaleTableView.Grid.CurrentColumn = SaleTableView.Grid.Columns[column];
                SaleTableView.ShowEditor();
            }), DispatcherPriority.Render);
        }

        private void PrintReport(PrintToolBase tool)
        {
            try
            {
                tool.PrinterSettings.PrinterName = Settings.Default.DefaultReceiptPrinter;
                tool.Print();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void QuantitySpin()
        {
            try
            {
                if (QuantitySpinEdit != null)
                {
                    _ = QuantitySpinEdit.Focus();
                    QuantitySpinEdit.SelectAll();
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion

        #region Public Voids

        [Command]
        public void SettingsKKM()
        {
            try
            {
                _cashRegisterMachineService.ShowProperties();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void OpenShiftKKM()
        {
            try
            {
                string result = _cashRegisterMachineService.OpenShift();
                if (!string.IsNullOrEmpty(result))
                {
                    MessageBoxService.ShowMessage(result, "ККМ", MessageButton.OK, MessageIcon.Information);
                }
                Settings.Default.IsKKMShiftOpen = true;
                Settings.Default.Save();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void CloseShiftKKM()
        {
            try
            {
                string result = _cashRegisterMachineService.CloseShift();
                if (!string.IsNullOrEmpty(result))
                {
                    MessageBoxService.ShowMessage(result, "ККМ", MessageButton.OK, MessageIcon.Information);
                }
                Settings.Default.IsKKMShiftOpen = false;
                Settings.Default.Save();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void PrintXReportKKM()
        {
            try
            {
                string result = _cashRegisterMachineService.PrintReportWithoutCleaning();
                if (!string.IsNullOrEmpty(result))
                {
                    MessageBoxService.ShowMessage(result, "ККМ", MessageButton.OK, MessageIcon.Information);
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void CancelReceiptKKM()
        {
            try
            {
                string result = _cashRegisterMachineService.CancelReceipt();
                if (!string.IsNullOrEmpty(result))
                {
                    MessageBoxService.ShowMessage(result, "ККМ", MessageButton.OK, MessageIcon.Information);
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public async void PrintXReport()
        {
            try
            {
                XReport xReport = await _reportService.CreateXReport();
                PrintToolBase tool = new(xReport.PrintingSystem);
                tool.PrinterSettings.PrinterName = Settings.Default.DefaultReceiptPrinter;
                tool.Print();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void CreatePostponeReceipt()
        {
            try
            {
                PostponeReceipts.Add(new PostponeReceipt
                {
                    Id = Guid.NewGuid(),
                    DateTime = DateTime.Now,
                    Total = ProductSales.Sum(sp => sp.Total),
                    Sales = ProductSales.ToList()
                });
                ProductSales.Clear();
                CashlessPaySum = 0;
                CashPaySum = 0;
            }
            catch (Exception)
            {
                //ignore
            }
        }

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
                    CashPaySum = Total - CashlessPaySum;
                    _ = CashPayTextEdit.Focus();                    
                    CashPayTextEdit.SelectAll();
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
                    CashlessPaySum = Total - CashPaySum;
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
        public async void PunchReceipt()
        {
            try
            {
                if (CanPunchReceipt)
                {
                    if (ProductSales.Any(p => p.Quantity != 0))
                    {
                        Receipt receipt = await _receiptService.CreateAsync(new Receipt()
                        {
                            DateOfPurchase = DateTime.Now,
                            AmountWithoutDiscount = AmountWithoutDiscount,
                            Total = Total,
                            PaidInCash = CashPaySum,
                            PaidInCashless = CashlessPaySum,
                            ShiftId = _shiftStore.CurrentShift.Id,
                            Change = Change,
                            ProductSales = ProductSales.Select(s =>
                                new ProductSale
                                {
                                    ProductId = s.Id,
                                    Quantity = s.Quantity,
                                    Total = s.Total,
                                    DiscountAmount = s.DiscountAmount,
                                    SalePrice = s.SalePrice,
                                    ArrivalPrice = s.ArrivalPrice
                                }).ToList()
                        }, Settings.Default.IsKeepRecords);

                        DiscountReceiptReport report = await _reportService.CreateDiscountReceiptReport(receipt, ProductSales);

                        PrintReport(new(report.PrintingSystem));

                        if (Settings.Default.IsKKMShiftOpen)
                        {
                            PrintCashRegisterMachine();
                        }

                        ProductSales.Clear();
                        CashPaySum = 0;
                        CashlessPaySum = 0;
                    }
                    else
                    {
                        SelectedProductSale = ProductSales.FirstOrDefault(p => p.Quantity == 0);
                    }
                }
                else
                {
                    CashPaySum = Total;
                    CashPay();
                }
            }            
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void QuantitySpinEditLoaded(object sender)
        {
            try
            {
                if (sender is RoutedEventArgs e)
                {
                    if (e.Source is UnitSpinEdit unitSpinEdit)
                    {
                        QuantitySpinEdit = unitSpinEdit;
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
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
                _postponeReceiptViewModel.PostponeReceipts = PostponeReceipts;
                WindowService.Show(nameof(PostponeReceiptView), _postponeReceiptViewModel);
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
        public void UserControlLoaded(object parameter)
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
                    RegisterHotKey(_windowHandle, HOTKEY_F7, MOD_NONE, VK_F8); //+
                }
            }
            BarcodeOpen();
            _barcodeService.OnBarcodeEvent += BarcodeService_OnBarcodeEvent;
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
                if (SelectedProductSale != null)
                {
                    if (SelectedProductSale.IsDiscountPercentage)
                    {
                        SelectedProductSale.DiscountAmount = decimal.Round(SelectedProductSale.AmountWithoutDiscount * (decimal)SelectedProductSale.DiscountPercent, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        SelectedProductSale.DiscountPercent = (double)SelectedProductSale.DiscountAmount / (double)SelectedProductSale.AmountWithoutDiscount;
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void DiscountEditValueChanging(object sender)
        {
            try
            {
                if (sender is EditValueChangingEventArgs e)
                {
                    if (SelectedProductSale != null && SelectedProductSale.IsDiscountPercentage)
                    {
                        if (MaximumPercentage < (double)e.NewValue)
                        {
                            SelectedProductSale.DiscountPercent = MaximumPercentage;
                        }
                    }
                    else
                    {
                        if (MaximumDiscount < (decimal)e.NewValue)
                        {
                            SelectedProductSale.DiscountAmount = MaximumDiscount;
                        }
                    }
                }
            }
            catch (Exception)
            {
                //ignore
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
