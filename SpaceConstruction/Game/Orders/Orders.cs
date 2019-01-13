using Engine;
using Engine.Extensions;
using Engine.Helpers;
using SpaceConstruction.Game.Items;
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
		private const int InitOrdersCount = 7;
		private List<Order> _actualOrders;
		/// <summary>
		/// Описания заказов
		/// </summary>
		public List<OrderInfo> OrderInfos;

		private OrderInfo _finalOrder;

		/// <summary>
		/// исходная информация о ресурсах
		/// </summary>
		public List<ResourceInfo> ResourceInfos { get; private set; } = new List<ResourceInfo>();
		public int ActualOrdersCount => _actualOrders.Count;
		public int MaxOrders { get; private set; }

		public Orders()
		{
			MaxOrders = InitOrdersCount;
			OrderInfos = new List<OrderInfo>();
			_actualOrders = new List<Order>();
			InitOrderInfos();
			InitResourceInfos();
		}

		public Order GetFinalOrder()
		{
			var order = new Order(_finalOrder);
			FillOrderResources(1, order, _finalOrder, ResourceInfos);
			_actualOrders.Add(order);
			return order;
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

		public void EndOrder(Order order)
		{
			_actualOrders.Remove(order);
		}

		/// <summary>
		/// Удалить все имеющиеся заказы
		/// </summary>
		public void Clear()
		{
			_actualOrders.Clear();
			_addOrders1 = false;
			_addOrders2 = false;
			_addOrders3 = false;
		}

		/// <summary>
		/// Формируем сколько ресурсов в заказе будет
		/// </summary>
		private void FillOrderResources(int level, Order order, OrderInfo orderInfo, List<ResourceInfo> resourceInfos)
		{
			foreach (var resGroup in orderInfo.ResourceGroupValues) {
				var resGroupInfo = GetResourceGroupInfo(resourceInfos, resGroup, level);
				FillOrderResources(order, resourceInfos, resGroupInfo, resGroup.Value);
			}
		}

		private void FillOrderResources(Order order, List<ResourceInfo> resourceInfos,
			List<ResourceInfo> resGroupInfo, int resGroupValue)
		{
			var amount = new ResourcesHolder(resourceInfos);
			int fill = 0;
			foreach (var res in resGroupInfo) {
				var available = resGroupValue - fill;// сколько осталось
				var rvalue = RandomHelper.Random(available);
				var vvalue = (int)(rvalue / res.VolumeCoefficient);
				if (vvalue > 0) {
					amount.Add(res.ResourceType, vvalue);
					fill += vvalue;// сохраняем ресурс
				}
			}
			var need = resGroupValue - fill;
			if (resGroupValue - fill > 0) {// добавляем оставшееся из первого ресурса
				var res = resGroupInfo[0];
				var vvalue = (int)(need / res.VolumeCoefficient);
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
			AddOrderInfo("Домик", "Строительство маленького домика", 1, "",
				150, 50, 50, 50);

			AddOrderInfo("Фазенда", "Строительство фазенды", 1, "",
				 250, 20, 20, 20);

			AddOrderInfo("Магазин", "Строительство магазина", 1, "",
				 100, 150, 100, 0);

			AddOrderInfo("Пристройка", "Строительство небольшой пристройки", 1, "",
				200, 50, 10, 20);

			AddOrderInfo("Торговый центр", "Строительство торгового центра", 2, "",
				 300, 100, 200, 100);

			AddOrderInfo("Товары", "Перевозка товаров для магазина", 2, "",
				 400, 100, 100, 0);

			AddOrderInfo("Товары", "Перевозка товаров для магазина", 2, "",
				 0, 200, 200, 200);

			AddOrderInfo("Товары", "Перевозка отделочных материалов", 2, "",
				0, 600, 0, 0);

			AddOrderInfo("Товары", "Перевозка мебели для магазина", 2, "",
				0, 0, 600, 0);

			AddOrderInfo("Инструменты", "Перевозка инструментов для магазина", 2, "",
				0, 0, 0, 600);

			AddOrderInfo("Парк", "Разбить большой парк", 2, "",
				200, 300, 100, 100);

			AddOrderInfo("Парк", "Облагородить территорию", 2, "",
				100, 100, 100, 300);

			AddOrderInfo("Дом", "Построить дом", 2, "",
				300, 200, 0, 100);

			AddOrderInfo("Ресторан", "Построить ресторан", 2, "",
				300, 200, 0, 100);

			_finalOrder = CreateOrderInfo("Главная стройка года", "Завезти материалы для стройки года", 99, "",
				8000000, 500000, 100000, 1200000);
		}

		private void AddOrderInfo(string name, string description, int level, string logo, int strMat, int decMat, int frnMat, int mchMat)
		{
			var order = CreateOrderInfo(name, description, level, logo, strMat, decMat, frnMat, mchMat);
			OrderInfos.Add(order);
		}

		private OrderInfo CreateOrderInfo(string name, string description, int level, string logo, int strMat, int decMat, int frnMat, int mchMat)
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

			return order;
		}

		private void InitResourceInfos()
		{
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesEnum.StrWood, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesEnum.StrStone, "", "Камень", "Камень", 3, 3);
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesEnum.StrConcrete, "", "Цемент", "Цемент", 2, 2);
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesEnum.StrBrick, "", "Кирпич", "Кирпич", 2, 2);
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesEnum.StrSteel, "", "Железо", "Железо", 5, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecTile, "", "Плитка", "Плитка", 3, 2);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecPaper, "", "Обои", "Обои", 2, 4);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecPlaster, "", "Штукатурка", "Штукатурка", 2, 2);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecPutty, "", "Шпаклевка", "Шпаклевка", 2, 2);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecGlass, "", "Стекло", "Стекло", 2, 5);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecPaint, "", "Краска", "Краска", 2, 3);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecLinoleum, "", "Линолеум", "Линолеум", 3, 5);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesEnum.DecPorcelain, "", "Фарфор", "Фарфор", 2, 4);
			AddOrderInfo(ResourcesGroupEnum.Furniture, ResourcesEnum.FurFurniture, "", "Мебель", "Мебель", 5, 1);
			AddOrderInfo(ResourcesGroupEnum.Furniture, ResourcesEnum.FurPlumbing, "", "Сантехника", "Сантехника", 2, 4);
			AddOrderInfo(ResourcesGroupEnum.Furniture, ResourcesEnum.FurDishes, "", "Посуда", "Посуда", 3, 2);
			AddOrderInfo(ResourcesGroupEnum.Furniture, ResourcesEnum.FurKitchen, "", "Кухня", "Кухня", 10, 3);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesEnum.MechShovels, "", "Лопаты", "Лопаты", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesEnum.MechFinishingTools, "", "Инструменты для отделки", "Инструменты для отделки", 2, 2);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesEnum.MechExcavator, "", "Эскаватор", "Эскаватор", 50, 3);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesEnum.MechCrane, "", "Кран", "Кран", 100, 30);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesEnum.MechPileDriving, "", "Сваезабивалка", "Сваезабивалка", 80, 50);
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

		private bool _addOrders1;
		private bool _addOrders2;
		private bool _addOrders3;

		public void UpdateResearchInfo()
		{
			if (!_addOrders1 && ItemsManager.IsResearchItemBuyed("AddOrders1")) {
				_addOrders1 = true;
				MaxOrders += GameConstants.AddOrders1;
				StateEngine.Log.AddLog("Количество заказов увеличено");
			}
			if (!_addOrders2 && ItemsManager.IsResearchItemBuyed("AddOrders2")) {
				_addOrders2 = true;
				MaxOrders += GameConstants.AddOrders2;
				StateEngine.Log.AddLog("Количество заказов увеличено");
			}
			if (!_addOrders3 && ItemsManager.IsResearchItemBuyed("AddOrders3")) {
				_addOrders3 = true;
				MaxOrders += GameConstants.AddOrders3;
				StateEngine.Log.AddLog("Количество заказов увеличено");
			}
		}

		private int _randomCounter;

		public Order GetRandomOrder()
		{
			_randomCounter++;
			if (_randomCounter >= ActualOrdersCount)
				_randomCounter = 0;
			return _actualOrders[_randomCounter];
		}
	}
}