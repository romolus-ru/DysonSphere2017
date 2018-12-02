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
		public static List<ItemManager> ItemsManaged = new List<ItemManager>();

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

		const string strUpAutoPilot = "Автоматическая система обработки заказов";

		private static void CreateItems()
		{
			Items.Clear();

			var costSign=
			CreateItemSigns("Знак выполненного контракта", "Знак выполненного контракта");
			CreateItemSigns("Особый знак выполненного контракта", "Особый знак выполненного контракта");

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

			CreateUpgradeItem(strUpExp, strUpExp, null, itemCost1, ItemUpgradeQualityEnum.Poor,
				new List<ItemUpgradeValue>() { upgradeExpPlus });
			CreateUpgradeItem(strUpExp, strUpExp, null, itemCost3, ItemUpgradeQualityEnum.Normal,
				new List<ItemUpgradeValue>() { upgradeExp, upgradeVolumePlus });
			CreateUpgradeItem(strUpExp, strUpExp, null, itemCost5, ItemUpgradeQualityEnum.Extra,
				new List<ItemUpgradeValue>() { upgradeExpExtra });

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


			ItemsManaged.Clear();
			foreach (var item in Items) {
				var im = new ItemManager(item, 1);
				ItemsManaged.Add(im);
			}

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
		private static Item CreateItemSigns(string itemName, string itemDescription, string texture = null, ItemManager cost = null)
		{
			var i = new Item()
			{
				Type = ItemTypeEnum.Signs,
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
	}
}