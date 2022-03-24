using DevExpress.Mvvm;
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
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

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
            get => _productSaleStore.Change;
            set { }
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
            PaymentComplexViewModel paymentComplexViewModel)
        {
            _userStore = userStore;
            _receiptService = receiptService;
            _authenticator = authenticator;
            _shiftStore = shiftStore;
            _productSaleStore = productSaleStore;
            _barcodeService = barcodeService;
            _paymentCashViewModel = paymentCashViewModel;
            _paymentComplexViewModel = paymentComplexViewModel;

            ProductsWithoutBarcodeViewModel = productsWithoutBarcodeViewModel;

            SaleProductsCollectionView = CollectionViewSource.GetDefaultView(ProductSales);
            BindingOperations.EnableCollectionSynchronization(ProductSales, _syncLock);

            _productSaleStore.OnProductSalesChanged += () => OnPropertyChanged(nameof(Sum));
            _productSaleStore.OnProductSale += ProductSaleStore_OnProductSale;
            _productSaleStore.OnPostponeReceiptChanged += ProductSaleStore_OnPostponeReceiptChanged;
        }

        #endregion

        #region Private Voids

        private void ProductSaleStore_OnPostponeReceiptChanged()
        {
            OnPropertyChanged(nameof(ProductSales));
            OnPropertyChanged(nameof(PostponeReceipts));
            OnPropertyChanged(nameof(Sum));            
        }

        private void ProductSaleStore_OnProductSale(bool obj)
        {
            if (!obj)
            {
                _ = MessageBoxService.ShowMessage("Что-то пошло не так.", "Sale Page", MessageButton.OK, MessageIcon.Error);
            }
            OnPropertyChanged(nameof(Sum));
            OnPropertyChanged(nameof(Change));
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
                    homeView.Focus();
                }
            }
            BarcodeOpen();
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
            else if (ProductSales.Count > 0)
            {
                _productSaleStore.DeleteProduct(ProductSales.LastOrDefault().Id);
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
