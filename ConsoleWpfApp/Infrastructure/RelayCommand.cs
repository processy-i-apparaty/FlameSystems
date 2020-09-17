using System;
using System.Windows.Input;

namespace ConsoleWpfApp.Infrastructure
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> _action;
        private readonly Predicate<object> _predicate;

        public RelayCommand(Action<object> action, Predicate<object> predicate = null)
        {
            _action = action;
            _predicate = predicate;
        }


        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return _predicate?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}