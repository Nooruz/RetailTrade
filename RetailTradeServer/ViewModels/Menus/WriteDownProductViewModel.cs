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
    public class WriteDownProductViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly IWriteDownProductService _writeDownProductService;
        private readonly IUIManager _manager;
        private Product _selectedProduct;
        private IEnumerable<Product> _products;
        private bool _canWriteDownProduct;

        #endregion

        #region Public Properties

        public IEnumerable<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public ObservableCollection<WriteDownProduct> WriteDownProducts { get; set; }
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }
        public bool CanWriteDownProduct
        {
            get => _canWriteDownProduct;
            set
            {
                _canWriteDownProduct = value;
                OnPropertyChanged(nameof(CanWriteDownProduct));
            }
        }

        #endregion

        #region Command

        public ICommand WriteDownProductCommand { get; }
        public ICommand ValidateRowCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand GetProductsAsyncCommand { get; }

        #endregion

        #region Constructor

        public WriteDownProductViewModel(IProductService productService,
            IWriteDownProductService writeDownProductService,
            IUIManager manager)
        {
            _productService = productService;
            _writeDownProductService = writeDownProductService;
            _manager = manager;

            WriteDownProducts = new();

            ValidateRowCommand = new ParameterCommand(parameter => ValidateRow(parameter));
            ClearCommand = new RelayCommand(Cleare);
            WriteDownProductCommand = new RelayCommand(WriteOffProducts);
            GetProductsAsyncCommand = new RelayCommand(async () => { Products = await GetProductsAsync(); });

            WriteDownProducts.CollectionChanged += WriteDownProducts_CollectionChanged;
        }

        #endregion

        #region Private Voids

        private async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _productService.Queryable();
        }

        private void Cleare()
        {
            WriteDownProducts.Clear();
        }

        private async void WriteOffProducts()
        {
            if (await _writeDownProductService.AddRangeAsync(WriteDownProducts.Where(ap => ap.ProductId != 0).ToList()))
            {
                _manager.ShowMessage("Все товары успешно списаны.", "", MessageBoxButton.OK);
                WriteDownProducts.Clear();
            }
            else
            {
                _manager.ShowMessage("Ошибка при списании товаров.", "", MessageBoxButton.OK);
            }
        }

        private void WriteDownProducts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
            if (e.PropertyName == nameof(WriteDownProduct.ProductId))
            {
                if (sender is WriteDownProduct writeDownProduct)
                {
                    if (SelectedProduct != null)
                    {
                        writeDownProduct.Product = SelectedProduct;
                    }
                }
            }
        }

        private void ValidateRow(object parameter)
        {
            if (parameter is GridRowValidationEventArgs e)
            {
                if (((WriteDownProduct)e.Row).ProductId == 0)
                {
                    e.IsValid = false;
                    e.ErrorContent = "Введите наименованине товара!";
                    _manager.ShowMessage("Введите наименованине товара!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (((WriteDownProduct)e.Row).Product != null)
                {
                    if (((WriteDownProduct)e.Row).Product.Quantity == 0)
                    {
                        e.IsValid = false;
                        e.ErrorContent = "Нет товара для списания на кладе";
                        _manager.ShowMessage("Нет товара для списания на кладе", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (((WriteDownProduct)e.Row).Product.Quantity > 0 && ((WriteDownProduct)e.Row).Quantity == 0)
                    {
                        e.IsValid = false;
                        e.ErrorContent = "Количество не должно быть 0!";
                        _manager.ShowMessage("Количество не должно быть 0!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (((WriteDownProduct)e.Row).Product.Quantity < ((WriteDownProduct)e.Row).Quantity)
                    {
                        e.IsValid = false;
                        e.ErrorContent = "Количество списываемого товара превышает остатка на складе";
                        _manager.ShowMessage("Количество списываемого товара превышает остатка на складе", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                CanWriteDownProduct = e.IsValid;
            }
        }

        #endregion
    }
}
