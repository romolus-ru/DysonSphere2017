using Engine;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceConstruction.Game
{
	/// <summary>
	/// Отображает кнопки управления кораблями
	/// </summary>
	public class ViewShipsPanel : ViewPanel
	{
		private List<ViewShipPanel> _shipsPanels = new List<ViewShipPanel>();
		private Ships _ships = null;
		private int _shipsCount = 0;
		private ViewButton btnBuyShip;
		public Action OnBuyShip;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			SetParams(500, 20, 900, 220, "ShipsPanel");

			btnBuyShip = new ViewButton();
			AddComponent(btnBuyShip);
			btnBuyShip.InitButton(BuyShip, "btnBuyShip", "Купить корабль");
			btnBuyShip.SetParams(160, 80, 140, 70, "btnBuyShip");
			btnBuyShip.InitTexture("textRB", "textRB");
			btnBuyShip.Enabled = false;
		}

		private void BuyShip()
		{
			OnBuyShip?.Invoke();
		}

		public void ShipBuyed()
		{
			var newCount = _ships.GetShipsCount();
			if (_shipsCount == newCount) return;
			_shipsCount = newCount;
			CreateShipPanel();
		}

		private void CreateShipPanel()
		{
			var create = _ships.GetShipsCount() - _shipsPanels.Count;
			if (create <= 0) return;
			for (int i = _shipsPanels.Count; i < _ships.GetShipsCount(); i++) {
				var shipPanel = new ViewShipPanel();
				_shipsPanels.Add(shipPanel);
				AddComponent(shipPanel);
				shipPanel.SetShip(_ships[i]);
			}
			UpdatePanelsPositions();
		}

		private void UpdatePanelsPositions()
		{
			var i = 0;
			foreach (var panel in _shipsPanels) {
				panel.SetCoordinates(10 + i * (panel.Width + 5), panel.Y);
				i++;
			}
			var last = _shipsPanels.Last();
			btnBuyShip.SetCoordinates(last.X + last.Width + 10, btnBuyShip.Y);
		}

		public void SetShips(Ships ships)
		{
			_ships = ships;
			_ships.OnShipBuyed += ShipBuyed;
			_ships.OnBuyButtonEnable += BuyButtonEnable;
			CreateShipPanel();
		}

		public void BuyButtonEnable()
		{
			btnBuyShip.Enabled = true;
		}
	}
}