using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
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

        private readonly IReceiptService _receiptService;
        private readonly IUserStore _userStore;
        private ObservableCollection<Receipt> _receipts = new();
        private Receipt _selectedReceipt;

        #endregion

        #region Public Properties

        public ObservableCollection<Receipt> Receipts
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
        public Receipt SelectedReceipt
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
            IUserStore userStore)
        {
            _receiptService = receiptService;
            _userStore = userStore;

            _userStore.StateChanged += () => OnPropertyChanged(nameof(CurrentUser));
        }

        #endregion

        #region Privte Voids

        private void ProductSaleGridControl_CustomGroupDisplayText(object sender, CustomGroupDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == nameof(Receipt.ShiftId))
            {
                if (e.Row is Receipt receipt)
                {
                    if (receipt.Shift.ClosingDate == null)
                    {
                        e.DisplayText = $"Смена №{receipt.Id:D5} от {receipt.Shift.OpeningDate:dd.MM.yyyy HH:mm} (открыта)";
                    }
                    else
                    {
                        e.DisplayText = $"Смена №{receipt.Id:D5} от {receipt.Shift.ClosingDate:dd.MM.yyyy HH:mm} (закрыта)";
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

                WindowService.Show(nameof(ProductSaleView), 
                    new ProductSaleViewModel() 
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
