using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class WareHouseDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IWareHouseService _wareHouseService;
        private string _name;
        private string _address;
        private int _typeWareHouseId;
        private WareHouse _editebleWareHouse;

        #endregion

        #region Public Properties

        public WareHouse EditableWareHouse
        {
            get => _editebleWareHouse;
            set
            {
                _editebleWareHouse = value;
                if (_editebleWareHouse != null)
                {
                    Name = _editebleWareHouse.Name;
                    Address = _editebleWareHouse.Address;
                    TypeWareHouseId = _editebleWareHouse.TypeWareHouseId;
                }
                OnPropertyChanged(nameof(EditableWareHouse));
            }
        }
        public Visibility CreateButtonVisibility => IsEditMode ? Visibility.Collapsed : Visibility.Visible;
        public Visibility SaveButtonVisibility => IsEditMode ? Visibility.Visible : Visibility.Collapsed;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(CanCreate));
            }
        }
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
                OnPropertyChanged(nameof(CanCreate));
            }
        }
        public int TypeWareHouseId
        {
            get => _typeWareHouseId;
            set
            {
                _typeWareHouseId = value;
                TitleChange();
                OnPropertyChanged(nameof(TypeWareHouseId));
            }
        }
        public IEnumerable<TypeWareHouse> TypeWareHouses { get; set; }
        public bool CanCreate => !string.IsNullOrEmpty(Name);
        public bool IsEditMode { get; set; }


        #endregion

        #region Constructor

        public WareHouseDialogFormModel(IWareHouseService wareHouseService)
        {
            _wareHouseService = wareHouseService;
            
            CreateCommand = new RelayCommand(Create);
            CloseCommand = new RelayCommand(() => CurrentWindowService.Close());
        }

        #endregion

        #region Private Voids

        private async void Create()
        {
            if (CanCreate)
            {
                await _wareHouseService.CreateAsync(new WareHouse
                {
                    Name = Name,
                    Address = Address,
                    TypeWareHouseId = TypeWareHouseId
                });
            }
            CurrentWindowService.Close();
        }
        private void TitleChange()
        {
            try
            {
                Title = TypeWareHouseId == 1 ? "Склад (создание)" : "Розничный магазин (создание)";
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion

        #region Public Voids

        [Command]
        public async void Save()
        {
            if (CanCreate)
            {
                EditableWareHouse.Name = Name;
                EditableWareHouse.Address = Address;
                EditableWareHouse.TypeWareHouseId = TypeWareHouseId;
                await _wareHouseService.UpdateAsync(EditableWareHouse.Id, EditableWareHouse);
                CurrentWindowService.Close();
            }
        }

        #endregion
    }
}
