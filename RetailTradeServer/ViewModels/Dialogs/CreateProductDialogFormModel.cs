using DevExpress.Xpf.Core;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

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
        private readonly IUIManager _manager;
        private readonly IMessageStore _messageStore;
        private int? _selectedProductCategoryId;
        private int? _selectedProductSubCategoryId;
        private int? _selectedUnitId;
        private int? _selectedSupplierId;
        private string _barcode;
        private string _name;
        private decimal _arrivalPrice;
        private decimal _salePrice;
        private string _tnved;

        #endregion

        #region Public Properties

        public GlobalMessageViewModel GlobalMessageViewModel { get; }
        public IEnumerable<ProductCategory> ProductCategories => _productCategoryService.GetAll();
        public IEnumerable<ProductSubcategory> ProductSubCategories => SelectedProductCategoryId != null ? _productSubcategoryService.GetAllByProductCategoryId(SelectedProductCategoryId.Value) : null;
        public IEnumerable<Unit> Units => _unitService.GetAll();
        public IEnumerable<Supplier> Suppliers => _supplierService.GetAll();
        public int? SelectedProductCategoryId
        {
            get => _selectedProductCategoryId;
            set
            {
                _selectedProductCategoryId = value;
                SelectedProductSubcategoryId = 0;
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
        public ICommand TabSelectionChangingCommand { get; }

        #endregion

        #region Constructor

        public CreateProductDialogFormModel(IProductCategoryService productCategoryService,
            IProductSubcategoryService productSubcategoryService,
            IDataService<Unit> unitService,
            IProductService productService,
            ISupplierService supplierService,
            IUIManager manager,
            GlobalMessageViewModel globalMessageViewModel,
            IMessageStore messageStore)
        {
            _productCategoryService = productCategoryService;
            _productSubcategoryService = productSubcategoryService;
            _unitService = unitService;
            _productService = productService;
            _supplierService = supplierService;
            _manager = manager;
            GlobalMessageViewModel = globalMessageViewModel;
            _messageStore = messageStore;

            SavePieceProductCommand = new RelayCommand(SavePieceProduct);
            SavePackagedProductCommand = new RelayCommand(SavePackagedProduct);
            SaveWeightProductCommand = new RelayCommand(SaveWeightProduct);
            SaveProductWithoutBarcodeCommand = new RelayCommand(SaveProductWithoutBarcode);
            CreateSupplierCommand = new RelayCommand(CreateSupplier);
            CreateProductCategoryCommand = new RelayCommand(CreateProductCategory);
            CreateProductSubcategoryCommand = new RelayCommand(CreateProductSubcategory);
            TabSelectionChangingCommand = new ParameterCommand(parameter => TabSelectionChanging(parameter));

            _supplierService.PropertiesChanged += SupplierService_PropertiesChanged;
            _productCategoryService.PropertiesChanged += ProductCategoryService_PropertiesChanged;
            _productSubcategoryService.PropertiesChanged += ProductSubcategoryService_PropertiesChanged;
        }

        #endregion

        #region Private Voids

        private void TabSelectionChanging(object parameter)
        {
            if (parameter is TabControlSelectionChangingEventArgs e)
            {
                if (!CanTabSelect)
                {
                    if (_manager.ShowMessage("Данные не сохранены. Продолжить?", "", MessageBoxButton.YesNo, MessageBoxImage.Question)
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
                else
                {
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
                Barcode = Barcode,
                Name = Name,
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
                ProductSubcategoryId = SelectedProductSubcategoryId.Value,
                UnitId = SelectedUnitId.Value,
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

        private void CreateSupplier()
        {
            _manager.ShowDialog(new CreateSupplierProductDialogFormModal(_supplierService, _manager)
                {
                    Title = "Поставщик (создания)"
                }, 
                new CreateSupplierProductDialogForm());
        }

        private void CreateProductCategory()
        {
            _manager.ShowDialog(new CreateProductCategoryDialogFormModel(_productCategoryService, _manager) { Title = "Группа товара (создания)" }, 
                new CreateProductCategoryDialogForm());
        }

        private void CreateProductSubcategory()
        {
            _manager.ShowDialog(new CreateProductSubcategoryDialogFormModel(_productSubcategoryService, _productCategoryService, _manager) { Title = "Категория товара (создания)" },
                new CreateProductSubcategoryDialogForm());
        }

        private void SupplierService_PropertiesChanged()
        {
            OnPropertyChanged(nameof(Suppliers));
        }

        private void ProductCategoryService_PropertiesChanged()
        {
            OnPropertyChanged(nameof(ProductCategories));
        }

        private void ProductSubcategoryService_PropertiesChanged()
        {
            OnPropertyChanged(nameof(ProductSubCategories));
        }

        #endregion
    }
}
