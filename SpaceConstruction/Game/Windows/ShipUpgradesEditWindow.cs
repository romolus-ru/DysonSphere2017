using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Items;
using System.Collections.Generic;

namespace SpaceConstruction.Game.Windows
{
	internal class ShipUpgradesEditWindow : FilteredScrollViewWindow
	{
		private Ship _ship;
		private List<ItemManager> _items;

		public void InitWindow(ViewManager viewManager, Ship ship)
		{
			_ship = ship;
			тут. заполнить список айтемов 
				вывести айтемы
				сделать диалог
			_items = new List<ItemManager>();
			foreach (var im in ItemsManager.ItemsManaged) {
				if (im.SetupCount <= 0)
					continue;

				var free = im.SetupCount - im.PlayerCount;
				if (free <= 0)
					continue;

				var item = im.Item;
				if (item.Type != ItemTypeEnum.Upgrade)
					continue;

				_items.Add(im);
			}
			InitWindow("Установка апгрейдов на корабль", viewManager, showOkButton: true, showCancelButton: false, showNewButton: false);
		}

		protected override void InitScrollItems()
		{
			/*var i = 1;
			foreach (var ri in _resourceInfos) {
				var scrollItem = new ResourcesInfosScrollItem(ri);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(1, 1, 400, 100, "ri" + i + " " + ri.Name);
				i++;
			}*/
		}
	}
}