using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using SalePageServer.State.Dialogs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class TypeProductDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly ITypeProductService _typeProductService;
        private readonly IDialogService _dialogService;
        private string _name;
        private TypeProduct _typeProduct;
        private int? _selectedTypeProductId;
        private IEnumerable<TypeProduct> _typeProducts;

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
        public bool IsGroup { get; set; }
        public TypeProduct TypeProduct
        {
            get => _typeProduct;
            set
            {
                _typeProduct = value;
                Name = _typeProduct.Name;
                SelectedTypeProductId = _typeProduct.SubGroupId;
                OnPropertyChanged(nameof(TypeProduct));
            }
        }
        public int? SelectedTypeProductId
        {
            get => _selectedTypeProductId;
            set
            {
                _selectedTypeProductId = value;
                OnPropertyChanged(nameof(SelectedTypeProductId));
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

        public ICommand SaveCommand { get; }
        public ICommand UserControlLoadedCommand { get; }

        #endregion

        #region Constructor

        public TypeProductDialogFormModel(ITypeProductService typeProductService,
            IDialogService dialogService)
        {
            _typeProductService = typeProductService;
            _dialogService = dialogService;

            CreateCommand = new RelayCommand(Create);
            SaveCommand = new RelayCommand(Save);
            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);
        }

        #endregion

        #region Private Voids

        private async void UserControlLoaded()
        {
            TypeProducts = await _typeProductService.GetGroupsAsync();
        }

        private async void Create()
        {
            if (string.IsNullOrEmpty(Name))
            {
                _dialogService.ShowMessage("Введите наименование!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            await _typeProductService.CreateAsync(new TypeProduct
            {
                Name = Name,
                IsGroup = IsGroup,
                SubGroupId = SelectedTypeProductId == null ? 1 : SelectedTypeProductId.Value
            });
            _dialogService.Close();
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(Name))
            {
                _dialogService.ShowMessage("Введите наименование!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            _ = await _typeProductService.UpdateAsync(TypeProduct.Id, new TypeProduct { Name = Name, SubGroupId = SelectedTypeProductId.Value });
            _dialogService.Close();
        }

        #endregion
    }
}
