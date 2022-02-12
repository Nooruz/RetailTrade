using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using SalePageServer.State.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class SettingProductPriceViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
        private readonly IDialogService _dialogService;
        private readonly IDialogService _localDialogService;
        private readonly IDataService<Unit> _unitService;
        private ObservableCollection<RevaluationProduct> _revaluationProducts = new();
        private IEnumerable<Unit> _units;
        private RevaluationProduct _selectedRevaluationProduct;
        private ObservableCollection<Product> _products;
        private ObservableCollection<Product> _filteringProducts;
        private string _productNameFilterCriteria;

        #endregion

        #region Public Properties

        public ObservableCollection<Product> Products
        {
            get => _products ?? new();
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public ObservableCollection<Product> FilteringProducts
        {
            get => _filteringProducts ?? new();
            set
            {
                _filteringProducts = value;
                OnPropertyChanged(nameof(FilteringProducts));
            }
        }
        public RevaluationProduct SelectedRevaluationProduct
        {
            get => _selectedRevaluationProduct;
            set
            {
                _selectedRevaluationProduct = value;
                OnPropertyChanged(nameof(SelectedRevaluationProduct));
            }
        }
        public ObservableCollection<RevaluationProduct> RevaluationProducts
        {
            get => _revaluationProducts;
            set
            {
                _revaluationProducts = value;
                OnPropertyChanged(nameof(RevaluationProducts));
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
        private RevaluationProduct EmptyRevaluationProduct => RevaluationProducts.FirstOrDefault(r => r.ProductId == 0);
        public string ProductNameFilterCriteria
        {
            get => _productNameFilterCriteria;
            set
            {
                _productNameFilterCriteria = value;
                OnPropertyChanged(nameof(ProductNameFilterCriteria));
            }
        }

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        public ICommand AddProductCommand => new RelayCommand(AddProduct);
        public ICommand ProductCommand => new RelayCommand(Create);

        #endregion

        #region Constructor

        public SettingProductPriceViewModel(IProductService productService,
            ITypeProductService typeProductService,
            IDialogService dialogService, 
            IDataService<Unit> unitService)
        {
            _productService = productService;
            _typeProductService = typeProductService;
            _dialogService = dialogService;
            _unitService = unitService;
            _localDialogService = new DialogService();
        }

        #endregion

        #region Private Voids

        private void Create()
        {
            ProductDialogFormModel viewModel = new(_productService, _typeProductService, _localDialogService)
            {
                Products = Products
            };
            _ = _localDialogService.ShowDialog(viewModel);
            if (viewModel.SelectedProduct != null)
            {
                if (RevaluationProducts.FirstOrDefault(r => r.ProductId == viewModel.SelectedProduct.Id) == null)
                {
                    EmptyRevaluationProduct.ArrivalPrice = viewModel.SelectedProduct.ArrivalPrice;
                    EmptyRevaluationProduct.SalePrice = viewModel.SelectedProduct.SalePrice;
                    EmptyRevaluationProduct.Product = viewModel.SelectedProduct;
                    EmptyRevaluationProduct.ProductId = viewModel.SelectedProduct.Id;
                }
                else
                {
                    _dialogService.ShowMessage("Такой товар уже введен.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }            
        }

        private void AddProduct()
        {
            if (EmptyRevaluationProduct != null)
            {
                _dialogService.ShowMessage("Товар не выбран.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                SelectedRevaluationProduct = EmptyRevaluationProduct;
            }
            else
            {
                RevaluationProducts.Add(new RevaluationProduct());
            }            
        }

        private async void UserControlLoaded()
        {
            Units = await _unitService.GetAllAsync();
            Products = new(await _productService.GetAllUnmarkedAsync());
            ShowLoadingPanel = false;
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
