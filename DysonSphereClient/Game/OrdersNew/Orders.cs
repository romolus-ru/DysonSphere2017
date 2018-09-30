using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game.OrdersNew
{
	/// <summary>
	/// Заказы
	/// </summary>
	public class Orders
	{
		/// <summary>
		/// Описания заказов
		/// </summary>
		private List<OrderInfo> _orderInfos = new List<OrderInfo>();

		private void InitOrderInfos()
		{
			var order = new OrderInfo()
			{
				Name = "Домик",
				Description = "Строительство маленького домика",
				Level = 0,
				OrderLogo = "",
				ResourceGroupValues = new List<ResourcesNew.ResourceGroupValue>()
				{
					тут
				}
			};

		}
	}
}