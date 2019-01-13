using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Items;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SpaceConstruction.Game.Windows
{
	internal class ShopUpgradesBuyWindow : FilteredScrollViewWindow
	{
		private List<ItemManager> _items;
		private Action _onBuy;
		private Action _onClose;
		private ItemManager _itemA; // автопилот. меняем его положение в списке, если уже куплено 10 автопилотов

		public void InitWindow(ViewManager viewManager, List<ItemManager> items, Action onBuy, Action onClose)
		{
			_onBuy = onBuy;
			_onClose = onClose;

			_items = items;
			_itemA = _items.FirstOrDefault(x => ((ItemUpgrade) x.Item).Quality == ItemUpgradeQualityEnum.Autopilot);
			_items.Remove(_itemA);

			InitWindow("Покупка улучшений для кораблей", viewManager, showOkButton: false, showCancelButton: true, showNewButton: false);
		}

		protected override void InitScrollItems()
		{
			var items = new List<ItemManager>(_items);
			if (_itemA != null) {
				if (_itemA.PlayerCount >= 10)
					items.Add(_itemA);
				else
					items.Insert(0, _itemA);
			}

			var i = 1;
			foreach (var item in items) {
				var scrollItem = new ShopUpgradesBuyScrollItem(item);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.OnAfterBuy = StartBuy;
				scrollItem.SetParams(1, 1, 980, 100, "ri" + i + " " + item.Item.Name);
				i++;
			}
		}

		private void StartBuy()
		{
			_onBuy?.Invoke();
			//ViewScroll.ClearItems();
			//InitScrollItems();
			//UpdateScrollViewSize();
		}

		protected override void InitButtonCancel(ViewButton btnCancel)
		{
			base.InitButtonCancel(btnCancel);
			btnCancel.SetCoordinatesRelative(-100, 0, 0);
			btnCancel.Caption = "закрыть";
			btnCancel.Hint = "закрыть магазин";
		}

		protected override void CloseWindow()
		{
			base.CloseWindow();
			_onClose?.Invoke();
		}

		internal void UpdateBuyButtons()
		{
			foreach (var scrollItem in ViewScroll.GetItems()) {
				var item = (ShopUpgradesBuyScrollItem) scrollItem;
				item.UpdateBuyButton();
			}
		}
	}
}