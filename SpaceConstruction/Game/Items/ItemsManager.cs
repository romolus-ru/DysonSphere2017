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

		private const string StrUpVolumeBase = "Увеличение объема грузоперевозок";
		private static string StrUpVolumeNormal = StrUpVolumeBase + " на " + GameConstants.CargoVolumeMaxNormal + GameConstants.MeasureUnits;
		private static string StrUpVolumeGood = StrUpVolumeBase + " на " + GameConstants.CargoVolumeMaxGood + GameConstants.MeasureUnits;
		private static string StrUpVolumeExtra = StrUpVolumeBase + " на " + GameConstants.CargoVolumeMaxExtra + GameConstants.MeasureUnits;

		private const string StrUpWeightBase = "Увеличение перевозимого веса";
		private static string StrUpWeightNormal = StrUpWeightBase + " на " + GameConstants.CargoWeightMaxNormal + GameConstants.MeasureUnits;
		private static string StrUpWeightGood = StrUpWeightBase + " на " + GameConstants.CargoWeightMaxGood + GameConstants.MeasureUnits;
		private static string StrUpWeightGood2 = StrUpWeightBase + " на " + GameConstants.CargoWeightMaxGood2 + GameConstants.MeasureUnits;
		private static string StrUpWeightExtra = StrUpWeightBase + " на " + GameConstants.CargoWeightMaxExtra + GameConstants.MeasureUnits;

		//const string StrUpExp = "Увеличение опыта";
		//const string StrUpExpPlus = StrUpExp + Small;
		//const string StrUpExpExtra = StrUpExp + Big;

		//const string StrUpTeleport = "Телепорт";
		//const string StrUpTeleportDistance = "Увеличение дальности телепорта";
		//const string StrUpTeleportDistancePlus = StrUpTeleport + Small;
		//const string StrUpTeleportDistanceExtra = StrUpTeleport + Big;

		private const string StrUpEngineBase = "Увеличение мощности двигателя";
		private static string StrUpEngineNormal = StrUpEngineBase + " на " + GameConstants.EngineSpeedNormal + GameConstants.MeasurePercent;
		private static string StrUpEngineGood = StrUpEngineBase + " на " + GameConstants.EngineSpeedGood + GameConstants.MeasurePercent;
		private static string StrUpEngineExtra = StrUpEngineBase + " на " + GameConstants.EngineSpeedExtra + GameConstants.MeasurePercent;

		private const string StrUpTakeOffBase = "Увеличение скорости взлёта/посадки";
		private static string StrUpTakeOffNormal = StrUpTakeOffBase + " на " + GameConstants.TakeOffNormal + GameConstants.MeasureMSec;
		private static string StrUpTakeOffGood = StrUpTakeOffBase + " на " + GameConstants.TakeOffGood + GameConstants.MeasureMSec;
		private static string StrUpTakeOffExtra = StrUpTakeOffBase + " на " + GameConstants.TakeOffExtra + GameConstants.MeasureMSec;

		private const string StrUpLoadingBase = "Увеличение скорости загрузки/разгрузки";
		private static string StrUpLoadingNormal = StrUpLoadingBase + " на " + GameConstants.LoadingNormal + GameConstants.MeasureMSec;
		private static string StrUpLoadingGood = StrUpLoadingBase + " на " + GameConstants.LoadingGood + GameConstants.MeasureMSec;
		private static string StrUpLoadingExtra = StrUpLoadingBase + " на " + GameConstants.LoadingExtra + GameConstants.MeasureMSec;

		const string StrUpAutoPilot = "Автопилот";
		private static string StrUpAutoPilotVolume = "Уменьшение перевозимого объема на " 
		                                             + Math.Abs(GameConstants.AutopilotVolumeDecrease) + GameConstants.MeasurePercent;
		private static string StrUpAutoPilotWeight = "Уменьшение перевозимого веса на " 
		                                             + Math.Abs(GameConstants.AutopilotWeightDecrease) + GameConstants.MeasurePercent;

		internal static ItemManager GetItemManager(ItemUpgrade itemUpgrade)
		{
			foreach (var item in ItemsManaged) {
				if (item.Item == itemUpgrade)
					return item;
			}
			return null;
		}


		public static void CreateItems()
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
			var itemCost10 = new ItemManager(costSign, 10);

			#region UpgradesForShip
			var upgradeVolumeExtra = CreateUpgradeValue(StrUpVolumeExtra, "CargoVolumeMax",
				GameConstants.CargoVolumeMaxExtra, ItemUpgradeQualityEnum.Extra);
			var upgradeVolumeGood = CreateUpgradeValue(StrUpVolumeGood, "CargoVolumeMax",
				GameConstants.CargoVolumeMaxGood, ItemUpgradeQualityEnum.Good);
			var upgradeVolumeNormal = CreateUpgradeValue(StrUpVolumeNormal, "CargoVolumeMax",
				GameConstants.CargoVolumeMaxNormal, ItemUpgradeQualityEnum.Normal);

			var upgradeWeightExtra = CreateUpgradeValue(StrUpWeightExtra, "CargoWeightMax",
				GameConstants.CargoWeightMaxExtra, ItemUpgradeQualityEnum.Extra);
			var upgradeWeightGood2 = CreateUpgradeValue(StrUpWeightGood2, "CargoWeightMax",
				GameConstants.CargoWeightMaxGood2, ItemUpgradeQualityEnum.Good);
			var upgradeWeightGood = CreateUpgradeValue(StrUpWeightGood, "CargoWeightMax",
				GameConstants.CargoWeightMaxGood, ItemUpgradeQualityEnum.Good);
			var upgradeWeightNormal = CreateUpgradeValue(StrUpWeightNormal, "CargoWeightMax",
				GameConstants.CargoWeightMaxNormal, ItemUpgradeQualityEnum.Normal);

			//var upgradeExpExtra = CreateUpgradeValue(strUpExpExtra, "Exp", 7);
			//var upgradeExp = CreateUpgradeValue(strUpExp, "Exp", 5);
			//var upgradeExpPlus = CreateUpgradeValue(strUpExpPlus, "Exp", 3);

			var upgradeAutopilot = CreateUpgradeValue(StrUpAutoPilot, "AutoPilot", 1, ItemUpgradeQualityEnum.Extra);
			var upgradeAutopilotVolume = CreateUpgradeValue(StrUpAutoPilotVolume, "CargoVolumePercent",
				GameConstants.AutopilotVolumeDecrease, ItemUpgradeQualityEnum.Bad);
			var upgradeAutopilotWeight = CreateUpgradeValue(StrUpAutoPilotWeight, "CargoWeightPercent",
				GameConstants.AutopilotWeightDecrease, ItemUpgradeQualityEnum.Bad);
			//var upgradeTeleport = CreateUpgradeValue(strUpTeleport, "Teleport", 1);// по умолчанию дистанция телепорта 1

			//var upgradeTeleportDistanceExtra = CreateUpgradeValue(strUpTeleportDistanceExtra, "TeleportDistance", 3);
			//var upgradeTeleportDistance = CreateUpgradeValue(strUpTeleportDistance, "TeleportDistance", 2);
			//var upgradeTeleportDistancePlus = CreateUpgradeValue(strUpTeleportDistancePlus, "TeleportDistance", 1);

			// накапливается расстояние и когда есть возможность - корабль перемещается на 2 точки вместо одной
			var upgradeEngineExtra = CreateUpgradeValue(StrUpEngineExtra, "EngineSpeed",
				GameConstants.EngineSpeedExtra, ItemUpgradeQualityEnum.Extra);
			var upgradeEngineGood = CreateUpgradeValue(StrUpEngineGood, "EngineSpeed",
				GameConstants.EngineSpeedGood, ItemUpgradeQualityEnum.Good);
			var upgradeEngineNormal = CreateUpgradeValue(StrUpEngineNormal, "EngineSpeed",
				GameConstants.EngineSpeedNormal, ItemUpgradeQualityEnum.Normal);

			// уменьшается время взлета
			var upgradeTakeOffExtra = CreateUpgradeValue(StrUpTakeOffExtra, "TakeOff",
				GameConstants.TakeOffExtra, ItemUpgradeQualityEnum.Extra);
			var upgradeTakeOffGood = CreateUpgradeValue(StrUpTakeOffGood, "TakeOff",
				GameConstants.TakeOffGood, ItemUpgradeQualityEnum.Good);
			var upgradeTakeOffNormal = CreateUpgradeValue(StrUpTakeOffNormal, "TakeOff",
				GameConstants.TakeOffNormal, ItemUpgradeQualityEnum.Normal);

			// уменьшается время погрузки/разгрузки
			var upgradeUploadingExtra = CreateUpgradeValue(StrUpLoadingExtra, "Loading",
				GameConstants.LoadingExtra, ItemUpgradeQualityEnum.Extra);
			var upgradeUploadingGood = CreateUpgradeValue(StrUpLoadingGood, "Loading",
				GameConstants.LoadingGood, ItemUpgradeQualityEnum.Good);
			var upgradeUploadingNormal = CreateUpgradeValue(StrUpLoadingNormal, "Loading",
				GameConstants.LoadingNormal, ItemUpgradeQualityEnum.Normal);
			#endregion

			CreateUpgradeItem("Улучшение скоростных характеристик корабля", "Улучшение скоростных характеристик корабля", 
				null, itemCost10, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() {upgradeEngineExtra, upgradeTakeOffExtra, upgradeUploadingExtra});

			CreateUpgradeItem(StrUpVolumeBase, StrUpVolumeNormal, null, itemCost1, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() {upgradeVolumeNormal});
			//CreateUpgradeItem(StrUpVolumeBase, StrUpVolumeGood, null, itemCost3, ItemUpgradeQualityEnum.Good,
			//	new List<ItemUpgradeValue>() {upgradeVolumeGood, upgradeWeightNormal});
			CreateUpgradeItem(StrUpVolumeBase, StrUpVolumeExtra, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() {upgradeVolumeExtra, upgradeWeightGood});

			CreateUpgradeItem(StrUpWeightBase, StrUpWeightNormal, null, itemCost1, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() {upgradeWeightNormal});
			//CreateUpgradeItem(StrUpWeightBase, StrUpWeightGood, null, itemCost3, ItemUpgradeQualityEnum.Good,
			//	new List<ItemUpgradeValue>() {upgradeWeightGood, upgradeVolumeNormal});
			CreateUpgradeItem(StrUpWeightBase, StrUpWeightExtra, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() {upgradeWeightExtra, upgradeVolumeGood});

			/*CreateUpgradeItem(strUpExp, strUpExp, null, itemCost1, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() { upgradeExpPlus });
			CreateUpgradeItem(strUpExp, strUpExp, null, itemCost3, ItemUpgradeQualityEnum.Good,
				new List<ItemUpgradeValue>() { upgradeExp, upgradeVolumePlus });
			CreateUpgradeItem(strUpExp, strUpExp, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeExpExtra });*/

			CreateUpgradeItem(StrUpAutoPilot, StrUpAutoPilot, null, itemCost1, ItemUpgradeQualityEnum.Autopilot,
				new List<ItemUpgradeValue>() {upgradeAutopilot, upgradeAutopilotVolume, upgradeAutopilotWeight});

			CreateUpgradeItem(StrUpEngineBase, StrUpEngineNormal, null, itemCost1, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() {upgradeEngineNormal});
			//CreateUpgradeItem(StrUpEngineBase, StrUpEngineGood, null, itemCost3, ItemUpgradeQualityEnum.Good,
			//	new List<ItemUpgradeValue>() {upgradeEngineGood, upgradeTakeOffNormal});
			//CreateUpgradeItem(StrUpEngineBase, StrUpEngineExtra, null, itemCost5, ItemUpgradeQualityEnum.Extra,
			//	new List<ItemUpgradeValue>() {upgradeEngineExtra, upgradeTakeOffGood});

			//CreateUpgradeItem(StrUpTakeOffBase, StrUpTakeOffNormal, null, itemCost1, ItemUpgradeQualityEnum.Normal,
			//	new List<ItemUpgradeValue>() {upgradeTakeOffNormal});
			CreateUpgradeItem(StrUpTakeOffBase, StrUpTakeOffGood, null, itemCost3, ItemUpgradeQualityEnum.Good,
				new List<ItemUpgradeValue>() {upgradeTakeOffGood, upgradeEngineNormal});
			//CreateUpgradeItem(StrUpTakeOffBase, StrUpTakeOffExtra, null, itemCost5, ItemUpgradeQualityEnum.Extra,
			//	new List<ItemUpgradeValue>() {upgradeTakeOffExtra, upgradeEngineGood});

			//CreateUpgradeItem(StrUpLoadingBase, StrUpLoadingNormal, null, itemCost3, ItemUpgradeQualityEnum.Normal,
			//	new List<ItemUpgradeValue>() {upgradeUploadingNormal});
			//CreateUpgradeItem(StrUpLoadingBase, StrUpLoadingGood, null, itemCost3, ItemUpgradeQualityEnum.Good,
			//	new List<ItemUpgradeValue>() {upgradeUploadingGood});
			//CreateUpgradeItem(StrUpLoadingBase, StrUpLoadingExtra, null, itemCost5, ItemUpgradeQualityEnum.Extra,
			//	new List<ItemUpgradeValue>() {upgradeUploadingExtra, upgradeWeightGood2});

			CreateResearchItem("Увеличить у всех кораблей перевозимый объем",
				"Перевозимый объем у всех кораблей увеличится на " + GameConstants.ShipVolume + GameConstants.MeasureUnits,
				costSign, 5, null, "ShipVolume");
			CreateResearchItem("Увеличить у всех кораблей перевозимый вес",
				"Перевозимый вес у всех кораблей увеличится на " + GameConstants.ShipWeight + GameConstants.MeasureUnits,
				costSign, 5, null, "ShipWeight");

			CreateResearchItem("Увеличить у всех кораблей перевозимый объем 2",
				"Перевозимый объем у всех кораблей увеличится на " + GameConstants.ShipVolume2 + GameConstants.MeasureUnits,
				costSign, 25, null, "ShipVolume2");
			CreateResearchItem("Увеличить у всех кораблей перевозимый вес 2",
				"Перевозимый вес у всех кораблей увеличится на " + GameConstants.ShipWeight2 + GameConstants.MeasureUnits,
				costSign, 25, null, "ShipWeight2");

			CreateResearchItem("Дополнительные корабли", "Получить " + GameConstants.AddShips1 + " дополнительных корабля", costSign, 1, null, "AddShips1");
			CreateResearchItem("Дополнительные корабли", "Получить " + GameConstants.AddShips2 + " дополнительных корабля", costSign, 3, null, "AddShips2");
			CreateResearchItem("Дополнительные корабли", "Получить " + GameConstants.AddShips3 + " дополнительных корабля", costSign, 5, null, "AddShips3");

			//CreateResearchItem("дополнительные 3 заказа", "дополнительные 3 заказа", costSign, 15, null, "AddOrders1");
			//CreateResearchItem("дополнительные 2 заказа", "дополнительные 2 заказа", costSign, 30, null, "AddOrders2");
			//CreateResearchItem("дополнительный 1 заказ",  "дополнительный 1 заказ",  costSign, 40, null, "AddOrders3");

			CreateResearchItem("Магазин улучшений для корабля", "Доступ к магазину улучшений для корабля", costSign, 10, null, "OpenShop");
			//CreateResearchItem("Расширение ассортимента магазина", "В магазине будут доступны хорошие улучшения", costSign, 15, null, "CanBuyGoodUpgrades");
			CreateResearchItem("Расширение ассортимента магазина", "В магазине будут доступны особые улучшения", costSign, 50, null, "CanBuyExtraUpgrades");
			CreateResearchItem("Расширение ассортимента магазина", "В магазине будет доступен автопилот", costSign, 11, null, "CanBuyAutopilot");

			CreateResearchItem("Лучшие заказы", "Более сложные, но и лучше оплачиваемые заказы", costSign, 12, null, "OpenTopOrders");

			CreateResearchItem("Заключить договор на Главный заказ (на время)", "Появится кнопка запуска выполнения главного заказа", costSign, 60, null, "StartFinalOrder");

			ItemsManaged.Clear();
			foreach (var item in Items) {
				var im = new ItemManager(item, 0);
				ItemsManaged.Add(im);
			}

			var it1 = GetItemByCode("Sign1");
			ItemsManaged.Remove(it1);
			it1 = new ItemManager(costSign, 0);
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
			var cost = item.Item.Cost; // ищем предмет который является деньгами для этого предмета
			var moneyItem = GetItemByCode(cost.Item.Code);

			return item.CanBuyItem(moneyItem);
		}

		internal static List<ItemManager> GetUpgrades(bool buyGoodUpgrades, bool buyExtraUpgrades, bool buyAutopilot)
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

			if (buyGoodUpgrades)
				foreach (var upgrade in upgrades) {
					if (((ItemUpgrade) upgrade.Item).Quality == ItemUpgradeQualityEnum.Good)
						result.Add(upgrade);
				}

			foreach (var upgrade in upgrades) {
				if (((ItemUpgrade) upgrade.Item).Quality == ItemUpgradeQualityEnum.Normal)
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
			var i = new Item() {
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
			ItemUpgradeQualityEnum quality = ItemUpgradeQualityEnum.Normal, List<ItemUpgradeValue> upgrades = null, int installOrder = 0)
		{
			if (upgrades == null) return;

			var i = new ItemUpgrade() {
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
			var i = new Item() {
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