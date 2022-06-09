using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Spreadsheet;
using RetailTrade.Barcode.Services;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Components;
using RetailTradeServer.Properties;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class CreateProductViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IUnitService _unitService;
        private readonly ISupplierService _supplierService;
        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
        private readonly IMessageStore _messageStore;
        private readonly IBarcodeService _barcodeService;
        private int? _selectedUnitId;
        private int? _selectedSupplierId;
        private int? _selectedTypeProductId;
        private string _barcode;
        private string _name;
        private string _tnved;
        private ObservableCollection<Unit> _units = new();
        private ObservableCollection<Supplier> _suppliers = new();
        private ObservableCollection<TypeProduct> _typeProducts = new();

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
        public int? SelectedUnitId
        {
            get => _selectedUnitId;
            set
            {
                _selectedUnitId = value;
                OnPropertyChanged(nameof(SelectedUnitId));
            }
        }
        public int? SelectedTypeProductId
        {
            get => _selectedTypeProductId;
            set
            {
                _selectedTypeProductId = value;
                OnPropertyChanged(nameof(SelectedTypeProductId));
            }
        }
        public int? SelectedSupplierId
        {
            get => _selectedSupplierId;
            set
            {
                _selectedSupplierId = value;
                OnPropertyChanged(nameof(SelectedSupplierId));
            }
        }
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged(nameof(Barcode));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string TNVED
        {
            get => _tnved;
            set
            {
                _tnved = value;
                OnPropertyChanged(nameof(TNVED));
            }
        }
        public CustomSpreadsheetControl CustomSpreadsheet { get; set; }

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
            IBarcodeService barcodeService)
        {
            _typeProductService = typeProductService;
            _unitService = unitService;
            _productService = productService;
            _supplierService = supplierService;
            _messageStore = messageStore;
            _barcodeService = barcodeService;
            GlobalMessageViewModel = new(_messageStore);

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
            SelectedTypeProductId = obj.Id;
        }

        private void SupplierService_OnSupplierCreated(Supplier supplier)
        {
            Suppliers.Add(supplier);
            SelectedSupplierId = supplier.Id;
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
            Barcode = barcode;
        }

        private void CleareAllItems()
        {
            SelectedSupplierId = null;
            SelectedTypeProductId = null;
            Barcode = string.Empty;
            Name = string.Empty;
            SelectedUnitId = null;
            TNVED = string.Empty;
        }

        private void UnitMeasurementDialogFormModel_OnSelected(Unit unit)
        {
            try
            {
                SelectedUnitId = unit.Id;
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
            if (SelectedTypeProductId == null || SelectedTypeProductId == 0)
            {
                _messageStore.SetCurrentMessage("Выберите вид товара.", MessageType.Error);
                return;
            }
            if (SelectedSupplierId == null || SelectedSupplierId == 0)
            {
                _messageStore.SetCurrentMessage("Выберите поставщика.", MessageType.Error);
                return;
            }
            if (string.IsNullOrEmpty(Name))
            {
                _messageStore.SetCurrentMessage("Введите наименование товара.", MessageType.Error);
                return;
            }
            if (SelectedUnitId == null || SelectedUnitId == 0)
            {
                _messageStore.SetCurrentMessage("Выберите единицу измерения.", MessageType.Error);
                return;
            }

            if (await _productService.SearchByBarcode(Barcode))
            {
                _ = MessageBoxService.ShowMessage($"Товар со штрих-кодом \"{Barcode}\" существует.", "", MessageButton.OK, MessageIcon.Error);
            }
            else
            {
                if (await _productService.CreateAsync(new Product
                {
                    Barcode = Barcode,
                    Name = Name,
                    SupplierId = SelectedSupplierId.Value,
                    UnitId = SelectedUnitId.Value,
                    TypeProductId = SelectedTypeProductId.Value,
                    TNVED = TNVED
                }) != null)
                {
                    _messageStore.SetCurrentMessage("Товар успешно добавлено.", MessageType.Success);
                    CleareAllItems();
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

        #endregion

        #region Dispose

        public override void Dispose()
        {
            base.Dispose();
        }

        #endregion
    }
}
