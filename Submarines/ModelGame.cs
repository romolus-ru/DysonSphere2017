using Engine.Models;

namespace Submarines
{
	internal class ModelGame : Model
	{
		private Ship _ship;
		private ShipController _shipController;

		public ModelGame(Ship ship, ShipController shipController)
		{
			_ship = ship;
			_shipController = shipController;
		}
		public override void Tick()
		{
		}

	}
}