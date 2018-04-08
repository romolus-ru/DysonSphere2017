using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using System.Windows.Forms;
using Engine.EventSystem;
using System.Drawing;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Отображение информации о корабле и управление им
	/// </summary>
	public class ViewShipPanel:ViewPanel
	{
		private ViewButton btnMoveToBase;
		private ViewButton btnBuyShip;

		private Ship _ship = null;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			SetParams(10, 10, 150, 200, "ShipPanel");

			btnMoveToBase = new ViewButton();
			AddComponent(btnMoveToBase);
			btnMoveToBase.InitButton(MoveToBase, "btnMoveToBase", "Вернуть на базу");
			btnMoveToBase.SetParams(5, 10, 140, 30, "btnMoveToBase");
			btnMoveToBase.InitTexture("textRB", "textRB");

			btnBuyShip = new ViewButton();
			AddComponent(btnBuyShip);
			btnBuyShip.InitButton(MoveToBase, "btnBuyShip", "Купить корабль");
			btnBuyShip.SetParams(5, 80, 140, 70, "btnBuyShip");
			btnBuyShip.InitTexture("textRB", "textRB");

			Checkers.AddToCheckOnce(CheckState);
		}

		public void SetShip(Ship ship)
		{
			_ship = ship;
			CheckState();
		}

		private void CheckState()
		{
			btnMoveToBase.SetVisible(true);// _ship != null);
			btnBuyShip.SetVisible(true);// _ship == null);
		}

		private void MoveToBase()
		{
			if (_ship == null) return;
			if (_ship.ShipCommand == ShipCommandEnum.ToBase) return;
			if (_ship.ShipCommand == ShipCommandEnum.NoCommand) return;
			_ship.MoveToBase();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Print(10, 10, "TXT");

		}
	}
}
