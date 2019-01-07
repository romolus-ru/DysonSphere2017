using System;
using System.Diagnostics;
using Engine.Utils;

namespace SpaceConstruction.Game.Items
{
	/// <summary>
	/// Менеджер одного айтема, обязательно включает в себя сам предмет и его количество
	/// </summary>
	[DebuggerDisplay("itemManager {Item.Name}")]
	internal class ItemManager
	{
		/// <summary>
		/// Количество айтемов у игрока
		/// </summary>
		public SafeInt PlayerCount { get; private set; }
		/// <summary>
		/// Количество установленных айтемов
		/// </summary>
		public int SetupCount;
		/// <summary>
		/// Какой айтем хранится
		/// </summary>
		public Item Item { get; }
		/// <summary>
		/// Имя таймера блокировки покупки
		/// </summary>
		public string RestrictBuyTimerName { get; }

		public int AvailableCount
			=> PlayerCount - SetupCount;

		public ItemManager(Item item, int count, string restrictBuyTimerName = null)
		{
			Item = item;
			PlayerCount = count;
			RestrictBuyTimerName = restrictBuyTimerName;
		}

		public bool IsAvailable => AvailableCount > 0;

		/// <summary>
		/// Покупаем предмет
		/// </summary>
		/// <param name="moneyItem">Количество денег у игрока</param>
		public bool BuyItem(ItemManager moneyItem)
		{
			var canBuy = CanBuyItem(moneyItem);
			if (canBuy) {
				var cost = Item.Cost.PlayerCount;
				moneyItem.PlayerCount -= cost;
				if (moneyItem.PlayerCount < 0)
					throw new InvalidOperationException("Нельзя купить предмет!");
				PlayerCount++;
			}
			return canBuy;
		}

		/// <summary>
		/// Проверяем, можно ли купить предмет
		/// </summary>
		/// <param name="moneyItem">Количество денег у игрока</param>
		public bool CanBuyItem(ItemManager moneyItem)
		{
			if (moneyItem.Item.Code != Item.Cost.Item.Code)
				return false;
			var cost = Item.Cost.PlayerCount;
			var money = moneyItem.PlayerCount;
			return money >= cost;
		}

		/// <summary>
		/// Выдать знак
		/// </summary>
		internal void GrantSigns(int count)
		{
			if (Item.Type != ItemTypeEnum.Signs)
				return;
			PlayerCount += count;
		}
	}
}