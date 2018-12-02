using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Items;
using System.Collections.Generic;
using System;

namespace SpaceConstruction.Game.Windows
{
	internal class ShipUpgradesEditWindow : FilteredScrollViewWindow
	{
		private Ship _ship;
		private List<ItemManager> _items;
		private ShipUpgradesViewShipInfo _viewShip;
		private List<ShipUpgradesLinkedItem> _shipUpgradesView = new List<ShipUpgradesLinkedItem>();
		private List<ShipUpgradesScrollItem> _shipUpgradesScrollItems = new List<ShipUpgradesScrollItem>();

		public void InitWindow(ViewManager viewManager, Ship ship)
		{
			ViewManager = viewManager;
			_ship = ship;
			_items = new List<ItemManager>();
			foreach (var im in ItemsManager.ItemsManaged) {
				if (im.PlayerCount <= 0)
					continue;

				var free = im.PlayerCount - im.SetupCount;
				if (free <= 0)
					continue;

				var item = im.Item;
				if (item.Type != ItemTypeEnum.Upgrade)
					continue;

				_items.Add(im);
			}
			InitWindow("Установка апгрейдов на корабль", viewManager, showOkButton: true, showCancelButton: false, showNewButton: false);

			_viewShip = new ShipUpgradesViewShipInfo(_ship);
			AddComponent(_viewShip);
			ShipUpgradesViewUpdate();
		}

		protected override void InitScrollItems()
		{
			var i = 1;
			foreach (var item in _items) {
				var scrollItem = new ShipUpgradesScrollItem(item);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(1, 1, 350, 100, "su" + i + " " + item.Item.Name);
				scrollItem.MoveUpgradeToShip = MoveUpgradeToShip;
				_shipUpgradesScrollItems.Add(scrollItem);
				i++;
			}
		}

		private void MoveUpgradeToShip(ShipUpgradesScrollItem scrollItem)
		{
			var item = scrollItem._itemManager;
			item.SetupCount++;
			_ship.Upgrades.Add(item.Item);
			ShipUpgradesViewUpdate();
		}

		private void MoveUpgradeToInventory(ItemUpgrade itemUpgrade)
		{
			тут. перенести улучшение обратно
			_ship.Upgrades.Remove(itemUpgrade);
			ShipUpgradesViewUpdate();
		}

		protected override void InitWindow(FilteredScrollViewWindow window, string header)
		{
			base.InitWindow(window, header);
			for (int i = 0; i < 5; i++) {
				var su = new ShipUpgradesLinkedItem();
				AddComponent(su);
				su.SetParams(350, 10 + i * 22, 300, 20, "su" + 1);
				su.OnRemoveItem += MoveUpgradeToInventory;
				_shipUpgradesView.Add(su);
			}
		}

		private void ShipUpgradesViewUpdate()
		{
			for (int i = 0; i < _shipUpgradesView.Count; i++) {
				var su = _shipUpgradesView[i];
				ItemUpgrade u =
					i < _ship.Upgrades.Count
					? _ship.Upgrades[i] as ItemUpgrade
					: null;
				su.SetUpgrade(u);
			}
			var shipFilled = _ship.Upgrades.Count > 4;
			foreach (var su in _shipUpgradesScrollItems) {
				var active = su._itemManager.IsActive && !shipFilled;
				su.SetCurrentState(active, isInventory: false);
			}
			_viewShip.UpdateShipInfo();
		}

		protected override void InitFilter(ViewInput filter)
		{
			base.InitFilter(filter);
			filter.SetParams(350, 150, 600, 30, "filter");
		}
		
		protected override void InitViewScroll(ViewScroll viewScroll)
		{
			base.InitViewScroll(viewScroll);
			viewScroll.SetParams(250, 200, 800, 460, "ViewScroll");
		}
	}
}