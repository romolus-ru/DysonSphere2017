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
		/// <summary>
		/// Текущие заказы
		/// </summary>
		private List<Order> _orders = new List<Order>();
		/// <summary>
		/// Заготовки заказов. считываются из файла
		/// </summary>
		private List<Order> _ordersBlanks = new List<Order>();

		public Dictionary<Planet, OrderViewInfo> OrdersViewInfo = new Dictionary<Planet, OrderViewInfo>();

		public int MaxOrders = 3;
		public Orders()
		{
			InitBlankOrders();
		}

		public IEnumerator<Order> GetEnumerator()
		{
			return _orders.GetEnumerator();
		}

		public Order this[int i] {
			get { return _orders[i]; }
		}
		
		/// <summary>
		/// Получить заказ
		/// </summary>
		/// <param name="orderLevel">Максимальный уровень заказа</param>
		/// <param name="hardness">Расчитываемая сложность заказа</param>
		/// <returns></returns>
		public Order GetRandomOrder(int orderLevel, int hardness)
		{
			var ordersLevel = _ordersBlanks.Where(o => o.Level <= orderLevel).ToList();
			var orderNum = RandomHelper.Random(ordersLevel.Count);
			var order = Order.Create(ordersLevel[orderNum], hardness);

			return order;
		}

		private void InitBlankOrders()
		{
			Order order;

			order = new Order();
			order.AmountResources = new Resources();
			order.AmountResources.Add(ResourcesEnum.RawMaterials, 2000);
			order.RewardRace = 3;
			order.Reward = 100;
			_ordersBlanks.Add(order);

			order = new Order();
			order.AmountResources = new Resources();
			order.AmountResources.Add(ResourcesEnum.Consumables, 500);
			order.RewardRace = 3;
			order.Reward = 100;
			_ordersBlanks.Add(order);

			order = new Order();
			order.AmountResources = new Resources();
			order.AmountResources.Add(ResourcesEnum.Tools, 100);
			order.RewardRace = 3;
			order.Reward = 100;
			_ordersBlanks.Add(order);
		}
	}
}
