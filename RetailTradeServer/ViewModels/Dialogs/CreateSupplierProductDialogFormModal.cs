using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateSupplierProductDialogFormModal : BaseDialogViewModel
    {
        #region Private Members

        private readonly ISupplierService _supplierService;
        private string _fullName;
        private string _shortName;
        private string _address;
        private string _phone;
        private string _inn;

        #endregion

        #region Public Properties

        public int Id { get; set; }

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

        public bool IsEditing { get; set; }

        #endregion

        #region Commands

        public ICommand CreateCommand { get; }
        public ICommand SaveCommand { get; }

        #endregion

        #region Constructor

        public CreateSupplierProductDialogFormModal(ISupplierService supplierService)
        {
            _supplierService = supplierService;
            CreateCommand = new RelayCommand(Create);
            SaveCommand = new RelayCommand(Save);
            CloseCommand = new RelayCommand(() => CurrentWindowService.Close());
        }

        #endregion

        #region Private Voids

        private async void Create()
        {
            if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(ShortName))
                return;
            _ = await _supplierService.CreateAsync(new Supplier
            {
                FullName = FullName,
                ShortName = ShortName,
                Address = Address,
                Phone = Phone,
                Inn = Inn,
                CreateDate = DateTime.Now
            });
            CurrentWindowService.Close();
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(ShortName))
                return;
            _ = await _supplierService.UpdateAsync(Id, new Supplier
            {
                FullName = FullName,
                ShortName = ShortName,
                Address = Address,
                Phone = Phone,
                Inn = Inn
            });
            CurrentWindowService.Close();
        }

        #endregion
    }
}
