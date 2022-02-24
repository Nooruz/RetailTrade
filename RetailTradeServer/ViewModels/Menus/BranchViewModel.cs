using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
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
        private readonly IUserService _userService;

        #endregion

        #region Public Properties

        public IEnumerable<Branch> Branches => _branchService.GetAll();

        #endregion

        #region Commands

        

        #endregion

        #region Constructor

        public BranchViewModel(IDataService<Branch> branchService,
            IUserService userService)
        {
            _branchService = branchService;
            _userService = userService;

            Header = "Филиалы";

            CreateCommand = new RelayCommand(CreateBranch);
            DeleteCommand = new RelayCommand(DeleteBranch);
        }

        #endregion

        #region Private Members

        private void CreateBranch()
        {
            WindowService.Show(nameof(CreateBranchDialogForm), new CreateBranchDialogFormModel(_branchService, _userService));
        }

        private void DeleteBranch()
        {

        }

        #endregion

        public override void Dispose()
        {            
            base.Dispose();
        }
    }
}
