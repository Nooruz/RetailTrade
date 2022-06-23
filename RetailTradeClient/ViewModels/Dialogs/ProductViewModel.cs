using DevExpress.Data.Filtering;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Views;
using RetailTradeClient.Components;
using RetailTradeClient.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class ProductViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
        private ObservableCollection<Nomenclature> _nomenclatures = new();
        private IEnumerable<TypeProduct> _typeProducts;
        private TypeProduct _selectedTypeProduct;
        private Nomenclature _selectedNomenclature;
        private bool _isTypesAndProperties;
        private bool _isByExactMatch;
        private string _searchText;

        #endregion

        #region Public Properties

        public bool IsByExactMatch
        {
            get => _isByExactMatch;
            set
            {
                _isByExactMatch = value;
                EditValueChanged();
                OnPropertyChanged(nameof(IsByExactMatch));
            }
        }
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        public bool IsTypesAndProperties
        {
            get => _isTypesAndProperties;
            set
            {
                _isTypesAndProperties = value;
                if (_isTypesAndProperties)
                {
                    ClearColumnFilter("TypeProductId");
                }
                else
                {
                    EditValueChanged();
                }
                OnPropertyChanged(nameof(IsTypesAndProperties));
            }
        }
        public ObservableCollection<Nomenclature> Nomenclatures
        {
            get => _nomenclatures;
            set
            {
                _nomenclatures = value;
                OnPropertyChanged(nameof(Nomenclatures));
            }
        }
        public Nomenclature SelectedNomenclature
        {
            get => _selectedNomenclature;
            set
            {
                _selectedNomenclature = value;
                OnPropertyChanged(nameof(SelectedNomenclature));
            }
        }
        public IEnumerable<TypeProduct> TypeProducts
        {
            get => _typeProducts;
            set
            {
                _typeProducts = value;
                OnPropertyChanged(nameof(TypeProducts));
            }
        }
        public CustomGridControl ProductGridControl { get; set; }
        public TableView ProductTableView { get; set; }
        public TypeProduct SelectedTypeProduct
        {
            get => _selectedTypeProduct;
            set
            {
                _selectedTypeProduct = value;
                EditValueChanged();
                OnPropertyChanged(nameof(SelectedTypeProduct));
            }
        }
        public CriteriaOperator FilterCriteria
        {
            get => ProductGridControl.FilterCriteria;
            set => ProductGridControl.FilterCriteria = value;
        }
        public CriteriaOperator NameFilter => ProductGridControl.GetColumnFilterCriteria("Name");
        public CriteriaOperator TypeProductIdFilter => ProductGridControl.GetColumnFilterCriteria("TypeProductId");
        public bool IsKeepRecords => Settings.Default.IsKeepRecords;

        #endregion

        #region Event Action

        public event Action<IEnumerable<Sale>> OnProductsSelected;

        #endregion

        #region Constructor

        public ProductViewModel(ITypeProductService typeProductService,
            IProductService productService)
        {
            _typeProductService = typeProductService;
            _productService = productService;
            Title = "Товары";
        }

        #endregion

        #region Private Voids

        private void ClearColumnFilter(string columName)
        {
            ProductGridControl.ClearColumnFilter(columName);
        }

        #endregion

        #region Public Voids

        [Command]
        public async void UserControlLoaded()
        {
            IEnumerable<ProductWareHouseView> productWareHouseViews = await _productService.GetProducts();
            //IEnumerable<Product> products = Settings.Default.IsKeepRecords ? await _productWareHouseService.GetProducts() :
            //    await _productService.PredicateSelect(p => p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name, Barcode = p.Barcode, TypeProductId = p.TypeProductId, SalePrice = p.SalePrice, ArrivalPrice = p.ArrivalPrice });
            Nomenclatures = new(productWareHouseViews.Select(p => new Nomenclature { Id = p.Id, Name = p.Name, Barcode = p.Barcode, SalePrice = p.SalePrice, QuantityInStock = p.Quantity, ArrivalPrice = p.ArrivalPrice }));
            TypeProducts = await _typeProductService.GetAllAsync();
        }

        [Command]
        public void GridControlLoaded(object parameter)
        {
            try
            {
                if (parameter is RoutedEventArgs e)
                {
                    if (e.Source is CustomGridControl gridControl)
                    {
                        ProductGridControl = gridControl;
                        ProductTableView = ProductGridControl.View as TableView;
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void Plus()
        {
            if (SelectedNomenclature != null)
            {
                SelectedNomenclature.Quantity++;
            }
        }

        [Command]
        public void Minus()
        {
            if (SelectedNomenclature != null && SelectedNomenclature.Quantity > 0)
            {
                SelectedNomenclature.Quantity--;
            }
        }

        [Command]
        public void Select()
        {
            try
            {
                OnProductsSelected?.Invoke(Nomenclatures.Where(n => n.Quantity > 0).Select(n => new Sale { Id = n.Id, Name = n.Name, Barcode = n.Barcode, Quantity = n.Quantity, QuantityInStock = n.QuantityInStock, SalePrice = n.SalePrice, ArrivalPrice = n.ArrivalPrice}).ToList());
                CurrentWindowService.Close();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void CleareSearchText()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchText = string.Empty;
                ProductGridControl.ClearColumnFilter("Name");
            }
        }

        [Obsolete]
        [Command]
        public void EditValueChanged()
        {
            if (SelectedTypeProduct != null)
            {
                ClearColumnFilter("TypeProductId");
                if (SelectedTypeProduct.Id != 1)
                {
                    if (FilterCriteria != null)
                    {
                        FilterCriteria &= FilterCriteria & new BinaryOperator("TypeProductId", SelectedTypeProduct.Id, BinaryOperatorType.Equal);
                    }
                    else
                    {
                        FilterCriteria = new BinaryOperator("TypeProductId", SelectedTypeProduct.Id, BinaryOperatorType.Equal);
                    }
                }
            }
            if (!string.IsNullOrEmpty(SearchText))
            {
                if (int.TryParse(SearchText, out _))
                {
                    ClearColumnFilter("Barcode");
                    if (FilterCriteria != null)
                    {
                        FilterCriteria &= new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("Barcode"), new OperandValue(SearchText));
                    }
                    else
                    {
                        FilterCriteria = new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("Barcode"), new OperandValue(SearchText));
                    }
                }
                else
                {
                    ClearColumnFilter("Name");
                    if (FilterCriteria != null)
                    {
                        FilterCriteria &= new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("Name"), new OperandValue(SearchText));
                    }
                    else
                    {
                        FilterCriteria = new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("Name"), new OperandValue(SearchText));
                    }
                }
            }
            else
            {
                ClearColumnFilter("Name");
                ClearColumnFilter("Barcode");
            }
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            base.Dispose();
        }

        #endregion
    }

    public class Nomenclature : Sale
    {
        #region Private Members

        private int _typeProductId;

        #endregion

        #region Public Properties

        public int TypeProductId
        {
            get => _typeProductId;
            set
            {
                _typeProductId = value;
                OnPropertyChanged(nameof(TypeProductId));
            }
        }

        #endregion
    }

}
