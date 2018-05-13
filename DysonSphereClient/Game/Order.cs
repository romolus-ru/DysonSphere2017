using Engine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Заказ на перевозку ресурсов
	/// </summary>
	public class Order
	{
		/// <summary>
		/// Требуемое количесто ресурсов
		/// </summary>
		public Resources AmountResources;
		/// <summary>
		/// Награда за перевозку всех ресурсов
		/// </summary>
		public int Reward;
		/// <summary>
		/// Награда за 1 рейс
		/// </summary>
		public int RewardRace;
		/// <summary>
		/// Уровень заказа, для различных стадий игры
		/// </summary>
		public int Level;
		/// <summary>
		/// Уровень сложности заказа. обычный/высокий/важный/военный
		/// </summary>
		/// <remarks>Влияет на награду. в начале игры все заказы обычные, более высокий уровень или сюжетный или связан с каким-либо событием)
		/// с покупкой товаров/ получением ачивок и т.п. - сложность растёт вместе с наградой</remarks>
		public int Hardness;
		public string OrderShortName;
		public string OrderDescription;
		/// <summary>
		/// Планета которая сделала заказ
		/// </summary>
		public Planet Destination;

		public Order() { }

		/// <summary>
		/// Создаём копию заказа что бы оригинальный заказ не трогать
		/// </summary>
		/// <param name="copyOrder"></param>
		public static Order Create(Order copyOrder, int hardness)
		{
			var order = new Order();
			order.Reward = copyOrder.Reward;
			order.RewardRace = copyOrder.RewardRace;
			order.AmountResources = copyOrder.AmountResources.GetCopy();
			order.OrderShortName = copyOrder.OrderShortName;
			order.OrderDescription = copyOrder.OrderDescription;
			if (hardness > 1) {
				var multiplier = RandomHelper.Random(hardness) / hardness;
				order.AmountResources.Increase(multiplier);
			}
			return order;
		}
		
		public List<string> GetInfo()
		{
			var ret = new List<string>();
			ret.Add("+" + Reward + " (+" + RewardRace + " за рейс)");
			ret.Add("Требуется перевезти");
			ret.Add(AmountResources.GetInfo());
			return ret;
		}

		/// <summary>
		/// Увеличить заказ
		/// </summary>
		/// <param name="order"></param>
		internal void AddOrder(Order order)
		{
			Reward += order.Reward;
			RewardRace = Math.Min(RewardRace, order.Reward);
			Level = Math.Max(Level, order.Level);
			AmountResources.Add(order.AmountResources);
		}

		internal int GetRewardForRace()
		{
			if (Reward <= 0) return 0;
			var ret = RewardRace;
			Reward -= ret;
			if (Reward < 0) {
				ret += Reward;
				Reward = 0;
			}
			return ret;
		}
	}
}