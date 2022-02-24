using DevExpress.Xpf.Grid;
using DevExpress.XtraEditors.DXErrorProvider;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using SalePageServer.State.Dialogs;
using System;
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
        private readonly IRevaluationService _revaluationService;
        private readonly IDataService<Unit> _unitService;
        private readonly IMenuNavigator _menuNavigator;
        private ObservableCollection<RevaluationProduct> _revaluationProducts = new();
        private IEnumerable<Unit> _units;
        private RevaluationProduct _selectedRevaluationProduct;
        private ObservableCollection<Product> _products;
        private ObservableCollection<Product> _filteringProducts;
        private string _productNameFilterCriteria;
        private string _comment;

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
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        public ICommand AddProductCommand => new RelayCommand(AddProduct);
        public ICommand ProductCommand => new RelayCommand(Create);
        public ICommand CellValueChangedCommand => new ParameterCommand(CellValueChanged);
        public ICommand ValidateCellCommand => new ParameterCommand(ValidateCell);
        public ICommand CreateAndCloseCommand => new RelayCommand(CreateAndClose);
        public ICommand DeleteRevaluationProductCommand => new RelayCommand(DeleteRevaluationProduct);

        #endregion

        #region Constructor

        public SettingProductPriceViewModel(IProductService productService,
            ITypeProductService typeProductService,
            IDialogService dialogService,
            IRevaluationService revaluationService,
            IDataService<Unit> unitService,
            IMenuNavigator menuNavigator)
        {
            _productService = productService;
            _typeProductService = typeProductService;
            _dialogService = dialogService;
            _revaluationService = revaluationService;
            _unitService = unitService;
            _menuNavigator = menuNavigator;
            _localDialogService = new DialogService();
        }

        #endregion

        #region Private Voids

        private void DeleteRevaluationProduct()
        {
            if (SelectedRevaluationProduct != null)
            {
                _ = RevaluationProducts.Remove(SelectedRevaluationProduct);
            }
        }

        private async void CreateAndClose()
        {
            if (RevaluationProducts.Any())
            {
                RevaluationProduct revaluationProduct = RevaluationProducts.FirstOrDefault(r => r.ProductId == 0);
                RevaluationProducts.ToList().ForEach(r => r.Product = null);
                if (revaluationProduct == null)
                {
                    _ = await _revaluationService.CreateAsync(new Revaluation
                    {
                        RevaluationDate = DateTime.Now,
                        Comment = Comment,
                        RevaluationProducts = RevaluationProducts
                    });
                    _menuNavigator.DeleteViewModel(this);
                }
                else
                {
                    MessageBox.Show("Товар не выбран.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    SelectedRevaluationProduct = revaluationProduct;
                }
            }
            else
            {
                MessageBox.Show("Не введено ни одной строки в список.", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ValidateCell(object parameter)
        {
            if (parameter is GridCellValidationEventArgs e)
            {
                if (e.Cell.Property == nameof(RevaluationProduct.ProductId))
                {
                    try
                    {
                        if (RevaluationProducts.Any(r => r.ProductId == (int)e.Value))
                        {
                            if (SelectedRevaluationProduct.ProductId != (int)e.Value)
                            {
                                e.ErrorContent = "Такой товар уже введен.";
                                e.ErrorType = ErrorType.Critical;
                                e.IsValid = false;
                            }                            
                        }
                    }
                    catch (Exception)
                    {
                        //ignore
                    }
                }
            }
        }

        private void CellValueChanged(object parameter)
        {
            if (parameter is CellValueChangedEventArgs e)
            {
                if (e.Cell.Property == nameof(RevaluationProduct.ProductId))
                {
                    try
                    {
                        Product selectedProduct = Products.FirstOrDefault(p => p.Id == (int)e.Cell.Value);
                        if (selectedProduct != null)
                        {
                            SelectedRevaluationProduct.Product = selectedProduct;
                        }
                    }
                    catch (Exception)
                    {
                        //ignore
                    }
                }
            }
        }

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
                    EmptyRevaluationProduct.Product = viewModel.SelectedProduct;
                    EmptyRevaluationProduct.ProductId = viewModel.SelectedProduct.Id;
                }
                else
                {
                    MessageBox.Show("Такой товар уже введен.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }            
        }

        private void AddProduct()
        {
            if (EmptyRevaluationProduct != null)
            {
                MessageBox.Show("Товар не выбран.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
