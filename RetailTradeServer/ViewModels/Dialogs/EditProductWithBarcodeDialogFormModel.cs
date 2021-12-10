using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class EditProductWithBarcodeDialogFormModel : BaseDialogViewModel
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
        private ObservableCollection<ProductCategory> _productCategories;
        private ObservableCollection<ProductSubcategory> _productSubcategory;
        private ObservableCollection<Unit> _units;
        private ObservableCollection<Supplier> _suppliers;
        private Product _editProduct;

        #endregion

        #region Public Properties

        public GlobalMessageViewModel GlobalMessageViewModel { get; }
        public Product EditProduct
        {
            get => _editProduct;
            set
            {
                _editProduct = value;
                if (_editProduct != null)
                {
                    Name = _editProduct.Name;
                    ArrivalPrice = _editProduct.ArrivalPrice;
                    SalePrice = _editProduct.SalePrice;
                    TNVED = _editProduct.TNVED;
                    Barcode = _editProduct.Barcode;
                    SelectedSupplierId = _editProduct.SupplierId;
                    SelectedUnitId = _editProduct.UnitId;
                    SelectedProductCategoryId = _editProduct.ProductSubcategory.ProductCategoryId;
                }
                OnPropertyChanged(nameof(EditProduct));
            }
        }
        public ObservableCollection<ProductCategory> ProductCategories
        {
            get => _productCategories ?? (new());
            set
            {
                _productCategories = value;
                OnPropertyChanged(nameof(ProductCategories));
            }
        }
        public ObservableCollection<ProductSubcategory> ProductSubCategories
        {
            get => _productSubcategory ?? (new());
            set
            {
                _productSubcategory = value;
                OnPropertyChanged(nameof(ProductSubCategories));
            }
        }
        public ObservableCollection<Unit> Units
        {
            get => _units ?? (new());
            set
            {
                _units = value;
                OnPropertyChanged(nameof(Units));
            }
        }
        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers ?? (new());
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
                SelectedProductSubcategoryId = 0;
                OnPropertyChanged(nameof(SelectedProductCategoryId));
                OnPropertyChanged(nameof(ProductSubCategories));
                SelectedProductCategoryChanged();
            }
        }
        public int? SelectedProductSubcategoryId
        {
            get => _selectedProductSubCategoryId;
            set
            {
                _selectedProductSubCategoryId = value;
                OnPropertyChanged(nameof(SelectedProductSubcategoryId));
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
        public decimal ArrivalPrice
        {
            get => _arrivalPrice;
            set
            {
                _arrivalPrice = value;
                OnPropertyChanged(nameof(ArrivalPrice));
            }
        }
        public decimal SalePrice
        {
            get => _salePrice;
            set
            {
                _salePrice = value;
                OnPropertyChanged(nameof(SalePrice));
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

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand { get; }
        public ICommand SaveCommand { get; }

        #endregion

        #region Constructor

        public EditProductWithBarcodeDialogFormModel(IProductCategoryService productCategoryService,
            IProductSubcategoryService productSubcategoryService,
            IDataService<Unit> unitService,
            IProductService productService,
            ISupplierService supplierService,
            IUIManager manager,
            IMessageStore messageStore,
            GlobalMessageViewModel globalMessageViewModel)
        {
            _productCategoryService = productCategoryService;
            _productSubcategoryService = productSubcategoryService;
            _unitService = unitService;
            _productService = productService;
            _supplierService = supplierService;
            _manager = manager;
            _messageStore = messageStore;
            GlobalMessageViewModel = globalMessageViewModel;

            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);
            SaveCommand = new RelayCommand(Save);
        }

        #endregion

        #region Private Voids

        private async void Save()
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

            EditProduct.Name = Name;
            EditProduct.ArrivalPrice = ArrivalPrice;
            EditProduct.SalePrice = SalePrice;
            EditProduct.TNVED = TNVED;
            EditProduct.Barcode = Barcode;
            EditProduct.SupplierId = SelectedSupplierId.Value;
            EditProduct.UnitId = SelectedUnitId.Value;
            EditProduct.ProductSubcategoryId = SelectedProductSubcategoryId.Value;
            EditProduct.WithoutBarcode = string.IsNullOrEmpty(Barcode);

            await _productService.UpdateAsync(EditProduct.Id, EditProduct);
            _manager.Close();
        }

        private async void SelectedProductCategoryChanged()
        {
            if (SelectedProductCategoryId != null)
            {
                ProductSubCategories = new(await _productSubcategoryService.GetAllByProductCategoryIdAsync(SelectedProductCategoryId.Value));
                SelectedProductSubcategoryId = EditProduct.ProductSubcategoryId;
            }
        }

        private async void UserControlLoaded()
        {
            Suppliers = new(await _supplierService.GetAllAsync());
            ProductCategories = new(await _productCategoryService.GetAllAsync());
            Units = new(await _unitService.GetAllAsync());
        }

        #endregion
    }
}
