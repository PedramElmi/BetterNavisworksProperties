using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BetterPropertiesDockpane.Helper
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Predicate<object> _canExecute;
        private readonly bool _oneCommandAtATime;

        private bool _isExecuting;

        public bool IsExecuting
        {
            get { return _isExecuting; }
            set { _isExecuting = value; }
        }


        public AsyncRelayCommand(Func<object, Task> execute, Predicate<object> canExecute = null, bool oneCommandAtATime = true)
        {
            _execute = execute;
            _canExecute = canExecute;
            _oneCommandAtATime = oneCommandAtATime;
            _isExecuting = false;
        }

        private async Task ExecuteAsync(object parameter)
        {
            await _execute.Invoke(parameter);
        }

        #region ICommand Interface

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }




        public bool CanExecute(object parameter)
        {
            if (_oneCommandAtATime && IsExecuting)
            {
                return false;
            }
            else
            {
                return _canExecute == null || _canExecute(parameter);
            }
        }

        public async void Execute(object parameter)
        {

            if (_oneCommandAtATime)
            {
                IsExecuting = true;
            }

            await ExecuteAsync(parameter);

            if (_oneCommandAtATime)
            {
                IsExecuting = false;
            }

        }

        #endregion

    }

}
