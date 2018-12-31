using Engine.Visualization;
using SpaceConstruction.Game.Items;
using SpaceConstruction.Game.Resources;
using SpaceConstruction.Game.States;
using System;
using System.Collections.Generic;

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
		public double EngineForce { get; private set; }
		private double EngineForceDefault = 1;
		public bool AutoPilot { get; private set; }

		private ResourcesHolder _cargoCurrent = null;

		public Func<ScreenPoint, ScreenPoint, List<ScreenPoint>> OnGetRoad;
		[Obsolete]
		public Action<Ship> OnRaceEnded;

		/// <summary>
		/// Текущий путь между двух планет
		/// </summary>
		public List<ScreenPoint> CurrentRoad;
		/// <summary>
		/// Текущая точка пути
		/// </summary>
		public int CurrentRoadPointNum;
		public List<Planet> OrderPlanets;
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
		private TimeSpan _timePassed = new TimeSpan();
		/// <summary>
		/// Сколько длится текущее состояние
		/// </summary>
		private TimeSpan _timeCurrentPass = new TimeSpan();
		/// <summary>
		/// Время для состояния взлёта/посадки
		/// </summary>
		public TimeSpan TimeLandingUp { get; private set; }
		/// <summary>
		/// Время для состояния взлёта/посадки по умолчанию
		/// </summary>
		private TimeSpan _timeLandingUpDefault = new TimeSpan(0, 0, 0, 0, milliseconds: 1500);
		/// <summary>
		/// Время для состояния загрузки/разгрузки
		/// </summary>
		public TimeSpan TimeLoading { get; private set; }
		/// <summary>
		/// Время для состояния загрузки/разгрузки по умолчанию
		/// </summary>
		private TimeSpan _timeLoadingDefault = new TimeSpan(0, 0, 0, 0, milliseconds: 1500);
		/// <summary>
		/// Предыдущее время расчета
		/// </summary>
		private DateTime _timeStore = DateTime.Now;
		private bool _needStateChange = false;

		/// <summary>
		/// Текущая команда корабля
		/// </summary>
		public ShipCommandsEnum ShipCommand { get { return _shipCommand; } }
		private ShipCommandsEnum _shipCommand = ShipCommandsEnum.NoCommand;

		/// <summary>
		/// Текущий список состояний корабля
		/// </summary>
		private List<ShipStatesEnum> ListStates = new List<ShipStatesEnum>();
		public ShipStatesEnum CurrentState = ShipStatesEnum.NoCommand;

		private ShipStates _statesProcessor = null;
		public bool _cargoLoaded = false;
		public bool _shipOnPlanet = true;
		public int StoredPercent = 0;

		internal List<Item> Upgrades = new List<Item>();

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
			CurrentState = ListStates[0];
			ListStates.RemoveAt(0);

			PreProcessCommands(ShipCommand, CurrentState);
		}

		/// <summary>
		/// Команды выполняемые при завершении состояния
		/// </summary>
		private void PostProcessCommands(ShipCommandsEnum oldShipCommand, ShipStatesEnum currentState)
		{
			if (currentState == ShipStatesEnum.Unloading) {
				(OrderPlanetDestination as Planet).Order.UnloadToPlanetStore(_cargoCurrent);
				_cargoCurrent.Clear();
				_cargoLoaded = false;
				if ((OrderPlanetDestination as Planet).Order.AmountResources.IsEmpty()) {
					MoveToBasePrepare();
					OnOrderEmpty?.Invoke();
				}
			}
			if (currentState == ShipStatesEnum.Takeoff)
				_shipOnPlanet = false;

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
				(OrderPlanetDestination as Planet).Order.LoadToShipStore(_cargoCurrent, CargoVolumeMax, CargoWeightMax);
				if (_cargoCurrent.IsEmpty()) {// если нечего загружать то ведём корабль на базу
					MoveToBasePrepare();
					return;
				}
				_cargoLoaded = true;
			}
			if (CurrentState == ShipStatesEnum.Landing)
				_shipOnPlanet = true;

			_timeStore = DateTime.Now;
			_timePassed = TimeSpan.Zero;
			_timeCurrentPass = TimeSpan.Zero;// _timeLandingUp;
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

		/// <summary>
		/// Основной цикл движения корабля
		/// </summary>
		public void ProcessMove()
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
			if (ListStates.Count == 0) {// получаем следующую команду
				_statesProcessor.SetChainCommand(out _shipCommand, _shipCommand, ListStates);
			}

			if (_shipCommand == ShipCommandsEnum.NoCommand)
				return;

			// получаем следующую команду из списка команд
			InitNextCommand();
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
			_statesProcessor.SwitchCommandTo(_shipCommand, ListStates, _cargoLoaded, _shipOnPlanet);
			InitNextCommand();

			return true;
		}

		public void MoveToBasePrepare()
		{
			if (!_cargoCurrent.IsEmpty()) {
				(OrderPlanetDestination as Planet).Order.CancelShipDelivery(_cargoCurrent);
				_cargoCurrent.Clear();
				_cargoLoaded = false;
			}

			if (_shipCommand == ShipCommandsEnum.CargoDelivery)
				_currentPlanet = OrderPlanetDestination;
			if (_shipCommand == ShipCommandsEnum.GetCargo)
				_currentPlanet = OrderPlanetSource;

			_shipCommand = ShipCommandsEnum.MoveToBase;
			_statesProcessor.SwitchCommandTo(_shipCommand, ListStates, _cargoLoaded, _shipOnPlanet);
			if (!IsStarFly())
				InitNextCommand();
		}

		/// <summary>
		/// Двигаемся дальше по пути
		/// </summary>
		public void MoveNext()
		{
			ProcessMove();
		}

		/// <summary>
		/// Обновляем данные о корабле - вместимость, скорость и т.п.
		/// </summary>
		public void UpdateShipValues()
		{
			CargoVolumeMax = 100 + OnGetGlobalVolume();// что бы влезала по умолчанию 1 единица самого большого груза
			CargoWeightMax = 50 + OnGetGlobalWeight();
			TeleportInstalled = false;
			TeleportDistance = 1;
			AutoPilot = false;
			
			TimeLandingUp = _timeLandingUpDefault;
			TimeLoading = _timeLoadingDefault;
			EngineForce = EngineForceDefault;
			XPAdd = XPAddDefault;

			foreach (ItemUpgrade upgrade in Upgrades) {
				SetupUpgrade(upgrade);
			}
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
						TimeLandingUp += new TimeSpan(0, 0, upgradeValue.UpValue);
						break;
					case "Uploading":
						TimeLoading += new TimeSpan(0, 0, upgradeValue.UpValue);
						break;
					default:
						break;
				}
			}
		}
	}
}