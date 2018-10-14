using DysonSphereClient.Game.ResourcesNew;
using Engine.Helpers;
using System.Collections.Generic;
using System.Linq;
using System;
using Engine.Extensions;

namespace DysonSphereClient.Game.OrdersNew
{
	/// <summary>
	/// Заказы
	/// </summary>
	public class Orders
	{
		private List<Order> _actualOrders;
		/// <summary>
		/// Описания заказов
		/// </summary>
		private List<OrderInfo> _orderInfos;
		/// <summary>
		/// исходная информация о ресурсах
		/// </summary>
		private List<ResourceInfo> _resourceInfos = null;

		public Orders()
		{
			_orderInfos = new List<OrderInfo>();
			_actualOrders = new List<Order>();
			InitOrderInfos();
			InitResourceInfos();
		}

		public Order GetNewOrder(int level)
		{
			var orderInfos = _orderInfos.Where(oi => oi.Level <= level).ToList();
			var num = RandomHelper.Random(orderInfos.Count);
			var orderInfo = orderInfos[num];
			var order = new Order(orderInfo);
			FillOrderResources(level, order, orderInfo, _resourceInfos);
			_actualOrders.Add(order);
			return order;
		}

		/// <summary>
		/// Формируем сколько ресурсов в заказе будет
		/// </summary>
		private void FillOrderResources(int level, Order order, OrderInfo orderInfo, List<ResourceInfo> resourceInfos)
		{
			foreach (var resGroup in orderInfo.ResourceGroupValues) {
				var resGroupInfo = GetResourceGroupInfo(resourceInfos, resGroup);// уровень не учитывается пока
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
		}

		/// <summary>
		/// Получаем список ресурсов, входящих в группу ресурсов, вперемешку
		/// </summary>
		private List<ResourceInfo> GetResourceGroupInfo(List<ResourceInfo> resourceInfos, ResourceGroupValue resGroupValue)
		{
			var resGroupInfo = resourceInfos.Where(ri => ri.ResourceGroup == resGroupValue.Group).ToList();
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

			_orderInfos.Add(order);
		}

		private void InitResourceInfos()
		{
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesNew.ResourcesEnum.StrWood, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesNew.ResourcesEnum.StrStone, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesNew.ResourcesEnum.StrConcrete, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesNew.ResourcesEnum.StrBrick, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.StructuralMaterial, ResourcesNew.ResourcesEnum.StrSteel, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesNew.ResourcesEnum.DecTile, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesNew.ResourcesEnum.DecPaper, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesNew.ResourcesEnum.DecPlaster, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesNew.ResourcesEnum.DecPutty, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesNew.ResourcesEnum.DecGlass, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesNew.ResourcesEnum.DecPaint, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesNew.ResourcesEnum.DecLinoleum, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.DecorationMaterial, ResourcesNew.ResourcesEnum.DecPorcelain, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Furniture, ResourcesNew.ResourcesEnum.FurFurniture, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Furniture, ResourcesNew.ResourcesEnum.FurPlumbing, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Furniture, ResourcesNew.ResourcesEnum.FurDishes, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Furniture, ResourcesNew.ResourcesEnum.FurKitchen, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesNew.ResourcesEnum.MechShovels, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesNew.ResourcesEnum.MechFinishingTools, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesNew.ResourcesEnum.MechExcavator, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesNew.ResourcesEnum.MechCrane, "", "Дерево", "Дерево", 1, 1);
			AddOrderInfo(ResourcesGroupEnum.Mechs, ResourcesNew.ResourcesEnum.MechPileDriving, "", "Дерево", "Дерево", 1, 1);
		}

		private void AddOrderInfo(ResourcesGroupEnum resourceGroup, ResourcesNew.ResourcesEnum resourceType, string texture,
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

			_resourceInfos.Add(resInfo);
		}
	}
}