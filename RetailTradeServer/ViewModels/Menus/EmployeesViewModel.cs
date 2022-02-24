using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
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

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        public ICommand CreateEmployeeCommand => new RelayCommand(CreateEmployee);
        public ICommand CreateGroupEmployeeCommand { get; }

        #endregion

        #region Constructor

        public EmployeesViewModel(IEmployeeService employeeService,
            IGroupEmployeeService groupEmployeeService,
            IDataService<Gender> genderService)
        {
            _groupEmployeeService = groupEmployeeService;
            _employeeService = employeeService;
            _genderService = genderService;

            Header = "Сотрудники";
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
            WindowService.Show(nameof(EmployeeDialogForm), new EmployeeDialogFormModel()
            {
                Genders = await _genderService.GetAllAsync(),
                GroupEmployees = await _groupEmployeeService.GetAllAsync(),
                SelectedGroupEmployeeDTOId = SelectedGroupEmployeeDTO != null ? SelectedGroupEmployeeDTO.SubGroupId : 0
            });
        }

        #endregion
    }
}
