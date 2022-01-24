using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Collections.Generic;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class WareHouseDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IWareHouseService _wareHouseService;
        private string _name;
        private string _address;
        private int _typeWareHouseId;

        #endregion

        #region Public Properties

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
                OnPropertyChanged(nameof(TypeWareHouseId));
            }
        }
        public IEnumerable<TypeWareHouse> TypeWareHouses { get; set; }
        public bool CanCreate => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Address);

        #endregion

        #region Commands



        #endregion

        #region Constructor

        public WareHouseDialogFormModel(IWareHouseService wareHouseService)
        {
            _wareHouseService = wareHouseService;

            CreateCommand = new RelayCommand(Create);
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
                    TypeWareHouseId = 1
                });
            }
        }

        #endregion
    }
}
