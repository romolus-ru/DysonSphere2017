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
			visualizationProvider.Print(550, 525, "curAng=" + _submarine.CurrentAngle);
			visualizationProvider.Print(550, 535, "strAng=" + _submarine.SteeringAngle);
			visualizationProvider.Print(550, 545, "svx=" + _submarine.SpeedVector.X);
			visualizationProvider.Print(550, 555, "svy=" + _submarine.SpeedVector.Y);
			visualizationProvider.Print(550, 565, "px=" + _submarine.Position.X);
			visualizationProvider.Print(550, 575, "py=" + _submarine.Position.Y);

			visualizationProvider.Rotate((int) _submarine.CurrentAngle + 90);
			visualizationProvider.OffsetAdd(700 + (int) _submarine.Position.X, 500 + (int) _submarine.Position.Y);
			visualizationProvider.DrawTexture(0, 0, "Submarines01map.pl01");
			visualizationProvider.RotateReset();

			if (_submarine.Geometry != null) {
				visualizationProvider.SetColor(_submarine.Geometry.Color);
				foreach (var line in _submarine.GeometryRotatedLines) {
					DrawLine(visualizationProvider, line.From, line.To);
				}
			}

			visualizationProvider.OffsetRemove();

		}

		private void DrawLine(VisualizationProvider visualizationProvider, Vector from, Vector to)
		{
			visualizationProvider.Line((int)from.X, (int)from.Y, (int)to.X, (int)to.Y);
		}
	}
}