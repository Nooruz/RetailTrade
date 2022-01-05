using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using SalePageServer.State.Dialogs;
using SalePageServer.Utilities;
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
    public class CreateArrivalProductDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IArrivalService _arrivalService;
        private readonly IDialogService _dialogService;
        private Supplier _selectedSupplier;
        private Product _selectedProduct;
        private string _comment;
        private IEnumerable<Supplier> _suppliers;
        private IEnumerable<Product> _products;
        private ObservableQueue<ArrivalProduct> _arrivalProducts;

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
        public IEnumerable<ArrivalProduct> ArrivalProducts => _arrivalProducts;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }
        public bool CanArrivalProduct => ArrivalProducts.Any() && !ArrivalProducts.Any(p => p.Quantity == 0);

        #endregion

        #region Commands

        public ICommand RowDoubleClickCommand { get; }
        public ICommand ValidateCellCommand { get; }
        public ICommand ArrivalProductCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand CellValueChangedCommand { get; }

        #endregion

        #region Constructor

        public CreateArrivalProductDialogFormModel(IProductService productService,
            ISupplierService supplierService,
            IArrivalService arrivalService,
            IDialogService dialogService)
        {
            _productService = productService;
            _supplierService = supplierService;
            _arrivalService = arrivalService;
            _dialogService = dialogService;

            _arrivalProducts = new();

            GetSupplier();

            RowDoubleClickCommand = new RelayCommand(RowDoubleClick);
            ValidateCellCommand = new ParameterCommand(parameter => ValidateCell(parameter));
            ArrivalProductCommand = new RelayCommand(CreateArrival);
            ClearCommand = new RelayCommand(Cleare);
            CellValueChangedCommand = new RelayCommand(CellValueChanged);
        }

        #endregion

        #region Private Voids

        private void CellValueChanged()
        {
            OnPropertyChanged(nameof(CanArrivalProduct));
        }

        private void RowDoubleClick()
        {
            if (SelectedProduct != null)
            {
                if (ArrivalProducts.FirstOrDefault(pr => pr.ProductId == SelectedProduct.Id) == null)
                {
                    _arrivalProducts.Enqueue(new ArrivalProduct
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
                if ((decimal)e.Value < 0)
                {
                    e.IsValid = false;
                    e.ErrorContent = "Количество не должно быть 0.";
                    _dialogService.ShowMessage("Количество не должно быть 0.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void CreateArrival()
        {
            if (CanArrivalProduct)
            {
                List<ArrivalProduct> arrivals = new();
                foreach (var item in ArrivalProducts)
                {
                    arrivals.Add(new ArrivalProduct
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }
                try
                {
                    Arrival arrival = await _arrivalService.CreateAsync(new Arrival
                    {
                        ArrivalDate = DateTime.Now,
                        SupplierId = SelectedSupplier.Id,
                        Comment = Comment,
                        ArrivalProducts = arrivals
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
            _arrivalProducts.Dequeue();
        }

        private async void GetProducts()
        {
            if (SelectedSupplier != null)
            {
                Products = await _productService.PredicateSelect(p => p.SupplierId == SelectedSupplier.Id, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity, Unit = p.Unit });
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
