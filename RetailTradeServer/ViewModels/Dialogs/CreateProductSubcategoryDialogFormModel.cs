using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Collections.Generic;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateProductSubcategoryDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductSubcategoryService _productSubcategoryService;
        private readonly IUIManager _manager;
        private string _name;

        #endregion

        #region Public Properties

        public IEnumerable<ProductCategory> ProductCategories => _productCategoryService.GetAll();
        public int SelectedProductCategoryId { get; set; }
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
            IUIManager manager)
        {
            _productSubcategoryService = productSubcategoryService;
            _productCategoryService = productCategoryService;
            _manager = manager;

            CreateCommand = new RelayCommand(Create);
            CloseCommand = new RelayCommand(() => _manager?.Close());
        }

        #endregion

        #region Private Voids

        private async void Create()
        {
            if (string.IsNullOrEmpty(Name) || SelectedProductCategoryId == 0)
                return;
            await _productSubcategoryService.CreateAsync(new ProductSubcategory
            {
                Name = Name,
                ProductCategoryId = SelectedProductCategoryId
            });
            _manager.Close();
        }

        #endregion
    }
}
