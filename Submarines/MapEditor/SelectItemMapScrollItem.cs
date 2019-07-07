using Engine;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using System;
using System.Drawing;
using System.Windows.Forms;
using Submarines.Items;

namespace Submarines.MapEditor
{
	internal class SelectItemMapScrollItem : ScrollItem
	{
		private ItemMap _map;
		private ViewButton _btnSelect;
		public Action<ItemMap> OnSelect;

		public SelectItemMapScrollItem(ItemMap map)
		{
			_map = map;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			_btnSelect = new ViewButton();
			AddComponent(_btnSelect);
			_btnSelect.InitButton(Select, "Выбрать", "Выбрать", Keys.None);
			_btnSelect.SetParams(10, 10, 200, 40, "Выбрать");
		}

		private void Select()
		{
			OnSelect?.Invoke(_map);
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.Azure);
			visualizationProvider.Print(X + 250, Y, "тип " + _map.MapCode+ " "+_map.MapName);
			visualizationProvider.Print(X + 250, Y + 10, _map.MapDescription);
			visualizationProvider.Print(X + 250, Y + 20, "geometry name =  " + _map.MapGeometryName);
			visualizationProvider.Print(X + 250, Y + 30, "spawn names count " + _map.MapSpawnsNames.Count.ToString());
		}
	}
}