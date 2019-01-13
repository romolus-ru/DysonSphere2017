using Engine.Visualization;
using SpaceConstruction.Game.Items;
using SpaceConstruction.Game.Resources;
using SpaceConstruction.Game.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceConstruction.Game
{
	public class Ship
	{
		/// <summary>
		/// Заказ полностью выполнен
		/// </summary>
		public Action OnOrderEmpty = null;
		public Func<int> OnGetGlobalVolume = null;
		public Func<int> OnGetGlobalWeight = null;
		public int CargoVolumeMax { get; private set; }
		public int CargoWeightMax { get; private set; }
		public int XPAdd { get; private set; }
		private int XPAddDefault = 25;
		public bool TeleportInstalled { get; private set; }
		public int TeleportDistance { get; private set; }
		public int EngineForce { get; private set; }
		private int _currentEngineForce;
		public bool AutoPilot { get; private set; }

		private ResourcesHolder _cargoCurrent;
		public Func<ScreenPoint, ScreenPoint, List<ScreenPoint>> OnGetRoad;

		/// <summary>
		/// Текущий путь между двух планет
		/// </summary>
		public List<ScreenPoint> CurrentRoad;
		/// <summary>
		/// Текущая точка пути
		/// </summary>
		public int CurrentRoadPointNum;
		/// <summary>
		/// Планета базирования кораблей
		/// </summary>
		public ScreenPoint Base;
		public ScreenPoint CurrentTarget;
		private ScreenPoint _currentPlanet;// текущее место нахождения корабля
		public ScreenPoint OrderPlanetSource;
		public ScreenPoint OrderPlanetDestination;

		public int ShipNum;

		/// <summary>
		/// Сколько времени прошло с начала состояния
		/// </summary>
		private TimeSpan _timePassed;
		/// <summary>
		/// Сколько длится текущее состояние
		/// </summary>
		private TimeSpan _timeCurrentPass;
		/// <summary>
		/// Время для состояния взлёта/посадки
		/// </summary>
		public TimeSpan TimeLandingUp { get; private set; }
		/// <summary>
		/// Время для состояния взлёта/посадки по умолчанию
		/// </summary>
		private TimeSpan _timeLandingUpDefault = new TimeSpan(0, 0, 0, 0, milliseconds: 2000);
		private TimeSpan _timeLandingUpMin = new TimeSpan(0, 0, 0, 0, milliseconds: 100);
		/// <summary>
		/// Время для состояния загрузки/разгрузки
		/// </summary>
		public TimeSpan TimeLoading { get; private set; }
		/// <summary>
		/// Время для состояния загрузки/разгрузки по умолчанию
		/// </summary>
		private TimeSpan _timeLoadingDefault = new TimeSpan(0, 0, 0, 0, milliseconds: 2000);
		private TimeSpan _timeLoadingMin = new TimeSpan(0, 0, 0, 0, milliseconds: 100);
		/// <summary>
		/// Предыдущее время расчета
		/// </summary>
		private DateTime _timeStore = DateTime.Now;
		private bool _needStateChange;

		/// <summary>
		/// Текущая команда корабля
		/// </summary>
		public ShipCommandsEnum ShipCommand => _shipCommand;

		private ShipCommandsEnum _shipCommand = ShipCommandsEnum.NoCommand;

		/// <summary>
		/// Текущий список состояний корабля
		/// </summary>
		private List<ShipStatesEnum> _listStates = new List<ShipStatesEnum>();
		public ShipStatesEnum CurrentState = ShipStatesEnum.NoCommand;

		private ShipStates _statesProcessor;
		public bool CargoLoaded;
		public bool ShipOnPlanet = true;
		public int StoredPercent;

		internal List<ItemUpgrade> Upgrades = new List<ItemUpgrade>();

		private void ProcessTime()
		{
			const int percentMax = 100;
			var dt = DateTime.Now;
			var ts = dt - _timeStore;
			_timePassed += ts;
			_timeStore = dt;
			int percent = (int)(percentMax * _timePassed.TotalMilliseconds / _timeCurrentPass.TotalMilliseconds);
			if (percent > percentMax) {
				percent = percentMax;
				_needStateChange = true;
			}
			StoredPercent = percent;
		}

		private void ProcessFly()
		{
			// полет между планетами
			CurrentRoadPointNum++;
			_currentEngineForce += EngineForce;
			while (_currentEngineForce>100) {
				CurrentRoadPointNum++;
				_currentEngineForce -= 100;
			}
			if (CurrentRoadPointNum >= (CurrentRoad?.Count ?? 0)) {
				_needStateChange = true;
			}
		}

		/// <summary>
		/// Полет в космосе
		/// </summary>
		/// <returns></returns>
		private bool IsStarFly()
			=> CurrentState == ShipStatesEnum.MoveCargo
			|| CurrentState == ShipStatesEnum.MoveToCargo
			|| CurrentState == ShipStatesEnum.MoveToBase;

		private void InitNextCommand()
		{
			CurrentState = _listStates[0];
			_listStates.RemoveAt(0);

			PreProcessCommands(ShipCommand, CurrentState);
		}

		/// <summary>
		/// Команды выполняемые при завершении состояния
		/// </summary>
		private void PostProcessCommands(ShipCommandsEnum oldShipCommand, ShipStatesEnum currentState)
		{
			if (currentState == ShipStatesEnum.Unloading) {
				var order = ((Planet) OrderPlanetDestination).Order;
				order.UnloadToPlanetStore(_cargoCurrent);
				_cargoCurrent.Clear();
				CargoLoaded = false;
				if (order.AmountResources.IsEmpty()) {
					MoveToBasePrepare();
					OnOrderEmpty?.Invoke();
				}
			}
			if (currentState == ShipStatesEnum.Takeoff)
				ShipOnPlanet = false;

			// сохраняем текущее местоположение корабля
			if (currentState == ShipStatesEnum.Landing) {
				if (oldShipCommand == ShipCommandsEnum.MoveToBase)
					_currentPlanet = Base;
				if (oldShipCommand == ShipCommandsEnum.CargoDelivery)
					_currentPlanet = OrderPlanetDestination;
				if (oldShipCommand == ShipCommandsEnum.GetCargo)
					_currentPlanet = OrderPlanetSource;
			}
		}

		/// <summary>
		/// Команды выполняемые при наступлении состояния
		/// </summary>
		private void PreProcessCommands(ShipCommandsEnum shipCommand, ShipStatesEnum currentState)
		{
			if (CurrentState == ShipStatesEnum.Loading) {// загружаем груз
				((Planet) OrderPlanetDestination).Order.LoadToShipStore(_cargoCurrent, CargoVolumeMax, CargoWeightMax);
				if (_cargoCurrent.IsEmpty()) {// если нечего загружать то ведём корабль на базу
					MoveToBasePrepare();
					return;
				}
				CargoLoaded = true;
			}
			if (CurrentState == ShipStatesEnum.Landing)
				ShipOnPlanet = true;

			_timeStore = DateTime.Now;
			_timePassed = TimeSpan.Zero;
			_timeCurrentPass = TimeSpan.Zero;
			if (currentState == ShipStatesEnum.Landing || currentState == ShipStatesEnum.Takeoff)
				_timeCurrentPass = TimeLandingUp;
			if (currentState == ShipStatesEnum.Loading || currentState == ShipStatesEnum.Unloading)
				_timeCurrentPass = TimeLoading;

			if (shipCommand == ShipCommandsEnum.MoveToBase || shipCommand == ShipCommandsEnum.NoCommand) {
				OrderPlanetDestination = null;
				OrderPlanetSource = null;
			}

			if (IsStarFly()) {// формируем путь полета
				ScreenPoint from = _currentPlanet;
				ScreenPoint to = null;
				if (CurrentState == ShipStatesEnum.MoveCargo) {
					to = OrderPlanetDestination;
				}
				if (CurrentState == ShipStatesEnum.MoveToCargo) {
					to = OrderPlanetSource;
				}
				if (CurrentState == ShipStatesEnum.MoveToBase) {
					to = Base;
				}
				CurrentRoad = OnGetRoad?.Invoke(from, to);
			}
		}

		public Ship(ScreenPoint shipBase, ResourcesHolder shipWarehouse, ShipStates statesProcessor)
		{
			Base = shipBase;
			_currentPlanet = Base;
			CurrentTarget = Base;
			_cargoCurrent = shipWarehouse;
			_statesProcessor = statesProcessor;
		}

		/// <summary>
		/// Двигаться к выполнению заказа
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns>Успешно ли запустили</returns>
		public bool MoveToOrder(Planet start, Planet end)
		{
			_shipCommand = ShipCommandsEnum.GetCargo;
			OrderPlanetSource = start;
			OrderPlanetDestination = end;
			_statesProcessor.SwitchCommandTo(_shipCommand, _listStates, CargoLoaded, ShipOnPlanet);
			InitNextCommand();

			return true;
		}

		public void MoveToBasePrepare()
		{
			if (!_cargoCurrent.IsEmpty()) {
				((Planet) OrderPlanetDestination).Order.CancelShipDelivery(_cargoCurrent);
				_cargoCurrent.Clear();
				CargoLoaded = false;
			}

			if (_shipCommand == ShipCommandsEnum.CargoDelivery)
				_currentPlanet = OrderPlanetDestination;
			if (_shipCommand == ShipCommandsEnum.GetCargo)
				_currentPlanet = OrderPlanetSource;

			_shipCommand = ShipCommandsEnum.MoveToBase;
			_statesProcessor.SwitchCommandTo(_shipCommand, _listStates, CargoLoaded, ShipOnPlanet);
			if (!IsStarFly())
				InitNextCommand();
		}

		/// <summary>
		/// Двигаемся дальше по пути
		/// </summary>
		public void MoveNext()
		{
			if (ShipCommand == ShipCommandsEnum.NoCommand)
				return;

			if (!_needStateChange) {
				if (IsStarFly())
					ProcessFly();
				else
					ProcessTime();
				return;
			}

			PostProcessCommands(ShipCommand, CurrentState);

			_needStateChange = false;
			CurrentRoadPointNum = 0;
			if (_listStates.Count == 0) {// получаем следующую команду
				_statesProcessor.SetChainCommand(out _shipCommand, _shipCommand, _listStates);
			}

			if (_shipCommand == ShipCommandsEnum.NoCommand)
				return;

			// получаем следующую команду из списка команд
			InitNextCommand();
		}

		/// <summary>
		/// Обновляем данные о корабле - вместимость, скорость и т.п.
		/// </summary>
		public void UpdateShipValues()
		{
			CargoVolumeMax = 100 + OnGetGlobalVolume(); // что бы влезала по умолчанию 1 единица самого большого груза
			CargoWeightMax = 90 + OnGetGlobalWeight();
			TeleportInstalled = false;
			TeleportDistance = 1;
			AutoPilot = false;

			TimeLandingUp = _timeLandingUpDefault;
			TimeLoading = _timeLoadingDefault;
			EngineForce = 0;
			XPAdd = XPAddDefault;

			foreach (ItemUpgrade upgrade in Upgrades.OrderBy(u => u.InstallOrder)) {
				SetupUpgrade(upgrade);
			}

			if (TimeLandingUp < _timeLandingUpMin)
				TimeLandingUp = _timeLandingUpMin;
			if (TimeLoading < _timeLoadingMin)
				TimeLoading = _timeLoadingMin;
		}

		private void SetupUpgrade(ItemUpgrade upgrade)
		{
			foreach (ItemUpgradeValue upgradeValue in upgrade.Upgrades) {
				switch (upgradeValue.UpName) {
					case "CargoVolumeMax":
						CargoVolumeMax += upgradeValue.UpValue;
						break;
					case "CargoWeightMax":
						CargoWeightMax += upgradeValue.UpValue;
						break;
					case "CargoVolumePercent":
						CargoVolumeMax += (CargoVolumeMax * upgradeValue.UpValue / 100);
						break;
					case "CargoWeightPercent":
						CargoWeightMax += (CargoWeightMax * upgradeValue.UpValue / 100);
						break;
					case "Exp":
						XPAdd += upgradeValue.UpValue;
						break;
					case "AutoPilot":
						AutoPilot = true;
						break;
					case "Teleport":
						TeleportInstalled = true;
						break;
					case "TeleportDistance":
						TeleportDistance += upgradeValue.UpValue;
						break;
					case "EngineSpeed":
						EngineForce += upgradeValue.UpValue;
						break;
					case "TakeOff":
						TimeLandingUp -= new TimeSpan(0, 0, 0, 0, milliseconds: upgradeValue.UpValue);
						break;
					case "Loading":
						TimeLoading -= new TimeSpan(0, 0, 0, 0, milliseconds: upgradeValue.UpValue);
						break;
				}
			}
		}
	}
}