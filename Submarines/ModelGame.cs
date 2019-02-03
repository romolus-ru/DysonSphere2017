using Engine.Models;
using Submarines.Submarines;

namespace Submarines
{
	internal class ModelGame : Model
	{
		private Submarine _submarine;
		private ShipController _shipController;

		public ModelGame(Submarine submarine, ShipController shipController)
		{
			_submarine = submarine;
			_shipController = shipController;
		}
		public override void Tick()
		{
			_submarine.CalculateMovement();
		}

	}
}