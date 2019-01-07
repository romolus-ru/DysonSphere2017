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
		private ViewButton _btnMoveToBase;
		public Action<Ship> OnUpgradeShip;

		private Ship _ship;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			SetParams(10, 10, 150, 200, "ShipPanel");

			_btnMoveToBase = new ViewButton();
			AddComponent(_btnMoveToBase);
			_btnMoveToBase.InitButton(MoveToBase, "На базу", "Вернуть корабль на базу");
			_btnMoveToBase.SetParams(5, 165, 140, 30, "btnMoveToBase");
			_btnMoveToBase.InitTexture("textRB", "textRB");

			var btnUpgrade = new ViewButtonPic();
			AddComponent(btnUpgrade);
			btnUpgrade.InitButton(UpgradeShip, "U", "Установка улучшений корабля");
			btnUpgrade.SetParams(120, 10, 25, 25, "btnUpgrade");
			btnUpgrade.InitTexture("textRB", "textRB");
			btnUpgrade.SetPic("Resources.Tools", 0.5f);
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

			var texture = _ship.ShipCommand == ShipCommandsEnum.NoCommand
				? "Resources.Infinity"
				: "Resources.Action";
			const int size = 50;
			visualizationProvider.DrawTexturePart(X + (Width - size) / 2 - 20, Y, texture, size, size);

			visualizationProvider.Print(X + 10, Y + 55, "объем: " + _ship.CargoVolumeMax);
			visualizationProvider.Print(X + 10, Y + 70, "вес  : " + _ship.CargoWeightMax);
			visualizationProvider.Print(X + 10, Y + 85, _ship.ShipCommand.ToString());
			visualizationProvider.Print(X + 10, Y + 100, _ship.CurrentState.ToString());
			visualizationProvider.Print(X + 10, Y + 115, "% " + _ship.StoredPercent);
			visualizationProvider.Print(X + 10, Y + 125, "загружен: " + _ship.CargoLoaded);
			visualizationProvider.Print(X + 10, Y + 135, "полёт: " + !_ship.ShipOnPlanet);
		}
	}
}