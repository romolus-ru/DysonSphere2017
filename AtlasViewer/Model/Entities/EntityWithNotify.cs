using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasViewer.Model.Entities
{
	/// <summary>
	/// Для обеспечения передачи сигналов обновления
	/// </summary>
	public class EntityWithNotify : INotifyPropertyChanged
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
