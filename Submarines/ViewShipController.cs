using System.Windows.Forms;
using Engine;
using Engine.Visualization;

namespace Submarines
{
	/// <summary>
	/// Получение команд с клавиатуры и передача их кораблю
	/// </summary>
	internal class ViewShipController:ViewComponent
	{
		private ShipController _shipController;

		public ViewShipController(ShipController shipController)
		{
			_shipController = shipController;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewShipController");

			input.AddKeyAction(SpeedUp, Keys.W);
			input.AddKeyAction(SpeedDown, Keys.S);
			input.AddKeyAction(SteeringLeft, Keys.A);
			input.AddKeyAction(SteeringRight, Keys.D);
			input.AddKeyAction(StopEngine, Keys.Back);
		}

		private void SteeringLeft()
		{
			_shipController.Steering(-5);
		}

		private void SteeringRight()
		{
			_shipController.Steering(5);
		}

		private void SpeedUp()
		{
			_shipController.SpeedUp(10);
		}

		private void SpeedDown()
		{
			_shipController.SpeedUp(-10);
		}

		private void StopEngine()
		{
			_shipController.StopEngine();
		}
	}
}