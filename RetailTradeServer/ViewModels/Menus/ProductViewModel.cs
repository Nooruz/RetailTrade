using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Barcode.Services;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Views;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.State.Reports;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.ViewModels.Factories;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ProductViewModel : BaseViewModel
    {
        #region Private Members

        private readonly ITypeProductService _typeProductService;        
        private readonly IProductService _productService;
        private readonly IReportService _reportService;
        private readonly IMenuNavigator _menuNavigator;
        private readonly IMenuViewModelFactory _menuViewModelFactory;
        private readonly IUnitService _unitService;
        private readonly ISupplierService _supplierService;
        private readonly IMessageStore _messageStore;
        private readonly IBarcodeService _barcodeService;
        private readonly IProductBarcodeService _productBarcodeService;
        private TypeProduct _selectedTypeProduct;
        private ObservableCollection<TypeProduct> _typeProducts = new();
        private ObservableCollection<ProductView> _productViews = new();
        private bool _canShowLoadingPanel = true;
        private ProductView _selectedProductView;
        private bool _isUseFilter = true;

        #endregion

        #region Public Properties

        public ObservableCollection<ProductView> ProductViews
        {
            get => _productViews;
            set
            {
                _productViews = value;
                OnPropertyChanged(nameof(ProductViews));
            }
        }
        public ObservableCollection<TypeProduct> TypeProducts
        {
            get => _typeProducts;
            set
            {
                _typeProducts = value;
                OnPropertyChanged(nameof(TypeProducts));
            }
        }
        public ProductView SelectedProductView
        {
            get => _selectedProductView;
            set
            {
                _selectedProductView = value;
                OnPropertyChanged(nameof(SelectedProductView));
            }
        }
        public GridControl ProductGridControl { get; set; }
        public TableView ProductTableView { get; set; }
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
                OnPropertyChanged(nameof(SelectedTypeProduct));
                FilterProductGridControl();
            }
        }
        public CriteriaOperator FilterCriteria
        {
            get => ProductGridControl.FilterCriteria;
            set => ProductGridControl.FilterCriteria = value;
        }
        public bool IsUseFilter
        {
            get => _isUseFilter;
            set
            {
                _isUseFilter = value;
                OnPropertyChanged(nameof(IsUseFilter));
                FilterProductGridControl();
            }
        }

        #endregion

        #region Commands

        public ICommand UpdateCurrentMenuViewModelCommand => new UpdateCurrentMenuViewModelCommand(_menuNavigator, _menuViewModelFactory);

        #endregion

        #region Constructor

        public ProductViewModel(ITypeProductService typeProductService,
            IProductService productService,
            IReportService reportService,
            IMenuNavigator menuNavigator,
            IMenuViewModelFactory menuViewModelFactory,
            IUnitService unitService,
            ISupplierService supplierService,
            IMessageStore messageStore,
            IBarcodeService barcodeService,
            IProductBarcodeService productBarcodeService)
        {
            _typeProductService = typeProductService;
            _productService = productService;
            _typeProductService = typeProductService;
            _reportService = reportService;
            _menuNavigator = menuNavigator;
            _menuViewModelFactory = menuViewModelFactory;
            _unitService = unitService;
            _supplierService = supplierService;
            _messageStore = messageStore;
            _barcodeService = barcodeService;
            _productBarcodeService = productBarcodeService;

            Header = "Товары";

            _productService.OnProductCreated += ProductService_OnProductCreated;
            _productService.OnProductUpdated += ProductService_OnProductUpdated;
            _typeProductService.OnTypeProductCreated += TypeProductService_OnTypeProductCreated;
            _typeProductService.OnTypeProductEdited += TypeProductService_OnTypeProductEdited;

            ViewmodelLoaded();
        }

        #endregion

        #region Private Voids

        private void ProductService_OnProductUpdated(ProductView productView)
        {
            try
            {
                SelectedProductView.Name = productView.Name;
                SelectedProductView.TypeProduct = productView.TypeProduct;
                SelectedProductView.Unit = productView.Unit;
                SelectedProductView.TNVED = productView.TNVED;
                SelectedProductView.Barcode = productView.Barcode;
                SelectedProductView.DeleteMark = productView.DeleteMark;
                FilterProductGridControl();
            }
            catch (Exception)
            {
                //ignore
            }
        }
        private int? GetGroupTypeProductId()
        {
            if (SelectedTypeProduct != null)
            {
                return SelectedTypeProduct.IsGroup ? SelectedTypeProduct.Id : SelectedTypeProduct.SubGroupId;
            }
            else
            {
                return null;
            }
        }
        private int? GetTypeProductId()
        {
            if (SelectedTypeProduct != null)
            {
                if (!SelectedTypeProduct.IsGroup)
                {
                    return SelectedTypeProduct.Id;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        private void TypeProductService_OnTypeProductEdited(TypeProduct obj)
        {
            if (obj != null)
            {
                SelectedTypeProduct.Name = obj.Name;
                SelectedTypeProduct.SubGroupId = obj.SubGroupId;
            }
        }
        private void TypeProductService_OnTypeProductCreated(TypeProduct obj)
        {
            TypeProducts.Add(obj);
        }
        private void ProductService_OnProductCreated(ProductView productView)
        {
            try
            {
                ProductViews.Add(productView);
                SelectedProductView = productView;
            }
            catch (Exception)
            {
                //ignore
            }
        }
        private async void ViewmodelLoaded()
        {
            TypeProducts = new(await _typeProductService.GetAllAsync());
            ProductViews = new(await _productService.GetProductViewsAsync());
            ShowLoadingPanel = false;
            SelectedProductView = ProductViews.LastOrDefault();
        }

        #endregion

        #region Public Voids

        [Command]
        public void FilterProductGridControl()
        {
            try
            {
                if (ProductGridControl != null)
                {
                    if (IsUseFilter)
                    {
                        if (SelectedTypeProduct != null && !SelectedTypeProduct.IsGroup)
                        {
                            FilterCriteria = new BinaryOperator("TypeProduct", SelectedTypeProduct.Name, BinaryOperatorType.Equal);
                            return;
                        }
                    }
                    ProductGridControl.FilterString = string.Empty;
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void CreateGroupTypeProduct()
        {
            WindowService.Show(nameof(TypeProductDialogForm), new TypeProductDialogFormModel(_typeProductService) { Title = "Виды товаров (создание группы)", IsGroup = true, SelectedGroupTypeProductId = GetGroupTypeProductId() });
        }

        [Command]
        public void CreateProduct()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.CreateProductView);
            //WindowService.Show(nameof(CreateProductDialogForm), new CreateProductDialogFormModel(_typeProductService, _unitService, _productService, _supplierService, _messageStore, _barcodeService, _productBarcodeService, _productPriceService) { Title = "Товары и услуги (создание)" });
        }

        [Command]
        public void EditTypeProduct()
        {
            if (SelectedTypeProduct == null)
            {
                _ = MessageBoxService.Show("Выберите группу видов или вид товара!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (SelectedTypeProduct.Id != 1)
            {
                WindowService.Show(nameof(TypeProductDialogForm), new TypeProductDialogFormModel(_typeProductService) { Title = $"{SelectedTypeProduct.Name} (Виды товаров)", TypeProduct = SelectedTypeProduct, IsEditMode = true });
            }
        }

        [Command]
        public async void DeleteMarkingProduct()
        {
            if (SelectedProductView != null)
            {
                if (MessageBoxService.Show(SelectedProductView.DeleteMark ? $"Снять пометку \"{SelectedProductView.Name}\"?" : $"Пометить \"{SelectedProductView.Name}\" на удаление?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _productService.MarkingForDeletion(SelectedProductView.Id);
                }
            }
            else
            {
                _ = MessageBoxService.Show("Выберите товар", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        [Command]
        public void GridControlLoaded(object sender)
        {
            if (sender is RoutedEventArgs e)
            {
                if (e.Source is GridControl gridControl)
                {
                    ProductGridControl = gridControl;
                    ProductTableView = gridControl.View as TableView;
                }
            }
        }

        [Command]
        public async void EditProduct()
        {
            if (SelectedProductView != null)
            {
                WindowService.Show(nameof(CreateProductDialogForm), new CreateProductDialogFormModel(_typeProductService, _unitService, _productService, _supplierService, _messageStore, _barcodeService, _productBarcodeService)
                {
                    Title = $"{SelectedProductView.Name} (Товары)",
                    EditProduct = await _productService.GetAsync(SelectedProductView.Id)
                });
            }
            else
            {
                _ = MessageBoxService.Show("Выберите товар", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        [Command]
        public void CreateTypeProduct()
        {
            WindowService.Show(nameof(TypeProductDialogForm), new TypeProductDialogFormModel(_typeProductService) { Title = "Виды товаров (создание)", SelectedGroupTypeProductId = GetGroupTypeProductId() });
        }

        [Command]
        public async void ReportProduct()
        {
            try
            {
                DocumentViewerService.Show(nameof(DocumentViewerView), new DocumentViewerViewModel() { PrintingDocument = await _reportService.CreateBalancesAndAvailabilityProducts() });
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public async void DeleteTypeProduct()
        {
            try
            {
                if (SelectedTypeProduct != null)
                {
                    if (await _typeProductService.CanDelete(SelectedTypeProduct))
                    {
                        if (MessageBoxService.ShowMessage($"Удалить \"{SelectedTypeProduct.Name}\"?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                        {
                            if (await _typeProductService.DeleteAsync(SelectedTypeProduct.Id))
                            {
                                _ = TypeProducts.Remove(SelectedTypeProduct);
                            }
                        }
                    }
                    else
                    {
                        MessageBoxService.ShowMessage("Не удается удалить связанный Вид товара или Группу видова товара. Удалите связанные элементы!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBoxService.ShowMessage("Выберите Вид товара или Группу видов товара!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion      
    }
}
