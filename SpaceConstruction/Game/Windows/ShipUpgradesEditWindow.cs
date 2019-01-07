using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Items;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SpaceConstruction.Game.Windows
{
	internal class ShipUpgradesEditWindow : FilteredScrollViewWindow
	{
		private Ships _ships;
		private Ship _ship;
		private ShipUpgradesViewShipInfo _viewShip;
		private List<ShipUpgradesLinkedItem> _shipUpgradesView = new List<ShipUpgradesLinkedItem>();
		private List<ShipUpgradesScrollItem> _shipUpgradesScrollItems = new List<ShipUpgradesScrollItem>();

		public void InitWindow(ViewManager viewManager, Ships ships, Ship ship)
		{
			ViewManager = viewManager;
			_ships = ships;
			_ship = ship ?? _ships.GetNextShip(null);

			InitWindow("Установка улучшений на корабль", viewManager, showOkButton: true, showCancelButton: false, showNewButton: false);

			_viewShip = new ShipUpgradesViewShipInfo();
			_viewShip.SetShip(_ship);
			AddComponent(_viewShip);
			ShipUpgradesViewUpdate();
		}

		protected override void InitScrollItems()
		{
			var items = new List<ItemManager>();
			foreach (var im in ItemsManager.ItemsManaged) {
				if (im.PlayerCount <= 0
				    || im.AvailableCount <= 0
				    || im.Item.Type != ItemTypeEnum.Upgrade)
					continue;

				items.Add(im);
			}

			ViewScroll.ClearItems();
			var i = 1;
			foreach (var item in items) {
				var scrollItem = new ShipUpgradesScrollItem(item);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(1, 1, 350, 100, "su" + i + " " + item.Item.Name);
				scrollItem.OnMoveUpgradeToShip = MoveUpgradeToShip;
				_shipUpgradesScrollItems.Add(scrollItem);
				scrollItem.ActivateButton();
				i++;
			}
		}

		private void InitLinkScrollItems()
		{
			for (int i = 0; i < 5; i++) {
				var su = new ShipUpgradesLinkedItem();
				AddComponent(su);
				su.SetParams(350, 10 + i * 22, 300, 20, "su" + 1);
				su.OnRemoveItem += MoveUpgradeToInventory;
				su.ActivateButton();
				_shipUpgradesView.Add(su);
			}
		}

		private void MoveUpgradeToShip(ShipUpgradesScrollItem scrollItem)
		{
			var item = scrollItem.ItemManager;
			item.SetupCount++;
			_ship.Upgrades.Add(item.Item as ItemUpgrade);
			ShipUpgradesViewUpdate();
		}

		private void MoveUpgradeToInventory(ItemUpgrade itemUpgrade)
		{
			ItemManager item = ItemsManager.GetItemManager(itemUpgrade);
			item.SetupCount--;
			_ship.Upgrades.Remove(itemUpgrade);
			ShipUpgradesViewUpdate();
		}

		protected override void InitWindow(FilteredScrollViewWindow window, string header)
		{
			base.InitWindow(window, header);
			InitLinkScrollItems();

			var btnNextShip = new ViewButton();
			AddComponent(btnNextShip);
			btnNextShip.InitButton(NextShip, "Следующий корабль", "Переключиться на следующий корабль", Keys.Right);
			btnNextShip.SetParams(10, 235, 200, 20, "btnNextShip");
			btnNextShip.InitTexture("textRB", "textRB");

			var btnPrevShip = new ViewButton();
			AddComponent(btnPrevShip);
			btnPrevShip.InitButton(PrevShip, "Предыдущий корабль", "Переключиться на предыдущий корабль", Keys.Left);
			btnPrevShip.SetParams(10, 260, 200, 20, "btnPrevShip");
			btnPrevShip.InitTexture("textRB", "textRB");

		}

		private void ShipUpgradesViewUpdate()
		{
			for (int i = 0; i < _shipUpgradesView.Count; i++) {
				var su = _shipUpgradesView[i];
				ItemUpgrade u =
					i < _ship.Upgrades.Count
					? _ship.Upgrades[i]
					: null;
				su.SetUpgrade(u);
			}
			var shipFilled = _ship.Upgrades.Count > 4;
			foreach (var su in _shipUpgradesScrollItems) {
				var active = su.ItemManager.IsAvailable && !shipFilled;
				su.SetCurrentState(active);
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

		private void NextShip()
		{
			SwitchTo(_ships.GetNextShip(_ship));
		}

		private void PrevShip()
		{
			SwitchTo(_ships.GetPrevShip(_ship));
		}

		private void SwitchTo(Ship ship)
		{
			_ship = ship;
			_viewShip.SetShip(_ship);
			ViewScroll.ClearItems();
			InitScrollItems();
			UpdateScrollViewSize();
			ShipUpgradesViewUpdate();
		}
	}
}