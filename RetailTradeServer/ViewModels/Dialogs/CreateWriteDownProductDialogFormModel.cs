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
    public class CreateWriteDownProductDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private Supplier _selectedSupplier;
        private LossProduct _selectedWriteDownProduct;
        private string _comment;
        private IEnumerable<Supplier> _suppliers;
        private IEnumerable<Product> _products;
        private ObservableQueue<LossProduct> _writeDownProducts;

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
        public IEnumerable<LossProduct> WriteDownProducts => _writeDownProducts;
        public LossProduct SelectedWriteDownProduct
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
            ISupplierService supplierService)
        {
            _productService = productService;
            _supplierService = supplierService;

            GetSupplier();

            ValidateCellCommand = new ParameterCommand(parameter => ValidateCell(parameter));
            WriteDownProductCommand = new RelayCommand(CreateArrival);
            ClearCommand = new RelayCommand(Cleare);
            CloseCommand = new RelayCommand(() => CurrentWindowService.Close());
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

                }
            }
            OnPropertyChanged(nameof(CanWriteDownProduct));
        }

        private void AddProductToWriteDown()
        {
            if (SelectedSupplier != null)
            {
                _writeDownProducts.Enqueue(new LossProduct());
                OnPropertyChanged(nameof(CanWriteDownProduct));
            }
            else
            {
                _ = MessageBoxService.ShowMessage("Выберите поставщика!", "", MessageButton.OK, MessageIcon.Exclamation);
            }
        }

        private void ValidateCell(object parameter)
        {
            if (parameter is GridCellValidationEventArgs e)
            {
                if (((LossProduct)e.Row).Product != null)
                {
                    
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
                    
                }
                catch (Exception e)
                {
                    //ignore
                }

                CurrentWindowService.Close();
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
                //Products = await _productService.PredicateSelect(p => p.SupplierId == SelectedSupplier.Id && p.Quantity > 0, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity });
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
