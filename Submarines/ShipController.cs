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

		/// <summary>
		/// Игрок стреляет по координатам (возможно добавятся группы выбранных орудий) (стрельба по готовности - команда ожидает пока оружие зарядится)
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Fire(float x, float y)
		{
			_submarine.ShootToCoordinates(x, y);
		}
	}
}
