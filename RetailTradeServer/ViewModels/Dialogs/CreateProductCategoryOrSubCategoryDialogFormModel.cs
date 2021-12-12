using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateProductCategoryOrSubCategoryDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductSubcategoryService _productSubcategoryService;
        private readonly IMessageStore _messageStore;
        private string _productCategoryName;
        private string _productSubCategoryName;
        private int? _selectedProductCategoryId;
        private ObservableCollection<ProductCategory> _productCategories;        

        #endregion

        #region Public Properties

        public string ProductCategoryName
        {
            get => _productCategoryName;
            set
            {
                _productCategoryName = value;
                OnPropertyChanged(nameof(ProductCategoryName));
            }
        }
        public string ProductSubCategoryName
        {
            get => _productSubCategoryName;
            set
            {
                _productSubCategoryName = value;
                OnPropertyChanged(nameof(ProductSubCategoryName));
            }
        }
        public int? SelectedProductCategoryId
        {
            get => _selectedProductCategoryId;
            set
            {
                _selectedProductCategoryId = value;
                OnPropertyChanged(nameof(SelectedProductCategoryId));
            }
        }
        public GlobalMessageViewModel GlobalMessageViewModel { get; }
        public ObservableCollection<ProductCategory> ProductCategories
        {
            get => _productCategories;
            set
            {
                _productCategories = value;
                OnPropertyChanged(nameof(ProductCategories));
            }
        }

        #endregion

        #region Commands

        public ICommand CreateProductCategoryCommand { get; }
        public ICommand CreateProductSubCategoryCommand { get; }
        public ICommand UserControlLoadedCommand { get; }
        public ICommand TabControlSelectionChangingCommand { get; }

        #endregion

        #region Constructor

        public CreateProductCategoryOrSubCategoryDialogFormModel(IProductCategoryService productCategoryService,
            IProductSubcategoryService productSubcategoryService,
            IMessageStore messageStore,
            GlobalMessageViewModel globalMessageViewModel)
        {
            CreateProductCategoryCommand = new RelayCommand(CreateProductCategory);
            CreateProductSubCategoryCommand = new RelayCommand(CreateProductSubCategory);
            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);
            TabControlSelectionChangingCommand = new RelayCommand(TabControlSelectionChanging);

            _productCategoryService = productCategoryService;
            _productSubcategoryService = productSubcategoryService;
            _messageStore = messageStore;
            GlobalMessageViewModel = globalMessageViewModel;

            _productCategoryService.OnProductCategoryCreated += ProductCategoryService_OnProductCategoryCreated;
            _productSubcategoryService.OnProductSubcategoryCreated += ProductSubcategoryService_OnProductSubcategoryCreated;
            _messageStore.Close();
        }

        #endregion

        #region Private Voids

        private async void CreateProductCategory()
        {
            if (!string.IsNullOrEmpty(ProductCategoryName))
            {
                await _productCategoryService.CreateAsync(new ProductCategory { Name = ProductCategoryName });
            }
            else
            {
                _messageStore.SetCurrentMessage("Введите наименование группы товаров.", MessageType.Error);
            }
        }

        private async void CreateProductSubCategory()
        {
            if (SelectedProductCategoryId != null)
            {
                if (!string.IsNullOrEmpty(ProductSubCategoryName))
                {
                    await _productSubcategoryService.CreateAsync(new ProductSubcategory
                    {
                        ProductCategoryId = SelectedProductCategoryId.Value,
                        Name = ProductSubCategoryName
                    });
                }
                else
                {
                    _messageStore.SetCurrentMessage("Введите наименование категории товаров.", MessageType.Error);
                }
            }
            else
            {
                _messageStore.SetCurrentMessage("Выберите группу товаров.", MessageType.Error);
            }
        }

        private async void UserControlLoaded()
        {
            ProductCategories = new(await _productCategoryService.GetAllAsync());
        }

        private void ProductCategoryService_OnProductCategoryCreated(ProductCategory productCategory)
        {
            if (productCategory != null)
            {
                ProductCategoryName = string.Empty;
                _messageStore.SetCurrentMessage($"Категория \"{productCategory.Name}\" создана", MessageType.Success);
                ProductCategories.Add(productCategory);
            }
            else
            {
                _messageStore.SetCurrentMessage("Ошибка при создании категорию", MessageType.Error);
            }
        }

        private void ProductSubcategoryService_OnProductSubcategoryCreated(ProductSubcategory productSubcategory)
        {
            if (productSubcategory != null)
            {
                ProductSubCategoryName = string.Empty;
                _messageStore.SetCurrentMessage($"Группа \"{productSubcategory.Name}\" создана", MessageType.Success);
            }
            else
            {
                _messageStore.SetCurrentMessage("Ошибка при создании группу", MessageType.Error);
            }
        }

        private void TabControlSelectionChanging()
        {
            _messageStore.Close();
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            _productCategoryService.OnProductCategoryCreated -= ProductCategoryService_OnProductCategoryCreated;
            _productSubcategoryService.OnProductSubcategoryCreated -= ProductSubcategoryService_OnProductSubcategoryCreated;
            base.Dispose();
        }

        #endregion
    }
}
