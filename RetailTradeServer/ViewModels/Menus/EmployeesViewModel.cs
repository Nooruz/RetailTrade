using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class EmployeesViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IEmployeeService _employeeService;
        private readonly IGroupEmployeeService _groupEmployeeService;
        private ObservableCollection<GroupEmployee> _groupEmployees;
        private ObservableCollection<Employee> _employees;

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
        public ObservableCollection<GroupEmployee> GroupEmployees
        {
            get => _groupEmployees ?? new();
            set
            {
                _groupEmployees = value;
                OnPropertyChanged(nameof(GroupEmployees));
            }
        }

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand { get; }

        #endregion

        #region Constructor

        public EmployeesViewModel(IEmployeeService employeeService,
            IGroupEmployeeService groupEmployeeService)
        {
            _groupEmployeeService = groupEmployeeService;
            _employeeService = employeeService;

            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);
        }

        #endregion

        #region Private Voids

        private async void UserControlLoaded()
        {
            ShowLoadingPanel = false;
            GroupEmployees = new(await _groupEmployeeService.GetAllAsync());
            Employees = new(await _employeeService.GetAllAsync());
        }

        #endregion
    }
}
