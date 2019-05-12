using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Submarines.Utils;

namespace Submarines.Items
{
	/// <summary>
	/// Основной класс для хранения информации о предмете
	/// </summary>
	internal class ItemBase
	{
		/// <summary>
		/// Можно ли купить этот предмет
		/// </summary>
		public bool IsCanBuy { get; private set; }

		/// <summary>
		/// Группа равнозначности - что бы можно было не показывать несколько одинаковых предметов а только 1 нужный в данный момент
		/// </summary>
		public string EqualityGroup { get; private set; }
		/// <summary>
		/// Группа предмета (мета-тип, например корпус и оружие относятся к снаряжению подлодки)
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public ItemGroup ItemGroup { get; protected set; }
		/// <summary>
		/// Тип предмета
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public ItemType ItemType { get; protected set; }
		/// <summary>
		/// Нестандартный тип (NonTypical)
		/// </summary>
		public string ItemCustomType { get; protected set; }
		/// <summary>
		/// Неизменяемый предмет. пока под вопросом, может быть и так ничего нельзя будет менять
		/// </summary>
		public bool IsImmutable { get; private set; }
		/// <summary>
		/// Ценность предмета
		/// </summary>
		public ItemRarity ItemRarity { get; protected set; }
		public ItemsCostContainer Cost { get; private set; }

		public string Name { get; private set; }

		public string Description { get; private set; }

		/// <summary>
		/// Отладочная информация
		/// </summary>
		public string DebugDescription { get; private set; }

		internal virtual void Init(Dictionary<string, string> values)
		{
			EqualityGroup = values.GetString("EqualityGroup");
			ItemGroup = values.GetString("ItemGroup").ToEnum(ItemGroup.Unknown);
			ItemType = values.GetString("ItemType").ToEnum(ItemType.Unknown);
			ItemCustomType = values.GetString("ItemCustomType");
			IsImmutable = values.GetString("IsImmutable").ToBool();
			ItemRarity = values.GetString("ItemRarity").ToEnum(ItemRarity.Unknown);
			//Cost = cost; цена по другому высчитывается. скорее всего будет искаться по коду готовая цена (которая может меняться в зависимости от внешних условий)
			Name = values.GetString("Name");
			Description = values.GetString("Description");
			DebugDescription = values.GetString("DebugDescription");
		}
	}
}