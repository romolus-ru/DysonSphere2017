using Engine.Visualization;
using System.Collections.Generic;
using System.Drawing;

namespace SpaceConstruction.Game.Windows
{
	internal class ShipUpgradesViewShipInfo : ViewComponent
	{
		private Ship _ship;
		private List<string> _info = new List<string>();

		public void SetShip(Ship ship) => _ship = ship;

		public void UpdateShipInfo()
		{
			_ship.UpdateShipValues();
			_info.Clear();

			_info.Add("Cкорость взлёта/посадки");
			_info.Add(_ship.TimeLandingUp.TotalMilliseconds + GameConstants.MeasureMSec);

			_info.Add("Cкорость погрузки/разгрузки");
			_info.Add(_ship.TimeLoading.TotalMilliseconds + GameConstants.MeasureMSec);

			_info.Add("Объем перевозимого груза");
			_info.Add(_ship.CargoVolumeMax + GameConstants.MeasureUnits);

			_info.Add("Вес перевозимого груза");
			_info.Add(_ship.CargoWeightMax + GameConstants.MeasureUnits);

			//_info.Add("Опыт за рейс");
			//_info.Add(_ship.XPAdd.ToString());

			_info.Add("Усиление двигателя");
			_info.Add(_ship.EngineForce + GameConstants.MeasurePercent);

			_info.Add("Автопилот " + (_ship.AutoPilot ? "установлен" : "отсутствует"));

			/*if (_ship.TeleportInstalled) {
				_info.Add("Телепорт установлен");
				_info.Add("Дистанция " + _ship.TeleportDistance);
			} else
				_info.Add("Телепорт отсутствует");*/

		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			if (_info.Count <= 0)
				return;
			base.DrawObject(visualizationProvider);
			int row = 0;
			visualizationProvider.SetColor(Color.Bisque);
			foreach (var str in _info) {
				visualizationProvider.Print(10, 10 + row * 15, str);
				row++;
			}
		}
	}
}