using System.Drawing;
using Engine.Visualization;
using Submarines.Submarines;

namespace Submarines
{
	public class ViewShip:ViewComponent
	{
		private Submarine _submarine;

		internal void SetShip(Submarine submarine)
		{
			_submarine = submarine;
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			if (_submarine==null)
				return;

			visualizationProvider.SetColor(Color.LightSalmon);
			visualizationProvider.Rectangle(500,500, 20,20);

			visualizationProvider.Line(450, 650, 450, (int)(650 - _submarine.EnginePercent));

			visualizationProvider.Print(550, 515, "VMax=" + _submarine.VMax);

		}
	}
}