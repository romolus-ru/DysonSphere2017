using Engine.Visualization;
using SpaceConstruction.Game.Resources;
using SpaceConstruction.Game.States;
using System;
using System.Collections.Generic;

namespace SpaceConstruction.Game
{
	public class Ship
	{
		public Action<int> OnUpdateOperationProgress = null;
		private ResourcesHolder _cargoMax = null;

		public Func<ScreenPoint, ScreenPoint, List<ScreenPoint>> OnGetRoad;
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
		public ScreenPoint OrderPlanetSource;
		public ScreenPoint OrderPlanetDestination;
		/// <summary>
		/// Время загрузки/разгрузки
		/// </summary>
		public int TimeToWaitMax;
		public int TimeToWaitCurrent;
		public ShipCommandsEnum TimeToWaitState = ShipCommandsEnum.NoCommand;
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
		/// Время для состояния взлёта
		/// </summary>
		private TimeSpan _timeLandingUp = new TimeSpan(0, 0, 5);
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
		public List<ShipStatesEnum> ListStates = new List<ShipStatesEnum>();
		public ShipStatesEnum _currentState = ShipStatesEnum.NoCommand;

		private ShipStates _statesProcessor = null;
		private bool _shipOnPlanet = false;
		private bool _shipOnBase = true;
		private bool _shipInSpace = false;
		public int StoredPercent = 0;

		private void ProcessTime()
		{
			const int percentMax= 100;
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
			OnUpdateOperationProgress?.Invoke(percent);
		}
		
		private void ProcessFly()
		{
			// полет между планетами
			тут
		}

		private void ProcessState()
		{
			if (!_needStateChange) {
				if (IsStarFly())
					ProcessFly();
				else
					ProcessTime();
				return;
			}

			PostProcessCommands(ShipCommand, _currentState);

			_needStateChange = false;
			if (ListStates.Count == 0) {// получаем следующую команду
				_statesProcessor.SetChainCommand(out _shipCommand, _shipCommand, ListStates);
			}

			if (_shipCommand == ShipCommandsEnum.NoCommand)
				return;

			// получаем следующую команду из списка команд
			InitNextCommand();

		}

		private bool IsStarFly() 
			=> _currentState == ShipStatesEnum.MoveCargo
			|| _currentState == ShipStatesEnum.MoveToCargo
			|| _currentState == ShipStatesEnum.MoveToBase;

		private void InitNextCommand()
		{
			_currentState = ListStates[0];
			ListStates.RemoveAt(0);

			PreProcessCommands(ShipCommand, _currentState);
		}

		// возможно часть надо перенести в ShipState - что бы там менялось состояние и при необходимости вызывались команды для корабля и для заказов изменения были
		/// <summary>
		/// Команды выполняемые при завершении состояния
		/// </summary>
		private void PostProcessCommands(ShipCommandsEnum shipCommand, ShipStatesEnum currentState)
		{
			// для разгрузки срабатывает уже после "разгрзуки" - и из трюма выкладывается заказчику
			// а для загрузки сразу в preprocess - потащили со склада к кораблю и загружаем
		}

		/// <summary>
		/// Команды выполняемые при наступлении состояния
		/// </summary>
		private void PreProcessCommands(ShipCommandsEnum shipCommand, ShipStatesEnum currentState)
		{
			_timePassed = new TimeSpan();
			_timeStore = DateTime.Now;
			_timeCurrentPass = _timeLandingUp;// для примера. для каждого состояния желательно ввести своё отдельное время
			if (IsStarFly()) {// формируем путь полета
				тут
			}
		}

		/// <summary>
		/// Основной цикл движения корабля
		/// </summary>
		public void ProcessMove()
		{
			if (ShipCommand == ShipCommandsEnum.NoCommand)
				return;
			ProcessState();
		}


		public Ship(ScreenPoint shipBase, ResourcesHolder cargoMax, ShipStates statesProcessor)
		{
			TimeToWaitMax = 10;
			TimeToWaitCurrent = 0;
			Base = shipBase;
			CurrentTarget = Base;
			_cargoMax = cargoMax;
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
			_statesProcessor.SwitchCommandTo(_shipCommand, ListStates);
			InitNextCommand();

			/*if (ShipCommand == ShipCommandEnum.NoCommand) {
				//CurrentTarget = start;// начинаем движение от начальной точки
				CurrentRoad?.Clear();
			}
			if (end != null) {
				var cargo = start.Building.BuilingType.GetResourceEnum();
				var needCount = end.Order.AmountResources.Value(cargo);
				if (needCount <= 0) return false;
			}*/
			return true;
		}

		public void MoveToBasePrepare()
		{
			/*ShipCommand = ShipCommandEnum.ToBase;
			if (CurrentRoadPointNum == -1)
				ProcessMoveToBase();
			*/
		}

		/// <summary>
		/// Двигаемся дальше по пути
		/// </summary>
		public void MoveNext()
		{
			ProcessMove();
			/*if (ShipCommand == ShipCommandEnum.NoCommand) return;

			if (TimeToWaitState == ShipCommandEnum.CargoLoad) {
				TimeToWaitCurrent++;
				if (TimeToWaitCurrent >= TimeToWaitMax)
					TimeToWaitState = ShipCommandEnum.NoCommand;
				if (ShipCommand == ShipCommandEnum.ToBasePrepare)
					TimeToWaitState = ShipCommandEnum.CargoUnload;
				return;
			}
			if (TimeToWaitState == ShipCommandEnum.CargoUnload) {
				TimeToWaitCurrent--;
				if (TimeToWaitCurrent <= 0)
					TimeToWaitState = ShipCommandEnum.NoCommand;
				return;
			}

			CurrentRoadPointNum++;
			if (CurrentRoadPointNum < (CurrentRoad?.Count ?? 0)) return;
			if (OrderEmpty()) { ShipCommand = ShipCommandEnum.ToBase; }
			if (ShipCommand == ShipCommandEnum.ToBasePrepare)
				ShipCommand = ShipCommandEnum.ToBase;

			CurrentRoadPointNum = -1;
			switch (ShipCommand) {
				case ShipCommandEnum.MoveToOrder:
					ProcessMoveToOrder();
					break;
				case ShipCommandEnum.Ordered:
					ProcessMoveOrder();
					break;
				case ShipCommandEnum.ToBase:
					ProcessMoveToBase();
					break;
				default:
					break;
			}*/
		}

		/*private bool OrderEmpty()
		{
			if (OrderPlanetDestination == null) return false;
			var dest = (Planet)OrderPlanetDestination;
			if (dest.Order == null || dest.Order.AmountResources.IsEmpty()) return true;
			return false;
		}

		/// <summary>
		/// Вернуть корабль на базу
		/// </summary>
		private void ProcessMoveToBase()
		{
			if (CurrentTarget != Base) {
				TimeToWaitState = ShipCommandEnum.CargoUnload;
				CurrentRoad = OnGetRoad?.Invoke(CurrentTarget, Base);
				CurrentTarget = Base;
				OrderPlanetSource = null;
				OrderPlanetDestination = null;
				return;
			}
			ShipCommand = ShipCommandEnum.NoCommand;
		}
		*/
		/// <summary>
		/// Перевозим заказ
		/// </summary>
		private void ProcessMoveOrder()
		{
			/*if (CurrentTarget != OrderPlanetDestination) {
				TimeToWaitState = ShipCommandEnum.CargoLoad;
				CurrentRoad = OnGetRoad(CurrentTarget, OrderPlanetDestination);
				CurrentTarget = OrderPlanetDestination;
				return;
			}

			// выкладываем груз
			var planet = (OrderPlanetDestination as Planet);
			var planetCargo = (OrderPlanetSource as Planet);
			var cargo = planetCargo.Building.BuilingType.GetResourceEnum();
			var cargoCount = _cargoMax.Value(cargo);
			var orderCount = planet.Order.AmountResources.Value(cargo);
			if (orderCount <= cargoCount) {
				planet.Order.AmountResources.Add(cargo, -orderCount);
				// раз ресурсов нету то там надо отправить все задействованные корабли назад
				OnRaceEnded?.Invoke(this);
			} else {
				planet.Order.AmountResources.Add(cargo, -cargoCount);
				OnRaceEnded?.Invoke(this);
				ShipCommand = ShipCommandEnum.MoveToOrder;
				ProcessMoveToOrder();
			}*/
		}

		/// <summary>
		/// Двигаемся к заказу и переходим в режим перевозки заказа
		/// </summary>
		/*private void ProcessMoveToOrder()
		{
			if (CurrentTarget != OrderPlanetSource) {
				TimeToWaitState = ShipCommandEnum.CargoUnload;
				CurrentRoad = OnGetRoad?.Invoke(CurrentTarget, OrderPlanetSource);
				CurrentTarget = OrderPlanetSource;
				return;
			}
			// корабль на планете откуда перевозят ресурсы - запускаем заказ
			ShipCommand = ShipCommandEnum.Ordered;
			ProcessMoveOrder();
		}*/
	}
}