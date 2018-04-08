using Engine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Заказы
	/// </summary>
	public class Orders
	{
		private List<Order> _orders = new List<Order>();
		public Orders()
		{
			var order = new Order();
			order.AmountResources = new Resources();
			order.AmountResources.Add(ResourcesEnum.Materials, 2000);
			order.RewardRace = 3;
			order.Reward = 100;
			_orders.Add(order);

			order = new Order();
			order.AmountResources = new Resources();
			order.AmountResources.Add(ResourcesEnum.Tools, 500);
			order.RewardRace = 3;
			order.Reward = 100;
			_orders.Add(order);

			order = new Order();
			order.AmountResources = new Resources();
			order.AmountResources.Add(ResourcesEnum.Personal, 100);
			order.RewardRace = 3;
			order.Reward = 100;
			_orders.Add(order);
		}

		/// <summary>
		/// Получить заказ
		/// </summary>
		/// <param name="orderLevel">Максимальный уровень заказа</param>
		/// <param name="hardness">Расчитываемая сложность заказа</param>
		/// <returns></returns>
		public Order GetRandomOrder(int orderLevel, int hardness)
		{
			var ordersLevel = _orders.Where(o => o.Level <= orderLevel);
			var orderNum = RandomHelper.Random(_orders.Count);
			return new Order(_orders[orderNum], hardness);
		}
	}
}
