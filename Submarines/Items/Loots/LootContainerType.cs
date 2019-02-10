namespace Submarines.Items.Loots
{
	/// <summary>
	/// Тип контейнера с лутом
	/// </summary>
	internal enum LootContainerType
	{
		Unknown,
		/// <summary>
		/// Небольшой контейнер, может храниться на складе и открываться когда хочется
		/// </summary>
		Small,
		/// <summary>
		/// Требует открытия - содержимое может упасть в океан
		/// </summary>
		NeedOpen,
	}
}