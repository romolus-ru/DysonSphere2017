using Engine;
using Engine.Visualization;
using System;

namespace SpaceConstruction.Game
{
	/// <summary>
	/// Отображение информации о корабле и управление им
	/// </summary>
	public class ViewShipPanel : ViewPanel
	{
		private ViewButton btnMoveToBase;
		private ViewButton btnUpgrade;
		public Action<Ship> OnUpgradeShip = null;

		private Ship _ship = null;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			SetParams(10, 10, 150, 200, "ShipPanel");

			btnMoveToBase = new ViewButton();
			AddComponent(btnMoveToBase);
			btnMoveToBase.InitButton(MoveToBase, "btnMoveToBase", "Вернуть на базу");
			btnMoveToBase.SetParams(5, 165, 140, 30, "btnMoveToBase");
			btnMoveToBase.InitTexture("textRB", "textRB");

			btnUpgrade = new ViewButton();
			AddComponent(btnUpgrade);
			btnUpgrade.InitButton(UpgradeShip, "U", "Купить улучшения корабля");
			btnUpgrade.SetParams(120, 10, 25, 25, "btnUpgrade");
			btnUpgrade.InitTexture("textRB", "textRB");
		}

		public void SetShip(Ship ship)
		{
			_ship = ship;
		}

		private void UpgradeShip()
		{
			OnUpgradeShip?.Invoke(_ship);
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

			texture = _ship.ShipCommand == ShipCommandsEnum.NoCommand
				? "Resources.Infinity"
				: "Resources.Action";
			const int size = 40;
			visualizationProvider.DrawTexturePart(X + (Width - size) / 2, Y + 15, texture, size, size);

			var l = visualizationProvider.TextLength(operation);
			visualizationProvider.Print(X + (Width - l) / 2, Y + 55, operation);

			visualizationProvider.Print(X + 10, Y + 85, _ship.ShipCommand.ToString());
			visualizationProvider.Print(X + 10, Y + 100, _ship.CurrentState.ToString());
			visualizationProvider.Print(X + 10, Y + 115, "% " + _ship.StoredPercent);
			visualizationProvider.Print(X + 10, Y + 125, "cargo " + _ship._cargoLoaded);
			visualizationProvider.Print(X + 10, Y + 135, "space " + !_ship._shipOnPlanet);
		}
	}
}