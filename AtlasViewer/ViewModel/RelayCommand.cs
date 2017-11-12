using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AtlasViewer.ViewModel
{
	public class RelayCommand : ICommand
	{
		public Predicate<object> CanExecuteDelegate { get; set; }
		public Action<object> ExecuteDelegate { get; set; }

		public RelayCommand(Action<object> action)
		{
			ExecuteDelegate = action;
		}

		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
		public bool CanExecute(object parameter)
		{
			if (CanExecuteDelegate != null) {
				return CanExecuteDelegate(parameter);
			}
			return true;
		}

		public void Execute(object parameter)
		{
			ExecuteDelegate?.Invoke(parameter);
		}
	}
}
