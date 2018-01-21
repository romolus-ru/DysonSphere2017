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
	public class ComponentsView:ViewWindow
	{
		private List<string> _list = new List<string>();
		private ViewComponent _root = null;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			_root = GetRoot(Parent);
		}

		private ViewComponent GetRoot(ViewComponent parent)
		{
			if (parent != null) {
				if (parent.Parent != null) return GetRoot(parent.Parent);
				return parent;
			}
			return null;
		}

		private void RefreshList()
		{
			if (_root != null)
				_list = _root.GetObjectsView();
		}
		
		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			/*RefreshList();
			var y = 0;
			visualizationProvider.SetColor(Color.Coral);
			foreach (var item in _list) {
				visualizationProvider.Print(10, y, item);
				y += 14;
			}
			*/
		}
	}
}
