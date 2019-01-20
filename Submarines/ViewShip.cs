using System.Drawing;
using Engine.Visualization;

namespace Submarines
{
	public class ViewShip:ViewComponent
	{
		private Ship _ship;

		internal void SetShip(Ship ship)
		{
			_ship = ship;
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			if (_ship==null)
				return;

			visualizationProvider.SetColor(Color.LightSalmon);
			visualizationProvider.Rectangle(500,500, 20,20);

			visualizationProvider.Line(450, 650, 450, 650 - _ship.EnginePercent);



			visualizationProvider.Line((int) (1000 + _ship.CurrentVector.Y * 400), 650, 1000, 650);

			visualizationProvider.Line(1000, (int) (650 + _ship.CurrentVector.Z * 400), 1000, 650);
		}
	}
}