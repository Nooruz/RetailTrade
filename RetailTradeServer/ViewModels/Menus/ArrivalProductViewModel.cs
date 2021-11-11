using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ArrivalProductViewModel : BaseViewModel
    {
        #region Private members

        private readonly IProductService _productService;
        private readonly IArrivalProductService _arrivalProductService;
        private readonly IUIManager _manager;
        private Product _selectedProduct;
        private IEnumerable<Product> _products;

        #endregion

        #region Public properties

        public IEnumerable<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public ObservableCollection<ArrivalProduct> ArrivalProducts { get; set; }
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        #endregion

        #region Commands

        public ICommand AddProductCommand { get; }
        public ICommand ValidateRowCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand GetProductsAsyncCommand { get; }

        #endregion

        #region Constructor

        public ArrivalProductViewModel(IProductService productService,
            IArrivalProductService arrivalProductService,
            IUIManager manager)
        {
            _productService = productService;
            _arrivalProductService = arrivalProductService;
            _manager = manager;

            ArrivalProducts = new();

            AddProductCommand = new RelayCommand(AddProduct);
            ClearCommand = new RelayCommand(Cleare);
            ValidateRowCommand = new ParameterCommand(parameter => ValidateRow(parameter));
            GetProductsAsyncCommand = new RelayCommand(async () => { Products = await GetProductsAsync(); });

            ArrivalProducts.CollectionChanged += ArrivalProducts_CollectionChanged;
        }        

        #endregion

        #region Private Voids

        private async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _productService.Queryable();
        }

        private async void AddProduct()
        {
            if (await _arrivalProductService.AddRangeAsync(ArrivalProducts.Where(ap => ap.ProductId != 0).ToList()))
            {
                _manager.ShowMessage("Все товары успешно добавлены.", "", MessageBoxButton.OK);
                ArrivalProducts.Clear();
            }
            else
            {
                _manager.ShowMessage("Ошибка при добавление товаров.", "", MessageBoxButton.OK);
            }
        }

        private void ArrivalProducts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    if (item != null)
                    {
                        item.PropertyChanged -= Item_PropertyChanged;
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    if (item != null)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ArrivalProduct.ProductId))
            {
                if (sender is ArrivalProduct arrivalProduct)
                {
                    arrivalProduct.Product = SelectedProduct;
                }
            }
        }

        private void ValidateRow(object parameter)
        {
            if (parameter is GridRowValidationEventArgs e)
            {
                if (((ArrivalProduct)e.Row).ProductId == 0)
                {
                    e.IsValid = false;
                    e.ErrorContent = "Введите наименованине товара!";
                    _manager.ShowMessage("Введите наименованине товара!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (((ArrivalProduct)e.Row).Quantity == 0)
                {
                    e.IsValid = false;
                    e.ErrorContent = "Количество не должно быть 0!";
                    _manager.ShowMessage("Количество не должно быть 0!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Cleare()
        {
            ArrivalProducts.Clear();
        }

        #endregion
    }
}
