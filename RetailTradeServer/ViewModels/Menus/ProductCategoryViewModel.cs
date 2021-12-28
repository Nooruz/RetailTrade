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
using SalePageServer.Utilities;
using System.Collections.Generic;
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
        private readonly ObservableQueue<Product> _getProducts;
        private readonly ObservableQueue<ProductCategory> _productCategories;
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
        public IEnumerable<ProductCategory> ProductCategories => _productCategories;
        public IEnumerable<Product> GetProducts => _getProducts;
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
            //SelectedItemChangedCommand = new RelayCommand(GetProductsAsync);
            OnShowNodeMenuCommand = new ParameterCommand(sender => OnShowNodeMenu(sender));
            CreateProductCategoryOrSubCategoryCommand = new RelayCommand(CreateProductCategoryOrSubCategory);
            EditProductCategoryOrSubCategoryCommand = new RelayCommand(EditProductCategoryOrSubCategory);
            GridControlLoadedCommand = new ParameterCommand(sender => GridControlLoaded(sender));

            _getProducts = new();
            _productCategories = new();

            GetProductCategories();

            _productCategoryService.OnProductCategoryCreated += ProductCategoryService_OnProductCategoryCreated;
            _productSubcategoryService.OnProductSubcategoryCreated += ProductSubcategoryService_OnProductSubcategoryCreated;
            _productService.OnProductCreated += ProductService_OnProductCreated;
        }

        #endregion

        #region Private Voids

        private void CreateProduct()
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
            if (SelectedProduct != null)
            {
                _dialogService.ShowDialog(new EditProductWithBarcodeDialogFormModel(_productCategoryService,
                _productSubcategoryService,
                _unitService,
                _productService,
                _supplierService,
                _dialogService,
                _messageStore,
                GlobalMessageViewModel)
                {
                    Title = "Товаровы (Редактировать)",
                    EditProduct = SelectedProduct
                },
                new EditProductWithBarcodeDialogForm());
            }
            else
            {
                _ = _dialogService.ShowMessage("Выберите товар", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void GetProductsAsync()
        {
            _getProducts.AddRange(await _productService.GetAllAsync());
        }

        private async void GetProductCategories()
        {
            _productCategories.AddRange(await _productCategoryService.GetAllListAsync());
        }

        private void ProductCategoryService_OnProductCategoryCreated(ProductCategory productCategory)
        {
            _productCategories.Enqueue(productCategory);
        }

        private void ProductSubcategoryService_OnProductSubcategoryCreated(ProductSubcategory productSubcategory)
        {
            ProductCategory editProductCategory = ProductCategories.FirstOrDefault(pc => pc.Id == productSubcategory.ProductCategoryId);
            if (editProductCategory.ProductSubcategories == null)
            {
                editProductCategory.ProductSubcategories = new();
            }
            editProductCategory.ProductSubcategories.Add(productSubcategory);
        }

        private void ProductService_OnProductCreated(Product product)
        {
            _getProducts.Enqueue(product);
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
            _ = _dialogService.ShowDialog(new CreateProductCategoryOrSubCategoryDialogFormModel(_productCategoryService, _productSubcategoryService, _messageStore, GlobalMessageViewModel) { Title = "Создать категорию или группу товаров" },
                new CreateProductCategoryOrSubCategoryDialogForm());
        }

        private void EditProductCategoryOrSubCategory()
        {
            if (SelectedProductGroup is ProductCategory productCategory)
            {
                var viewModel = new CreateProductCategoryDialogFormModel(_productCategoryService, _dialogService)
                {
                    Title = $"Категория товаров ({productCategory.Name})",
                    EditProductCategory = productCategory,
                    Name = productCategory.Name,
                    IsEditMode = true
                };
                _ = _dialogService.ShowDialog(viewModel, new CreateProductCategoryDialogForm());
            }

            if (SelectedProductGroup is ProductSubcategory productSubcategory)
            {

            }
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            GlobalMessageViewModel.Dispose();
            //GetProducts = null;
            SelectedProductGroup = null;

            base.Dispose();
        }

        #endregion        
    }
}
