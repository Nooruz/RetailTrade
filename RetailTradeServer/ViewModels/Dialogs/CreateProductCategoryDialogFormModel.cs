using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateProductCategoryDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductCategoryService _productCategoryService;
        private readonly IUIManager _manager;
        private string _name;

        #endregion

        #region Public Properties

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public bool IsEditMode { get; set; }
        public ProductCategory EditProductCategory { get; set; }

        #endregion

        #region Commands

        public ICommand SaveCommand { get; }

        #endregion

        #region Constructor

        public CreateProductCategoryDialogFormModel(IProductCategoryService productCategoryService,
            IUIManager manager)
        {
            _productCategoryService = productCategoryService;
            _manager = manager;

            CreateCommand = new RelayCommand(Create);
            SaveCommand = new RelayCommand(Save);
        }

        #endregion

        #region Private Voids

        private async void Create()
        {
            if (string.IsNullOrEmpty(Name))
                return;
            await _productCategoryService.CreateAsync(new ProductCategory
            {
                Name = Name
            });
            _manager.Close();
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(Name))
                return;
            EditProductCategory.Name = Name;
            await _productCategoryService.UpdateAsync(EditProductCategory.Id, EditProductCategory);
            _manager.Close();
        }

        #endregion
    }
}
