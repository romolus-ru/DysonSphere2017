using Engine.Helpers;
using SpaceConstruction.Game.Resources;
using System.Collections.Generic;

namespace SpaceConstruction.Game.Orders
{
	public class Order
	{
		/// <summary>
		/// Информация о заказе
		/// </summary>
		public OrderInfo OrderInfo;
		/// <summary>
		/// Оставшееся количество ресурсов для перевозки
		/// </summary>
		public ResourcesHolder AmountResources;
		/// <summary>
		/// Перевозимое в текущий момент количество ресурсов (что бы лишнего не навозить)
		/// </summary>
		public ResourcesHolder AmountResourcesInProgress;
		/// <summary>
		/// Уже перевезенное количество ресурсов (чтоб показывать прогресс перевозки)
		/// </summary>
		public ResourcesHolder AmountResourcesDelivered;
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