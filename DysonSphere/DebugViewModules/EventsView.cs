using Engine;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphere.DebugViewModules
{
	public class EventsView : ViewWindow
	{
		private List<string> _list = new List<string>();
		private int pause = 0;

		private void RefreshList()
		{
			pause--;
			if (pause > 0) return;
			pause = 10;
			_list = Input.GetActionsLists();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			RefreshList();
			var y = 0;
			visualizationProvider.SetColor(Color.Coral);
			foreach (var item in _list) {
				visualizationProvider.Print(X + 10, Y + y, item);
				y += 14;
			}
		}
	}
}
