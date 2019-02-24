using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Engine.Helpers;
using Engine.Utils;
using Newtonsoft.Json;
using Submarines.Geometry;
using Submarines.Utils;

namespace Submarines.Items
{
	/// <summary>
	/// Загрузка, инициализация и доступ к предметам
	/// (только к ItemHolder и ItemsLootContainer. ItemBase должны создаваться только тут)
	/// </summary>
	internal static class ItemsManager
	{
		private const string ItemTypesFile = DataSupportFileHelper.DataFileDirectory + GameConstants.DataDirectory +
		                                     "/ItemsTypes" + DataSupportFileHelper.DataFileExtension;

		private const string ItemSubmarinesFile = DataSupportFileHelper.DataFileDirectory + GameConstants.DataDirectory +
		                                     "/ItemsSubmarines" + DataSupportFileHelper.DataFileExtension;

		private const string GeometrySubmarinesFile = DataSupportFileHelper.DataFileDirectory + GameConstants.DataDirectory +
												  "/GeometrySubmarines" + DataSupportFileHelper.DataFileExtension;

		private const string tstFile = DataSupportFileHelper.DataFileDirectory + GameConstants.DataDirectory +
		                               "/tst" + DataSupportFileHelper.DataFileExtension;

		private static Dictionary<string, ItemBase> _items = new Dictionary<string, ItemBase>();
		private static Dictionary<string, ItemsCostContainer> _money = new Dictionary<string, ItemsCostContainer>();
		private static Dictionary<string, GeometryBase> _geometries = new Dictionary<string, GeometryBase>();

		static ItemsManager()
		{
			LoadItems();
		}

		public static bool IsInited = false;

		/// <summary>
		/// Загружаем все данные о предметах
		/// </summary>
		private static void LoadItems()
		{
			//var tst = new List<GeometryBase>();
			//tst.Add(new GeometryBase() {
			//	GeometryType = GeometryType.Hull,
			//	Name = "name",
			//	Color = Color.Red,
			//	Lines = new List<LineInfo>() {
			//		new LineInfo(new Vector(1, 1, 0), new Vector(1, 1, 1)),
			//		new LineInfo(new Vector(1, 1, 0), new Vector(1, 1, 1))
			//	}
			//});
			//var data1 = JsonConvert.SerializeObject(tst, Formatting.Indented);
			//File.WriteAllText(tstFile, data1);

			


			//загрузка линий и добавление их в корпусу для вывода на экран


			var data = FileUtils.LoadStringFromFile(GeometrySubmarinesFile);
			var listGeometries = JsonConvert.DeserializeObject<List<GeometryBase>>(data);
			foreach (var geometry in listGeometries) {
				_geometries.Add(geometry.Name, geometry);
			}

			Dictionary<string, Dictionary<string, string>> objValues = new Dictionary<string, Dictionary<string, string>>();

			data = FileUtils.LoadStringFromFile(ItemTypesFile);
			var listValues = JsonConvert.DeserializeObject<List<List<string>>>(data);
			foreach (var values in listValues) {
				AddLoadedInfoToDictionary(objValues, values);
			}

			data = FileUtils.LoadStringFromFile(ItemSubmarinesFile);
			listValues = JsonConvert.DeserializeObject<List<List<string>>>(data);
			foreach (var values in listValues) {
				AddLoadedInfoToDictionary(objValues, values);
			}

			foreach (var objData in objValues) {
				var item = CreateAndInitItem(objData.Value);
				_items.Add(item.Name, item);
			}

		}

		private static ItemBase CreateAndInitItem(Dictionary<string, string> values)
		{
			var item = CreateItem(values.GetString("ItemType").ToEnum(ItemType.Unknown));
			item.Init(values);
			return item;
		}

		private static ItemBase CreateItem(ItemType itemType)
		{
			switch (itemType) {
				case ItemType.Engine: return new ItemEngine();
				case ItemType.Hull: return new ItemHull();
				case ItemType.ManeuverDevice: return new ItemManeuverDevice();
				case ItemType.Submarine: return new ItemSubmarine();
			}

			throw new Exception("нету нужного типа " + itemType);
		}

		private static void AddLoadedInfoToDictionary(Dictionary<string, Dictionary<string, string>> objValues, List<string> values)
		{
			var dict = new Dictionary<string, string>();
			foreach (var value1 in values) {
				var pos = value1.IndexOf(":", StringComparison.Ordinal);
				var name = value1.Substring(0, pos).Trim();
				var value = value1.Substring(pos + 1).Trim();
				if (!dict.ContainsKey(name))
					dict.Add(name, value);
				else {
					Debug.WriteLine($" rewrite values {name} : {dict[name]} to {value}");
					dict[name] = value;
				}
			}

			if (!dict.ContainsKey("Name")) {
				Debug.WriteLine($"without name {string.Join(",", values)}");
				return;
			}

			var objName = dict["Name"];
			objValues.Add(objName, dict);
		}

		internal static ItemBase GetItemBase(string itemBaseName)
			=> _items.ContainsKey(itemBaseName)
				? _items[itemBaseName]
				: null;

		internal static GeometryBase GetGeometry(string geometryName)
			=> _geometries.ContainsKey(geometryName)
				? _geometries[geometryName]
				: null;

		// for editor
		internal static List<GeometryBase> GetAllGeometries()
		{
			var ret = new List<GeometryBase>();
			foreach (var geometry in _geometries) {
				ret.Add(geometry.Value);
			}
			return ret;
		}

		// for editor
		internal static void SaveGeometries()
		{
			var geometries = GetAllGeometries();
			var data = JsonConvert.SerializeObject(geometries, Formatting.Indented);
			File.WriteAllText(GeometrySubmarinesFile, data);
		}


	}
}