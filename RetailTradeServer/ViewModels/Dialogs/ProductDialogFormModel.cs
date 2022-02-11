using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
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

        public string FilterString
        {
            get => ProductGridControl.FilterString;
            set
            {
                ProductGridControl.FilterString = value;
                OnPropertyChanged(nameof(FilterString));
            }
        }
        public bool IsFilterStringNullOrEmpty => string.IsNullOrEmpty(ProductGridControl.FilterString);
        public bool IsByExactMatch
        {
            get => _isByExactMatch;
            set
            {
                _isByExactMatch = value;
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
        public string OldSearchText { get; set; }
        public bool IsTypesAndProperties
        {
            get => _isTypesAndProperties;
            set
            {
                _isTypesAndProperties = value;
                if (_isTypesAndProperties)
                {
                    ProductGridControl.FilterString = string.Empty;
                }
                else
                {
                    ProductGridControl.FilterString = SelectedTypeProduct.Id == 1 ? string.Empty : $"[TypeProductId] = {SelectedTypeProduct.Id}";
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
                ProductGridControl.FilterString = _selectedTypeProduct.Id == 1 ? string.Empty : $"[TypeProductId] = {_selectedTypeProduct.Id}";
                OnPropertyChanged(nameof(SelectedTypeProduct));
            }
        }

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        public ICommand GridControlLoadedCommand => new ParameterCommand(GridControlLoaded);
        public ICommand SelectCommand => new RelayCommand(Select);
        public ICommand CleareSearchTextCommand => new RelayCommand(CleareSearchText);
        public ICommand SearchCommand => new RelayCommand(Filtering);

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

        private void CleareSearchText()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchText = string.Empty;
            }
            Replace($"[Name] = '{OldSearchText}'", string.Empty);
            Replace($"Contains([Name], '{OldSearchText}')", string.Empty);
            OldSearchText = string.Empty;
        }

        private void Select()
        {
            
        }

        private void Replace(string oldValue, string newValue)
        {
            ProductGridControl.FilterString = ProductGridControl.FilterString.Replace(oldValue, newValue);
        }

        private void Filtering()
        {
            if (IsByExactMatch)
            {
                if (IsFilterStringNullOrEmpty)
                {
                    ProductGridControl.FilterString = $"[Name] = '{SearchText}'";
                }
                else
                {
                    Replace($"And Contains([Name], '{SearchText}')", string.Empty);
                    ProductGridControl.FilterString += $" And [Name] = '{SearchText}'";
                }
            }
            else
            {
                if (IsFilterStringNullOrEmpty)
                {
                    ProductGridControl.FilterString = $"Contains([Name], '{SearchText}')";
                }
                else
                {
                    Replace($"And [Name] = '{SearchText}'", string.Empty);
                    ProductGridControl.FilterString += $" And Contains([Name], '{SearchText}')";
                }
            }
            OldSearchText = SearchText;
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
            Products = new(await _productService.GetAllAsync());            
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
