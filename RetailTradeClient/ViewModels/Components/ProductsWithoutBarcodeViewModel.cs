using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.Properties;
using RetailTradeClient.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Components
{
    public class ProductsWithoutBarcodeViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private ObservableCollection<Product> _products = new();

        #endregion

        #region Public Properties

        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public bool IsKeepRecords => Settings.Default.IsKeepRecords;

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        public ICommand AddProductToSaleCommand => new ParameterCommand(pc => AddProductToSale(pc));

        #endregion

        #region Constructor

        public ProductsWithoutBarcodeViewModel(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region Private Voids

        private async void UserControlLoaded()
        {
            Products = IsKeepRecords ? new(await _productService.PredicateSelect(p => p.Quantity > 0 && p.WithoutBarcode && p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity })) :
                new(await _productService.PredicateSelect(p => p.WithoutBarcode == true && p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name }));
        }

        private async void AddProductToSale(object parameter)
        {
            if (parameter is int id)
            {
                if (IsKeepRecords)
                {
                    Product product = Products.FirstOrDefault(p => p.Id == id);
                    product.Quantity -= 1;
                    if (product.Quantity <= 0)
                    {
                        _ = Products.Remove(product);
                    }
                }
            }            
        }

        #endregion
    }
}
