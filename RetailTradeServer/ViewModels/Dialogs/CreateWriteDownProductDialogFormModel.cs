using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using SalePageServer.State.Dialogs;
using SalePageServer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateWriteDownProductDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IWriteDownService _writeDownService;
        private readonly IDialogService _dialogService;
        private Supplier _selectedSupplier;
        private WriteDownProduct _selectedWriteDownProduct;
        private string _comment;
        private IEnumerable<Supplier> _suppliers;
        private IEnumerable<Product> _products;
        private ObservableQueue<WriteDownProduct> _writeDownProducts;

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
        public IEnumerable<WriteDownProduct> WriteDownProducts => _writeDownProducts;
        public WriteDownProduct SelectedWriteDownProduct
        {
            get => _selectedWriteDownProduct;
            set
            {
                _selectedWriteDownProduct = value;
                OnPropertyChanged(nameof(SelectedWriteDownProduct));
            }
        }
        public bool CanWriteDownProduct => WriteDownProducts.Any() && !WriteDownProducts.Any(p => p.Quantity == 0);

        #endregion

        #region Commands

        public ICommand AddProductToWriteDownCommand { get; }
        public ICommand ValidateCellCommand { get; }
        public ICommand WriteDownProductCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand CellValueChangedCommand { get; }

        #endregion

        #region Constructor

        public CreateWriteDownProductDialogFormModel(IProductService productService,
            ISupplierService supplierService,
            IWriteDownService writeDownService,
            IDialogService dialogService)
        {
            _productService = productService;
            _supplierService = supplierService;
            _writeDownService = writeDownService;
            _dialogService = dialogService;

            GetSupplier();

            ValidateCellCommand = new ParameterCommand(parameter => ValidateCell(parameter));
            WriteDownProductCommand = new RelayCommand(CreateArrival);
            ClearCommand = new RelayCommand(Cleare);
            AddProductToWriteDownCommand = new RelayCommand(AddProductToWriteDown);
            CellValueChangedCommand = new ParameterCommand(p => CellValueChanged(p));

            _writeDownProducts = new();
        }

        #endregion

        #region Private Voids

        private void CellValueChanged(object parameter)
        {
            if (parameter is CellValueChangedEventArgs e)
            {
                if (e.Cell.Property == "ProductId")
                {
                    SelectedWriteDownProduct.Product = new() { Quantity = Products.FirstOrDefault(wp => wp.Id == (int)e.Cell.Value).Quantity };
                }
            }
            OnPropertyChanged(nameof(CanWriteDownProduct));
        }

        private void AddProductToWriteDown()
        {
            if (SelectedSupplier != null)
            {
                _writeDownProducts.Enqueue(new WriteDownProduct());
                OnPropertyChanged(nameof(CanWriteDownProduct));
            }
            else
            {
                _dialogService.ShowMessage("Выберите поставщика!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ValidateCell(object parameter)
        {
            if (parameter is GridCellValidationEventArgs e)
            {
                if (((WriteDownProduct)e.Row).Product != null)
                {
                    if (Convert.ToDouble(e.Value) > ((WriteDownProduct)e.Row).Product.Quantity)
                    {
                        e.IsValid = false;
                        e.ErrorContent = "Количество списание товаров не должно превышать количество на складе.";
                        _dialogService.ShowMessage("Количество списание товаров не должно превышать количество на складе.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            OnPropertyChanged(nameof(CanWriteDownProduct));
        }

        private async void CreateArrival()
        {
            if (CanWriteDownProduct)
            {
                try
                {
                    _ = await _writeDownService.CreateAsync(new WriteDown
                    {
                        WriteDownDate = DateTime.Now,
                        SupplierId = SelectedSupplier.Id,
                        Comment = Comment,
                        WriteDownProducts = WriteDownProducts.Select(p => new WriteDownProduct { Quantity = p.Quantity, ProductId = p.ProductId }).ToList()
                    });
                }
                catch (Exception e)
                {
                    //ignore
                }

                _dialogService.Close();
            }
        }

        private void Cleare()
        {
            _writeDownProducts.Clear();
        }

        private async void GetProducts()
        {
            if (SelectedSupplier != null)
            {
                Products = await _productService.PredicateSelect(p => p.SupplierId == SelectedSupplier.Id && p.Quantity > 0, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity });
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
