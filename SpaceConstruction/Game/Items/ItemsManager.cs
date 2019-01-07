using System;
using System.Collections.Generic;
using System.Linq;

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
		public static List<ItemManager> ItemsManaged = new List<ItemManager>();

		static ItemsManager()
		{
			CreateItems();
		}

		const string Small = " малое";
		const string Big = " большое";

		const string StrUpVolume = "Увеличение объема грузоперевозок";
		const string StrUpVolumePlus = StrUpVolume + Small;
		const string StrUpVolumeExtra = StrUpVolume + Big;

		const string StrUpWeight = "Увеличение перевозимого веса";
		const string StrUpWeightPlus = StrUpWeight + Small;
		const string StrUpWeightExtra = StrUpWeight + Big;

		//const string StrUpExp = "Увеличение опыта";
		//const string StrUpExpPlus = StrUpExp + Small;
		//const string StrUpExpExtra = StrUpExp + Big;

		//const string StrUpTeleport = "Телепорт";
		//const string StrUpTeleportDistance = "Увеличение дальности телепорта";
		//const string StrUpTeleportDistancePlus = StrUpTeleport + Small;
		//const string StrUpTeleportDistanceExtra = StrUpTeleport + Big;

		const string StrUpEngine = "Увеличение мощности двигателя";
		const string StrUpEnginePlus = StrUpEngine + Small;
		const string StrUpEngineExtra = StrUpEngine + Big;

		const string StrUpTakeOff = "Увеличение скорости взлёта/посадки";
		const string StrUpTakeOffPlus = StrUpTakeOff + Small;
		const string StrUpTakeOffExtra = StrUpTakeOff + Big;

		const string StrUpLoading = "Увеличение скорости загрузки/разгрузки";
		const string StrUpLoadingPlus = StrUpLoading + Small;
		const string StrUpLoadingExtra = StrUpLoading + Big;

		const string StrUpAutoPilot = "Автоматическая система обработки заказов";
		const string StrUpAutoPilotVolume = "Уменьшение перевозимого объема в процентах";
		const string StrUpAutoPilotWeight = "Уменьшение перевозимого веса в процентах";

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


		private static void CreateItems()
		{
			Items.Clear();

			var costSign =
			CreateItemSigns("Знак выполненного контракта", "Знак выполненного контракта",
			texture: "Resources.Sign1", code: "Sign1");
			
			CreateItemSigns("Особый знак выполненного контракта", "Особый знак выполненного контракта",
			texture: "Resources.Sign2", code: "Sign2");

			var itemCost1 = new ItemManager(costSign, 1);
			var itemCost3 = new ItemManager(costSign, 3);
			var itemCost5 = new ItemManager(costSign, 5);

			#region UpgradesForShip
			var upgradeVolumeExtra = CreateUpgradeValue(StrUpVolumeExtra, "CargoVolumeMax", 70, ItemUpgradeQualityEnum.Extra);
			var upgradeVolume = CreateUpgradeValue(StrUpVolume, "CargoVolumeMax", 40, ItemUpgradeQualityEnum.Normal);
			var upgradeVolumePlus = CreateUpgradeValue(StrUpVolumePlus, "CargoVolumeMax", 20, ItemUpgradeQualityEnum.Poor);

			var upgradeWeightExtra = CreateUpgradeValue(StrUpWeightExtra, "CargoWeightMax", 70, ItemUpgradeQualityEnum.Extra);
			var upgradeWeight = CreateUpgradeValue(StrUpWeight, "CargoWeightMax", 40, ItemUpgradeQualityEnum.Normal);
			var upgradeWeightPlus = CreateUpgradeValue(StrUpWeightPlus, "CargoWeightMax", 20, ItemUpgradeQualityEnum.Poor);

			//var upgradeExpExtra = CreateUpgradeValue(strUpExpExtra, "Exp", 7);
			//var upgradeExp = CreateUpgradeValue(strUpExp, "Exp", 5);
			//var upgradeExpPlus = CreateUpgradeValue(strUpExpPlus, "Exp", 3);

			var upgradeAutopilot = CreateUpgradeValue(StrUpAutoPilot, "AutoPilot", 1, ItemUpgradeQualityEnum.Extra);
			var upgradeAutopilotVolume = CreateUpgradeValue(StrUpAutoPilotVolume, "CargoVolumePercent", -35, ItemUpgradeQualityEnum.Bad);
			var upgradeAutopilotWeight = CreateUpgradeValue(StrUpAutoPilotWeight, "CargoWeightPercent", -35, ItemUpgradeQualityEnum.Bad);
			//var upgradeTeleport = CreateUpgradeValue(strUpTeleport, "Teleport", 1);// по умолчанию дистанция телепорта 1

			//var upgradeTeleportDistanceExtra = CreateUpgradeValue(strUpTeleportDistanceExtra, "TeleportDistance", 3);
			//var upgradeTeleportDistance = CreateUpgradeValue(strUpTeleportDistance, "TeleportDistance", 2);
			//var upgradeTeleportDistancePlus = CreateUpgradeValue(strUpTeleportDistancePlus, "TeleportDistance", 1);

			// накапливается расстояние и когда есть возможность - корабль перемещается на 2 точки вместо одной
			var upgradeEngineExtra = CreateUpgradeValue(StrUpEngineExtra, "EngineSpeed", 70, ItemUpgradeQualityEnum.Extra);
			var upgradeEngine = CreateUpgradeValue(StrUpEnginePlus, "EngineSpeed", 40, ItemUpgradeQualityEnum.Normal);
			var upgradeEnginePlus = CreateUpgradeValue(StrUpEnginePlus, "EngineSpeed", 15, ItemUpgradeQualityEnum.Poor);

			// уменьшается время взлета. при всех установленных улучшениях должно быть не меньше 1 секунды
			var upgradeTakeOffExtra = CreateUpgradeValue(StrUpTakeOffExtra, "TakeOff", 1800, ItemUpgradeQualityEnum.Extra);
			var upgradeTakeOff = CreateUpgradeValue(StrUpTakeOff, "TakeOff", 1200, ItemUpgradeQualityEnum.Normal);
			var upgradeTakeOffPlus = CreateUpgradeValue(StrUpTakeOffPlus, "TakeOff", 800, ItemUpgradeQualityEnum.Poor);

			// уменьшается время погрузки/разгрузки. при всех установленных улучшениях должно быть не меньше 1 секунды
			var upgradeUploadingExtra = CreateUpgradeValue(StrUpLoadingExtra, "Uploading", 1800, ItemUpgradeQualityEnum.Extra);
			var upgradeUploading = CreateUpgradeValue(StrUpLoadingPlus, "Uploading", 1200, ItemUpgradeQualityEnum.Normal);
			var upgradeUploadingPlus = CreateUpgradeValue(StrUpLoadingExtra, "Uploading", 800, ItemUpgradeQualityEnum.Poor);
			#endregion

			CreateUpgradeItem(StrUpVolume, StrUpVolume, null, itemCost1, ItemUpgradeQualityEnum.Poor,
				new List<ItemUpgradeValue>() { upgradeVolume });
			CreateUpgradeItem(StrUpVolume, StrUpVolume, null, itemCost3, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() { upgradeVolume, upgradeWeightPlus });
			CreateUpgradeItem(StrUpVolume, StrUpVolume, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeVolumeExtra });

			CreateUpgradeItem(StrUpWeight, StrUpWeight, null, itemCost1, ItemUpgradeQualityEnum.Poor,
				new List<ItemUpgradeValue>() { upgradeWeight });
			CreateUpgradeItem(StrUpWeight, StrUpWeight, null, itemCost3, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() { upgradeWeight, upgradeVolumePlus });
			CreateUpgradeItem(StrUpWeight, StrUpWeight, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeWeightExtra });

			/*CreateUpgradeItem(strUpExp, strUpExp, null, itemCost1, ItemUpgradeQualityEnum.Poor,
				new List<ItemUpgradeValue>() { upgradeExpPlus });
			CreateUpgradeItem(strUpExp, strUpExp, null, itemCost3, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() { upgradeExp, upgradeVolumePlus });
			CreateUpgradeItem(strUpExp, strUpExp, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeExpExtra });*/

			CreateUpgradeItem(StrUpAutoPilot, StrUpAutoPilot, null, itemCost1, ItemUpgradeQualityEnum.Autopilot,
				new List<ItemUpgradeValue>() { upgradeAutopilot, upgradeAutopilotVolume, upgradeAutopilotWeight });

			CreateUpgradeItem(StrUpEngine, StrUpEngine, null, itemCost1, ItemUpgradeQualityEnum.Poor,
				new List<ItemUpgradeValue>() { upgradeEngine });
			CreateUpgradeItem(StrUpEngine, StrUpEngine, null, itemCost3, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() { upgradeEngine, upgradeTakeOffPlus });
			CreateUpgradeItem(StrUpEngine, StrUpEngine, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeEngineExtra });

			CreateUpgradeItem(StrUpTakeOff, StrUpTakeOff, null, itemCost1, ItemUpgradeQualityEnum.Poor,
				new List<ItemUpgradeValue>() { upgradeTakeOff });
			CreateUpgradeItem(StrUpTakeOff, StrUpTakeOff, null, itemCost3, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() { upgradeTakeOff, upgradeEnginePlus });
			CreateUpgradeItem(StrUpTakeOff, StrUpTakeOff, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeTakeOffExtra });

			CreateUpgradeItem(StrUpLoading, StrUpLoading, null, itemCost1, ItemUpgradeQualityEnum.Poor,
				new List<ItemUpgradeValue>() { upgradeUploadingPlus });
			CreateUpgradeItem(StrUpLoading, StrUpLoading, null, itemCost3, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() { upgradeUploading });
			CreateUpgradeItem(StrUpLoading, StrUpLoading, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeUploadingExtra });

			CreateResearchItem("увеличить у всех кораблей перевозимый объем", "увеличить у всех кораблей перевозимый объем", costSign, 5, null, "ShipVolume");
			CreateResearchItem("увеличить у всех кораблей перевозимый вес", "увеличить у всех кораблей перевозимый вес", costSign, 5, null, "ShipWeight");

			CreateResearchItem("увеличить у всех кораблей перевозимый объем 2", "увеличить у всех кораблей перевозимый объем 2", costSign, 25, null, "ShipVolume2");
			CreateResearchItem("увеличить у всех кораблей перевозимый вес 2", "увеличить у всех кораблей перевозимый вес 2", costSign, 25, null, "ShipWeight2");

			CreateResearchItem("дополнительные 4 корабля", "дополнительные 4 корабля", costSign, 1, null, "AddShips1");
			CreateResearchItem("дополнительные 3 корабля", "дополнительные 3 корабля", costSign, 3, null, "AddShips2");
			CreateResearchItem("дополнительные 2 корабля", "дополнительные 2 корабля", costSign, 5, null, "AddShips3");

			CreateResearchItem("дополнительные 3 заказа", "дополнительные 3 заказа", costSign, 15, null, "AddOrders1");
			CreateResearchItem("дополнительные 2 заказа", "дополнительные 2 заказа", costSign, 30, null, "AddOrders2");
			CreateResearchItem("дополнительный 1 заказ",  "дополнительный 1 заказ",  costSign, 40, null, "AddOrders3");

			CreateResearchItem("Магазин улучшений для корабля", "Доступ к магазину улучшений для корабля", costSign, 10, null, "OpenShop");

			CreateResearchItem("покупка обычных улучшений", "в магазине будут доступны обычные улучшения", costSign, 50, null, "CanBuyNormalUpgrades");
			CreateResearchItem("покупка особых улучшений", "в магазине будут доступны особые улучшения", costSign, 150, null, "CanBuyExtraUpgrades");
			CreateResearchItem("Купить технологию автопилотирования", "ассортимент магазина пополнится автопилотом", costSign, 11, null, "CanBuyAutopilot");

			CreateResearchItem("Лучшие заказы", "Более тяжелые но и лучше оплачиваемые заказы", costSign, 30, null, "OpenTopOrders");

			CreateResearchItem("запустить финальный заказ (на время)", "запустить финальный заказ (на время)", costSign, 60, null, "StartFinalOrder");
			
			ItemsManaged.Clear();
			foreach (var item in Items) {
				var im = new ItemManager(item, 0);
				ItemsManaged.Add(im);
			}

			var it1 = GetItemByCode("Sign1");
			ItemsManaged.Remove(it1);
			it1 = new ItemManager(costSign, 100);
			ItemsManaged.Add(it1);
		}

		internal static bool IsAvailableResearchesToBuy()
		{
			var researches = GetResearches();
			foreach (var research in researches) {
				if (IsCanBuyResearch(research))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Исследование покупается только одно, поэтому проверяем что оно уже куплено
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		internal static bool IsCanBuyResearch(ItemManager item)
		{
			return item.PlayerCount <= 0 && IsCanBuyItem(item);
		}

		internal static bool IsCanBuyItem(ItemManager item)
		{
			var cost = item.Item.Cost;// ищем предмет который является деньгами для этого предмета
			var moneyItem = GetItemByCode(cost.Item.Code);

			return item.CanBuyItem(moneyItem);
		}

		internal static List<ItemManager> GetUpgrades(bool buyNormalUpgrades, bool buyExtraUpgrades, bool buyAutopilot)
		{
			var result = new List<ItemManager>();
			var upgrades = ItemsManaged.Where(item => item.Item.Type == ItemTypeEnum.Upgrade).ToList();
			if (buyAutopilot)
				foreach (var upgrade in upgrades) {
					if (((ItemUpgrade) upgrade.Item).Quality == ItemUpgradeQualityEnum.Autopilot)
						result.Add(upgrade);
				}
			if (buyExtraUpgrades)
				foreach (var upgrade in upgrades) {
					if (((ItemUpgrade) upgrade.Item).Quality == ItemUpgradeQualityEnum.Extra)
						result.Add(upgrade);
				}
			if (buyNormalUpgrades)
				foreach (var upgrade in upgrades) {
					if (((ItemUpgrade) upgrade.Item).Quality == ItemUpgradeQualityEnum.Normal)
						result.Add(upgrade);
				}
			foreach (var upgrade in upgrades) {
				if (((ItemUpgrade) upgrade.Item).Quality == ItemUpgradeQualityEnum.Poor)
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
			return BuyItem(item);
		}

		/// <summary>
		/// Купить предмет. возвращаем успешность покупки
		/// </summary>
		/// <returns></returns>
		internal static bool BuyItem(ItemManager item)
		{
			var cost = item.Item.Cost;
			var moneyItem = GetItemByCode(cost.Item.Code);
			return item.BuyItem(moneyItem);
		}

		public static void GrantSigns(string itemCode, int count)
		{
			var item = GetItemByCode(itemCode);
			item.GrantSigns(count);
		}

		internal static ItemManager GetItemByCode(string code)
		{
			var result = ItemsManaged.Where(it => it.Item.Code == code).ToList();
			if (result.Count > 1)
				throw new Exception("не уникальный айтем с кодом " + code);
			return result.FirstOrDefault();
		}

		private static ItemUpgradeValue CreateUpgradeValue(string name, string upName, int upValue, ItemUpgradeQualityEnum quality)
		{
			var iv = new ItemUpgradeValue() {
				Name = name,
				UpName = upName,
				UpValue = upValue,
				Quality = quality,
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
			ItemUpgradeQualityEnum quality = ItemUpgradeQualityEnum.Poor, List<ItemUpgradeValue> upgrades = null, int installOrder = 0)
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
				InstallOrder = installOrder,
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
			var i = new Item()
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