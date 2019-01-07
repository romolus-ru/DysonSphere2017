namespace SpaceConstruction.Game.Items
{
	/// <summary>
	/// Одно улучшение корабля
	/// </summary>
	internal class ItemUpgradeValue
	{
		public string Name;
		/// <summary>
		/// Имя улучшаемого значения
		/// </summary>
		public string UpName;
		/// <summary>
		/// Величина улучшаемого значения
		/// </summary>
		public int UpValue;
		/// <summary>
		/// Качество улучшения для отображения в магазине
		/// </summary>
		public ItemUpgradeQualityEnum Quality;
	}
}