namespace Submarines.Items.Loots
{
	/// <summary>
	/// Хранит характеристики предмета входящего в состав лутконтейнера
	/// </summary>
	internal class ItemLootHolder : ItemHolder
	{
		/// <summary>
		/// Порядок предмета в наборе - может влиять наа расположение предмета в корпусе корабля
		/// </summary>
		public int Order { get;private set; }

		/// <summary>
		/// Минимальное количество предметов
		/// </summary>
		public int CountMin { get; private set; }

		/// <summary>
		/// Максимальное количество предметов
		/// </summary>
		public int CountMax { get; private set; }

		/// <summary>
		/// Шанс выпадения предмета для данного лутконтейнера
		/// </summary>
		public float Chance { get; private set; }

		/// <summary>
		/// Использовать ли рандом для изменения количества данного предмета
		/// (например для монет надо чтоб игрок получил от 0 до 100 монет)
		/// </summary>
		/// <remarks>должна использоваться отдельная последовательность рандома для чисел от 0 до 1000</remarks>
		public bool UseRandomForItems { get; private set; }

		/// <summary>
		/// Название группы для расчёта вероятности у данной группы предметов
		/// </summary>
		/// <remarks>Например </remarks>
		public string RandomCodeGroup { get; private set; }

		/// <summary>
		/// Стоимость одного предмета при подсчете выпадения лута (для возможности или предупреждения выпадения множества ценных предметов одновременно)
		/// </summary>
		public int LootCost { get; private set; }
	}
}