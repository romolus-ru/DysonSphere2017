using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConstruction.Game.Items
{
	/// <summary>
	/// Генерирует информацию об айтемах магазина (пока не переехало в базу и в файл)
	/// </summary>
	internal static class ItemsManager
	{
		/// <summary>
		/// Список всех предметов которые только возможны
		/// </summary>
		public static List<Item> Items = new List<Item>();
		/// <summary>
		/// Предметы которые доступны пользователю
		/// </summary>
		public readonly static List<ItemManager> ItemsManaged = new List<ItemManager>();

		static ItemsManager()
		{
			CreateItems();
		}

		const string small = " малое";
		const string big = " большое";

		const string strUpVolume = "Увеличение объема грузоперевозок";
		const string strUpVolumePlus = strUpVolume + small;
		const string strUpVolumeExtra = strUpVolume + big;

		const string strUpWeight = "Увеличение перевозимого веса";
		const string strUpWeightPlus = strUpWeight + small;
		const string strUpWeightExtra = strUpWeight + big;

		const string strUpExp = "Увеличение опыта";
		const string strUpExpPlus = strUpExp + small;
		const string strUpExpExtra = strUpExp + big;

		const string strUpTeleport = "Телепорт";
		const string strUpTeleportDistance = "Увеличение дальности телепорта";
		const string strUpTeleportDistancePlus = strUpTeleport + small;
		const string strUpTeleportDistanceExtra = strUpTeleport + big;

		const string strUpEngine = "Увеличение мощности двигателя";
		const string strUpEnginePlus = strUpEngine + small;
		const string strUpEngineExtra = strUpEngine + big;

		const string strUpTakeOff = "Увеличение скорости взлёта/посадки";
		const string strUpTakeOffPlus = strUpTakeOff + small;
		const string strUpTakeOffExtra = strUpTakeOff + big;

		const string strUpLoading = "Увеличение скорости загрузки/разгрузки";
		const string strUpLoadingPlus = strUpLoading + small;
		const string strUpLoadingExtra = strUpLoading + big;

		internal static ItemManager GetItemManager(ItemUpgrade itemUpgrade)
		{
			ItemManager ret = null;
			foreach (var item in ItemsManaged) {
				if (item.Item != itemUpgrade)
					continue;
				ret = item;
				break;
			}
			return ret;
		}

		const string strUpAutoPilot = "Автоматическая система обработки заказов";

		private static void CreateItems()
		{
			Items.Clear();

			var costSign =
			CreateItemSigns("Знак выполненного контракта", "Знак выполненного контракта", 
			texture: "Resources.Sign1", code: "Sign1");
			var costSign2 =
			CreateItemSigns("Особый знак выполненного контракта", "Особый знак выполненного контракта", 
			texture: "Resources.Sign2", code: "Sign2");

			var itemCost1 = new ItemManager(costSign, 1);
			var itemCost3 = new ItemManager(costSign, 3);
			var itemCost5 = new ItemManager(costSign, 5);

			#region UpgradesForShip
			var upgradeVolumeExtra = CreateUpgradeValue(strUpVolumeExtra, "CargoVolumeMax", 15);
			var upgradeVolume = CreateUpgradeValue(strUpVolume, "CargoVolumeMax", 10);
			var upgradeVolumePlus = CreateUpgradeValue(strUpVolumePlus, "CargoVolumeMax", 7);

			var upgradeWeightExtra = CreateUpgradeValue(strUpWeightExtra, "CargoWeightMax", 15);
			var upgradeWeight = CreateUpgradeValue(strUpWeight, "CargoWeightMax", 10);
			var upgradeWeightPlus = CreateUpgradeValue(strUpWeightPlus, "CargoWeightMax", 7);

			var upgradeExpExtra = CreateUpgradeValue(strUpExpExtra, "Exp", 7);
			var upgradeExp = CreateUpgradeValue(strUpExp, "Exp", 5);
			var upgradeExpPlus = CreateUpgradeValue(strUpExpPlus, "Exp", 3);

			var upgradeAutopilot = CreateUpgradeValue(strUpAutoPilot, "AutoPilot", 1);
			//var upgradeTeleport = CreateUpgradeValue(strUpTeleport, "Teleport", 1);// по умолчанию дистанция телепорта 1

			//var upgradeTeleportDistanceExtra = CreateUpgradeValue(strUpTeleportDistanceExtra, "TeleportDistance", 3);
			//var upgradeTeleportDistance = CreateUpgradeValue(strUpTeleportDistance, "TeleportDistance", 2);
			//var upgradeTeleportDistancePlus = CreateUpgradeValue(strUpTeleportDistancePlus, "TeleportDistance", 1);

			// накапливается расстояние и когда есть возможность - корабль перемещается на 2 точки вместо одной
			var upgradeEngineExtra = CreateUpgradeValue(strUpEngineExtra, "EngineSpeed", 3);
			var upgradeEngine = CreateUpgradeValue(strUpEnginePlus, "EngineSpeed", 2);
			var upgradeEnginePlus = CreateUpgradeValue(strUpEnginePlus, "EngineSpeed", 1);

			// уменьшается время взлета. при всех установленных улучшениях должно быть не меньше 1 секунды
			var upgradeTakeOffExtra = CreateUpgradeValue(strUpTakeOffExtra, "TakeOff", 3);
			var upgradeTakeOff = CreateUpgradeValue(strUpTakeOff, "TakeOff", 2);
			var upgradeTakeOffPlus = CreateUpgradeValue(strUpTakeOffPlus, "TakeOff", 1);

			// уменьшается время погрузки/разгрузки. при всех установленных улучшениях должно быть не меньше 1 секунды
			var upgradeUploadingExtra = CreateUpgradeValue(strUpLoadingExtra, "Uploading", 3);
			var upgradeUploading = CreateUpgradeValue(strUpLoadingPlus, "Uploading", 2);
			var upgradeUploadingPlus = CreateUpgradeValue(strUpLoadingExtra, "Uploading", 1);
			#endregion

			CreateUpgradeItem(strUpVolume, strUpVolume, null, itemCost1, ItemUpgradeQualityEnum.Poor,
				new List<ItemUpgradeValue>() { upgradeVolume });
			CreateUpgradeItem(strUpVolume, strUpVolume, null, itemCost3, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() { upgradeVolume, upgradeWeightPlus });
			CreateUpgradeItem(strUpVolume, strUpVolume, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeVolumeExtra });

			CreateUpgradeItem(strUpWeight, strUpWeight, null, itemCost1, ItemUpgradeQualityEnum.Poor,
				new List<ItemUpgradeValue>() { upgradeWeight });
			CreateUpgradeItem(strUpWeight, strUpWeight, null, itemCost3, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() { upgradeWeight, upgradeVolumePlus });
			CreateUpgradeItem(strUpWeight, strUpWeight, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeWeightExtra });

			/*CreateUpgradeItem(strUpExp, strUpExp, null, itemCost1, ItemUpgradeQualityEnum.Poor,
				new List<ItemUpgradeValue>() { upgradeExpPlus });
			CreateUpgradeItem(strUpExp, strUpExp, null, itemCost3, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() { upgradeExp, upgradeVolumePlus });
			CreateUpgradeItem(strUpExp, strUpExp, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeExpExtra });*/

			CreateUpgradeItem(strUpAutoPilot, strUpAutoPilot, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeAutopilot });

			CreateUpgradeItem(strUpEngine, strUpEngine, null, itemCost1, ItemUpgradeQualityEnum.Poor,
				new List<ItemUpgradeValue>() { upgradeEngine });
			CreateUpgradeItem(strUpEngine, strUpEngine, null, itemCost3, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() { upgradeEngine, upgradeTakeOffPlus });
			CreateUpgradeItem(strUpEngine, strUpEngine, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeEngineExtra });

			CreateUpgradeItem(strUpTakeOff, strUpTakeOff, null, itemCost1, ItemUpgradeQualityEnum.Poor,
				new List<ItemUpgradeValue>() { upgradeTakeOff });
			CreateUpgradeItem(strUpTakeOff, strUpTakeOff, null, itemCost3, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() { upgradeTakeOff, upgradeEnginePlus });
			CreateUpgradeItem(strUpTakeOff, strUpTakeOff, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeTakeOffExtra });

			CreateUpgradeItem(strUpLoading, strUpLoading, null, itemCost1, ItemUpgradeQualityEnum.Poor,
				new List<ItemUpgradeValue>() { upgradeUploadingPlus });
			CreateUpgradeItem(strUpLoading, strUpLoading, null, itemCost3, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() { upgradeUploading });
			CreateUpgradeItem(strUpLoading, strUpLoading, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeUploadingExtra });

			CreateResearchItem("улучшить перевозимый объем у кораблей", "улучшить перевозимый объем у кораблей", costSign, 5, null, "ShipVolume");
			CreateResearchItem("улучшить перевозимый вес у кораблей", "улучшить перевозимый вес у кораблей", costSign, 5, null, "ShipWeight");

			CreateResearchItem("улучшить перевозимый объем у кораблей 2", "улучшить перевозимый объем у кораблей 2", costSign, 5, null, "ShipVolume2");
			CreateResearchItem("улучшить перевозимый вес у кораблей 2", "улучшить перевозимый вес у кораблей 2", costSign, 5, null, "ShipWeight2");

			CreateResearchItem("дополнительные 4 корабля", "дополнительные 4 корабля", costSign, 1, null, "AddShips1");
			CreateResearchItem("дополнительные 3 корабля", "дополнительные 3 корабля", costSign, 2, null, "AddShips2");
			CreateResearchItem("дополнительные 2 корабля", "дополнительные 2 корабля", costSign, 3, null, "AddShips3");

			CreateResearchItem("дополнительные 3 заказа", "дополнительные 3 заказа", costSign, 20, null, "AddOrders1");
			CreateResearchItem("дополнительные 2 заказа", "дополнительные 2 заказа", costSign, 50, null, "AddOrders2");
			CreateResearchItem("дополнительный заказ", "дополнительный заказ", costSign, 500, null, "AddOrders3");

			CreateResearchItem("купить доступ к магазину", "купить доступ к магазину", costSign, 50, null, "OpenShop");

			CreateResearchItem("покупка обычных улучшений", "покупка обычных улучшений", costSign, 100, null, "CanBuyNormalUpgrades");
			CreateResearchItem("покупка особых улучшений", "покупка особых улучшений", costSign, 250, null, "CanBuyExtraUpgrades");

			//CreateResearchItem("открыть доступ к лучшим заказам", "открыть доступ к лучшим заказам", costSign, 500, null, "OpenTopOrders");

			CreateResearchItem("запустить финальный заказ (на время)", "запустить финальный заказ (на время)", costSign, 300, null, "StartFinalOrder");


			ItemsManaged.Clear();
			foreach (var item in Items) {
				var im = new ItemManager(item, 0);
				ItemsManaged.Add(im);
			}

			var it1 = GetItemByCode("Sign1");
			ItemsManaged.Remove(it1);
			it1 = new ItemManager(costSign, 1000);
			ItemsManaged.Add(it1);
		}

		internal static List<ItemManager> GetUpgrades(bool buyNormalUpgrades, bool buyExtraUpgrades)
		{
			var result = new List<ItemManager>();
			var upgrades = ItemsManaged.Where(item => item.Item.Type == ItemTypeEnum.Upgrade);
			if (buyExtraUpgrades)
				foreach (var upgrade in upgrades) {
					if ((upgrade.Item as ItemUpgrade).Quality == ItemUpgradeQualityEnum.Extra)
						result.Add(upgrade);
				}
			if (buyNormalUpgrades)
				foreach (var upgrade in upgrades) {
					if ((upgrade.Item as ItemUpgrade).Quality == ItemUpgradeQualityEnum.Normal)
						result.Add(upgrade);
				}
			foreach (var upgrade in upgrades) {
				if ((upgrade.Item as ItemUpgrade).Quality == ItemUpgradeQualityEnum.Poor)
					result.Add(upgrade);
			}
			return result;
		}

		internal static IEnumerable<ItemManager> GetResearches()
		{
			return ItemsManaged.Where(item => item.Item.Type == ItemTypeEnum.Research);
		}

		internal static ItemManager GetResearchItem(string researchCode)
		{
			foreach (var mItem in ItemsManaged) {
				if (mItem.PlayerCount == 0
					|| mItem.Item.Type != ItemTypeEnum.Research
					|| mItem.Item.Code != researchCode)
					continue;
				return mItem;
			}
			return null;
		}

		internal static bool IsResearchItemBuyed(string researchCode)
		{
			var mItem = GetResearchItem(researchCode);
			return mItem != null && mItem.PlayerCount > 0;
		}

		/// <summary>
		/// Купить предмет. возвращаем успешность покупки
		/// </summary>
		/// <param name="itemCode"></param>
		/// <returns></returns>
		internal static bool BuyItem(string itemCode)
		{
			var item = GetItemByCode(itemCode);
			var cost = item.Item.Cost;
			var moneyItem = GetItemByCode(cost.Item.Code);
			return item.BuyItem(moneyItem);
		}

		/// <summary>
		/// Купить предмет. возвращаем успешность покупки
		/// </summary>
		/// <returns></returns>
		internal static bool BuyUpgrade(ItemManager upgrade)
		{
			var cost = upgrade.Item.Cost;
			var moneyItem = GetItemByCode(cost.Item.Code);
			return upgrade.BuyItem(moneyItem);
		}

		public static void BuySign(string itemCode)
		{
			var item = GetItemByCode(itemCode);
			item.BuySign();
		}

		internal static ItemManager GetItemByCode(string code)
		{
			var result = ItemsManaged.Where(it => it.Item.Code == code);
			if (result.Count() > 1)
				throw new Exception("не уникальный айтем с кодом " + code);
			return result.FirstOrDefault();
		}

		private static ItemUpgradeValue CreateUpgradeValue(string name, string upName, int upValue)
		{
			var iv = new ItemUpgradeValue()
			{
				Name = name,
				UpName = upName,
				UpValue = upValue,
			};
			return iv;
		}

		/// <summary>
		/// Создать обычный предмет
		/// </summary>
		/// <returns></returns>
		private static Item CreateItemSigns(string itemName, string itemDescription, string texture = null, ItemManager cost = null, string code = null)
		{
			var i = new Item()
			{
				Type = ItemTypeEnum.Signs,
				Code = code,
				Name = itemName,
				Description = itemDescription,
				Texture = texture,
				Cost = cost
			};
			Items.Add(i);
			return i;
		}

		/// <summary>
		/// Создать предмет-улучшение
		/// </summary>
		private static void CreateUpgradeItem(string itemName, string itemDescription, string texture = null, ItemManager cost = null,
			ItemUpgradeQualityEnum quality = ItemUpgradeQualityEnum.Poor, List<ItemUpgradeValue> upgrades = null)
		{
			if (upgrades == null) return;

			var i = new ItemUpgrade()
			{
				Type = ItemTypeEnum.Upgrade,
				Name = itemName,
				Description = itemDescription,
				Texture = texture,
				Cost = cost,
				Quality = quality,
				Upgrades = upgrades,
			};
			Items.Add(i);
		}

		/// <summary>
		/// Создать предмет-исследование
		/// </summary>
		private static void CreateResearchItem(string itemName, string itemDescription, Item costItem, int costCount,
			string texture = null, string researchCode = null)
		{
			if (string.IsNullOrEmpty(researchCode)) return;

			var cost = new ItemManager(costItem, costCount);
			var i = new ItemResearch()
			{
				Type = ItemTypeEnum.Research,
				Name = itemName,
				Description = itemDescription,
				Texture = texture,
				Cost = cost,
				Code = researchCode,
			};
			Items.Add(i);
		}
	}
}