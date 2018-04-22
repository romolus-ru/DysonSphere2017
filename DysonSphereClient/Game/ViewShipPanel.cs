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
	public class ViewShipPanel : ViewPanel
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

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			if (_ship == null) return;
			string texture = null;
			string operation = "Ожидание";
			if (_ship.OrderPlanetDestination != null) {
				operation = "Перевозка";
				texture = ResourcesHelper.GetTexture(
					(_ship.OrderPlanetSource as Planet)
					.Building.BuilingType.GetResourceEnum());
			}
			if (_ship.ShipCommand == ShipCommandEnum.ToBase)
				operation = "На базу";
			if (_ship.TimeToWaitState == ShipCommandEnum.CargoLoad)
				operation = "Загрузка";
			if (_ship.TimeToWaitState == ShipCommandEnum.CargoUnload)
				operation = "Разгрузка";
			if (_ship.ShipCommand == ShipCommandEnum.NoCommand)
				texture = "Resources.Infinity";
			if (!string.IsNullOrEmpty(texture)) {
				const int size = 40;
				visualizationProvider.DrawTexturePart((Width) / 2 - size / 4, 15, texture, size, size);
			}
			var l = visualizationProvider.TextLength(operation);
			visualizationProvider.Print(Width / 2 - l / 4, 55, operation);

		}
	}
}