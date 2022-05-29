﻿using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using DevExpress.XtraEditors.DXErrorProvider;
using RetailTrade.Barcode.Services;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using RetailTradeServer.Properties;
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
        private readonly IDataService<Unit> _unitService;
        private readonly IWareHouseService _warehouseService;
        private int? _selectedSupplierId;
        private int? _selectedWareHouseId;
        private ArrivalProduct _selectedArrivalProduct;
        private string _comment;
        private IEnumerable<Supplier> _suppliers;
        private IEnumerable<Product> _products;
        private IEnumerable<Unit> _units;
        private IEnumerable<WareHouse> _wareHouses;
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
        public IEnumerable<Unit> Units
        {
            get => _units;
            set
            {
                _units = value;
                OnPropertyChanged(nameof(Units));
            }
        }
        public IEnumerable<WareHouse> WareHouses
        {
            get => _wareHouses;
            set
            {
                _wareHouses = value;
                OnPropertyChanged(nameof(WareHouses));
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
        public int? SelectedWareHouseId
        {
            get => _selectedWareHouseId;
            set
            {
                _selectedWareHouseId = value;
                OnPropertyChanged(nameof(SelectedWareHouseId));
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
        public ICommand ClearCommand => new RelayCommand(Cleare);
        public ICommand CellValueChangedCommand => new ParameterCommand(p => CellValueChanged(p));
        public ICommand AddProductToArrivalCommand => new RelayCommand(AddProductToArrival);

        #endregion

        #region Constructor

        public CreateArrivalProductDialogFormModel(IProductService productService,
            ISupplierService supplierService,
            IArrivalService arrivalService,
            ITypeProductService typeProductService,
            IBarcodeService barcodeService,
            IArrivalProductService arrivalProductService,
            IDataService<Unit> unitService,
            IWareHouseService warehouseService)
        {
            _productService = productService;
            _supplierService = supplierService;
            _arrivalService = arrivalService;
            _typeProductService = typeProductService;
            _barcodeService = barcodeService;
            _arrivalProductService = arrivalProductService;
            _unitService = unitService;
            _warehouseService = warehouseService;

            BindingOperations.EnableCollectionSynchronization(ArrivalProducts, _syncLock);

            CloseCommand = new RelayCommand(() => CurrentWindowService.Close());
        }

        #endregion

        #region Private Voids

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
                        OnPropertyChanged(nameof(CanArrivalProduct));
                        _ = ArrivalTableView.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            ArrivalTableView.Grid.UpdateTotalSummary();
                        }), DispatcherPriority.Render);
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
            if (SelectedSupplierId != null)
            {
                ArrivalProducts.Add(new ArrivalProduct
                {
                    ProductId = product.Id,
                    ArrivalPrice = product.ArrivalPrice,
                    WareHouseId = SelectedSupplierId.Value,
                    Quantity = 1
                });
                ShowEditor(1);
            }
            else
            {
                MessageBoxService.ShowMessage("Выберите склад или розничный магазин!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _barcodeService.OnBarcodeEvent -= BarcodeService_OnBarcodeEvent;
                if (Enum.IsDefined(typeof(BarcodeDevice), Settings.Default.BarcodeDefaultDevice))
                {
                    _barcodeService.Close(Enum.Parse<BarcodeDevice>(Settings.Default.BarcodeDefaultDevice));
                }
            }
            catch (Exception)
            {
                //ignore
            }
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

        private void ProductDialogFormModel_OnProductsSelected(IEnumerable<Product> products)
        {
            products.ToList().ForEach(product =>
            {
                try
                {

                    ArrivalProduct selectedArrivalProduct = ArrivalProducts.FirstOrDefault(r => r.ProductId == product.Id);
                    if (selectedArrivalProduct == null)
                    {
                        SelectedArrivalProduct.ProductId = product.Id;
                        SelectedArrivalProduct.Product = product;
                        SelectedArrivalProduct.ArrivalPrice = product.ArrivalPrice;
                        SelectedArrivalProduct.Quantity = 1;
                    }
                }
                catch (Exception)
                {
                    //ignore
                }
            });
        }

        private void ArrivalTableView_ShownEditor(object sender, EditorEventArgs e)
        {
            ArrivalTableView.Grid.View.ActiveEditor.SelectAll();
        }

        private void AddProductToArrival()
        {
            if (SelectedSupplierId != null)
            {
                if (ArrivalProducts.Any())
                {
                    SelectedArrivalProduct = ArrivalProducts.FirstOrDefault(a => a.ProductId == 0);
                    if (SelectedArrivalProduct == null)
                    {
                        ArrivalProducts.Add(new ArrivalProduct());
                        SelectedArrivalProduct = EmptyArrivalProduct;
                        ShowEditor(0);
                    }
                    else
                    {
                        ShowEditor(0);
                    }
                }
                else
                {
                    ArrivalProducts.Add(new ArrivalProduct());
                    SelectedArrivalProduct = EmptyArrivalProduct;
                    ShowEditor(0);
                }
            }
            else
            {
                _ = MessageBoxService.ShowMessage("Выберите поставщика!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
            }
        }

        private void CellValueChanged(object parameter)
        {
            try
            {
                if (parameter is CellValueChangedEventArgs e)
                {
                    if (e.Cell.Property == "ProductId")
                    {
                        if (SelectedArrivalProduct != null && e.Value != null)
                        {
                            Product product = Products.FirstOrDefault(p => p.Id == (int)e.Value);
                            SelectedArrivalProduct.ArrivalPrice = product.ArrivalPrice;
                            SelectedArrivalProduct.Product = product;
                            ShowEditor(1);
                        }
                    }
                    ArrivalTableView.PostEditor();
                    ArrivalTableView.Grid.UpdateGroupSummary();
                }
                OnPropertyChanged(nameof(CanArrivalProduct));
            }
            catch (Exception)
            {
                //ignore
            }
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
                Products = await _productService.PredicateSelect(p => p.SupplierId == SelectedSupplierId.Value && p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name, ArrivalPrice = p.ArrivalPrice, TypeProductId = p.TypeProductId, Barcode = p.Barcode, UnitId = p.UnitId });
            }
        }

        private async void GetSupplier()
        {
            Suppliers = await _supplierService.GetAllAsync();
            Units = await _unitService.GetAllAsync();
            WareHouses = await _warehouseService.GetAllAsync();
        }

        #endregion

        #region Public Voids

        [Command]
        public async void CreateArrival()
        {
            if (SelectedWareHouseId == null)
            {
                MessageBoxService.ShowMessage("Выберите склад или точку продажи.", "Sale Page", MessageButton.OK);
                return;
            }
            if (CanArrivalProduct)
            {
                try
                {
                    //ArrivalProducts.ToList().ForEach((a) => a.Product = null);
                    Arrival arrival = await _arrivalService.CreateAsync(new Arrival
                    {
                        ArrivalDate = DateTime.Now,
                        InvoiceNumber = InvoiceNumber,
                        InvoiceDate = InvoiceDate,
                        SupplierId = SelectedSupplierId.Value,
                        WareHouseId = SelectedWareHouseId.Value,
                        Comment = Comment,
                        Sum = ArrivalProducts.Sum(ap => ap.ArrivalSum),
                        //ArrivalProducts = //ArrivalProducts.ToList().ForEach((a) => a.Product = null)
                    });
                    _ = await _arrivalProductService.CreateRangeAsync(SelectedWareHouseId.Value, arrival.Id, ArrivalProducts);
                }
                catch (Exception)
                {
                    //ignore
                }
                CurrentWindowService.Close();
            }
        }

        [Command]
        public void RemoveSelectedArrivalProduct()
        {
            if (ArrivalTableView != null && ArrivalTableView.ActiveEditor == null)
            {
                ArrivalProducts.Remove(SelectedArrivalProduct);
                if (ArrivalProducts != null && ArrivalProducts.Any())
                {
                    SelectedArrivalProduct = ArrivalProducts.LastOrDefault();
                }
            }
        }

        [Command]
        public void GridControlLoaded(object parameter)
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

        [Command]
        public void OpenProductDialog()
        {
            ProductDialogFormModel viewModel = new(_typeProductService) { Products = new(Products) };
            viewModel.OnProductSelected += ProductDialogFormModel_OnProductSelected;
            viewModel.OnProductsSelected += ProductDialogFormModel_OnProductsSelected;
            WindowService.Show(nameof(ProductDialogForm), viewModel);
        }

        [Command]
        public void UserControlLoaded(object parameter)
        {
            try
            {
                if (parameter is RoutedEventArgs e)
                {
                    if (e.Source is UserControl userControl)
                    {
                        userControl.Unloaded += UserControl_Unloaded;
                    }
                }
                GetSupplier();
                if (Enum.IsDefined(typeof(BarcodeDevice), Settings.Default.BarcodeDefaultDevice))
                {
                    _barcodeService.Open(Enum.Parse<BarcodeDevice>(Settings.Default.BarcodeDefaultDevice));
                }
                _barcodeService.OnBarcodeEvent += BarcodeService_OnBarcodeEvent;
            }
            catch (Exception)
            {
                //ignore
            }
        }

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

        [Command]
        public void BarcodeSearch()
        {
            try
            {
                BarcodeSearchDialogFormModel viewModel = new();
                UICommand result = DialogService.ShowDialog(dialogCommands: viewModel.Commands, "Введите штрихкод", nameof(BarcodeSearchDialogForm), viewModel);
                if (result != null && result.Id is MessageBoxResult messageResult && messageResult == MessageBoxResult.OK)
                {
                    BarcodeService_OnBarcodeEvent(viewModel.Barcode);
                }
            }
            catch (Exception)
            {
                //ignore
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
