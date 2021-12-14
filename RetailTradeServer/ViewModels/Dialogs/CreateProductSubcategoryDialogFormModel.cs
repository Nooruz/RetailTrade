using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using SalePageServer.State.Dialogs;
using System.Collections.Generic;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateProductSubcategoryDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductSubcategoryService _productSubcategoryService;
        private readonly IDialogService _dialogService;
        private string _name;

        #endregion

        #region Public Properties

        public IEnumerable<ProductCategory> ProductCategories => _productCategoryService.GetAll();
        public int? SelectedProductCategoryId { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }        

        #endregion

        #region Commands

        public ICommand CreateCommand { get; }
        public ICommand CloseCommand { get; }

        #endregion

        #region Constructor

        public CreateProductSubcategoryDialogFormModel(IProductSubcategoryService productSubcategoryService,
            IProductCategoryService productCategoryService,
            IDialogService dialogService)
        {
            _productSubcategoryService = productSubcategoryService;
            _productCategoryService = productCategoryService;
            _dialogService = dialogService;

            CreateCommand = new RelayCommand(Create);
            CloseCommand = new RelayCommand(() => _dialogService?.Close());
        }

        #endregion

        #region Private Voids

        private async void Create()
        {
            if (string.IsNullOrEmpty(Name) || SelectedProductCategoryId == null)
                return;
            await _productSubcategoryService.CreateAsync(new ProductSubcategory
            {
                Name = Name,
                ProductCategoryId = SelectedProductCategoryId.Value
            });
            _dialogService.Close();
        }

        #endregion
    }
}
