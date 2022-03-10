using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class TypeProductDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly ITypeProductService _typeProductService;
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

        public ICommand SaveCommand => new RelayCommand(Save);
        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);

        #endregion

        #region Constructor

        public TypeProductDialogFormModel(ITypeProductService typeProductService)
        {
            _typeProductService = typeProductService;

            CreateCommand = new RelayCommand(Create);
            CloseCommand = new RelayCommand(() => CurrentWindowService.Close());
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
                _ = MessageBoxService.ShowMessage("Введите наименование!", "", MessageButton.OK, MessageIcon.Exclamation);
                return;
            }
            _ = await _typeProductService.CreateAsync(new TypeProduct
            {
                Name = Name,
                IsGroup = IsGroup,
                SubGroupId = SelectedTypeProductId == null ? 1 : SelectedTypeProductId.Value
            });
            CurrentWindowService.Close();
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(Name))
            {
                _ = MessageBoxService.ShowMessage("Введите наименование!", "", MessageButton.OK, MessageIcon.Exclamation);
                return;
            }
            _ = await _typeProductService.UpdateAsync(TypeProduct.Id, new TypeProduct { Name = Name, SubGroupId = SelectedTypeProductId.Value });
            CurrentWindowService.Close();
        }

        #endregion
    }
}
