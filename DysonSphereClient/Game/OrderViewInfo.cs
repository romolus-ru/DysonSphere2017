using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Информация о заказе для вывода на экран. формируется и хранится в Orders
	/// </summary>
	public class OrderViewInfo
	{
		/// <summary>
		/// Планета нахождения заказа
		/// </summary>
		public Planet TargetPlanet;
		/// <summary>
		/// Количество требуемых ресурсов
		/// </summary>
		public Resources Amount;
		public List<ResourceViewInfo> Resources = new List<ResourceViewInfo>();
	}
}