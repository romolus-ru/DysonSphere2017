using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.EventSystem;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Отображает кнопки управления кораблями
	/// </summary>
	public class ViewShipsPanel:ViewPanel
	{
		private ViewShipPanel _shipPanel1 = null;
		private List<Ship> _ships = null;
		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			SetParams(500, 20, 900, 220, "ShipsPanel");

			_shipPanel1 = new ViewShipPanel();
			AddComponent(_shipPanel1);
		}

		public void SetShips(List<Ship> ships)
		{
			_ships = ships;
			if (_ships.Count > 0)
				_shipPanel1.SetShip(_ships[0]);
		}
	}
}