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
		/// Здание на планете. Производит или потребляет ресурсы
		/// </summary>
		public Building Building;
		/// <summary>
		/// Заказ на перевозку ресурсов
		/// </summary>
		public Order Order = null;
		/// <summary>
		/// Ископаемые ресурсы
		/// </summary>
		public Resources Source = null;
		public Planet()
		{
			
		}

		public override string ToString()
		{
			return base.ToString() + 
				" o=" + (Order != null) + 
				" b=" + (Building == null ? "null" : Building.BuilingType.ToString());
		}
	}
}