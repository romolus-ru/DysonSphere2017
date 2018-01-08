using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using System.Drawing;

namespace DysonSphere
{
	public class DebugView:ViewComponent
	{
		private List<string> _list = new List<string>();
		private ViewComponent _root = null;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			//Input.AddInputStringAction(GetString);
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

		private void GetString(string pressed)
		{
			_list.Insert(0, pressed);
			const int maxCount = 30;
			if (_list.Count > maxCount) _list.RemoveAt(maxCount - 1);
		}

		private void RefreshList()
		{
			if (_root != null)
				_list = _root.GetObjectsView();
		}


		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			RefreshList();
			var y = 0;
			visualizationProvider.SetColor(Color.Coral);
			foreach (var item in _list) {
				visualizationProvider.Print(10, y, item);
				y += 16;
			}
		}
	}
}
