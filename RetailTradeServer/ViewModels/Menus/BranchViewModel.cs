using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System.Collections.Generic;

namespace RetailTradeServer.ViewModels.Menus
{
    public class BranchViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IDataService<Branch> _branchService;
        private readonly IUIManager _manager;
        private readonly IUserService _userService;

        #endregion

        #region Public Properties

        public IEnumerable<Branch> Branches => _branchService.GetAll();

        #endregion

        #region Commands

        

        #endregion

        #region Constructor

        public BranchViewModel(IDataService<Branch> branchService,
            IUIManager manager,
            IUserService userService)
        {
            _branchService = branchService;
            _manager = manager;
            _userService = userService;

            CreateCommand = new RelayCommand(CreateBranch);
            DeleteCommand = new RelayCommand(DeleteBranch);
        }

        #endregion

        #region Private Members

        private void CreateBranch()
        {
            _manager.ShowDialog(new CreateBranchDialogFormModel(_branchService, _userService), 
                new CreateBranchDialogForm());
        }

        private void DeleteBranch()
        {

        }

        #endregion
    }
}
