using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphere.DebugViewModules
{
	class KeysView : ViewWindow
	{
		private string _keys = null;
		private int pause = 0;

		private void RefreshList()
		{
			pause--;
			if (pause > 0) return;
			pause = 10;
			_keys = Input.GetCurrentKeysPressed();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			RefreshList();
			visualizationProvider.SetColor(Color.Coral);
			visualizationProvider.Print(X + 10, Y + 10, _keys);
		}
	}
}
