using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateBranchDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IDataService<Branch> _branchService;
        private readonly IUserService _userService;
        private string _name;
        private string _address;
        private int? _selectedUserId;

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

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public int? SelectedUserId
        {
            get => _selectedUserId;
            set
            {
                _selectedUserId = value;
                OnPropertyChanged(nameof(SelectedUserId));
            }
        }

        public IEnumerable<User> Users => _userService.GetCashiers();

        #endregion

        #region Command

        public ICommand CreateBranchCommand { get; }

        #endregion

        #region Constructor

        public CreateBranchDialogFormModel(IDataService<Branch> branchService,
            IUserService userService)
        {
            _branchService = branchService;
            _userService = userService;

            CreateBranchCommand = new RelayCommand(CreateBranch);
        }

        #endregion

        #region Private Voids

        private async void CreateBranch()
        {
            if (string.IsNullOrEmpty(Name))
            {
                MessageBox.Show("Name");
                return;
            }
            if (string.IsNullOrEmpty(Address))
            {
                MessageBox.Show("Address");
                return;
            }
            if (SelectedUserId == null)
            {
                MessageBox.Show("SelectedUserId");
                return;
            }
            await _branchService.CreateAsync(new Branch
            {
                Name = Name,
                Address = Address,
                UserId = SelectedUserId.Value
            });
            CurrentWindowService.Close();
        }

        #endregion
    }
}
