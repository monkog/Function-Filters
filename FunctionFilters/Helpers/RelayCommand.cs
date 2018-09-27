using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace FunctionFilters.Helpers
{
	public class RelayCommand : ICommand
	{
		private readonly Action _action;

		public RelayCommand(Action action)
		{
			_action = action;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		[ExcludeFromCodeCoverage]
		public event EventHandler CanExecuteChanged
		{
			add { }
			remove { }
		}

		public void Execute(object parameter)
		{
			_action();
		}
	}
}