using Engine.Visualization;
using SpaceConstruction.Game.Resources;
using SpaceConstruction.Game.States;
using System;
using System.Collections.Generic;
using System.Linq;
using SpaceConstruction.Game.Orders;
using SpaceConstruction.Game.Items;

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
		[Obsolete]
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
		public Action OnUpdateShipsPanel;
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

		private void CreateShipOne()
		{
			var ship = new Ship(_shipBase, new ResourcesHolder(_resourceInfos), _shipStates);
			ship.OnGetRoad = _onGetShipRoad;
			ship.OnRaceEnded = RaceEnd;
			ship.OnGetGlobalVolume = GlobalShipsVolume;
			ship.OnGetGlobalWeight = GlobalShipsWeight;
			ship.OnOrderEmpty = OnFinishOrder;
			ship.ShipNum = _ships.Count + 1;
			ship.UpdateShipValues();
			_ships.Add(ship);
		}

		internal void CreateShip(int count = 1)
		{
			for (int i = 0; i < count; i++)
				CreateShipOne();
			OnUpdateShipsPanel?.Invoke();
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
		[Obsolete]
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

		public int AddVolume1 { get; private set; } = 0;
		public int AddWeight1 { get; private set; } = 0;
		public int AddVolume2 { get; private set; } = 0;
		public int AddWeight2 { get; private set; } = 0;

		private int GlobalShipsVolume() => AddVolume1 + AddVolume2;
		private int GlobalShipsWeight() => AddWeight1 + AddWeight2;

		private bool _shipVolume = false;
		private bool _shipWeight = false;
		private bool _shipVolume2 = false;
		private bool _shipWeight2 = false;
		private bool _addShips1 = false;
		private bool _addShips2 = false;
		private bool _addShips3 = false;

		public void UpdateResearchInfo()
		{
			if (!_shipVolume && ItemsManager.IsResearchItemBuyed("ShipVolume")) {
				_shipVolume = true;
				AddVolume1 = 100;
				UpdateShipsValues();
			}
			if (!_shipWeight && ItemsManager.IsResearchItemBuyed("ShipWeight")) {
				_shipWeight = true;
				AddWeight1 = 100;
				UpdateShipsValues();
			}
			if (!_shipVolume2 && ItemsManager.IsResearchItemBuyed("ShipVolume2")) {
				_shipVolume2 = true;
				AddVolume2 = 100;
				UpdateShipsValues();
			}
			if (!_shipWeight2 && ItemsManager.IsResearchItemBuyed("ShipWeight2")) {
				_shipWeight2 = true;
				AddWeight2 = 100;
				UpdateShipsValues();
			}


			if (!_addShips1 && ItemsManager.IsResearchItemBuyed("AddShips1")) {
				_addShips1 = true;
				CreateShip(4);
			}
			if (!_addShips2 && ItemsManager.IsResearchItemBuyed("AddShips2")) {
				_addShips2 = true;
				CreateShip(3);
			}
			if (!_addShips3 && ItemsManager.IsResearchItemBuyed("AddShips3")) {
				_addShips3 = true;
				CreateShip(2);
			}
		}

		private bool UpdateResearchInfoShips(string upgradeName, int count)
		{
			var mItem = ItemsManager.GetResearchItem(upgradeName);
			if (mItem != null && mItem.PlayerCount > 0) {
				CreateShip(count);
				return true;
			}
			return false;
		}

		private void UpdateShipsValues()
		{
			foreach (var ship in _ships) {
				ship.UpdateShipValues();
			}
		}
	}
}