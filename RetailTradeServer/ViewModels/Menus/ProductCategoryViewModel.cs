using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ProductCategoryViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IUIManager _manager;
        private readonly IProductSubcategoryService _productSubcategoryService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductService _productService;
        private readonly IDataService<Unit> _unitService;
        private readonly ISupplierService _supplierService;
        private readonly IMessageStore _messageStore;
        private object _selectedProductGroup;
        private IEnumerable<Product> _getProducts;
        private bool _canShowLoadingPanel = true;

        #endregion

        #region Command

        public ICommand CreateProductCategoryCommand { get; }
        public ICommand CreateProductSubcategoryCommand { get; }
        public ICommand CreateProductCommand { get; }
        public ICommand GetProductsCommandAsync { get; set; }
        public ICommand SelectedItemChangedCommand { get; }

        #endregion

        #region Public Properties

        public GlobalMessageViewModel GlobalMessageViewModel { get; }
        public List<ProductCategory> ProductCategories => _productCategoryService.GetAllList();
        public IEnumerable<Product> GetProducts
        {
            get => _getProducts;
            set
            {
                _getProducts = value;
                OnPropertyChanged(nameof(GetProducts));
                //CanShowLoadingPanel = !GetProducts.Any();
            }
        }
        public object SelectedProductGroup
        {
            get => _selectedProductGroup;
            set
            {
                _selectedProductGroup = value;
                OnPropertyChanged(nameof(SelectedProductGroup));
            }
        }
        public bool CanShowLoadingPanel
        {
            get => _canShowLoadingPanel;
            set
            {
                _canShowLoadingPanel = value;
                OnPropertyChanged(nameof(CanShowLoadingPanel));
            }
        }

        #endregion

        #region Constructor

        public ProductCategoryViewModel(IProductSubcategoryService productSubcategoryService,
            IProductCategoryService productCategoryService,
            IProductService productService,
            IUIManager manager,
            IDataService<Unit> unitService,
            ISupplierService supplierService,
            GlobalMessageViewModel globalMessageViewModel,
            IMessageStore messageStore)
        {
            _productSubcategoryService = productSubcategoryService;
            _productCategoryService = productCategoryService;
            _productService = productService;
            _manager = manager;
            _unitService = unitService;
            _supplierService = supplierService;
            GlobalMessageViewModel = globalMessageViewModel;
            _messageStore = messageStore;

            CreateProductCategoryCommand = new RelayCommand(CreateProductCategory);
            CreateProductSubcategoryCommand = new RelayCommand(CreateProductSubcategory);
            CreateProductCommand = new RelayCommand(CreateProduct);
            GetProductsCommandAsync = new RelayCommand(GetProductsAsync);
            SelectedItemChangedCommand = new RelayCommand(GetProductsAsync);

            _productCategoryService.PropertiesChanged += ProductCategoryService_PropertiesChanged;
            _productSubcategoryService.PropertiesChanged += ProductSubcategoryService_PropertiesChanged;
            _productService.PropertiesChanged += GetProductsAsync;
        }

        #endregion

        #region Private Voids

        private void CreateProductCategory()
        {
            _manager.ShowDialog(new CreateProductCategoryDialogFormModel(_productCategoryService, _manager)
            {
                Title = "Группа товаров (Создать)"
            },
            new CreateProductCategoryDialogForm());
        }

        private void CreateProductSubcategory()
        {
            _manager.ShowDialog(new CreateProductSubcategoryDialogFormModel(_productSubcategoryService,
                _productCategoryService,
                _manager)
            {
                Title = "Категории товаров (Создать)",
                SelectedProductCategoryId = SelectedProductGroup is ProductCategory productCategory ? productCategory.Id : 0
            },
            new CreateProductSubcategoryDialogForm());
        }

        private void CreateProduct()
        {
            _manager.ShowDialog(new CreateProductDialogFormModel(_productCategoryService,
                _productSubcategoryService,
                _unitService,
                _productService,
                _supplierService,
                _manager,
                GlobalMessageViewModel,
                _messageStore)
            {
                Title = "Товаровы (Создать)",
                SelectedProductCategoryId = SelectedProductGroup is ProductCategory productCategory ? productCategory.Id : 0,
                SelectedProductSubcategoryId = SelectedProductGroup is ProductSubcategory productSubcategory ? productSubcategory.Id : 0
            },
            new CreateProductDialogForm());
        }

        private void ProductCategoryService_PropertiesChanged()
        {
            OnPropertyChanged(nameof(ProductCategories));
        }

        private void ProductSubcategoryService_PropertiesChanged()
        {
            OnPropertyChanged(nameof(ProductCategories));
        }

        private async void GetProductsAsync()
        {
            if (SelectedProductGroup is ProductCategory productCategory)
            {
                if (productCategory.Id != 0)
                {
                    GetProducts = await _productService.GetByProductCategoryIdAsync(productCategory.Id);
                    return;
                }
            }
            if (SelectedProductGroup is ProductSubcategory productSubcategory)
            {
                GetProducts = await _productService.GetByProductSubcategoryIdAsync(productSubcategory.Id);
                return;
            }
            GetProducts = await _productService.GetAllAsync();
        }

        #endregion

        public override void Dispose()
        {
            GlobalMessageViewModel.Dispose();
            GetProducts = null;
            SelectedProductGroup = null;

            base.Dispose();
        }
    }
}
