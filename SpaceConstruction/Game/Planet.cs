using Engine.Visualization;
using SpaceConstruction.Game.Orders;

namespace SpaceConstruction.Game
{
	public class Planet : ScreenPoint
	{
		public bool IsDepot;

		/// <summary>
		/// Заказ на перевозку ресурсов
		/// </summary>
		public Order Order;

		public override string ToString()
		{
			return base.ToString()
				+ " o=" + (Order != null);
		}
	}
}