using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System;
using System.Collections.Generic;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class EmployeeDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IDataService<Gender> _genderService;
        private int? _selectedGroupEmployeeDTOId;

        #endregion

        #region Public Properties

        public string FullName { get; set; }
        public string Inn { get; set; }
        public Gender SelectedGender { get; set; }
        public DateTime BirthDate { get; set; }
        public int? SelectedGroupEmployeeDTOId
        {
            get => _selectedGroupEmployeeDTOId;
            set
            {
                _selectedGroupEmployeeDTOId = value;
                OnPropertyChanged(nameof(SelectedGroupEmployeeDTOId));
            }
        }
        public IEnumerable<GroupEmployee> GroupEmployees { get; set; }
        public IEnumerable<Gender> Genders { get; set; }

        #endregion

        #region Commands



        #endregion

        #region Constructor

        public EmployeeDialogFormModel()
        {

        }

        #endregion

        #region Private Voids



        #endregion
    }
}
