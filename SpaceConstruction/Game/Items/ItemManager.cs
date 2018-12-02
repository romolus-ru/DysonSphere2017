
namespace SpaceConstruction.Game.Items
{
	/// <summary>
	/// Менеджер одного айтема, обязательно включает в себя сам предмет и его количество
	/// </summary>
	internal class ItemManager
	{
		/// <summary>
		/// Количество айтемов у игрока
		/// </summary>
		public int PlayerCount { get; private set; }
		/// <summary>
		/// Количество установленных айтемов
		/// </summary>
		public int SetupCount;
		/// <summary>
		/// Какой айтем хранится
		/// </summary>
		public Item Item { get; private set; }
		/// <summary>
		/// Имя таймера блокировки покупки
		/// </summary>
		public string RestrictBuyTimerName { get; private set; }

		public ItemManager(Item item, int count, string restrictBuyTimerName = null)
		{
			Item = item;
			PlayerCount = count;
			RestrictBuyTimerName = restrictBuyTimerName;
		}

		public bool IsActive {
			get {
				return PlayerCount - SetupCount > 0;
			}
		}
	}
}