using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConstruction.Game
{
	public class Planet : ScreenPoint
	{
		/// <summary>
		/// Заказ на перевозку ресурсов
		/// </summary>
		public Order Order = null;
		/// <summary>
		/// Ископаемые ресурсы
		/// </summary>
		public ResourcesHolder Source = null;
		public Planet()
		{

		}

		public override string ToString()
		{
			return base.ToString() +
				" o=" + (Order != null);
		}
	}
}