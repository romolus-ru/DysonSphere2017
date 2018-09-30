using DysonSphereClient.Game.ResourcesNew;
using System.Collections.Generic;

namespace DysonSphereClient.Game.OrdersNew
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