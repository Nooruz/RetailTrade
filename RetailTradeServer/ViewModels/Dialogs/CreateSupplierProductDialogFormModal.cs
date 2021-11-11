using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateSupplierProductDialogFormModal : BaseDialogViewModel
    {
        #region Private Members

        private readonly ISupplierService _supplierService;
        private readonly IUIManager _manager;
        private string _fullName;
        private string _shortName;
        private string _address;
        private string _phone;
        private string _inn;

        #endregion

        #region Public Properties

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string ShortName
        {
            get => _shortName;
            set
            {
                _shortName = value;
                OnPropertyChanged(nameof(ShortName));
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        public string Inn
        {
            get => _inn;
            set
            {
                _inn = value;
                OnPropertyChanged(nameof(Inn));
            }
        }

        #endregion

        #region Commands

        public ICommand CreateCommand { get; }

        #endregion

        #region Constructor

        public CreateSupplierProductDialogFormModal(ISupplierService supplierService,
            IUIManager manager)
        {
            _supplierService = supplierService;
            _manager = manager;
            CreateCommand = new RelayCommand(Create);
        }

        #endregion

        #region Private Voids

        private async void Create()
        {
            if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(ShortName))
                return;
            await _supplierService.CreateAsync(new Supplier
            {
                FullName = FullName,
                ShortName = ShortName,
                Address = Address,
                Phone = Phone,
                Inn = Inn,
                CreateDate = DateTime.Now
            });
            _manager.Close();
        }

        #endregion
    }
}
