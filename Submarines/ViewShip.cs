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

			visualizationProvider.Line(450, 650, 450, 650 - _ship.EngineSpeed);
		}
	}
}