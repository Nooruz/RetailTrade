using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateRefundToSupplierDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IRefundToSupplierService _refundToSupplierService;
        private readonly IUIManager _manager;
        private Supplier _selectedSupplier;
        private Product _selectedProduct;
        private string _comment;
        private IEnumerable<Supplier> _suppliers;
        private IEnumerable<Product> _products;

        #endregion

        #region Public Properties

        public IEnumerable<Supplier> Suppliers
        {
            get => _suppliers;
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
            }
        }
        public Supplier SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));
                OnPropertyChanged(nameof(Products));
                Cleare();
                GetProducts();
            }
        }
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }
        public IEnumerable<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public ObservableCollection<RefundToSupplierProduct> RefundToSupplierProducts { get; set; }
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }
        public bool CanRefundToSupplierProduct => RefundToSupplierProducts.Count == 0 ? false : RefundToSupplierProducts.FirstOrDefault(p => p.Quantity == 0) == null;

        #endregion

        #region Commands

        public ICommand RowDoubleClickCommand { get; }
        public ICommand ValidateCellCommand { get; }
        public ICommand RefundToSupplierProductCommand { get; }
        public ICommand ClearCommand { get; }

        #endregion

        #region Constructor

        public CreateRefundToSupplierDialogFormModel(IProductService productService,
            ISupplierService supplierService,
            IRefundToSupplierService refundToSupplierService,
            IUIManager manager)
        {
            _productService = productService;
            _supplierService = supplierService;
            _refundToSupplierService = refundToSupplierService;
            _manager = manager;

            RefundToSupplierProducts = new();

            GetSupplier();

            RowDoubleClickCommand = new RelayCommand(RowDoubleClick);
            ValidateCellCommand = new ParameterCommand(parameter => ValidateCell(parameter));
            RefundToSupplierProductCommand = new RelayCommand(CreateRefundToSupplierProduct);
            ClearCommand = new RelayCommand(Cleare);

            RefundToSupplierProducts.CollectionChanged += ProductRefunds_CollectionChanged;
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
            OnPropertyChanged(nameof(RefundToSupplierProducts));
            OnPropertyChanged(nameof(CanRefundToSupplierProduct));
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RefundToSupplierProducts));
            OnPropertyChanged(nameof(CanRefundToSupplierProduct));
        }

        private void RowDoubleClick()
        {
            if (SelectedProduct != null)
            {
                if (RefundToSupplierProducts.FirstOrDefault(pr => pr.ProductId == SelectedProduct.Id) == null)
                {
                    RefundToSupplierProducts.Add(new RefundToSupplierProduct
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
                    if (((RefundToSupplierProduct)e.Row).Product.Quantity < (double)e.Value)
                    {
                        e.IsValid = false;
                        e.ErrorContent = "Количество не должно превышать количество товаров на складе.";
                        _manager.ShowMessage("Количество не должно превышать количество товаров на складе.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            OnPropertyChanged(nameof(CanRefundToSupplierProduct));
        }

        private async void CreateRefundToSupplierProduct()
        {
            if (CanRefundToSupplierProduct)
            {
                List<RefundToSupplierProduct> refundToSupplierProducts = new();
                foreach (var item in RefundToSupplierProducts)
                {
                    refundToSupplierProducts.Add(new RefundToSupplierProduct
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }
                try
                {
                    _ = await _refundToSupplierService.CreateAsync(new RefundToSupplier
                    {
                        RefundToSupplierDate = DateTime.Now,
                        SupplierId = SelectedSupplier.Id,
                        Comment = Comment,
                        RefundToSupplierProducts = refundToSupplierProducts
                    });
                }
                catch (Exception e)
                {
                    //ignore
                }

                _manager.Close();
            }
        }

        private void Cleare()
        {
            RefundToSupplierProducts.Clear();
        }

        private async void GetProducts()
        {
            if (SelectedSupplier != null)
            {
                Products = await _productService.PredicateSelect(p => p.SupplierId == SelectedSupplier.Id && p.Quantity > 0, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, Unit = p.Unit });
            }
        }

        private async void GetSupplier()
        {
            Suppliers = await _supplierService.GetAllAsync();
        }

        #endregion
    }
}
