using System;
using System.Windows.Input;

namespace FunctionFilters.Helpers
{
	public class RelayCommand<T> : ICommand
	{
		private readonly Action<T> _action;

		public RelayCommand(Action<T> action)
		{
			_action = action;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			_action((T)parameter);
		}
	}
}