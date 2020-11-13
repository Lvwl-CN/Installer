using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Shared
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Func<object, bool> Can { get; set; }
        public Action<object> Do { get; set; }
        public DelegateCommand(Action<object> action, Func<object, bool> func = null)
        {
            this.Do = action;
            this.Can = func;
        }

        public bool CanExecute(object parameter)
        {
            return Can == null ? true : Can.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            Do?.Invoke(parameter);
        }

        public void CanChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
