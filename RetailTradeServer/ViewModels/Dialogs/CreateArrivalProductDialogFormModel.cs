using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using DevExpress.XtraEditors.DXErrorProvider;
using RetailTrade.Barcode.Services;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using SalePageServer.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateArrivalProductDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IArrivalService _arrivalService;
        private readonly IArrivalProductService _arrivalProductService;
        private readonly ITypeProductService _typeProductService;
        private readonly IBarcodeService _barcodeService;
        private int? _selectedSupplierId;
        private ArrivalProduct _selectedArrivalProduct;
        private string _comment;
        private IEnumerable<Supplier> _suppliers;
        private IEnumerable<Product> _products;
        private ObservableCollection<ArrivalProduct> _arrivalProducts = new();
        private object _syncLock = new();
        private Arrival _arrival;

        #endregion

        #region Public Properties

        public bool IsEditMode { get; set; }
        public Arrival Arrival
        {
            get => _arrival;
            set
            {
                _arrival = value;
                if (_arrival != null)
                {
                    IsEditMode = true;
                    SelectedSupplierId = _arrival.SupplierId;
                    ArrivalProducts = new(_arrival.ArrivalProducts);
                    InvoiceNumber = _arrival.InvoiceNumber;
                    InvoiceDate = _arrival.InvoiceDate;
                }
                OnPropertyChanged(nameof(Arrival));
            }
        }
        public ICollectionView ArrivalProductsCollectionView => CollectionViewSource.GetDefaultView(ArrivalProducts);
        public IEnumerable<Supplier> Suppliers
        {
            get => _suppliers;
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
            }
        }
        public int? SelectedSupplierId
        {
            get => _selectedSupplierId;
            set
            {
                _selectedSupplierId = value;
                OnPropertyChanged(nameof(SelectedSupplierId));
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
        public ObservableCollection<ArrivalProduct> ArrivalProducts
        {
            get => _arrivalProducts;
            set
            {
                _arrivalProducts = value;
                OnPropertyChanged(nameof(ArrivalProducts));
            }
        }
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
        public GridControl ArrivalGridControl { get; set; }
        public TableView ArrivalTableView { get; set; }
        public ArrivalProduct EmptyArrivalProduct => ArrivalProducts.FirstOrDefault(a => a.ProductId == 0);

        #endregion

        #region Commands

        public ICommand ValidateCellCommand => new ParameterCommand(parameter => ValidateCell(parameter));
        public ICommand ArrivalProductCommand => new RelayCommand(CreateArrival);
        public ICommand ClearCommand => new RelayCommand(Cleare);
        public ICommand CellValueChangedCommand => new ParameterCommand(p => CellValueChanged(p));
        public ICommand AddProductToArrivalCommand => new RelayCommand(AddProductToArrival);
        public ICommand GridControlLoadedCommand => new ParameterCommand((object p) => GridControlLoaded(p));
        public ICommand ProductCommand => new RelayCommand(OpenProductDialog);
        public ICommand UserControlLoadedCommand => new ParameterCommand(sender => UserControlLoaded(sender));

        #endregion

        #region Constructor

        public CreateArrivalProductDialogFormModel(IProductService productService,
            ISupplierService supplierService,
            IArrivalService arrivalService,
            ITypeProductService typeProductService,
            IBarcodeService barcodeService,
            IArrivalProductService arrivalProductService)
        {
            _productService = productService;
            _supplierService = supplierService;
            _arrivalService = arrivalService;
            _typeProductService = typeProductService;
            _barcodeService = barcodeService;
            _arrivalProductService = arrivalProductService;

            BindingOperations.EnableCollectionSynchronization(ArrivalProducts, _syncLock);

            CloseCommand = new RelayCommand(() => CurrentWindowService.Close());
        }

        #endregion

        #region Private Voids

        private void UserControlLoaded(object parameter)
        {
            if (parameter is RoutedEventArgs e)
            {
                if (e.Source is UserControl userControl)
                {
                    userControl.Unloaded += UserControl_Unloaded;
                }
            }
            GetSupplier();
            _barcodeService.Open(BarcodeDevice.Com, Settings.Default.BarcodeCom, Settings.Default.BarcodeSpeed);
            _barcodeService.OnBarcodeEvent += BarcodeService_OnBarcodeEvent;
        }

        private void BarcodeService_OnBarcodeEvent(string barcode)
        {
            try
            {
                if (Products.Any())
                {
                    Product product = Products.FirstOrDefault(p => p.Barcode == barcode);
                    if (product != null)
                    {
                        if (ArrivalProducts.Any())
                        {
                            ArrivalProduct arrivalProduct = ArrivalProducts.FirstOrDefault(r => r.ProductId == product.Id);
                            if (arrivalProduct == null)
                            {
                                Add(product);
                            }
                            else
                            {
                                arrivalProduct.Quantity++;
                            }
                        }
                        else
                        {
                            Add(product);
                        }
                        ArrivalTableView.Grid.UpdateGroupSummary();
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void Add(Product product)
        {
            ArrivalProducts.Add(new ArrivalProduct
            {
                ProductId = product.Id,
                Product = product,
                ArrivalPrice = product.ArrivalPrice,
                Quantity = 1
            });
            ShowEditor(1);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _barcodeService.OnBarcodeEvent -= BarcodeService_OnBarcodeEvent;
            _barcodeService.Close(BarcodeDevice.Com);
        }

        private void OpenProductDialog()
        {
            ProductDialogFormModel viewModel = new(_typeProductService) { Products = new(Products) };
            viewModel.OnProductSelected += ProductDialogFormModel_OnProductSelected;
            WindowService.Show(nameof(ProductDialogForm), viewModel);
        }

        private void ProductDialogFormModel_OnProductSelected(Product product)
        {
            if (product != null)
            {
                ArrivalProduct selectedArrivalProduct = ArrivalProducts.FirstOrDefault(r => r.ProductId == product.Id);
                if (selectedArrivalProduct == null)
                {
                    SelectedArrivalProduct.ProductId = product.Id;
                    SelectedArrivalProduct.Product = product;
                    SelectedArrivalProduct.ArrivalPrice = product.ArrivalPrice;
                    SelectedArrivalProduct.Quantity = 1;
                    ShowEditor(1);
                }
                else
                {
                    _ = MessageBoxService.Show("Такой товар уже введен.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void GridControlLoaded(object parameter)
        {
            if (parameter is RoutedEventArgs e)
            {
                if (e.Source is GridControl gridControl)
                {
                    ArrivalGridControl = gridControl;
                    ArrivalTableView = ArrivalGridControl.View as TableView;
                    ArrivalTableView.ShownEditor += ArrivalTableView_ShownEditor;
                }
            }
        }

        private void ArrivalTableView_ShownEditor(object sender, EditorEventArgs e)
        {
            ArrivalTableView.Grid.View.ActiveEditor.SelectAll();
        }

        private void AddProductToArrival()
        {
            if (SelectedSupplierId != null)
            {
                ArrivalProducts.Add(new ArrivalProduct());
                SelectedArrivalProduct = EmptyArrivalProduct;
                ShowEditor(0);
            }
            else
            {
                _ = MessageBoxService.ShowMessage("Выберите поставщика!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
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
                        ShowEditor(1);
                    }
                }
                ArrivalTableView.PostEditor();
                ArrivalTableView.Grid.UpdateGroupSummary();
            }            
            OnPropertyChanged(nameof(CanArrivalProduct));
        }

        private void ValidateCell(object parameter)
        {
            if (parameter is GridCellValidationEventArgs e)
            {
                if (e.Cell.Property == nameof(ArrivalProduct.Quantity))
                {
                    if ((decimal)e.Value < 0)
                    {
                        _ = MessageBoxService.ShowMessage("Количество не должно быть 0.", "", MessageButton.OK, MessageIcon.Error);
                        e.ErrorContent = "Количество не должно быть 0.";                        
                        e.ErrorType = ErrorType.Critical;
                        e.IsValid = false;
                    }
                }
                if (e.Cell.Property == nameof(ArrivalProduct.ProductId))
                {
                    try
                    {
                        if (ArrivalProducts.Any(r => r.ProductId == (int)e.Value))
                        {
                            if (SelectedArrivalProduct.ProductId != (int)e.Value)
                            {
                                _ = MessageBoxService.Show("Такой товар уже введен.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                                e.ErrorContent = "Такой товар уже введен.";
                                e.ErrorType = ErrorType.Critical;
                                e.IsValid = false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //ignore
                    }
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
                        SupplierId = SelectedSupplierId.Value,
                        Comment = Comment,
                        Sum = ArrivalProducts.Sum(ap => ap.ArrivalSum),
                        ArrivalProducts = ArrivalProducts.ToList()
                    });
                }
                catch (Exception e)
                {
                    //ignore
                }
                CurrentWindowService.Close();
            }
        }

        private void ShowEditor(int column)
        {
            ArrivalTableView.FocusedRowHandle = ArrivalProducts.IndexOf(SelectedArrivalProduct);
            ArrivalTableView.Grid.CurrentColumn = ArrivalTableView.Grid.Columns[column];
            _ = ArrivalTableView.Dispatcher.BeginInvoke(new Action(() =>
            {
                ArrivalTableView.ShowEditor();
            }), DispatcherPriority.Render);
        }

        private void Cleare()
        {
            ArrivalProducts.Clear();
        }

        private async void GetProducts()
        {
            if (SelectedSupplierId != null)
            {
                Products = await _productService.PredicateSelect(p => p.SupplierId == SelectedSupplierId.Value && p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name, ArrivalPrice = p.ArrivalPrice, TypeProductId = p.TypeProductId, Barcode = p.Barcode });
            }
        }

        private async void GetSupplier()
        {
            Suppliers = await _supplierService.GetAllAsync();
        }

        #endregion

        #region Public Voids

        [Command]
        public async void SaveArrivalProduct()
        {
            try
            {
                foreach (ArrivalProduct newArrivalProduct in ArrivalProducts.Where(a => a.Id != 0).ToList())
                {
                    _ = await _arrivalProductService.EditAsync(newArrivalProduct);
                }
                foreach (ArrivalProduct item in ArrivalProducts.Where(a => a.Id == 0).ToList())
                {
                    item.ArrivalId = Arrival.Id;
                    _ = await _arrivalProductService.CreateAsync(item);
                }

                Arrival.Sum = ArrivalProducts.Sum(a => a.ArrivalSum);
                Arrival.InvoiceDate = InvoiceDate;
                Arrival.InvoiceNumber = InvoiceNumber;
                Arrival.Comment = Comment;
                Arrival.ArrivalProducts = null;
                Arrival.Supplier = null;

                _ = await _arrivalService.UpdateAsync(Arrival.Id, Arrival);

                CurrentWindowService.Close();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void DeleteSelectedRow()
        {
            if (SelectedArrivalProduct != null)
            {
                if (IsEditMode)
                {
                    if (SelectedArrivalProduct.Id == 0)
                    {
                        ArrivalProducts.Remove(SelectedArrivalProduct);
                    }
                }
                else
                {
                    ArrivalProducts.Remove(SelectedArrivalProduct);
                }
                
            }
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
