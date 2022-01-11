using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class CashShiftsViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IShiftService _shiftService;

        #endregion

        #region Public Properties



        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand { get; }

        #endregion

        #region Constructor

        public CashShiftsViewModel(object ds)
        {
            //_shiftService = shiftService;

            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);
        }

        #endregion

        #region Private Voids

        private void UserControlLoaded()
        {
            ShowLoadingPanel = false;
        }

        #endregion
    }
}
