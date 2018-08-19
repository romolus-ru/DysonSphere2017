using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AtlasViewer.ViewModel
{
	public class ViewModelSplitCount : ViewModelBase
	{
		public string DialogResult = "None";
		private Dictionary<string, int> _editingValues;

		/// <summary>
		/// Для закрытия вспомогательных окон
		/// </summary>
		public event EventHandler RequestClose;

		///// <summary>
		///// Для обновления вспомогательных окон
		///// </summary>
		//public event EventHandler RequestRefreshWindow;

		public string ByWidth { get; set; }
		public string ByHeight { get; set; }

		public ICommand StoreChangesCommand { get; set; }
		public ViewModelSplitCount()
		{
		}

		public ViewModelSplitCount(Dictionary<string, int> editingValues)
		{
			_editingValues = editingValues;
			ByWidth = editingValues["Width"].ToString();
			ByHeight = editingValues["Height"].ToString();
			StoreChangesCommand = new RelayCommand(arg => StoreChanges());
		}

		public void StoreChanges()
		{
			var changes =
				_editingValues["Width"].ToString() != ByWidth ||
				_editingValues["Height"].ToString() != ByHeight;
			if (changes) {
				int value;
				if (int.TryParse(ByWidth, out value))
					_editingValues["Width"] = value;
				if (int.TryParse(ByHeight, out value))
					_editingValues["Height"] = value;
				DialogResult = "Changed";
			}
			RequestClose(this, new EventArgs());
		}

	}
}
