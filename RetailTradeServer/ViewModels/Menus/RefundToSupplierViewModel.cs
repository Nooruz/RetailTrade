using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using SalePageServer.State.Dialogs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class RefundToSupplierViewModel : BaseViewModel
    {
        #region Private members

        private readonly IProductService _productService;
        private readonly IRefundToSupplierService _refundToSupplierService;
        private readonly IRefundToSupplierServiceProduct _refundToSupplierServiceProduct;
        private readonly ISupplierService _supplierService;
        private readonly IDialogService _dialogService;
        private RefundToSupplier _selectedRefundToSupplier;
        private IEnumerable<RefundToSupplier> _refundToSuppliers;

        #endregion

        #region Public properties

        public IEnumerable<RefundToSupplier> RefundsToSuppliers
        {
            get => _refundToSuppliers;
            set
            {
                _refundToSuppliers = value;
                OnPropertyChanged(nameof(RefundsToSuppliers));
            }
        }
        public RefundToSupplier SelectedRefundToSupplier
        {
            get => _selectedRefundToSupplier;
            set
            {
                _selectedRefundToSupplier = value;
                OnPropertyChanged(nameof(SelectedRefundToSupplier));
            }
        }

        #endregion

        #region Commands

        public ICommand LoadedCommand { get; }

        #endregion

        #region Constructor

        public RefundToSupplierViewModel(IProductService productService,
            IRefundToSupplierService refundToSupplierService,
            IRefundToSupplierServiceProduct refundToSupplierServiceProduct,
            ISupplierService supplierService,
            IDialogService dialogService)
        {
            _productService = productService;
            _refundToSupplierService = refundToSupplierService;
            _refundToSupplierServiceProduct = refundToSupplierServiceProduct;
            _supplierService = supplierService;
            _dialogService = dialogService;

            LoadedCommand = new RelayCommand(GetRefundToSuppliersAsync);
            CreateCommand = new RelayCommand(Create);
            DeleteCommand = new RelayCommand(Delete);

            _refundToSupplierService.PropertiesChanged += GetRefundToSuppliersAsync;
        }

        #endregion

        #region Private Voids

        private async void GetRefundToSuppliersAsync()
        {
            RefundsToSuppliers = await _refundToSupplierService.GetAllAsync();
            ShowLoadingPanel = false;
        }

        private async void Create()
        {
            await _dialogService.ShowDialog(new CreateRefundToSupplierDialogFormModel(_productService, _supplierService, _refundToSupplierService, _dialogService) { Title = "Возврат поставщику (новый)" });
        }

        private async void Delete()
        {
            if (SelectedRefundToSupplier != null)
            {
                if (_dialogService.ShowMessage("Вы точно хотите удалить выбранный элемент?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _ = await _refundToSupplierService.DeleteAsync(SelectedRefundToSupplier.Id);
                }
            }
            else
            {
                _dialogService.ShowMessage("Выберите элемент!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion

        public override void Dispose()
        {
            RefundsToSuppliers = null;
            SelectedRefundToSupplier = null;
            base.Dispose();
        }
    }
}
