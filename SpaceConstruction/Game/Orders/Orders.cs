using Engine.Extensions;
using Engine.Helpers;
using SpaceConstruction.Game.Resources;
using System.Collections.Generic;
using System.Linq;

namespace SpaceConstruction.Game.Orders
{
	/// <summary>
	/// Заказы
	/// </summary>
	public class Orders
	{
		private const int INITORDERSCOUNT = 3;
		private List<Order> _actualOrders;
		/// <summary>
		/// Описания заказов
		/// </summary>
		public readonly List<OrderInfo> OrderInfos;
		/// <summary>
		/// исходная информация о ресурсах
		/// </summary>
		public List<ResourceInfo> ResourceInfos { get; private set; } = new List<ResourceInfo>();
		public int ActualOrdersCount { get { return _actualOrders.Count; } }
		public int MaxOrders { get; private set; }

		public Orders()
		{
			MaxOrders = INITORDERSCOUNT;
			OrderInfos = new List<OrderInfo>();
			_actualOrders = new List<Order>();
			InitOrderInfos();
			InitResourceInfos();
		}

		public Order GetNewOrder(int level)
		{
			var orderInfos = OrderInfos.Where(oi => oi.Level <= level).ToList();
			var num = RandomHelper.Random(orderInfos.Count);
			var orderInfo = orderInfos[num];
			var order = new Order(orderInfo);
			FillOrderResources(level, order, orderInfo, ResourceInfos);
			_actualOrders.Add(order);
			return order;
		}

		/// <summary>
		/// Удалить все имеющиеся заказы
		/// </summary>
		public void Clear()
		{
			_actualOrders.Clear();
		}

		/// <summary>
		/// Формируем сколько ресурсов в заказе будет
		/// </summary>
		private void FillOrderResources(int level, Order order, OrderInfo orderInfo, List<ResourceInfo> resourceInfos)
		{
			foreach (var resGroup in orderInfo.ResourceGroupValues) {
				var resGroupInfo = GetResourceGroupInfo(resourceInfos, resGroup, level);
				FillOrderResources(level, order, orderInfo, resourceInfos, resGroupInfo, resGroup.Value);
			}
		}

		private void FillOrderResources(int level, Order order, OrderInfo orderInfo, List<ResourceInfo> resourceInfos,
			List<ResourceInfo> resGroupInfo, int resGroupValue)
		{
			var amount = new ResourcesHolder(resourceInfos);
			int fill = 0;
			var need = 0;
			foreach (var res in resGroupInfo) {
				need = resGroupValue - fill;// сколько осталось
				var rvalue = RandomHelper.Random(need);
				int vvalue = (int)(rvalue / res.VolumeCoefficient);
				if (vvalue > 0) {
					amount.Add(res.ResourceType, vvalue);
					fill += vvalue;// сохраняем ресурс
				}
			}
			need = resGroupValue - fill;
			if (resGroupValue - fill > 0) {// добавляем оставшееся из первого ресурса
				var res = resGroupInfo[0];
				int vvalue = (int)(need / res.VolumeCoefficient);
				if (vvalue > 0)
					amount.Add(res.ResourceType, vvalue);
			}
			order.AmountResources = amount;
			order.AmountResourcesDelivered = new ResourcesHolder(resourceInfos);
			order.AmountResourcesInProgress = new ResourcesHolder(resourceInfos);
			order.InitProgress();
		}

		/// <summary>
		/// Получаем список ресурсов, входящих в группу ресурсов, вперемешку
		/// </summary>
		private List<ResourceInfo> GetResourceGroupInfo(List<ResourceInfo> resourceInfos, ResourceGroupValue resGroupValue, int level)
		{
			var count = 3;// сколько ресурсов будет в заказе для этой группы ресурсов
			if (level > 1) count = 500;
			var resGroupInfo = resourceInfos.Where(ri => ri.ResourceGroup == resGroupValue.Group)
				.Take(count).ToList();
			resGroupInfo.Shuffle();
			return resGroupInfo;
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

			OrderInfos.Add(order);
		}

		private void InitResourceInfos()
		{
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesEnum.StrWood, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesEnum.StrStone, "", "Камень", "Камень", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesEnum.StrConcrete, "", "Цемент", "Цемент", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesEnum.StrBrick, "", "Кирпич", "Киприч", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesEnum.StrSteel, "", "Железо", "Железо", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecTile, "", "Плитка", "Плитка", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecPaper, "", "Обои", "Обои", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecPlaster, "", "Штукатурка", "Штукатурка", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecPutty, "", "Шпаклевка", "Шпаклевка", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecGlass, "", "Стекло", "Стекло", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecPaint, "", "Краска", "Краска", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecLinoleum, "", "Линолеум", "Линолеум", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecPorcelain, "", "Фарфор", "Фарфор", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Furniture, ResourcesEnum.FurFurniture, "", "Мебель", "Мебель", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Furniture, ResourcesEnum.FurPlumbing, "", "Сантехника", "Сантехника", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Furniture, ResourcesEnum.FurDishes, "", "Посуда", "Посуда", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Furniture, ResourcesEnum.FurKitchen, "", "Кухня", "Куня", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesEnum.MechShovels, "", "Лопаты", "Лопаты", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesEnum.MechFinishingTools, "", "Инструменты для отделки", "Инструменты для отделки", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesEnum.MechExcavator, "", "Эскаватор", "Эскаватор", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesEnum.MechCrane, "", "Кран", "Кран", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesEnum.MechPileDriving, "", "Сваезабивалка", "Сваезабивалка", 1, 1);
		}

		private void AddOrderInfo(ResourcesGroupEnum resourceGroup, ResourcesEnum resourceType, string texture,
			string name, string description, float volumeCoefficient, float dencityCoefficient)
		{
			var resInfo = new ResourceInfo()
			{
				ResourceGroup = resourceGroup,
				ResourceType = resourceType,
				Texture = texture,
				Name = name,
				Description = description,
				VolumeCoefficient = volumeCoefficient,
				DencityCoefficient = dencityCoefficient
			};

			ResourceInfos.Add(resInfo);
		}
	}
}