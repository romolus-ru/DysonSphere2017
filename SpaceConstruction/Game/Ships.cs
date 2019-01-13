using Engine.Visualization;
using SpaceConstruction.Game.Items;
using SpaceConstruction.Game.Resources;
using SpaceConstruction.Game.States;
using System;
using System.Collections.Generic;
using System.Linq;

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
		
		private Func<ScreenPoint, ScreenPoint, List<ScreenPoint>> _onGetShipRoad;
		private List<ResourceInfo> _resourceInfos;

		public Ships(Orders.Orders orders)
		{
			_resourceInfos = orders.ResourceInfos;
		}

		public void Clear()
		{
			_ships.Clear();
			_shipVolume = false;
			_shipWeight = false;
			_shipVolume2 = false;
			_shipWeight2 = false;
			_addShips1 = false;
			_addShips2 = false;
			_addShips3 = false;
		}

		public Ship GetFreeShip() =>
			_ships.FirstOrDefault(ship => ship.ShipCommand == ShipCommandsEnum.NoCommand);

		public int GetShipsCount() => _ships.Count;

		public IEnumerator<Ship> GetEnumerator()
		{
			return _ships.GetEnumerator();
		}

		public Ship this[int i] => _ships[i];

		private void CreateShipOne()
		{
			var ship = new Ship(_shipBase, new ResourcesHolder(_resourceInfos), _shipStates);
			ship.OnGetRoad = _onGetShipRoad;
			ship.OnGetGlobalVolume = GlobalShipsVolume;
			ship.OnGetGlobalWeight = GlobalShipsWeight;
			ship.OnOrderEmpty = OnFinishOrder;
			ship.ShipNum = _ships.Count + 1;
			ship.UpdateShipValues();
			_ships.Add(ship);
		}

		internal void CreateShip(int count = 1)
		{
			for (var i = 0; i < count; i++)
				CreateShipOne();
		}

		internal void Init(Planet shipBase, Func<ScreenPoint, ScreenPoint, List<ScreenPoint>> getShipRoad)
		{
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
			var ship = GetFreeShip();
			return ship?.MoveToOrder(start, end) ?? false;
		}

		public int AddVolume1 { get; private set; }
		public int AddWeight1 { get; private set; }
		public int AddVolume2 { get; private set; }
		public int AddWeight2 { get; private set; }

		private int GlobalShipsVolume() => AddVolume1 + AddVolume2;
		private int GlobalShipsWeight() => AddWeight1 + AddWeight2;

		private bool _shipVolume;
		private bool _shipWeight;
		private bool _shipVolume2;
		private bool _shipWeight2;
		private bool _addShips1;
		private bool _addShips2;
		private bool _addShips3;

		public void UpdateResearchInfo()
		{
			if (!_shipVolume && ItemsManager.IsResearchItemBuyed("ShipVolume")) {
				_shipVolume = true;
				AddVolume1 = GameConstants.ShipVolume;
				UpdateShipsValues();
			}
			if (!_shipWeight && ItemsManager.IsResearchItemBuyed("ShipWeight")) {
				_shipWeight = true;
				AddWeight1 = GameConstants.ShipWeight;
				UpdateShipsValues();
			}
			if (!_shipVolume2 && ItemsManager.IsResearchItemBuyed("ShipVolume2")) {
				_shipVolume2 = true;
				AddVolume2 = GameConstants.ShipVolume2;
				UpdateShipsValues();
			}
			if (!_shipWeight2 && ItemsManager.IsResearchItemBuyed("ShipWeight2")) {
				_shipWeight2 = true;
				AddWeight2 = GameConstants.ShipWeight2;
				UpdateShipsValues();
			}

			if (!_addShips1 && ItemsManager.IsResearchItemBuyed("AddShips1")) {
				_addShips1 = true;
				CreateShip(GameConstants.AddShips1);
			}
			if (!_addShips2 && ItemsManager.IsResearchItemBuyed("AddShips2")) {
				_addShips2 = true;
				CreateShip(GameConstants.AddShips2);
			}
			if (!_addShips3 && ItemsManager.IsResearchItemBuyed("AddShips3")) {
				_addShips3 = true;
				CreateShip(GameConstants.AddShips3);
			}
		}

		public void CancelOrder(Planet destination)
		{
			foreach (var ship in _ships) {
				if (ship.OrderPlanetDestination != destination)
					continue;
				ship.MoveToBasePrepare();
			}
		}

		private void UpdateShipsValues()
		{
			foreach (var ship in _ships) {
				ship.UpdateShipValues();
			}
		}

		/// <summary>
		/// Для редактирования кораблей
		/// </summary>
		/// <param name="ship"></param>
		/// <returns></returns>
		public Ship GetNextShip(Ship ship)
		{
			int num = 0;
			if (ship != null) {
				num = _ships.IndexOf(ship);
				num++;
			}

			if (num >= _ships.Count)
				num = 0;
			return _ships[num];
		}

		/// <summary>
		/// Для редактирования кораблей
		/// </summary>
		/// <param name="ship"></param>
		/// <returns></returns>
		public Ship GetPrevShip(Ship ship)
		{
			var num = _ships.IndexOf(ship);
			num--;
			if (num < 0)
				num = _ships.Count - 1;
			return _ships[num];
		}
	}
}