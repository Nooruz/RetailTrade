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

        #endregion

        #region Commands

        public ICommand CreateCommand { get; }
        public ICommand CloseCommand { get; }

        #endregion

        #region Constructor

        public CreateProductCategoryDialogFormModel(IProductCategoryService productCategoryService,
            IUIManager manager)
        {
            _productCategoryService = productCategoryService;
            _manager = manager;

            CreateCommand = new RelayCommand(Create);
            CloseCommand = new RelayCommand(() => _manager?.Close());
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

        #endregion
    }
}
