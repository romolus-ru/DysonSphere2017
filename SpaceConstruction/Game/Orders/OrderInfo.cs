using SpaceConstruction.Game.Resources;
using System.Collections.Generic;

namespace SpaceConstruction.Game.Orders
{
	/// <summary>
	/// Описание заказа
	/// </summary>
	public class OrderInfo
	{
		public string Name;
		public string Description;
		public int Level;
		public List<ResourceGroupValue> ResourceGroupValues;
		public string OrderLogo;
	}
}