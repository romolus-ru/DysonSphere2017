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
		
		private Func<ScreenPoint, ScreenPoint, List<ScreenPoint>> _onGetShipRoad;
		private const int _defaultMaxShips = 1;
		public Action OnUpdateShipsPanel;
		private Orders.Orders _orders;
		private List<ResourceInfo> _resourceInfos;

		public Ships(Orders.Orders orders)
		{
			_orders = orders;
			_resourceInfos = _orders.ResourceInfos;
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