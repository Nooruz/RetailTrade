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
        private DateTime? _startDateTime;
        private DateTime? _endDateTime;

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
        public DateTime? StartDateTime
        {
            get => _startDateTime;
            set
            {
                _startDateTime = value;
                OnPropertyChanged(nameof(StartDateTime));
            }
        }
        public DateTime? EndDateTime
        {
            get => _endDateTime;
            set
            {
                _endDateTime = value;
                OnPropertyChanged(nameof(EndDateTime));
            }
        }
        public GridControl ShiftGridControl { get; set; }

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        public ICommand GridControlLoadedCommand => new ParameterCommand((p) => GridControlLoaded(p));
        public ICommand SearchCommand => new RelayCommand(Search);
        public ICommand CleareSelectedUserIdCommand => new RelayCommand(() => SelectedUserId = null);
        public ICommand CleareStartDateCommand => new RelayCommand(() => StartDateTime = null);
        public ICommand CleareEndDateCommand => new RelayCommand(() => EndDateTime = null);

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

        private void Search()
        {
            string filter;
            filter = SelectedUserId != null ? $"[UserId] = {SelectedUserId}" : string.Empty;
            filter = string.IsNullOrEmpty(filter) ? StartDateTime != null ? $"[OpeningDate] >= '{StartDateTime}'" : string.Empty : StartDateTime != null ? filter + $" AND [OpeningDate] >= '{StartDateTime}'" : filter;
            filter = string.IsNullOrEmpty(filter) ? EndDateTime != null ? $"[OpeningDate] <= '{EndDateTime}'" : string.Empty : EndDateTime != null ? filter + $" AND [OpeningDate] <= '{EndDateTime}'" : filter;
            ShiftGridControl.FilterString = filter;
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
