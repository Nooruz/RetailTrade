﻿using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using SalePageServer.State.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class EmployeesViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IEmployeeService _employeeService;
        private readonly IGroupEmployeeService _groupEmployeeService;
        private readonly IDialogService _dialogService;
        private readonly IDataService<Gender> _genderService;
        private ObservableCollection<GroupEmployeeDTO> _groupEmployees;
        private ObservableCollection<Employee> _employees;
        private GroupEmployeeDTO _selectedGroupEmployeeDTO;

        #endregion

        #region Public Properties

        public ObservableCollection<Employee> Employees
        {
            get => _employees ?? new();
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }
        public ObservableCollection<GroupEmployeeDTO> GroupEmployees
        {
            get => _groupEmployees ?? new();
            set
            {
                _groupEmployees = value;
                OnPropertyChanged(nameof(GroupEmployees));
            }
        }
        public GroupEmployeeDTO SelectedGroupEmployeeDTO
        {
            get => _selectedGroupEmployeeDTO;
            set
            {
                _selectedGroupEmployeeDTO = value;
                OnPropertyChanged(nameof(SelectedGroupEmployeeDTO));
            }
        }

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand { get; }
        public ICommand CreateEmployeeCommand { get; }
        public ICommand CreateGroupEmployeeCommand { get; }

        #endregion

        #region Constructor

        public EmployeesViewModel(IEmployeeService employeeService,
            IGroupEmployeeService groupEmployeeService,
            IDialogService dialogService,
            IDataService<Gender> genderService)
        {
            _groupEmployeeService = groupEmployeeService;
            _employeeService = employeeService;
            _dialogService = dialogService;
            _genderService = genderService;

            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);
            CreateEmployeeCommand = new RelayCommand(CreateEmployee);
        }

        #endregion

        #region Private Voids

        private async void UserControlLoaded()
        {            
            Employees = new(await _employeeService.GetAllAsync());
            GroupEmployees = new(await _groupEmployeeService.GetDTOAllAsync());
            int id = GroupEmployees.Count;
            foreach (GroupEmployeeDTO item in Employees.Select(e => new GroupEmployeeDTO { EmployeeId = e.Id, SubGroupId = e.GroupEmployeeId, Name = e.FullName, NotFolder = true }).ToList())
            {
                id++;
                item.Id = id;
                GroupEmployees.Add(item);
            }
            ShowLoadingPanel = false;
        }

        private async void CreateEmployee()
        {
            _ = _dialogService.ShowDialog(new EmployeeDialogFormModel()
            {
                Genders = await _genderService.GetAllAsync(),
                GroupEmployees = await _groupEmployeeService.GetAllAsync(),
                SelectedGroupEmployeeDTOId = SelectedGroupEmployeeDTO != null ? SelectedGroupEmployeeDTO.SubGroupId : 0
            });
        }

        #endregion
    }
}