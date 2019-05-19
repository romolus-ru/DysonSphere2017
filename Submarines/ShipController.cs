using System;
using Submarines.Submarines;

namespace Submarines
{
	/// <summary>
	/// Для управления кораблем
	/// </summary>
	internal class ShipController
	{
		private Submarine _submarine;
		public Action<SubmarineBase, Weapon, float, float> OnFire;

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
		/// Игрок стреляет по координатам
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Fire(float x, float y)
		{
			OnFire?.Invoke(_submarine, _submarine.Weapon, x, y);
		}
	}
}
