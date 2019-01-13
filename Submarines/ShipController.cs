using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarines
{
	/// <summary>
	/// Для управления кораблем
	/// </summary>
	internal class ShipController
	{
		private Ship _ship;

		public ShipController(Ship ship)
		{
			_ship = ship;
		}

		public void SpeedUp(int delta)
		{
			_ship.AddSpeed(delta);
		}

		public void StopEngine()
		{
			_ship.StopEngine();
		}
	}
}
