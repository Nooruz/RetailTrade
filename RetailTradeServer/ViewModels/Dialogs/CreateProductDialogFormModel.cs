using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Barcode;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateProductDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IDataService<Unit> _unitService;
        private readonly ISupplierService _supplierService;
        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
        private readonly IMessageStore _messageStore;
        private readonly IZebraBarcodeScanner _zebraBarcodeScanner;
        private readonly IComBarcodeService _comBarcodeService;
        private int? _selectedUnitId;
        private int? _selectedSupplierId;
        private int? _selectedTypeProductId;
        private string _barcode;
        private string _name;
        private decimal _arrivalPrice;
        private decimal _salePrice;
        private string _tnved;
        private IEnumerable<Unit> _units;
        private ObservableCollection<Supplier> _suppliers;
        private ObservableCollection<TypeProduct> _typeProducts;

        #endregion

        #region Public Properties

        public GlobalMessageViewModel GlobalMessageViewModel { get; }
        public IEnumerable<Unit> Units
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
            get => _suppliers ?? new();
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
            }
        }
        public ObservableCollection<TypeProduct> TypeProducts
        {
            get => _typeProducts ?? new();
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
                OnPropertyChanged(nameof(CanTabSelect));
            }
        }
        public int? SelectedTypeProductId
        {
            get => _selectedTypeProductId;
            set
            {
                _selectedTypeProductId = value;
                OnPropertyChanged(nameof(SelectedTypeProductId));
                OnPropertyChanged(nameof(CanTabSelect));
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
                OnPropertyChanged(nameof(CanTabSelect));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(CanTabSelect));
            }
        }
        public decimal ArrivalPrice
        {
            get => _arrivalPrice;
            set
            {
                _arrivalPrice = value;
                OnPropertyChanged(nameof(ArrivalPrice));
                OnPropertyChanged(nameof(CanTabSelect));
            }
        }
        public decimal SalePrice
        {
            get => _salePrice;
            set
            {
                _salePrice = value;
                OnPropertyChanged(nameof(SalePrice));
                OnPropertyChanged(nameof(CanTabSelect));
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
        public bool CanTabSelect => string.IsNullOrEmpty(Barcode) &&
                                    string.IsNullOrEmpty(Name) &&
                                    SelectedUnitId == null &&
                                    SelectedTypeProductId == null &&
                                    ArrivalPrice == 0 &&
                                    SalePrice == 0;

        #endregion

        #region Commands

        /// <summary>
        /// Команда сохранение штучного товара
        /// </summary>
        public ICommand SavePieceProductCommand => new RelayCommand(SavePieceProduct);
        /// <summary>
        /// Команда сохранение товара без штрих-кода
        /// </summary>
        public ICommand SaveProductWithoutBarcodeCommand => new RelayCommand(SaveProductWithoutBarcode);
        public ICommand CreateSupplierCommand => new RelayCommand(() => WindowService.Show(nameof(CreateSupplierProductDialogForm), new CreateSupplierProductDialogFormModal(_supplierService) { Title = "Поставщик (создания)" }));
        public ICommand CreateTypeProductCommand => new RelayCommand(() => WindowService.Show(nameof(TypeProductDialogForm), new TypeProductDialogFormModel(_typeProductService) { Title = "Виды товаров (создание)" }));
        public ICommand TabControlLoadedCommand => new ParameterCommand(sender => TabControlLoaded(sender));
        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);

        #endregion

        #region Constructor

        public CreateProductDialogFormModel(ITypeProductService typeProductService,
            IDataService<Unit> unitService,
            IProductService productService,
            ISupplierService supplierService,
            IMessageStore messageStore,
            IZebraBarcodeScanner zebraBarcodeScanner,
            IComBarcodeService comBarcodeService)
        {
            _typeProductService = typeProductService;
            _unitService = unitService;
            _productService = productService;
            _supplierService = supplierService;
            _messageStore = messageStore;
            _zebraBarcodeScanner = zebraBarcodeScanner;
            _comBarcodeService = comBarcodeService;
            GlobalMessageViewModel = new(_messageStore);

            _messageStore.Close();

            CloseCommand = new RelayCommand(() => CurrentWindowService.Close());

            _supplierService.OnSupplierCreated += SupplierService_OnSupplierCreated;
            _typeProductService.OnTypeProductCreated += TypeProductService_OnTypeProductCreated;
        }

        #endregion

        #region Private Voids

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

        private async void UserControlLoaded()
        {
            Suppliers = new(await _supplierService.GetAllAsync());
            Units = await _unitService.GetAllAsync();
            TypeProducts = new(await _typeProductService.GetTypesAsync());

            //_zebraBarcodeScanner.Open();
            //_zebraBarcodeScanner.OnBarcodeEvent += ZebraBarcodeScanner_OnBarcodeEvent;
            _comBarcodeService.Open();
            _comBarcodeService.OnBarcodeEvent += ComBarcodeService_OnBarcodeEvent;
        }

        private void ComBarcodeService_OnBarcodeEvent(string obj)
        {
            Barcode = obj;
        }

        private void ZebraBarcodeScanner_OnBarcodeEvent(string obj)
        {
            Barcode = obj;
        }

        private void TabControlLoaded(object sender)
        {
            if (sender is RoutedEventArgs e)
            {
                if (e.Source is DXTabControl tabControl)
                {
                    tabControl.SelectionChanging += TabControl_SelectionChanging;
                }
            }
        }

        private void TabControl_SelectionChanging(object sender, TabControlSelectionChangingEventArgs e)
        {
            if (CanTabSelect)
            {
                _messageStore.Close();
            }
            else
            {
                if (MessageBoxService.ShowMessage("Данные не сохранены. Продолжить?", "", MessageButton.YesNo, MessageIcon.Question)
                    == MessageResult.No)
                {
                    e.Cancel = false;
                    CleareAllItems();
                }
                else
                {
                    e.Cancel = true;
                    _messageStore.Close();
                }
            }
        }

        private void CleareAllItems()
        {
            SelectedSupplierId = null;
            SelectedTypeProductId = null;
            Barcode = string.Empty;
            Name = string.Empty;
            SelectedUnitId = null;
            SalePrice = 0;
            ArrivalPrice = 0;
            TNVED = string.Empty;
        }

        /// <summary>
        /// Сохранение штучного товара
        /// </summary>
        private async void SavePieceProduct()
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
            if (string.IsNullOrEmpty(Barcode))
            {
                _messageStore.SetCurrentMessage("Введите штрих-код товара.", MessageType.Error);
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
            if (ArrivalPrice <= 0)
            {
                _messageStore.SetCurrentMessage("Введите цену прихода товара.", MessageType.Error);
                return;
            }
            if (SalePrice <= 0)
            {
                _messageStore.SetCurrentMessage("Введите цену продажа товара.", MessageType.Error);
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
                    TNVED = TNVED,
                    ArrivalPrice = ArrivalPrice,
                    SalePrice = SalePrice
                }) != null)
                {
                    _messageStore.SetCurrentMessage("Товар успешно добавлено.", MessageType.Success);
                    CleareAllItems();
                }
            }            
        }

        /// <summary>
        /// Сохранение товара без штрих-кода
        /// </summary>
        private async void SaveProductWithoutBarcode()
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
            if (ArrivalPrice <= 0)
            {
                _messageStore.SetCurrentMessage("Введите цену прихода товара.", MessageType.Error);
                return;
            }
            if (SalePrice <= 0)
            {
                _messageStore.SetCurrentMessage("Введите цену продажа товара.", MessageType.Error);
                return;
            }

            if (await _productService.CreateAsync(new Product
            {
                Name = Name,
                UnitId = SelectedUnitId.Value,
                SupplierId = SelectedSupplierId.Value,
                TypeProductId = SelectedTypeProductId.Value,
                TNVED = TNVED,
                ArrivalPrice = ArrivalPrice,
                SalePrice = SalePrice,
                WithoutBarcode = true
            }) != null)
            {
                _messageStore.SetCurrentMessage("Товар успешно добавлено.", MessageType.Success);
                CleareAllItems();
            }
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            _comBarcodeService.OnBarcodeEvent -= ComBarcodeService_OnBarcodeEvent;
            _supplierService.OnSupplierCreated -= SupplierService_OnSupplierCreated;
            _typeProductService.OnTypeProductCreated -= TypeProductService_OnTypeProductCreated;
            _comBarcodeService.Close();
            base.Dispose();
        }

        #endregion
    }
}
