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
    public class CreateArrivalProductDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IArrivalService _arrivalService;
        private readonly IDialogService _dialogService;
        private Supplier _selectedSupplier;
        private ArrivalProduct _selectedArrivalProduct;
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
        public ArrivalProduct SelectedArrivalProduct
        {
            get => _selectedArrivalProduct;
            set
            {
                _selectedArrivalProduct = value;
                OnPropertyChanged(nameof(SelectedArrivalProduct));
            }
        }
        public bool CanArrivalProduct => ArrivalProducts.Any() && !ArrivalProducts.Any(p => p.Quantity == 0);
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }

        #endregion

        #region Commands

        public ICommand ValidateCellCommand { get; }
        public ICommand ArrivalProductCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand CellValueChangedCommand { get; }
        public ICommand AddProductToArrivalCommand { get; }

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

            ValidateCellCommand = new ParameterCommand(parameter => ValidateCell(parameter));
            ArrivalProductCommand = new RelayCommand(CreateArrival);
            ClearCommand = new RelayCommand(Cleare);
            CellValueChangedCommand = new ParameterCommand(p => CellValueChanged(p));
            AddProductToArrivalCommand = new RelayCommand(AddProductToArrival);
        }

        #endregion

        #region Private Voids

        private void AddProductToArrival()
        {
            if (SelectedSupplier != null)
            {
                _arrivalProducts.Enqueue(new ArrivalProduct());
            }
            else
            {
                _dialogService.ShowMessage("Выберите поставщика!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void CellValueChanged(object parameter)
        {
            if (parameter is CellValueChangedEventArgs e)
            {
                if (e.Cell.Property == "ProductId")
                {
                    if (SelectedArrivalProduct != null)
                    {
                        SelectedArrivalProduct.ArrivalPrice = Products.FirstOrDefault(p => p.Id == (int)e.Value).ArrivalPrice;
                    }
                }
                TableView tableView = e.Source as TableView;
                tableView.PostEditor();
                tableView.Grid.UpdateTotalSummary();
            }            
            OnPropertyChanged(nameof(CanArrivalProduct));
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
                try
                {
                    _ = await _arrivalService.CreateAsync(new Arrival
                    {
                        ArrivalDate = DateTime.Now,
                        InvoiceNumber = InvoiceNumber,
                        InvoiceDate = InvoiceDate,
                        SupplierId = SelectedSupplier.Id,
                        Comment = Comment,
                        Sum = ArrivalProducts.Sum(ap => ap.ArrivalSum),
                        ArrivalProducts = ArrivalProducts.ToList()
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
            _arrivalProducts.Clear();
        }

        private async void GetProducts()
        {
            if (SelectedSupplier != null)
            {
               Products = await _productService.PredicateSelect(p => p.SupplierId == SelectedSupplier.Id, p => new Product { Id = p.Id, Name = p.Name, ArrivalPrice = p.ArrivalPrice });
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
