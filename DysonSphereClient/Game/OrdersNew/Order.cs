using DysonSphereClient.Game.ResourcesNew;
using Engine.Helpers;
using System;
using System.Collections.Generic;

namespace DysonSphereClient.Game.OrdersNew
{
	public class Order
	{
		/// <summary>
		/// Информация о заказе
		/// </summary>
		public OrderInfo OrderInfo;
		/// <summary>
		/// Требуемое количесто ресурсов
		/// </summary>
		public ResourcesHolder AmountResources;
		/// <summary>
		/// Уровень заказа, для различных стадий игры
		/// </summary>
		public int Level;
		public string OrderName;
		public string OrderDescription;
		/// <summary>
		/// Планета которая сделала заказ
		/// </summary>
		public Planet Destination;
		/// <summary>
		/// Планета откуда надо взять ресурсы
		/// </summary>
		public Planet Source;

		public Order() { }

		/// <summary>
		/// Создаём копию заказа что бы оригинальный заказ не трогать
		/// </summary>
		/// <param name="orderInfo"></param>
		private Order Create(OrderInfo orderInfo, int hardness)
		{
			var order = new Order();
			//order.AmountResources = copyOrder.AmountResources.GetCopy();
			order.OrderName = orderInfo.Name;
			order.OrderDescription = orderInfo.Description;
			if (hardness > 1) {
				var multiplier = RandomHelper.Random(hardness) / hardness;
				order.AmountResources.Increase(multiplier);
			}
			return order;
		}

		public List<string> GetInfo()
		{
			var ret = new List<string>();
			ret.Add("Требуется перевезти");
			ret.Add(AmountResources.GetInfo());
			return ret;
		}
	}
}