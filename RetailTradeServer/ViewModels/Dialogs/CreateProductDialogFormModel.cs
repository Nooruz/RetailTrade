using DevExpress.Xpf.Core;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Barcode;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Dialogs.Base;
using SalePageServer.State.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DialogService = SalePageServer.State.Dialogs.DialogService;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateProductDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductCategoryService _productCategoryService;
        private readonly IDataService<Unit> _unitService;
        private readonly ISupplierService _supplierService;
        private readonly IProductSubcategoryService _productSubcategoryService;
        private readonly IProductService _productService;
        private readonly IDialogService _dialogService;
        private readonly IMessageStore _messageStore;
        private readonly IZebraBarcodeScanner _zebraBarcodeScanner;
        private readonly IComBarcodeService _comBarcodeService;
        private int? _selectedProductCategoryId;
        private int? _selectedProductSubCategoryId;
        private int? _selectedUnitId;
        private int? _selectedSupplierId;
        private string _barcode;
        private string _name;
        private decimal _arrivalPrice;
        private decimal _salePrice;
        private string _tnved;
        private ObservableCollection<ProductCategory> _productCategories;
        private ObservableCollection<ProductSubcategory> productSubcategories;
        private IEnumerable<Unit> _units;
        private ObservableCollection<Supplier> _suppliers;

        #endregion

        #region Public Properties

        public GlobalMessageViewModel GlobalMessageViewModel { get; }
        public ObservableCollection<ProductCategory> ProductCategories
        {
            get => _productCategories ?? new();
            set
            {
                _productCategories = value;
                OnPropertyChanged(nameof(ProductCategories));
            }
        }
        public ObservableCollection<ProductSubcategory> ProductSubCategories
        {
            get => productSubcategories ?? new();
            set
            {
                productSubcategories = value;
                OnPropertyChanged(nameof(ProductSubCategories));
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
        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers ?? new();
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
            }
        }
        public int? SelectedProductCategoryId
        {
            get => _selectedProductCategoryId;
            set
            {
                _selectedProductCategoryId = value;
                SelectedProductSubcategoryId = null;
                OnPropertyChanged(nameof(SelectedProductCategoryId));
                OnPropertyChanged(nameof(ProductSubCategories));
                OnPropertyChanged(nameof(CanTabSelect));
            }
        }
        public int? SelectedProductSubcategoryId
        {
            get => _selectedProductSubCategoryId;
            set
            {
                _selectedProductSubCategoryId = value;
                OnPropertyChanged(nameof(SelectedProductSubcategoryId));
                OnPropertyChanged(nameof(CanTabSelect));
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
                                    SelectedProductCategoryId == null &&
                                    SelectedProductSubcategoryId == null &&
                                    SelectedUnitId == null &&
                                    ArrivalPrice == 0 &&
                                    SalePrice == 0;

        #endregion

        #region Piece Product Properties



        #endregion

        #region Commands

        /// <summary>
        /// Команда сохранение штучного товара
        /// </summary>
        public ICommand SavePieceProductCommand { get; }

        /// <summary>
        /// Команда сохранение фасованного товара
        /// </summary>
        public ICommand SavePackagedProductCommand { get; }

        /// <summary>
        /// Команда сохранение весового товара
        /// </summary>
        public ICommand SaveWeightProductCommand { get; }

        /// <summary>
        /// Команда сохранение товара без штрих-кода
        /// </summary>
        public ICommand SaveProductWithoutBarcodeCommand { get; }
        public ICommand CreateSupplierCommand { get; }
        public ICommand CreateProductCategoryCommand { get; }
        public ICommand CreateProductSubcategoryCommand { get; }
        public ICommand TabControlLoadedCommand { get; }
        public ICommand UserControlLoadedCommand { get; }
        public ICommand SelectedProductCategoryChangedCommand { get; }

        #endregion

        #region Constructor

        public CreateProductDialogFormModel(IProductCategoryService productCategoryService,
            IProductSubcategoryService productSubcategoryService,
            IDataService<Unit> unitService,
            IProductService productService,
            ISupplierService supplierService,
            GlobalMessageViewModel globalMessageViewModel,
            IMessageStore messageStore,
            IZebraBarcodeScanner zebraBarcodeScanner,
            IComBarcodeService comBarcodeService)
        {
            _productCategoryService = productCategoryService;
            _productSubcategoryService = productSubcategoryService;
            _unitService = unitService;
            _productService = productService;
            _supplierService = supplierService;
            _dialogService = new DialogService();
            GlobalMessageViewModel = globalMessageViewModel;
            _messageStore = messageStore;
            _zebraBarcodeScanner = zebraBarcodeScanner;
            _comBarcodeService = comBarcodeService;

            _messageStore.Close();

            SavePieceProductCommand = new RelayCommand(SavePieceProduct);
            SavePackagedProductCommand = new RelayCommand(SavePackagedProduct);
            SaveWeightProductCommand = new RelayCommand(SaveWeightProduct);
            SaveProductWithoutBarcodeCommand = new RelayCommand(SaveProductWithoutBarcode);
            CreateSupplierCommand = new RelayCommand(CreateSupplier);
            CreateProductCategoryCommand = new RelayCommand(CreateProductCategory);
            CreateProductSubcategoryCommand = new RelayCommand(CreateProductSubcategory);
            TabControlLoadedCommand = new ParameterCommand(sender => TabControlLoaded(sender));
            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);
            SelectedProductCategoryChangedCommand = new RelayCommand(SelectedProductCategoryChanged);

            _supplierService.OnSupplierCreated += SupplierService_OnSupplierCreated;
            _productCategoryService.OnProductCategoryCreated += ProductCategoryService_OnProductCategoryCreated;
            _productSubcategoryService.OnProductSubcategoryCreated += ProductSubcategoryService_OnProductSubcategoryCreated;
        }

        #endregion

        #region Private Voids

        private void SupplierService_OnSupplierCreated(Supplier supplier)
        {
            Suppliers.Add(supplier);
            SelectedSupplierId = supplier.Id;
        }

        private async void SelectedProductCategoryChanged()
        {
            if (SelectedProductCategoryId != null)
            {
                ProductSubCategories.Clear();
                ProductSubCategories = new(await _productSubcategoryService.GetAllByProductCategoryIdAsync(SelectedProductCategoryId.Value));
            }
        }

        private async void UserControlLoaded()
        {
            ProductCategories = new(await _productCategoryService.GetAllAsync());
            Suppliers = new(await _supplierService.GetAllAsync());
            Units = await _unitService.GetAllAsync();

            _zebraBarcodeScanner.Open();
            _zebraBarcodeScanner.OnBarcodeEvent += ZebraBarcodeScanner_OnBarcodeEvent;
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

        private void ProductCategoryService_OnProductCategoryCreated(ProductCategory productCategory)
        {
            ProductCategories.Add(productCategory);
            SelectedProductCategoryId = productCategory.Id;
        }

        private void ProductSubcategoryService_OnProductSubcategoryCreated(ProductSubcategory productSubcategory)
        {
            ProductSubCategories.Add(productSubcategory);
            SelectedProductSubcategoryId = productSubcategory.Id;
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
                if (_dialogService.ShowMessage("Данные не сохранены. Продолжить?", "", MessageBoxButton.YesNo, MessageBoxImage.Question)
                    == MessageBoxResult.No)
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
            Barcode = string.Empty;
            Name = string.Empty;
            SelectedProductCategoryId = null;
            SelectedProductSubcategoryId = null;
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
            if (SelectedProductCategoryId == null || SelectedProductCategoryId == 0)
            {
                _messageStore.SetCurrentMessage("Выберите группу товара.", MessageType.Error);
                return;
            }
            if (SelectedProductSubcategoryId == null || SelectedProductSubcategoryId == 0)
            {
                _messageStore.SetCurrentMessage("Выберите категории товара.", MessageType.Error);
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

            var product = await _productService.CreateAsync(new Product
            {
                Barcode = Barcode,
                Name = Name,
                ProductCategoryId = SelectedProductCategoryId.Value,
                ProductSubcategoryId = SelectedProductSubcategoryId.Value,
                UnitId = SelectedUnitId.Value,
                SupplierId = SelectedSupplierId.Value,
                TNVED = TNVED,
                ArrivalPrice = ArrivalPrice,
                SalePrice = SalePrice
            });

            if (product != null)
            {
                _messageStore.SetCurrentMessage("Товар успешно добавлено.", MessageType.Success);
                CleareAllItems();
            }
        }

        /// <summary>
        /// Сохранение фасованного товара
        /// </summary>
        private async void SavePackagedProduct()
        {
            if (string.IsNullOrEmpty(Name))
            {
                _messageStore.SetCurrentMessage("Введите наименование товара.", MessageType.Error);
                return;
            }
            if (SelectedProductCategoryId == null || SelectedProductCategoryId == 0)
            {
                _messageStore.SetCurrentMessage("Выберите группу товара.", MessageType.Error);
                return;
            }
            if (SelectedProductSubcategoryId == null || SelectedProductSubcategoryId == 0)
            {
                _messageStore.SetCurrentMessage("Выберите категори товара.", MessageType.Error);
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

            var product = await _productService.CreateAsync(new Product
            {
                Name = Name,
                ProductCategoryId = SelectedProductCategoryId.Value,
                ProductSubcategoryId = SelectedProductSubcategoryId.Value,
                UnitId = SelectedUnitId.Value,
                TNVED = TNVED,
                ArrivalPrice = ArrivalPrice,
                SalePrice = SalePrice
            });

            if (product != null)
            {
                _messageStore.SetCurrentMessage("Товар успешно добавлено.", MessageType.Success);
                CleareAllItems();
            }
        }

        /// <summary>
        /// Сохранение весового товара
        /// </summary>
        private async void SaveWeightProduct()
        {
            if (string.IsNullOrEmpty(Name))
            {
                _messageStore.SetCurrentMessage("Введите наименование товара.", MessageType.Error);
                return;
            }
            if (SelectedProductCategoryId == null || SelectedProductCategoryId == 0)
            {
                _messageStore.SetCurrentMessage("Выберите группу товара.", MessageType.Error);
                return;
            }
            if (SelectedProductSubcategoryId == null || SelectedProductSubcategoryId == 0)
            {
                _messageStore.SetCurrentMessage("Выберите категори товара.", MessageType.Error);
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

            var product = await _productService.CreateAsync(new Product
            {
                Name = Name,
                ProductCategoryId = SelectedProductCategoryId.Value,
                ProductSubcategoryId = SelectedProductSubcategoryId.Value,
                UnitId = SelectedUnitId.Value,
                TNVED = TNVED,
                ArrivalPrice = ArrivalPrice,
                SalePrice = SalePrice
            });

            if (product != null)
            {
                _messageStore.SetCurrentMessage("Товар успешно добавлено.", MessageType.Success);
                CleareAllItems();
            }
        }

        /// <summary>
        /// Сохранение товара без штрих-кода
        /// </summary>
        private async void SaveProductWithoutBarcode()
        {
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
            if (SelectedProductCategoryId == null || SelectedProductCategoryId == 0)
            {
                _messageStore.SetCurrentMessage("Выберите группу товара.", MessageType.Error);
                return;
            }
            if (SelectedProductSubcategoryId == null || SelectedProductSubcategoryId == 0)
            {
                _messageStore.SetCurrentMessage("Выберите категори товара.", MessageType.Error);
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

            var product = await _productService.CreateAsync(new Product
            {
                Name = Name,
                ProductCategoryId = SelectedProductCategoryId.Value,
                ProductSubcategoryId = SelectedProductSubcategoryId.Value,
                UnitId = SelectedUnitId.Value,
                SupplierId = SelectedSupplierId.Value,
                TNVED = TNVED,
                ArrivalPrice = ArrivalPrice,
                SalePrice = SalePrice,
                WithoutBarcode = true
            });

            if (product != null)
            {
                _messageStore.SetCurrentMessage("Товар успешно добавлено.", MessageType.Success);
                CleareAllItems();
            }
        }

        private async void CreateSupplier()
        {
            await _dialogService.ShowDialog(new CreateSupplierProductDialogFormModal(_supplierService, _dialogService)
            {
                Title = "Поставщик (создания)"
            });
        }

        private async void CreateProductCategory()
        {
            await _dialogService.ShowDialog(new CreateProductCategoryDialogFormModel(_productCategoryService, _dialogService) { Title = "Категория товара (создания)" });
        }

        private async void CreateProductSubcategory()
        {
            await _dialogService.ShowDialog(new CreateProductSubcategoryDialogFormModel(_productSubcategoryService, _productCategoryService, _dialogService) { Title = "Группа товара (создания)" });
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            _productCategoryService.OnProductCategoryCreated -= ProductCategoryService_OnProductCategoryCreated;
            _productSubcategoryService.OnProductSubcategoryCreated -= ProductSubcategoryService_OnProductSubcategoryCreated;
            base.Dispose();
        }

        #endregion
    }
}
