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
		public Order(OrderInfo orderInfo, int hardness = 1)
		{
			//order.AmountResources = copyOrder.AmountResources.GetCopy();
			OrderName = orderInfo.Name;
			OrderDescription = orderInfo.Description;
			// генерируем сколько  нужно для заказа

			if (hardness > 1) {
				var multiplier = RandomHelper.Random(hardness) / hardness;
				AmountResources.Increase(multiplier);
			}
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