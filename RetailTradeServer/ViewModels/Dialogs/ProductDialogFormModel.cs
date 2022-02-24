using DevExpress.Data.Filtering;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class ProductDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
        private ObservableCollection<Product> _products;
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
                Search();
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
                    Search();
                }
                OnPropertyChanged(nameof(IsTypesAndProperties));
            }
        }
        public ObservableCollection<Product> Products
        {
            get => _products ?? new();
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
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
        public IEnumerable<TypeProduct> TypeProducts
        {
            get => _typeProducts;
            set
            {
                _typeProducts = value;
                OnPropertyChanged(nameof(TypeProducts));
            }
        }
        public GridControl ProductGridControl { get; set; }
        public TypeProduct SelectedTypeProduct
        {
            get => _selectedTypeProduct;
            set
            {
                _selectedTypeProduct = value;
                Search();                
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

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        public ICommand GridControlLoadedCommand => new ParameterCommand(GridControlLoaded);
        public ICommand SelectCommand => new RelayCommand(Select);
        public ICommand CleareSearchTextCommand => new RelayCommand(CleareSearchText);
        public ICommand SearchCommand => new RelayCommand(Search);

        #endregion

        #region Constructor

        public ProductDialogFormModel(IProductService productService,
            ITypeProductService typeProductService)
        {
            _productService = productService;
            _typeProductService = typeProductService;
        }

        #endregion

        #region Private Voids

        [Obsolete]
        private void Search()
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
                ClearColumnFilter("Name");
                if (FilterCriteria != null)
                {
                    FilterCriteria &= IsByExactMatch ? new BinaryOperator("Name", SearchText, BinaryOperatorType.Equal) :
                            new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("Name"), new OperandValue(SearchText));
                }
                else
                {
                    FilterCriteria = IsByExactMatch ? new BinaryOperator("Name", SearchText, BinaryOperatorType.Equal) :
                            new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("Name"), new OperandValue(SearchText));
                }
            }
            else
            {
                ClearColumnFilter("Name");
            }                        
        }

        private void ClearColumnFilter(string columName)
        {
            ProductGridControl.ClearColumnFilter(columName);
        }

        private void CleareSearchText()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchText = string.Empty;
                ProductGridControl.ClearColumnFilter("Name");
            }
        }

        private void Select()
        {
            if (SelectedProduct != null)
            {
                CurrentWindowService.Close();
            }
        }

        private void GridControlLoaded(object parameter)
        {
            if (parameter is RoutedEventArgs e)
            {
                if (e.Source is GridControl gridControl)
                {
                    ProductGridControl = gridControl;
                }
            }
        }

        private async void UserControlLoaded()
        {
            TypeProducts = await _typeProductService.GetAllAsync();
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
