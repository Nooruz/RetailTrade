using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class TypeProductDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly ITypeProductService _typeProductService;
        private string _name;
        private TypeProduct _typeProduct;
        private int? _selectedGroupTypeProductId;
        private ObservableCollection<TypeProduct> _typeProducts = new();

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
                SelectedGroupTypeProductId = _typeProduct.SubGroupId;
                OnPropertyChanged(nameof(TypeProduct));
            }
        }
        public int? SelectedGroupTypeProductId
        {
            get => _selectedGroupTypeProductId;
            set
            {
                _selectedGroupTypeProductId = value;
                OnPropertyChanged(nameof(SelectedGroupTypeProductId));
            }
        }
        public ObservableCollection<TypeProduct> TypeProducts
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
            try
            {
                TypeProducts = new(await _typeProductService.GetGroupsAsync());
                if (IsEditMode)
                {
                    if (TypeProduct.IsGroup)
                    {
                        if (TypeProducts.Any())
                        {
                            TypeProducts.Remove(TypeProduct);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
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
                SubGroupId = SelectedGroupTypeProductId == null ? 1 : SelectedGroupTypeProductId.Value
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
            TypeProduct.Name = Name;
            TypeProduct.SubGroupId = SelectedGroupTypeProductId.Value;
            _ = await _typeProductService.UpdateAsync(TypeProduct.Id, TypeProduct);
            CurrentWindowService.Close();
        }

        #endregion
    }
}
