using Engine.Visualization;
using SpaceConstruction.Game.Resources;
using SpaceConstruction.Game.States;
using System;
using System.Collections.Generic;
using System.Linq;
using SpaceConstruction.Game.Orders;

namespace SpaceConstruction.Game
{
	/// <summary>
	/// Централизация работы с кораблями (покупка, задание команд)
	/// </summary>
	public class Ships
	{
		private List<Ship> _ships = new List<Ship>();
		/// <summary>
		/// Планета базирования кораблей
		/// </summary>
		private Planet _shipBase;
		private ShipStates _shipStates = new ShipStates();
		/// <summary>
		/// Корабли перевезли весь нужный груз и заказ закрыт
		/// </summary>
		public Action OnFinishOrder;
		/// <summary>
		/// Перечисление заработанных денег
		/// </summary>
		public Action<int> OnRaceEnded;
		/// <summary>
		/// Разрешаем кнопку покупки
		/// </summary>
		public Action OnBuyButtonEnable;
		private bool BuyButtonActive = false;
		private Func<ScreenPoint, ScreenPoint, List<ScreenPoint>> _onGetShipRoad;
		private int _currentMaxShips = 0;
		private const int _defaultMaxShips = 1;
		private int _tutorialAddShips = 0;
		public Action OnShipBuyed;
		private Orders.Orders _orders;
		private List<ResourceInfo> _resourceInfos;

		public Ships(Orders.Orders orders)
		{
			_orders = orders;
			_resourceInfos = _orders.ResourceInfos;
		}

		/// <summary>
		/// Цена корабля, с учётом уже купленных и многих других факторов
		/// </summary>
		/// <returns></returns>
		public int GetShipCost()
		{
			return 3;
		}

		public void Clear() => _ships.Clear();

		public Ship GetFreeShip() =>
			_ships.FirstOrDefault(ship => ship.ShipCommand == ShipCommandsEnum.NoCommand);

		public int GetShipsCount() => _ships.Count;

		public IEnumerator<Ship> GetEnumerator()
		{
			return _ships.GetEnumerator();
		}

		public Ship this[int i] {
			get { return _ships[i]; }
		}

		internal void CreateShip()
		{
			var res = GetDefaultCargoCapacity();
			var ship = new Ship(_shipBase, new ResourcesHolder(_resourceInfos), res, _shipStates);
			ship.OnGetRoad = _onGetShipRoad;
			ship.OnRaceEnded = RaceEnd;
			ship.ShipNum = _ships.Count + 1;
			_ships.Add(ship);
			OnShipBuyed?.Invoke();
		}

		private ResourcesHolder GetDefaultCargoCapacity()
		{
			var ret = new ResourcesHolder(_resourceInfos);
			// теперь будет задаваться объем трюма на корабле
			return ret;
		}

		internal void Init(Planet shipBase, Func<ScreenPoint, ScreenPoint, List<ScreenPoint>> getShipRoad)
		{
			_currentMaxShips = 1;
			_shipBase = shipBase;
			_onGetShipRoad = getShipRoad;
			// создаём первый корабль
			CreateShip();
		}

		/// <summary>
		/// Запускаем корабль
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns>если false - то все корабли заняты</returns>
		internal bool SendShip(Planet start, Planet end)
		{
			Ship ship = GetFreeShip();
			if (ship == null) return false;
			return ship.MoveToOrder(start, end);
		}

		/// <summary>
		/// Корабль прилетел к планете назначению и разгрузился
		/// </summary>
		/// <param name="shipEndOrder"></param>
		private void RaceEnd(Ship shipEndOrder)
		{
			var money = 0;
			var planet = ((Planet)shipEndOrder.OrderPlanetDestination);
			var order = planet.Order;
			if (order == null) return;
			if (order.AmountResources.IsEmpty()) {
				// заказ выполнен - отзываем все корабли связанные с этим заказом
				//money = order.Reward;
				planet.Order = null;
				//planet.Building.BuilingType = BuildingEnum.Nope;
				foreach (var ship in _ships) {
					if (ship.OrderPlanetDestination != planet) continue;
					ship.MoveToBasePrepare();
				}
				OnFinishOrder.Invoke();//CreateRandomOrder();
			} else {
				//money = order.GetRewardForRace();
				var planetCargo = (Planet)shipEndOrder.OrderPlanetSource;
				/*var cargo = planetCargo.Building.BuilingType.GetResourceEnum();
				if (order.AmountResources.Value(cargo) <= 0) {
					// один из ресурсов заказа выполнен - отзываем все корабли связанные с этим ресурсом
					foreach (var ship in _ships) {
						if (ship.OrderPlanetDestination != planet) continue;
						if (ship.OrderPlanetSource != planetCargo) continue;
						ship.MoveToBasePrepare();
					}
				}*/
			}
			// сигналим сколько денег заработал корабль
			if (money > 0) OnRaceEnded?.Invoke(money);
		}

		public void BuyShip()
		{
			CreateShip();
		}
	}
}