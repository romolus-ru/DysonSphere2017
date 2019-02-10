using System.Collections.Generic;

namespace Submarines.Items.Loots
{
	/// <summary>
	/// Хранит несколько предметов
	/// </summary>
	internal class ItemsLootContainer
	{
		/// <summary>
		/// Контейнер с жёстко ограниченным уровнем лута
		/// т.е. сначала смотрится выпадение более ценных предметов
		/// и далее суммируется стоимость контейнера и если
		/// общее значение превышает предельное - предметы не выдаются
		/// </summary>
		public bool StrictlyContainer { get; private set; }

		public int StrictlyLimit { get; private set; } = 0;

		/// <summary>
		/// Предмет-описатель для лутконтейнера
		/// </summary>
		public ItemBase ItemLootContainer { get; private set; }

		public LootContainerType ContainerType { get; private set; }

		public string ContainerName { get; private set; }
		private List<ItemLootHolder> _items = new List<ItemLootHolder>();

		public int ItemsCount => _items.Count;

		public ItemLootHolder this[int i] => _items[i];
	}
}