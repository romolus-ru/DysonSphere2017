using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	public class Ship
	{
		private Resources _cargo = new Resources();
		private Resources _cargoMax = null;

		public Ship(Resources cargoMax)
		{
			_cargoMax = cargoMax;
		}
	}
}
