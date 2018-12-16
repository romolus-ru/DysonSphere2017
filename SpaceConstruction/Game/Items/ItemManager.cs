
using System;

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

		/// <summary>
		/// покупаем предмет
		/// </summary>
		/// <param name="moneyItem">Количество денег у игрока</param>
		public bool BuyItem(ItemManager moneyItem)
		{
			if (moneyItem.Item.Code != Item.Cost.Item.Code)
				return false;// фальшивка
			var cost = Item.Cost.PlayerCount;
			var money = moneyItem.PlayerCount;
			if (money < cost)
				return false;// мало денег

			moneyItem.PlayerCount -= cost;
			PlayerCount++;
			return true;
		}

		internal void BuySign()
		{
			if (Item.Type != ItemTypeEnum.Signs)
				return;
			PlayerCount++;
		}
	}
}