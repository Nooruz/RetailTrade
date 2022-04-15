using System;
using System.Windows.Input;

namespace RetailTrade.SQLServerConnectionDialog.Commands
{
    public class ParameterCommand : ICommand
    {
        private Action<object?> _action;
        public ParameterCommand(Action<object?> action)
        {
            _action = action;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }
        public event EventHandler? CanExecuteChanged;

        public void Execute(object? parameter)
        {
            _action(parameter);
        }
    }
}
