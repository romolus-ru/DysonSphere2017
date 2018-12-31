using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Items;
using System.Collections.Generic;
using System.Linq;

namespace SpaceConstruction.Game.Windows
{
	internal class ShopUpgradesBuyWindow : FilteredScrollViewWindow
	{
		private List<ItemManager> _items;

		public void InitWindow(ViewManager viewManager, List<ItemManager> items)
		{
			_items = items;
			InitWindow("Покупка улучшений для кораблей", viewManager, showOkButton: true, showNewButton: false, showCancelButton: false);
		}

		protected override void InitScrollItems()
		{
			var items = _items.Where(x => x.PlayerCount < 1).ToList();
			items.AddRange(_items.Where(x => x.PlayerCount > 0));
			var i = 1;
			foreach (var item in items) {
				var scrollItem = new ShopUpgradesBuyScrollItem(item);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(1, 1, 980, 100, "ri" + i + " " + item.Item.Name);
				i++;
			}
		}

		protected override void InitButtonOk(ViewButton btnOk)
		{
			base.InitButtonOk(btnOk);
			btnOk.Caption = "закрыть";
			btnOk.Hint = "закрыть магазин";
		}
	}
}