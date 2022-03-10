using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using SalePageServer.Utilities;
using System;
using System.Collections.Generic;
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
        private Supplier _selectedSupplier;
        private RefundToSupplierProduct _refundToSupplierProduct;
        private string _comment;
        private IEnumerable<Supplier> _suppliers;
        private IEnumerable<Product> _products;
        private ObservableQueue<RefundToSupplierProduct> _refundToSupplierProducts;

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
        public IEnumerable<RefundToSupplierProduct> RefundToSupplierProducts => _refundToSupplierProducts;
        public RefundToSupplierProduct SelectedRefundToSupplierProduct
        {
            get => _refundToSupplierProduct;
            set
            {
                _refundToSupplierProduct = value;
                OnPropertyChanged(nameof(SelectedRefundToSupplierProduct));
            }
        }
        public bool CanRefundToSupplierProduct => RefundToSupplierProducts.Any() && !RefundToSupplierProducts.Any(p => p.Quantity == 0);

        #endregion

        #region Commands

        public ICommand ValidateCellCommand => new ParameterCommand(parameter => ValidateCell(parameter));
        public ICommand RefundToSupplierProductCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand AddProductToRefundCommand { get; }
        public ICommand CellValueChangedCommand { get; }

        #endregion

        #region Constructor

        public CreateRefundToSupplierDialogFormModel(IProductService productService,
            ISupplierService supplierService,
            IRefundToSupplierService refundToSupplierService)
        {
            _productService = productService;
            _supplierService = supplierService;
            _refundToSupplierService = refundToSupplierService;

            _refundToSupplierProducts = new();

            GetSupplier();

            RefundToSupplierProductCommand = new RelayCommand(CreateRefundToSupplierProduct);
            ClearCommand = new RelayCommand(Cleare);
            AddProductToRefundCommand = new RelayCommand(AddProductToRefund);
            CellValueChangedCommand = new ParameterCommand(p => CellValueChanged(p));
        }

        #endregion

        #region Private Voids

        private void CellValueChanged(object parameter)
        {
            if (parameter is CellValueChangedEventArgs e)
            {
                if (e.Cell.Property == nameof(WriteDownProduct.ProductId))
                {
                    SelectedRefundToSupplierProduct.Product = new() { Quantity = Products.FirstOrDefault(p => p.Id == (int)e.Value).Quantity };
                }
            }
            OnPropertyChanged(nameof(CanRefundToSupplierProduct));
        }

        private void AddProductToRefund()
        {
            if (SelectedSupplier != null)
            {
                _refundToSupplierProducts.Enqueue(new RefundToSupplierProduct());
            }
            else
            {
                _ = MessageBoxService.ShowMessage("Выберите поставщика!", "", MessageButton.OK, MessageIcon.Exclamation);
            }
            OnPropertyChanged(nameof(CanRefundToSupplierProduct));
        }

        private void ValidateCell(object parameter)
        {
            if (parameter is GridCellValidationEventArgs e)
            {
                if (((RefundToSupplierProduct)e.Row).Product != null)
                {
                    if (((RefundToSupplierProduct)e.Row).Product.Quantity < Convert.ToDouble(e.Value))
                    {
                        e.IsValid = false;
                        e.ErrorContent = "Количество не должно превышать количество товаров на складе.";
                        _ = MessageBoxService.ShowMessage("Количество не должно превышать количество товаров на складе.", "", MessageButton.OK, MessageIcon.Error);
                    }
                }
            }
            OnPropertyChanged(nameof(CanRefundToSupplierProduct));
        }

        private async void CreateRefundToSupplierProduct()
        {
            if (CanRefundToSupplierProduct)
            {
                try
                {
                    _ = await _refundToSupplierService.CreateAsync(new RefundToSupplier
                    {
                        RefundToSupplierDate = DateTime.Now,
                        SupplierId = SelectedSupplier.Id,
                        Comment = Comment,
                        RefundToSupplierProducts = RefundToSupplierProducts.Select(p => new RefundToSupplierProduct { ProductId = p.ProductId, Quantity = p.Quantity }).ToList()
                    });
                }
                catch (Exception)
                {
                    //ignore
                }

                CurrentWindowService.Close();
            }
        }

        private void Cleare()
        {
            _refundToSupplierProducts.Clear();
        }

        private async void GetProducts()
        {
            if (SelectedSupplier != null)
            {
                //Products = await _productService.PredicateSelect(p => p.SupplierId == SelectedSupplier.Id && p.Quantity > 0, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, Unit = p.Unit });
            }
        }

        private async void GetSupplier()
        {
            Suppliers = await _supplierService.GetAllAsync();
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            base.Dispose();
        }

        #endregion
    }
}
