using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using SalePageServer.State.Dialogs;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ProductCategoryViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IDialogService _dialogService;
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
        private Product _selectedProduct;

        #endregion

        #region Command

        public ICommand CreateProductCategoryCommand { get; }
        public ICommand CreateProductSubcategoryCommand { get; }
        public ICommand CreateProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand GetProductsCommandAsync { get; set; }
        public ICommand SelectedItemChangedCommand { get; }
        public ICommand OnShowNodeMenuCommand { get; }
        public ICommand CreateProductCategoryOrSubCategoryCommand { get; }
        public ICommand EditProductCategoryOrSubCategoryCommand { get; }
        public ICommand DeleteProductCategoryOrSubCategoryCommand { get; }
        public ICommand GridControlLoadedCommand { get; }

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
                _ = _productCategories.Where(p => p.Id != 0).OrderByDescending(p => p.Name);
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
            }
        }
        public object SelectedProductGroup
        {
            get => _selectedProductGroup;
            set
            {
                _selectedProductGroup = value;
                if (_selectedProductGroup is ProductCategory productCategory)
                {
                    ProductGridControl.FilterString = productCategory.Id == 0 ? string.Empty : $"[ProductSubcategory.ProductCategoryId] = {productCategory.Id}";
                }
                if (_selectedProductGroup is ProductSubcategory productSubcategory)
                {
                    ProductGridControl.FilterString = $"[ProductSubcategoryId] = {productSubcategory.Id}";
                }
                OnPropertyChanged(nameof(SelectedProductGroup));
            }
        }
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }
        public GridControl ProductGridControl { get; set; }
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
            IDialogService dialogService,
            IDataService<Unit> unitService,
            ISupplierService supplierService,
            GlobalMessageViewModel globalMessageViewModel,
            IMessageStore messageStore)
        {
            _productSubcategoryService = productSubcategoryService;
            _productCategoryService = productCategoryService;
            _productService = productService;
            _unitService = unitService;
            _supplierService = supplierService;
            GlobalMessageViewModel = globalMessageViewModel;
            _messageStore = messageStore;
            _dialogService = dialogService;

            CreateProductCategoryCommand = new RelayCommand(CreateProductCategory);
            CreateProductSubcategoryCommand = new RelayCommand(CreateProductSubcategory);
            CreateProductCommand = new RelayCommand(CreateProduct);
            EditProductCommand = new RelayCommand(EditProduct);
            GetProductsCommandAsync = new RelayCommand(GetProductsAsync);
            SelectedItemChangedCommand = new RelayCommand(GetProductsAsync);
            OnShowNodeMenuCommand = new ParameterCommand(sender => OnShowNodeMenu(sender));
            CreateProductCategoryOrSubCategoryCommand = new RelayCommand(CreateProductCategoryOrSubCategory);
            EditProductCategoryOrSubCategoryCommand = new RelayCommand(EditProductCategoryOrSubCategory);
            GridControlLoadedCommand = new ParameterCommand(sender => GridControlLoaded(sender));

            GetProductCategories();

            _productCategoryService.OnProductCategoryCreated += ProductCategoryService_OnProductCategoryCreated;
            _productSubcategoryService.OnProductSubcategoryCreated += ProductSubcategoryService_OnProductSubcategoryCreated;
            _productService.OnProductCreated += ProductService_OnProductCreated;
            _productService.OnProductEdited += ProductService_OnProductEdited;
        }

        #endregion

        #region Public Voids

        public void CreateProduct()
        {
            _dialogService.ShowDialog(new CreateProductDialogFormModel(_productCategoryService,
                _productSubcategoryService,
                _unitService,
                _productService,
                _supplierService,
                GlobalMessageViewModel,
                _messageStore)
                {
                    Title = "Товаровы (Создать)",
                    SelectedProductCategoryId = SelectedProductGroup is ProductCategory productCategory ? productCategory.Id : 0,
                    SelectedProductSubcategoryId = SelectedProductGroup is ProductSubcategory productSubcategory ? productSubcategory.Id : 0
                }, new CreateProductDialogForm());
        }

        #endregion

        #region Private Voids

        private void GridControlLoaded(object sender)
        {
            if (sender is RoutedEventArgs e)
            {
                if (e.Source is GridControl gridControl)
                {
                    ProductGridControl = gridControl;
                }
            }
        }

        private void CreateProductCategory()
        {
            //_dialogService.ShowDialog(new CreateProductCategoryDialogFormModel(_productCategoryService, _dialogService)
            //{
            //    Title = "Группа товаров (Создать)"
            //},
            //new CreateProductCategoryDialogForm());
        }

        private void CreateProductSubcategory()
        {
            //_dialogService.ShowDialog(new CreateProductSubcategoryDialogFormModel(_productSubcategoryService,
            //    _productCategoryService,
            //    _dialogService)
            //{
            //    Title = "Категории товаров (Создать)",
            //    SelectedProductCategoryId = SelectedProductGroup is ProductCategory productCategory ? productCategory.Id : 0
            //},
            //new CreateProductSubcategoryDialogForm());
        }

        private void EditProduct()
        {
            //if (SelectedProduct != null)
            //{
            //    _dialogService.ShowDialog(new EditProductWithBarcodeDialogFormModel(_productCategoryService,
            //    _productSubcategoryService,
            //    _unitService,
            //    _productService,
            //    _supplierService,
            //    _dialogService,
            //    _messageStore,
            //    GlobalMessageViewModel)
            //    {
            //        Title = "Товаровы (Редактировать)",
            //        EditProduct = SelectedProduct
            //    },
            //    new EditProductWithBarcodeDialogForm());
            //}
            //else
            //{
            //    _ = _dialogService.ShowMessage("Выберите товар", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //}
        }

        private async void GetProductsAsync()
        {
            GetProducts = new(await _productService.GetAllAsync());
            GetProducts.CollectionChanged += GetProducts_CollectionChanged;
        }

        private void GetProducts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    if (item != null)
                    {
                        item.PropertyChanged -= Item_PropertyChanged;
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    if (item != null)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(GetProducts));
        }

        private async void GetProductCategories()
        {
            ProductCategories = await _productCategoryService.GetAllListAsync();
        }

        private void ProductCategoryService_OnProductCategoryCreated(ProductCategory productCategory)
        {
            productCategory.ProductSubcategories = new();
            ProductCategories.Add(productCategory);
        }

        private void ProductSubcategoryService_OnProductSubcategoryCreated(ProductSubcategory productSubcategory)
        {
            if (productSubcategory != null)
            {
                ProductCategory updateProductCategory = ProductCategories.FirstOrDefault(p => p.Id == productSubcategory.ProductCategoryId);
                updateProductCategory.ProductSubcategories.Add(productSubcategory);
            }
        }

        private void ProductService_OnProductCreated(Product product)
        {
            GetProducts.Add(product);
        }

        private void ProductService_OnProductEdited(Product product)
        {
            Product editProduct = GetProducts.FirstOrDefault(p => p.Id == product.Id);
            editProduct.Name = product.Name;
            editProduct.Barcode = product.Barcode;
            editProduct.ArrivalPrice = product.ArrivalPrice;
            editProduct.SalePrice = product.SalePrice;
            editProduct.WithoutBarcode = product.WithoutBarcode;
            editProduct.ProductSubcategoryId = product.ProductSubcategoryId;
            editProduct.ProductSubcategory = product.ProductSubcategory;
            editProduct.UnitId = product.UnitId;
            editProduct.SupplierId = product.SupplierId;
        }

        private void OnShowNodeMenu(object sender)
        {
            if (sender is TreeViewNodeMenuEventArgs e)
            {
                if (e.Node.Level == 1)
                {
                    e.Customizations.Add(new RemoveAction { ElementName = "CreateSubCategory" });
                }
                if (SelectedProductGroup is ProductCategory productCategory && productCategory.Id == 0)
                {
                    e.Customizations.Add(new RemoveAction { ElementName = "EditCategory" });
                    e.Customizations.Add(new RemoveAction { ElementName = "DeleteCategory" });
                    e.Customizations.Add(new RemoveAction { ElementName = "CreateSubCategory" });
                }
            }
        }

        private void CreateProductCategoryOrSubCategory()
        {
            //_ = _dialogService.ShowDialog(new CreateProductCategoryOrSubCategoryDialogFormModel(_productCategoryService, _productSubcategoryService, _messageStore, GlobalMessageViewModel) { Title = "Создать категорию или группу товаров" },
            //    new CreateProductCategoryOrSubCategoryDialogForm());
        }

        private void EditProductCategoryOrSubCategory()
        {
            //if (SelectedProductGroup is ProductCategory productCategory)
            //{
            //    var viewModel = new CreateProductCategoryDialogFormModel(_productCategoryService, _dialogService) 
            //    { 
            //        Title = $"Категория товаров ({productCategory.Name})",
            //        EditProductCategory = productCategory,
            //        Name = productCategory.Name,
            //        IsEditMode = true
            //    };
            //    _ = _dialogService.ShowDialog(viewModel, new CreateProductCategoryDialogForm());
            //}

            //if (SelectedProductGroup is ProductSubcategory productSubcategory)
            //{

            //}
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            GlobalMessageViewModel.Dispose();
            GetProducts = null;
            SelectedProductGroup = null;

            base.Dispose();
        }

        #endregion        
    }
}
