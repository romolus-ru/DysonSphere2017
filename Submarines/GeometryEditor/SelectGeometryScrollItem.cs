using System;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using Submarines.Geometry;

namespace Submarines.GeometryEditor
{
	internal class SelectGeometryScrollItem : ScrollItem
	{
		private GeometryBase _geometry;
		private ViewButton _btnSelect;
		public Action<GeometryBase> OnSelect;

		public SelectGeometryScrollItem(GeometryBase geometry)
		{
			_geometry = geometry;
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
			OnSelect?.Invoke(_geometry);
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.Azure);
			visualizationProvider.Print(X + 250, Y, "тип " + _geometry.GeometryType);
			visualizationProvider.Print(X + 250, Y + 10, _geometry.Color.ToString());
			visualizationProvider.Print(X + 250, Y + 20, _geometry.Name);
			visualizationProvider.Print(X + 250, Y + 30, "lines count = " + _geometry.Lines.Count);
		}
	}
}