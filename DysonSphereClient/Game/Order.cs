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
		public Resources Value;
		/// <summary>
		/// Награда за перевозку всех ресурсов
		/// </summary>
		public int Reward;
		/// <summary>
		/// Награда за 1 рейс
		/// </summary>
		public int RewardRace;

		public List<string> GetInfo()
		{
			var ret = new List<string>();
			ret.Add("+" + Reward + " (+" + RewardRace + " за рейс)");
			ret.Add("Требуется перевезти");
			ret.Add(Value.GetInfo());
			return ret;
		}
	}
}