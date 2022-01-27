using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Barcode;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
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
        private TypeProduct _selectedTypeProduct;
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
        public ICommand EditTypeProductCommand { get; }
        public ICommand SelectedItemChangedCommand { get; }
        public ICommand OnShowNodeMenuCommand { get; }
        public ICommand GridControlLoadedCommand { get; }
        public ICommand DeleteMarkingProductCommand { get; }
        public ICommand CreateGroupTypeProductCommand { get; }
        public ICommand CreateTypeProductCommand { get; }

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
        public TypeProduct SelectedTypeProduct
        {
            get => _selectedTypeProduct;
            set
            {
                _selectedTypeProduct = value;
                ProductGridControl.FilterString = _selectedTypeProduct.Id == 1 ? string.Empty : $"[TypeProductId] = {_selectedTypeProduct.Id}";
                OnPropertyChanged(nameof(SelectedTypeProduct));
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
            _typeProductService = typeProductService;

            CreateProductCommand = new RelayCommand(() => _dialogService.ShowDialog(new CreateProductDialogFormModel(_typeProductService, _unitService, _productService, _supplierService, _messageStore, _zebraBarcodeScanner, _comBarcodeService) { Title = "Товаровы (Создать)", SelectedTypeProductId = SelectedTypeProduct?.Id }));
            EditProductCommand = new RelayCommand(EditProduct);
            GridControlLoadedCommand = new ParameterCommand(sender => GridControlLoaded(sender));
            DeleteMarkingProductCommand = new RelayCommand(DeleteMarkingProduct);
            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);
            CreateGroupTypeProductCommand = new RelayCommand(() => _dialogService.ShowDialog(new TypeProductDialogFormModel(_typeProductService, _dialogService) { Title = "Виды товаров (создание группы)", IsGroup = true }));
            CreateTypeProductCommand = new RelayCommand(() => _dialogService.ShowDialog(new TypeProductDialogFormModel(_typeProductService, _dialogService) { Title = "Виды товаров (создание)" }));
            EditTypeProductCommand = new RelayCommand(EditTypeProduct);

            _productService.OnProductCreated += ProductService_OnProductCreated;
            _typeProductService.OnTypeProductCreated += TypeProductService_OnTypeProductCreated;
        }

        #endregion

        #region Private Voids

        private void EditTypeProduct()
        {
            if (SelectedTypeProduct == null)
            {
                _dialogService.ShowMessage("Выберите группу видов или вид товара!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (SelectedTypeProduct.Id != 1)
            {
                _ = _dialogService.ShowDialog(new TypeProductDialogFormModel(_typeProductService, _dialogService) { Title = $"{SelectedTypeProduct.Name} (Виды товаров)", IsEditMode = true, TypeProduct = SelectedTypeProduct });
            }            
        }

        private void TypeProductService_OnTypeProductCreated(TypeProduct obj)
        {
            TypeProducts.Add(obj);
        }

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
            if (SelectedProduct != null)
            {
                await _dialogService.ShowDialog(new EditProductWithBarcodeDialogFormModel(_unitService,
                _productService,
                _supplierService,
                _dialogService,
                _messageStore)
                {
                    Title = $"{SelectedProduct.Name} (Товары)",
                    EditProduct = SelectedProduct
                });
            }
            else
            {
                _ = _dialogService.ShowMessage("Выберите товар", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
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
