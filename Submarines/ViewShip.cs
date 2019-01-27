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

			visualizationProvider.Print(550, 500, "z  = " + _ship.CurrentVector.Z);
			visualizationProvider.Print(550, 515, "VMax=" + _ship.VMax);

			visualizationProvider.Line((int) (1000 + _ship.CurrentVector.Y * 2), 650, 1000, 650);

			visualizationProvider.Line(1000, (int) (650 - _ship.CurrentVector.Z * 2), 1000, 650);
		}
	}
}