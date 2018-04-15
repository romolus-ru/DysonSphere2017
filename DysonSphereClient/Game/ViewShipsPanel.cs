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
		private Ships _ships = null;
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
			
			_shipPanel1 = new ViewShipPanel();
			AddComponent(_shipPanel1);
		}

		private void BuyShip()
		{
			OnBuyShip?.Invoke();
		}

		public void SetShips(Ships ships)
		{
			_ships = ships;
			_shipPanel1.SetShip(_ships.GetFreeShip());
		}
	}
}