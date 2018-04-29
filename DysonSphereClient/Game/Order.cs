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

		public Order() { }
		/// <summary>
		/// Создаём копию заказа что бы оригинальный заказ не трогать
		/// </summary>
		/// <param name="copyOrder"></param>
		public Order(Order copyOrder)
		{
			Reward = copyOrder.Reward;
			RewardRace = copyOrder.RewardRace;
			AmountResources = copyOrder.AmountResources.GetCopy();
		}

		/// <summary>
		/// Создать копию заказа
		/// </summary>
		/// <param name="copyOrder">Исходный заказ</param>
		/// <param name="hardness">Добавляемая сложность</param>
		public Order(Order copyOrder, int hardness):this(copyOrder)
		{
			if (hardness <= 1) return;
			var multiplier = RandomHelper.Random(hardness) / hardness;
			AmountResources.Increase(multiplier);
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

		internal int GetRewarForRace()
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