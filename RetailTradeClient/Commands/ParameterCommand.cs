using System;
using System.Windows.Input;

namespace RetailTradeClient.Commands
{
    public class ParameterCommand : ICommand
    {
        private Action<object> _action;

        public ParameterCommand(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
