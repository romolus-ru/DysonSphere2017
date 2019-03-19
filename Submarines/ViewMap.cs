using Engine.Visualization;
using Submarines.Submarines;
using System.Drawing;
using Submarines.Maps;

namespace Submarines
{
	internal class ViewMap : ViewComponent
	{
		private Submarine _submarine;
		private MapBase _map;

		internal void SetMap(MapBase map)
		{
			_map = map;
			_submarine = (Submarine)_map.FocusedShip;
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			if (_submarine == null)
				return;

			visualizationProvider.SetColor(Color.LightSalmon);
			visualizationProvider.Rectangle(500, 500, 20, 20);

			visualizationProvider.OffsetAdd(700, 500);

			visualizationProvider.Rotate((int)_submarine.CurrentAngle + 90);
			visualizationProvider.DrawTexture(0, 0, "Submarines01map.pl01");
			visualizationProvider.RotateReset();

			if (_submarine.Geometry != null) {
				visualizationProvider.SetColor(_submarine.Geometry.Color);
				foreach (var line in _submarine.GeometryRotatedLines) {
					DrawLine(visualizationProvider, line.From, line.To);
				}
			}

			visualizationProvider.OffsetRemove();

			visualizationProvider.OffsetAdd(700 - (int)_submarine.Position.X, 500 - (int)_submarine.Position.Y);

			visualizationProvider.SetColor(_map.Geometry.Color);
			foreach (var line in _map.Geometry.Lines) {
				DrawLine(visualizationProvider, line.From, line.To);
			}

			visualizationProvider.OffsetRemove();

		}

		private void DrawLine(VisualizationProvider visualizationProvider, Vector from, Vector to)
		{
			visualizationProvider.Line((int)from.X, (int)from.Y, (int)to.X, (int)to.Y);
		}
	}
}