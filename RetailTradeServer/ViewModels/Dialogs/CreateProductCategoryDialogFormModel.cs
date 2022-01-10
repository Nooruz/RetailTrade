﻿using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using SalePageServer.State.Dialogs;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateProductCategoryDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductSubcategoryService _productSubcategoryService;
        private readonly IDialogService _dialogService;
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
        public ProductSubcategory EditProductSubcategory { get; set; }

        #endregion

        #region Commands

        public ICommand SaveCommand { get; }

        #endregion

        #region Constructor

        public CreateProductCategoryDialogFormModel(IProductCategoryService productCategoryService,
            IDialogService dialogService)
        {
            _productCategoryService = productCategoryService;
            _dialogService = dialogService;

            CreateCommand = new RelayCommand(Create);
            SaveCommand = new RelayCommand(Save);
        }

        public CreateProductCategoryDialogFormModel(IProductSubcategoryService productSubcategoryService,
            IDialogService dialogService)
        {
            _productSubcategoryService = productSubcategoryService;
            _dialogService = dialogService;

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
            _dialogService.Close();
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(Name))
            {
                _dialogService.ShowMessage("Введите имя!", "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
            }
            else
            {
                if (EditProductCategory != null)
                {
                    EditProductCategory.Name = Name;
                    await _productCategoryService.UpdateAsync(EditProductCategory.Id, EditProductCategory);
                    _dialogService.Close();
                }
                if (EditProductSubcategory != null)
                {
                    EditProductSubcategory.Name = Name;
                    await _productSubcategoryService.UpdateAsync(EditProductSubcategory.Id, EditProductSubcategory);
                    _dialogService.Close();
                }
            }            
        }

        #endregion
    }
}
