using DysonSphereClient.Game.ResourcesNew;
using System.Collections.Generic;

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

		public Order GetOrder()
		{
			return null;
		}

		private void InitOrderInfos()
		{
			AddOrderInfo("Домик", "Строительство маленького домика", 0, "",
				200, 0, 0, 0);

			AddOrderInfo("Фазенда", "Строительство фазенды", 0, "",
				 400, 0, 0, 0);

			AddOrderInfo("Магазин", "Строительство магазина", 0, "",
				 500, 150, 100, 0);

			AddOrderInfo("Торговый центр", "Строительство торгового центра", 0, "",
				 800, 200, 300, 50);

			AddOrderInfo("Товары", "Перевозка товаров для магазина", 0, "",
				 0, 100, 100, 100);

			AddOrderInfo("Товары", "Перевозка товаров для магазина", 0, "",
				 0, 100, 100, 100);

			AddOrderInfo("Инструменты", "Перевозка инструментов для магазина", 0, "",
				0, 0, 0, 1000);

			AddOrderInfo("Парк", "Разбить большой парк", 0, "",
				400, 200, 200, 100);

		}

		private void AddOrderInfo(string name, string description, int level, string logo, int strMat, int decMat, int frnMat, int mchMat)
		{
			var order = new OrderInfo()
			{
				Name = name,
				Description = description,
				Level = level,
				OrderLogo = logo,
				ResourceGroupValues = new List<ResourceGroupValue>()
			};

			if (strMat > 0) order.ResourceGroupValues.Add(new ResourceGroupValue(ResourcesGroupEnum.StructuralMaterial, strMat));
			if (frnMat > 0) order.ResourceGroupValues.Add(new ResourceGroupValue(ResourcesGroupEnum.Furniture, frnMat));
			if (decMat > 0) order.ResourceGroupValues.Add(new ResourceGroupValue(ResourcesGroupEnum.DecorationMaterial, decMat));
			if (mchMat > 0) order.ResourceGroupValues.Add(new ResourceGroupValue(ResourcesGroupEnum.Mechs, mchMat));

			_orderInfos.Add(order);
		}
	}
}