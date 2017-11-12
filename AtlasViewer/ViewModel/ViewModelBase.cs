using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasViewer.ViewModel
{
	/// <summary>
	/// Унифицикация. INotifyPropertyChanged
	/// </summary>
	public class ViewModelBase : INotifyPropertyChanged
	{
		/// <summary>
		///  PropertyChangedEventHandler
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		public virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
