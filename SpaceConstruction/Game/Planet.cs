using Engine.Visualization;
using SpaceConstruction.Game.Orders;

namespace SpaceConstruction.Game
{
	public class Planet : ScreenPoint
	{
		public bool IsDepot = false;
		/// <summary>
		/// Заказ на перевозку ресурсов
		/// </summary>
		public Order Order = null;
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