using Submarines.Submarines;

namespace Submarines
{
	/// <summary>
	/// Для управления кораблем
	/// </summary>
	internal class ShipController
	{
		private Submarine _submarine;

		public ShipController(Submarine submarine)
		{
			_submarine = submarine;
		}

		public void SpeedUp(int delta)
		{
			_submarine.AddSpeed(delta);
		}

		public void StopEngine()
		{
			_submarine.StopEngine();
		}

		public void Steering(int delta)
		{
			_submarine.AddSteering(delta);
		}
		
	}
}
