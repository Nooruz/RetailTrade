using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using RetailTrade.Barcode.Services;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Views;
using RetailTradeServer.Commands;
using RetailTradeServer.Components;
using RetailTradeServer.Properties;
using RetailTradeServer.State.Messages;
using RetailTradeServer.Validation;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class CreateProductViewModel : BaseViewModel, IDataErrorInfo
    {
        #region Private Members

        private readonly IUnitService _unitService;
        private readonly ISupplierService _supplierService;
        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
        private readonly IMessageStore _messageStore;
        private readonly IBarcodeService _barcodeService;
        private readonly IProductBarcodeService _productBarcodeService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IArrivalProductService _arrivalProductService;
        private readonly IProductSaleService _productSaleService;
        private Product _createdProduct = new();
        private ObservableCollection<Unit> _units = new();
        private ObservableCollection<Supplier> _suppliers = new();
        private ObservableCollection<TypeProduct> _typeProducts = new();
        private ObservableCollection<ProductBarcode> _productBarcodes = new();
        private ProductBarcode _selectedProductBarcode;
        private IEnumerable<WareHouseView> _wareHouseViews;
        private IEnumerable<ArrivalProduct> _arrivalProducts;
        private IEnumerable<ProductSale> _productSales;

        #endregion

        #region Public Properties

        public GlobalMessageViewModel GlobalMessageViewModel { get; }
        public ObservableCollection<Unit> Units
        {
            get => _units;
            set
            {
                _units = value;
                OnPropertyChanged(nameof(Units));
            }
        }
        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers;
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
            }
        }
        public ObservableCollection<TypeProduct> TypeProducts
        {
            get => _typeProducts;
            set
            {
                _typeProducts = value;
                OnPropertyChanged(nameof(TypeProducts));
            }
        }
        public ObservableCollection<ProductBarcode> ProductBarcodes
        {
            get => _productBarcodes;
            set
            {
                _productBarcodes = value;
                OnPropertyChanged(nameof(ProductBarcodes));
            }
        }
        public CustomSpreadsheetControl CustomSpreadsheet { get; set; }
        public Product CreatedProduct
        {
            get => _createdProduct;
            set
            {
                _createdProduct = value;
                OnPropertyChanged(nameof(CreatedProduct));
                if (CreatedProduct != null && CreatedProduct.Id != 0)
                {
                    GetWeareHouse(CreatedProduct.Id);
                }
            }
        }
        public ProductBarcode SelectedProductBarcode
        {
            get => _selectedProductBarcode;
            set
            {
                _selectedProductBarcode = value;
                OnPropertyChanged(nameof(SelectedProductBarcode));
            }
        }
        public TableView BarcodeTableView { get; set; }
        public GridControl BarcodeGridControl { get; set; }
        public IEnumerable<WareHouseView> WareHouseViews
        {
            get => _wareHouseViews;
            set
            {
                _wareHouseViews = value;
                OnPropertyChanged(nameof(WareHouseViews));
            }
        }
        public IEnumerable<ArrivalProduct> ArrivalProducts
        {
            get => _arrivalProducts;
            set
            {
                _arrivalProducts = value;
                OnPropertyChanged(nameof(ArrivalProducts));
            }
        }
        public IEnumerable<ProductSale> ProductSales
        {
            get => _productSales;
            set
            {
                _productSales = value;
                OnPropertyChanged(nameof(ProductSales));
            }
        }

        #endregion

        #region Commands

        public ICommand CreateSupplierCommand => new RelayCommand(() => WindowService.Show(nameof(CreateSupplierProductDialogForm), new CreateSupplierProductDialogFormModal(_supplierService) { Title = "Поставщик (создания)" }));
        public ICommand CreateTypeProductCommand => new RelayCommand(() => WindowService.Show(nameof(TypeProductDialogForm), new TypeProductDialogFormModel(_typeProductService) { Title = "Виды товаров (создание)" }));

        #endregion

        #region Constructor

        public CreateProductViewModel(ITypeProductService typeProductService,
            IUnitService unitService,
            IProductService productService,
            ISupplierService supplierService,
            IMessageStore messageStore,
            IBarcodeService barcodeService,
            IProductBarcodeService productBarcodeService,
            IWareHouseService wareHouseService,
            IProductSaleService productSaleService,
            IArrivalProductService arrivalProductService)
        {
            _typeProductService = typeProductService;
            _unitService = unitService;
            _productService = productService;
            _supplierService = supplierService;
            _messageStore = messageStore;
            _barcodeService = barcodeService;
            _productBarcodeService = productBarcodeService;
            _wareHouseService = wareHouseService;
            _productSaleService = productSaleService;
            _arrivalProductService = arrivalProductService;
            GlobalMessageViewModel = new(_messageStore);

            Header = "Товары (создание)";

            _messageStore.Close();

            CloseCommand = new RelayCommand(() => CurrentWindowService.Close());

            _supplierService.OnSupplierCreated += SupplierService_OnSupplierCreated;
            _typeProductService.OnTypeProductCreated += TypeProductService_OnTypeProductCreated;
            _unitService.OnCreated += UnitService_OnCreated;
            _unitService.OnEdited += UnitService_OnEdited;
        }

        #endregion

        #region Private Voids

        private void UnitService_OnEdited(Unit unit)
        {
            try
            {
                Unit editedUnit = Units.FirstOrDefault(u => u.Id == unit.Id);
                editedUnit.ShortName = unit.ShortName;
                editedUnit.LongName = unit.LongName;
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void UnitService_OnCreated(Unit unit)
        {
            try
            {
                Units.Add(unit);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void TypeProductService_OnTypeProductCreated(TypeProduct obj)
        {
            TypeProducts.Add(obj);
            CreatedProduct.TypeProductId = obj.Id;
        }

        private void SupplierService_OnSupplierCreated(Supplier supplier)
        {
            Suppliers.Add(supplier);
            CreatedProduct.SupplierId = supplier.Id;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _supplierService.OnSupplierCreated -= SupplierService_OnSupplierCreated;
                _typeProductService.OnTypeProductCreated -= TypeProductService_OnTypeProductCreated;
                _barcodeService.OnBarcodeEvent -= BarcodeService_OnBarcodeEvent;
                if (Enum.IsDefined(typeof(BarcodeDevice), Settings.Default.BarcodeDefaultDevice))
                {
                    _barcodeService.Close(Enum.Parse<BarcodeDevice>(Settings.Default.BarcodeDefaultDevice));
                }
                _barcodeService.OnBarcodeEvent -= BarcodeService_OnBarcodeEvent;
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void BarcodeService_OnBarcodeEvent(string barcode)
        {
            //Barcode = barcode;
        }

        private void UnitMeasurementDialogFormModel_OnSelected(Unit unit)
        {
            try
            {
                //SelectedUnitId = unit.Id;
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void CustomSpreadsheet_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Windows.Point point = e.GetPosition(CustomSpreadsheet);
                Cell cell = CustomSpreadsheet.GetCellFromPoint(new PointF((float)point.X, (float)point.Y));
                string barcodeCell = cell.GetReferenceA1();
                if (barcodeCell == "B5")
                {
                    MessageBox.Show("B5");
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void CustomSpreadsheet_HyperlinkClick(object sender, DevExpress.XtraSpreadsheet.HyperlinkClickEventArgs e)
        {
            try
            {
                string cellAddress = e.TargetRange.GetReferenceA1();
                switch (cellAddress)
                {
                    case "B5":
                        MessageBox.Show("B5");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void BarcodeTableView_ValidateCell(object sender, GridCellValidationEventArgs e)
        {
            try
            {
                if (e.Value == null)
                {
                    MessageBoxService.ShowMessage($"Штрихкод обязателен для заполнения.", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
                    e.ErrorContent = $"Штрихкод обязателен для заполнения.";
                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                    e.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                _ = MessageBoxService.ShowMessage(ex.Message, "Sale Page", MessageButton.OK, MessageIcon.Error);
                e.ErrorContent = ex.Message;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.IsValid = false;
            }
        }

        private async void GetWeareHouse(int productId)
        {
            try
            {
                WareHouseViews = await _wareHouseService.GetByProductId(productId);
                ArrivalProducts = await _arrivalProductService.GetByProductId(productId);
                ProductSales = await _productSaleService.GetByProductId(productId);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion

        #region Public Voids

        [Command]
        public void UnitDialogForm()
        {
            try
            {
                UnitMeasurementDialogFormModel viewModel = new(_unitService);
                viewModel.OnSelected += UnitMeasurementDialogFormModel_OnSelected;
                WindowService.Show(nameof(UnitMeasurementDialogForm), viewModel);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public async void UserControlLoaded(object parameter)
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
                Suppliers = new(await _supplierService.GetAllAsync());
                Units = new(await _unitService.GetAllAsync());
                TypeProducts = new(await _typeProductService.GetTypesAsync());

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
        public async void SaveProduct()
        {
            string error = EnableValidationAndGetError();
            if (error != null)
            {
                _messageStore.SetCurrentMessage(error, MessageType.Error);
                return;
            }

            if (CreatedProduct.Id == 0)
            {
                if (await _productService.CreateAsync(CreatedProduct) != null)
                {
                    _messageStore.SetCurrentMessage("Товар создан.", MessageType.Success);
                    Header = $"Товары ({CreatedProduct.Name})";
                }
            }
            else
            {
                if (await _productService.UpdateAsync(CreatedProduct.Id, CreatedProduct) != null)
                {
                    _messageStore.SetCurrentMessage("Товар сохранен.", MessageType.Success);
                    Header = $"Товары ({CreatedProduct.Name})";
                }
            }
        }

        [Command]
        public void SpreadsheetControlLoaded(object sender)
        {
            if (sender is RoutedEventArgs e)
            {
                if (e.Source is CustomSpreadsheetControl spreadsheetControl)
                {
                    CustomSpreadsheet = spreadsheetControl;
                    CustomSpreadsheet.ActiveWorksheet["L2"].Value = "Товар 1";
                    CustomSpreadsheet.ActiveWorksheet["L4"].Value = "00001";
                    CustomSpreadsheet.ActiveWorksheet["J7"].Value = "Напидки";
                    CustomSpreadsheet.ActiveWorksheet["J8"].Value = "шт";

                    Hyperlink barcodeHyperlink = CustomSpreadsheet.ActiveWorksheet.Hyperlinks.Add(CustomSpreadsheet.ActiveWorksheet.Cells["B5"], "Лист1!B5", false, "Штрихкоды (0)");
                    barcodeHyperlink.TooltipText = "Штрихкоды";

                    CustomSpreadsheet.HyperlinkClick += CustomSpreadsheet_HyperlinkClick;
                }
            }
        }

        [Command]
        public void GenerateBarcode()
        {
            try
            {
                if (CreatedProduct.Id == 0)
                {
                    if (MessageBoxService.ShowMessage("Товар еще не создан.\nСоздать товар?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                    {
                        SaveProduct();
                    };
                }
                else
                {
                    ((ButtonEdit)BarcodeTableView.ActiveEditor).SetValue(BaseEdit.EditValueProperty, $"2{new('0', 12 - CreatedProduct.Id.ToString().Length)}{CreatedProduct.Id}");
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void DeleteBarcode()
        {
            try
            {
                if (SelectedProductBarcode != null)
                {
                    if (CreatedProduct.ProductBarcodes != null && CreatedProduct.ProductBarcodes.Any())
                    {
                        CreatedProduct.ProductBarcodes.Remove(SelectedProductBarcode);
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void BarcodeGridControlLoaded(object sender)
        {
            if (sender is RoutedEventArgs e)
            {
                if (e.Source is GridControl gridControl)
                {
                    BarcodeGridControl = gridControl;
                    BarcodeTableView = BarcodeGridControl.View as TableView;
                    BarcodeTableView.ValidateCell += BarcodeTableView_ValidateCell;
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

        #region Validate

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                this.RaisePropertiesChanged();
                return error;
            }
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = CreatedProduct;
                string error =
                    me[BindableBase.GetPropertyName(() => CreatedProduct.Name)] + 
                    me[BindableBase.GetPropertyName(() => CreatedProduct.TypeProductId)] +
                    me[BindableBase.GetPropertyName(() => CreatedProduct.SupplierId)] +
                    me[BindableBase.GetPropertyName(() => CreatedProduct.UnitId)];
                if (!string.IsNullOrEmpty(error))
                    return "Пожалуйста, проверьте введенные данные.\n" + error;
                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
