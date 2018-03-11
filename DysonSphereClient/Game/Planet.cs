using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	public class Planet : ScreenPoint
	{
		/// <summary>
		/// Для открытия планеты за деньги
		/// </summary>
		public bool IsLocked = false;
		/// <summary>
		/// Имеющиеся на планете ресурсы
		/// </summary>
		public Resources Resources;
		/// <summary>
		/// Ресурсы, необходимые для строительства
		/// </summary>
		public Resources ToBuild = null;

		/// <summary>
		/// Награда за перевозку груза
		/// </summary>
		public Resources Reward;

		public Planet(Resources resources, Resources reward)
		{
			Reward = reward;
			Resources = resources;
		}
	}
}