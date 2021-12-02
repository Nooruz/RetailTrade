using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
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
        private ObservableCollection<Product> _getProducts;
        private ObservableCollection<ProductCategory> _productCategories;
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
        public ObservableCollection<ProductCategory> ProductCategories
        {
            get
            {
                if (_productCategories != null)
                {
                    return _productCategories;
                }
                _productCategories = new();
                return _productCategories;
            }
            set
            {
                _productCategories = value;
                OnPropertyChanged(nameof(ProductCategories));
            }
        }
        public ObservableCollection<Product> GetProducts
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

            GetProductCategories();

            _productCategoryService.OnProductCategoryCreated += ProductCategoryService_OnProductCategoryCreated;
            _productSubcategoryService.OnProductSubcategoryCreated += ProductSubcategoryService_OnProductSubcategoryCreated;
            _productService.OnProductCreated += ProductService_OnProductCreated;
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

        private async void GetProductsAsync()
        {
            if (SelectedProductGroup is ProductCategory productCategory)
            {
                if (productCategory.Id != 0)
                {
                    GetProducts = new(await _productService.GetByProductCategoryIdAsync(productCategory.Id));
                    return;
                }
            }
            if (SelectedProductGroup is ProductSubcategory productSubcategory)
            {
                GetProducts = new(await _productService.GetByProductSubcategoryIdAsync(productSubcategory.Id));
                return;
            }
            GetProducts = new(await _productService.GetAllAsync());
        }

        private async void GetProductCategories()
        {
            ProductCategories = await _productCategoryService.GetAllListAsync();
        }

        private void ProductCategoryService_OnProductCategoryCreated(ProductCategory productCategory)
        {
            ProductCategories.Add(productCategory);
        }

        private void ProductSubcategoryService_OnProductSubcategoryCreated(ProductSubcategory productSubcategory)
        {
            if (SelectedProductGroup is ProductCategory productCategory)
            {
                ProductCategory updateProductCategory = ProductCategories.FirstOrDefault(p => p.Id == productCategory.Id);
                updateProductCategory.ProductSubcategories.Add(productSubcategory);
            }
        }

        private void ProductService_OnProductCreated(Product product)
        {
            GetProducts.Add(product);
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
