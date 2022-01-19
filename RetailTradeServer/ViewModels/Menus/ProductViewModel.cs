using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Barcode;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Base;
using SalePageServer.State.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ProductViewModel : BaseViewModel
    {
        #region Private Members

        private readonly ITypeProductService _typeProductService;        
        private readonly IDialogService _dialogService;
        private readonly IProductService _productService;
        private readonly IDataService<Unit> _unitService;
        private readonly ISupplierService _supplierService;
        private readonly IMessageStore _messageStore;
        private readonly IZebraBarcodeScanner _zebraBarcodeScanner;
        private readonly IComBarcodeService _comBarcodeService;
        private object _selectedProductGroup;
        private ObservableCollection<Product> _getProducts;
        private ObservableCollection<TypeProduct> _typeProducts;
        private IEnumerable<Unit> _units;
        private bool _canShowLoadingPanel = true;
        private Product _selectedProduct;

        #endregion

        #region Command

        public ICommand UserControlLoadedCommand { get; }
        public ICommand CreateProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand SelectedItemChangedCommand { get; }
        public ICommand OnShowNodeMenuCommand { get; }
        public ICommand GridControlLoadedCommand { get; }
        public ICommand DeleteMarkingProductCommand { get; }

        #endregion

        #region Public Properties

        public IEnumerable<Unit> Units
        {
            get => _units;
            set
            {
                _units = value;
                OnPropertyChanged(nameof(Units));
            }
        }
        public ObservableCollection<Product> GetProducts
        {
            get => _getProducts ?? new();
            set
            {
                _getProducts = value;
                OnPropertyChanged(nameof(GetProducts));
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

        public ProductViewModel(ITypeProductService typeProductService,
            IProductService productService,
            IDialogService dialogService,
            IDataService<Unit> unitService,
            ISupplierService supplierService,
            IMessageStore messageStore,
            IZebraBarcodeScanner zebraBarcodeScanner,
            IComBarcodeService comBarcodeService)
        {
            _typeProductService = typeProductService;
            _productService = productService;
            _unitService = unitService;
            _supplierService = supplierService;            
            _messageStore = messageStore;
            _dialogService = dialogService;
            _zebraBarcodeScanner = zebraBarcodeScanner;
            _comBarcodeService = comBarcodeService;

            CreateProductCommand = new RelayCommand(CreateProduct);
            EditProductCommand = new RelayCommand(EditProduct);
            GridControlLoadedCommand = new ParameterCommand(sender => GridControlLoaded(sender));
            DeleteMarkingProductCommand = new RelayCommand(DeleteMarkingProduct);
            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);

            _productService.OnProductCreated += ProductService_OnProductCreated;
        }

        #endregion

        #region Private Voids

        private async void UserControlLoaded()
        {
            TypeProducts = new(await _typeProductService.GetAllAsync());
            GetProducts = new(await _productService.GetAllAsync());
            Units = await _unitService.GetAllAsync();
            ShowLoadingPanel = false;
        }

        private async void DeleteMarkingProduct()
        {
            if (SelectedProduct != null)
            {
                if (_dialogService.ShowMessage(SelectedProduct.DeleteMark ? $"Снять пометку \"{SelectedProduct.Name}\"?" : $"Пометить \"{SelectedProduct.Name}\" на удаление?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _productService.MarkingForDeletion(SelectedProduct);
                }
            }
            else
            {
                _ = _dialogService.ShowMessage("Выберите товар", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void CreateProduct()
        {
            //CreateProductDialogFormModel viewModel = new(_productCategoryService,
            //    _productSubcategoryService,
            //    _unitService,
            //    _productService,
            //    _supplierService,
            //    _messageStore,
            //    _zebraBarcodeScanner,
            //    _comBarcodeService)
            //{
            //    Title = "Товаровы (Создать)"
            //};

            //if (SelectedProductGroup is ProductCategory productCategory)
            //{
            //    viewModel.SelectedProductCategoryId = productCategory.Id;
            //}
            //if (SelectedProductGroup is ProductSubcategory productSubcategory)
            //{
            //    viewModel.SelectedProductCategoryId = productSubcategory.ProductCategoryId;
            //    viewModel.SelectedProductSubcategoryId = productSubcategory.Id;
            //}
            //await _dialogService.ShowDialog(viewModel);
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

        private async void EditProduct()
        {
            //if (SelectedProduct != null)
            //{
            //    await _dialogService.ShowDialog(new EditProductWithBarcodeDialogFormModel(_productCategoryService,
            //    _productSubcategoryService,
            //    _unitService,
            //    _productService,
            //    _supplierService,
            //    _dialogService,
            //    _messageStore)
            //    {
            //        Title = "Товаровы (Редактировать)",
            //        EditProduct = SelectedProduct
            //    });
            //}
            //else
            //{
            //    _ = _dialogService.ShowMessage("Выберите товар", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //}
        }

        private void ProductService_OnProductCreated(Product product)
        {
            GetProducts.Add(product);
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
