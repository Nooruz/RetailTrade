using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Views;
using RetailTrade.POS.States.Users;
using RetailTrade.POS.ViewModels.Dialogs;
using RetailTrade.POS.Views.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTrade.POS.ViewModels.Menus
{
    public class RefundViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductSaleService _productSaleService;
        private readonly IReceiptService _receiptService;
        private readonly IUserStore _userStore;
        private ObservableCollection<ReceiptView> _receipts = new();
        private ReceiptView _selectedReceipt;

        #endregion

        #region Public Properties

        public ObservableCollection<ReceiptView> Receipts
        {
            get => _receipts;
            set
            {
                _receipts = value;
                OnPropertyChanged(nameof(Receipts));
            }
        }
        public GridControl ProductSaleGridControl { get; set; }
        public TableView ProductSaleTableView { get; set; }
        public User CurrentUser => _userStore.CurrentUser;
        public ReceiptView SelectedReceipt
        {
            get => _selectedReceipt;
            set
            {
                _selectedReceipt = value;
                OnPropertyChanged(nameof(SelectedReceipt));
            }
        }

        #endregion

        #region Constructor

        public RefundViewModel(IReceiptService receiptService,
            IUserStore userStore,
            IProductSaleService productSaleService)
        {
            _receiptService = receiptService;
            _userStore = userStore;
            _productSaleService = productSaleService;

            _userStore.StateChanged += () => OnPropertyChanged(nameof(CurrentUser));
        }

        #endregion

        #region Privte Voids

        private void ProductSaleGridControl_CustomGroupDisplayText(object sender, CustomGroupDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == nameof(ReceiptView.ShiftId))
            {
                if (e.Row is ReceiptView receipt)
                {
                    if (receipt.ClosingDate == null)
                    {
                        e.DisplayText = $"Смена №{receipt.ShiftId:D5} от {receipt.OpeningDate:dd.MM.yyyy HH:mm} (открыта)";
                    }
                    else
                    {
                        e.DisplayText = $"Смена №{receipt.ShiftId:D5} от {receipt.ClosingDate:dd.MM.yyyy HH:mm} (закрыта)";
                    }
                }
            }
        }

        private void ProductSaleTableView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                GetRowType(ProductSaleGridControl.View.GetRowHandleByMouseEventArgs(e));
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void GetRowType(int rowHandle)
        {
            try
            {
                if (ProductSaleGridControl.IsGroupRowHandle(rowHandle))
                    return;
                if (rowHandle == DataControlBase.AutoFilterRowHandle)
                    return;
                if (rowHandle == DataControlBase.NewItemRowHandle)
                    return;
                if (rowHandle == DataControlBase.InvalidRowHandle)
                    return;

                DocumentViewerService.Show(nameof(RefundProductSaleView), 
                    new RefundProductSaleViewModel(_productSaleService) 
                    { 
                        Title = $"Продажа №{SelectedReceipt.Id:D5} от {SelectedReceipt.DateOfPurchase:dd.MM.yyyy HH:mm}",
                        EditedReceipt = SelectedReceipt
                    });

            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion

        #region Public Voids

        [Command]
        public async void UserControlLoaded()
        {
            Receipts = new(await _receiptService.GetAllAsync(CurrentUser.Id, Properties.Settings.Default.PointSaleId));
        }

        [Command]
        public void ProductSaleGridControlLoading(object sender)
        {
            if (sender is RoutedEventArgs e)
            {
                if (e.Source is GridControl gridControl)
                {
                    ProductSaleGridControl = gridControl;
                    ProductSaleTableView = ProductSaleGridControl.View as TableView;
                    ProductSaleTableView.SearchColumns = string.Join(";", ProductSaleGridControl.Columns.Select(g => g.FieldName));
                    ProductSaleTableView.MouseLeftButtonDown += ProductSaleTableView_MouseLeftButtonDown;
                    ProductSaleGridControl.CustomGroupDisplayText += ProductSaleGridControl_CustomGroupDisplayText;
                }
            }
        }

        #endregion
    }
}
