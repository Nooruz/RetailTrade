using DevExpress.Data.Filtering;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
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
        private IEnumerable<Product> _products;
        private IEnumerable<TypeProduct> _typeProducts;
        private TypeProduct _selectedTypeProduct;
        private Product _selectedProduct;
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
        public IEnumerable<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public ObservableCollection<Product> SelectedProducts { get; } = new();
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
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

        #endregion

        #region Event Action

        public event Action<IEnumerable<Product>> OnProductsSelected;

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
            Products = Settings.Default.IsKeepRecords ? await _productService.PredicateSelect(p => p.Quantity > 0 && p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name, Barcode = p.Barcode, TypeProductId = p.TypeProductId, SalePrice = p.SalePrice, Quantity = p.Quantity }) :
                await _productService.PredicateSelect(p => p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name, Barcode = p.Barcode, TypeProductId = p.TypeProductId, SalePrice = p.SalePrice });
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
        public void Select()
        {
            if (SelectedProduct != null)
            {
                try
                {
                    OnProductsSelected?.Invoke(ProductGridControl.MySelectedItems.Cast<Product>().ToList());
                    CurrentWindowService.Close();
                }
                catch (Exception)
                {
                    //ignore
                }
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
}
