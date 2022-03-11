using DevExpress.Data.Filtering;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class CashShiftsViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IShiftService _shiftService;
        private readonly IUserService _userService;
        private IEnumerable<Shift> _shifts;
        private IEnumerable<User> _users;
        private int? _selectedUserId;

        #endregion

        #region Public Properties

        public IEnumerable<Shift> Shifts
        {
            get => _shifts;
            set
            {
                _shifts = value;
                OnPropertyChanged(nameof(Shifts));
            }
        }
        public IEnumerable<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
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
        public GridControl ShiftGridControl { get; set; }
        public CriteriaOperator FilterCriteria
        {
            get => ShiftGridControl.FilterCriteria;
            set => ShiftGridControl.FilterCriteria = value;
        }
        public CriteriaOperator OpeningDateFilter => ShiftGridControl.GetColumnFilterCriteria("OpeningDate");
        public CriteriaOperator UserIdFilter => ShiftGridControl.GetColumnFilterCriteria("UserId");

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        public ICommand GridControlLoadedCommand => new ParameterCommand((p) => GridControlLoaded(p));
        public ICommand SearchCommand => new RelayCommand(Search);
        public ICommand CleareSelectedUserIdCommand => new RelayCommand(ClearColumnFilter);

        #endregion

        #region Constructor

        public CashShiftsViewModel(IShiftService shiftService,
            IUserService userService)
        {
            _shiftService = shiftService;
            _userService = userService;

            Header = "Кассовые смены";
        }

        #endregion

        #region Private Voids

        [Obsolete]
        private void Search()
        {
            if (SelectedUserId != null)
            {
                FilterCriteria = new BinaryOperator(nameof(Shift.UserId), SelectedUserId.Value, BinaryOperatorType.Equal);
            }
        }

        private void ClearColumnFilter()
        {
            ShiftGridControl.ClearColumnFilter(nameof(Shift.UserId));
            SelectedUserId = null;
        }

        private async void UserControlLoaded()
        {
            Shifts = await _shiftService.GetAllAsync();
            Users = await _userService.GetCashiersAsync();
            ShowLoadingPanel = false;
        }

        private void GridControlLoaded(object parameter)
        {
            if (parameter is RoutedEventArgs e)
            {
                if (e.Source is GridControl gridControl)
                {
                    ShiftGridControl = gridControl;
                    ShiftGridControl.View.MoveLastRow();
                }
            }
        }

        #endregion
    }
}
