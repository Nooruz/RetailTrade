using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Collections.Generic;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class ProductDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
        private IEnumerable<Product> _products;
        private IEnumerable<TypeProduct> _typeProducts;

        #endregion

        #region Public Properties

        public IEnumerable<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Product));
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

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);

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

        private async void UserControlLoaded()
        {
            Products = await _productService.GetAllAsync();
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
