using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Barcode.Services;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Reports;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ProductViewModel : BaseViewModel
    {
        #region Private Members

        private readonly ITypeProductService _typeProductService;        
        private readonly IProductService _productService;
        private readonly IDataService<Unit> _unitService;
        private readonly ISupplierService _supplierService;
        private readonly IMessageStore _messageStore;
        private readonly IBarcodeService _barcodeService;
        private readonly IReportService _reportService;
        private TypeProduct _selectedTypeProduct;
        private ObservableCollection<Product> _getProducts;
        private ObservableCollection<TypeProduct> _typeProducts;
        private IEnumerable<Unit> _units;
        private bool _canShowLoadingPanel = true;
        private Product _selectedProduct;
        private bool _isUseFilter = true;

        #endregion

        #region Public Properties

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

        #endregion

        #region Constructor

        public ProductViewModel(ITypeProductService typeProductService,
            IProductService productService,
            IDataService<Unit> unitService,
            ISupplierService supplierService,
            IMessageStore messageStore,
            IBarcodeService barcodeService,
            IReportService reportService)
        {
            _typeProductService = typeProductService;
            _productService = productService;
            _unitService = unitService;
            _supplierService = supplierService;            
            _messageStore = messageStore;
            _barcodeService = barcodeService;
            _typeProductService = typeProductService;
            _reportService = reportService;

            Header = "Товары";

            _productService.OnProductCreated += ProductService_OnProductCreated;
            _productService.OnProductEdited += ProductService_OnProductEdited;
            _typeProductService.OnTypeProductCreated += TypeProductService_OnTypeProductCreated;
            _typeProductService.OnTypeProductEdited += TypeProductService_OnTypeProductEdited;
        }

        #endregion

        #region Private Voids

        private void FilterProductGridControl()
        {
            try
            {
                if (IsUseFilter)
                {
                    if (SelectedTypeProduct != null && SelectedTypeProduct.Id != 1)
                    {
                        FilterCriteria = new BinaryOperator("TypeProductId", SelectedTypeProduct.Id, BinaryOperatorType.Equal);
                    }
                    else
                    {
                        ProductGridControl.FilterString = string.Empty;
                    }
                }
                else
                {
                    ProductGridControl.FilterString = string.Empty;
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }
        private void ProductService_OnProductEdited(Product obj)
        {
            Product product = GetProducts.FirstOrDefault(p => p.Id == obj.Id);
            product.Barcode = obj.Barcode;
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
        private void ProductService_OnProductCreated(Product product)
        {
            GetProducts.Add(product);
        }

        #endregion

        #region Public Voids

        [Command]
        public void CreateGroupTypeProduct()
        {
            WindowService.Show(nameof(TypeProductDialogForm), new TypeProductDialogFormModel(_typeProductService) { Title = "Виды товаров (создание группы)", IsGroup = true, SelectedGroupTypeProductId = GetGroupTypeProductId() });
        }

        [Command]
        public void CreateProduct()
        {
            WindowService.Show(nameof(CreateProductDialogForm), new CreateProductDialogFormModel(_typeProductService, _unitService, _productService, _supplierService, _messageStore, _barcodeService) { Title = "Товаровы (Создать)", SelectedTypeProductId = GetTypeProductId() });
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
        public async void UserControlLoaded()
        {
            TypeProducts = new(await _typeProductService.GetAllAsync());
            GetProducts = new(await _productService.GetAllAsync());
            Units = await _unitService.GetAllAsync();
            ShowLoadingPanel = false;
        }

        [Command]
        public async void DeleteMarkingProduct()
        {
            if (SelectedProduct != null)
            {
                if (MessageBoxService.Show(SelectedProduct.DeleteMark ? $"Снять пометку \"{SelectedProduct.Name}\"?" : $"Пометить \"{SelectedProduct.Name}\" на удаление?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _productService.MarkingForDeletion(SelectedProduct);
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
        public void EditProduct()
        {
            if (SelectedProduct != null)
            {
                WindowService.Show(nameof(EditProductWithBarcodeDialogForm), new EditProductWithBarcodeDialogFormModel(_typeProductService,
                _unitService,
                _productService,
                _supplierService,
                _messageStore,
                _barcodeService)
                {
                    Title = $"{SelectedProduct.Name} (Товары)",
                    EditProduct = SelectedProduct
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

        #region Dispose

        public override void Dispose()
        {
            _productService.OnProductCreated -= ProductService_OnProductCreated;
            _typeProductService.OnTypeProductCreated -= TypeProductService_OnTypeProductCreated;
            _typeProductService.OnTypeProductEdited -= TypeProductService_OnTypeProductEdited;
            base.Dispose();
        }

        #endregion        
    }
}
