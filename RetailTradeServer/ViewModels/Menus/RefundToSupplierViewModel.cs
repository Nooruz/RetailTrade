using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class RefundToSupplierViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IUIManager _manager;
        private readonly IRefundToSupplierServiceProduct _productRefundToSupplierService;
        private int? _selectedSupplierId;
        private Product _selectedProduct;

        #endregion

        #region Public Properties

        public IEnumerable<Supplier> Suppliers => _supplierService.GetOnlyNames();        

        public int? SelectedSupplierId
        {
            get => _selectedSupplierId;
            set 
            { 
                _selectedSupplierId = value;
                OnPropertyChanged(nameof(SelectedSupplierId));
                OnPropertyChanged(nameof(Products));
            }
        }

        public IEnumerable<Product> Products => SelectedSupplierId != null ? _productService.GetForRefund(SelectedSupplierId.Value) : null;

        public ObservableCollection<RefundToSupplierProduct> ProductRefunds { get; set; }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        public bool CanRefundProduct => ProductRefunds.FirstOrDefault(p => p.Quantity == 0) == null;

        #endregion

        #region Commmands

        public ICommand RowDoubleClickCommand { get; }
        public ICommand ValidateCellCommand { get; }
        public ICommand RefundProductCommand { get; }
        public ICommand ClearCommand { get; }

        #endregion

        #region Constructor

        public RefundToSupplierViewModel(IProductService productService,
            ISupplierService supplierService,
            IUIManager manager,
            IRefundToSupplierServiceProduct productRefundToSupplierService)
        {
            _productService = productService;
            _supplierService = supplierService;
            _manager = manager;
            _productRefundToSupplierService = productRefundToSupplierService;

            ProductRefunds = new();

            RowDoubleClickCommand = new RelayCommand(RowDoubleClick);
            ValidateCellCommand = new ParameterCommand(parameter => ValidateCell(parameter));
            RefundProductCommand = new RelayCommand(RefundProduct);
            ClearCommand = new RelayCommand(Cleare);

            ProductRefunds.CollectionChanged += ProductRefunds_CollectionChanged;
        }

        #endregion

        #region Private Voids

        private void ProductRefunds_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    if (item != null)
                    {
                        item.PropertyChanged -= Item_PropertyChanged;
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    if (item != null)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
            }
            OnPropertyChanged(nameof(ProductRefunds));
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ProductRefunds));
        }

        private void RowDoubleClick()
        {
            if (SelectedProduct != null)
            {
                if (ProductRefunds.FirstOrDefault(pr => pr.ProductId == SelectedProduct.Id) == null)
                {
                    ProductRefunds.Add(new RefundToSupplierProduct
                    {
                        Product = SelectedProduct,
                        ProductId = SelectedProduct.Id
                    });
                }
            }
        }

        private void ValidateCell(object parameter)
        {
            if (parameter is GridCellValidationEventArgs e)
            {
                if (((RefundToSupplierProduct)e.Row).Product != null)
                {
                    if (((RefundToSupplierProduct)e.Row).Product.Quantity < (decimal)e.Value)
                    {
                        e.IsValid = false;
                        e.ErrorContent = "Количество возврата не должно превышать количество на складе.";
                        _manager.ShowMessage("Количество возврата не должно превышать количество на складе.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void RefundProduct()
        {
            if (ProductRefunds.Count > 0)
            {
                if (await _productRefundToSupplierService.AddRangeAsync(ProductRefunds.ToList()))
                {
                    _manager.ShowMessage("Операция успешно выполнена.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    Cleare();
                }
                else
                {
                    _manager.ShowMessage("Ошибка при выполнении операции.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Cleare()
        {
            ProductRefunds.Clear();
        }

        #endregion
    }
}
