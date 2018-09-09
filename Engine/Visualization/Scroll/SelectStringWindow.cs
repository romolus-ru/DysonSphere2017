using Engine.Data;
using System;
using System.Collections.Generic;

namespace Engine.Visualization.Scroll
{
	public class SelectStringWindow : FilteredScrollViewWindow
	{
		private Action<string> _selectedString;
		private Action _cancel;
		private ViewManager _viewManager;
		private MiniGames _miniGame;
		private List<string> _values;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
		}

		public void InitWindow(ViewManager viewManager, List<string> values, Action<string> selectedString, Action cancel)
		{
			_values = values;
			_selectedString = selectedString;
			_cancel = cancel;
			_viewManager = viewManager;

			InitWindow("Выбор строки", viewManager, showOkButton: false, showCancelButton: true, showNewButton: false);
		}

		protected override void InitScrollItems()
		{
			var i = 2;
			foreach (var str in _values) {
				var scrollItem = new SelectStringScrollItem(str);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(10, (i - 1) * 50 + 10, 950, 50, str);
				scrollItem.OnSelect += SelectSection;
				i++;
			}
		}

		private void SelectSection(string value)
		{
			_selectedString?.Invoke(value);
			CloseWindow();
		}

		protected override void CloseWindow()
		{
			base.CloseWindow();
			_selectedString = null;
			_cancel = null;
		}

		protected override void CancelCommand()
		{
			_cancel?.Invoke();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(System.Drawing.Color.Black, 50);
			visualizationProvider.Box(X, Y, Width, Height);
			base.DrawObject(visualizationProvider);
		}
	}
}