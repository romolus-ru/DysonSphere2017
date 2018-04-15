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

		private Ship _ship = null;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			SetParams(10, 10, 150, 200, "ShipPanel");

			btnMoveToBase = new ViewButton();
			AddComponent(btnMoveToBase);
			btnMoveToBase.InitButton(MoveToBase, "btnMoveToBase", "Вернуть на базу");
			btnMoveToBase.SetParams(5, 150, 140, 30, "btnMoveToBase");
			btnMoveToBase.InitTexture("textRB", "textRB");

			Checkers.AddToCheckOnce(CheckState);
		}

		public void SetShip(Ship ship)
		{
			_ship = ship;
			CheckState();
		}

		private void CheckState()
		{
			btnMoveToBase.SetVisible(_ship != null);
		}

		private void MoveToBase()
		{
			if (_ship == null) return;
			if (_ship.ShipCommand == ShipCommandEnum.ToBase) return;
			if (_ship.ShipCommand == ShipCommandEnum.NoCommand) return;
			_ship.MoveToBase();
		}
	}
}
