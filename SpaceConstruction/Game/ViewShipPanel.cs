using Engine;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceConstruction.Game
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

			//Checkers.AddToCheckOnce(CheckState);
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
			if (_ship.ShipCommand == ShipCommandsEnum.MoveToBase) return;
			if (_ship.ShipCommand == ShipCommandsEnum.NoCommand) return;
			_ship.MoveToBasePrepare();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			if (_ship == null) return;
			string texture = null;
			string operation = "---";
			/*if (_ship.OrderPlanetDestination != null) {
				operation = "Перевозка";
				texture = ResourcesHelper.GetTexture(
					(_ship.OrderPlanetSource as Planet)
					.Building.BuilingType.GetResourceEnum());
			}
			if ((_ship.ShipCommand == ShipCommandEnum.ToBase) || (_ship.ShipCommand == ShipCommandEnum.ToBasePrepare))
				operation = "На базу";
			if (_ship.ShipCommand == ShipCommandEnum.ToBasePrepare)
				operation = "Готовимся";
			if (_ship.TimeToWaitState == ShipCommandEnum.CargoLoad)
				operation = "Загрузка";
			if (_ship.TimeToWaitState == ShipCommandEnum.CargoUnload)
				operation = "Разгрузка";
			if (_ship.ShipCommand == ShipCommandEnum.NoCommand)*/
				texture = "Resources.Infinity";
			if (!string.IsNullOrEmpty(texture)) {
				const int size = 40;
				visualizationProvider.DrawTexturePart(X + (Width - size) / 2, Y + 15, texture, size, size);
			}
			var l = visualizationProvider.TextLength(operation);
			visualizationProvider.Print(X + (Width - l) / 2, Y + 55, operation);

			visualizationProvider.Print(X + 10, Y + 70, "ИД=" + _ship.ShipNum.ToString());
			visualizationProvider.Print(X + 10, Y + 85, "Команда " + _ship.ShipCommand);
			visualizationProvider.Print(X + 10, Y + 100, "Состояние " + _ship._currentState);
			visualizationProvider.Print(X + 10, Y + 115, "% " + _ship.StoredPercent);
			visualizationProvider.Print(X + 10, Y + 125, "cargo " + _ship._cargoLoaded);
			visualizationProvider.Print(X + 10, Y + 135, "space " + !_ship._shipOnPlanet);
		}
	}
}