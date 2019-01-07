using Engine.Visualization;
using System;
using System.Collections.Generic;

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
		public Action<Ship> OnUpgradeShip = null;

		public void CreateNewShipPanels()
		{
			if (GameState.IsResearchesOpen)
				return;
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
				shipPanel.OnUpgradeShip = OnUpgradeShip;
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
		}

		public void SetShips(Ships ships)
		{
			foreach (var shipPanel in _shipsPanels)
				RemoveComponent(shipPanel);
			_shipsPanels.Clear();
			_ships = ships;
			CreateShipPanel();
		}
	}
}