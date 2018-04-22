using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Visualization;

namespace DysonSphereClient.Game
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
		/// <summary>
		/// Корабли перевезли весь нужный груз и заказ закрыт
		/// </summary>
		public Action OnFinishOrder;
		/// <summary>
		/// Перечисление заработанных денег
		/// </summary>
		public Action<int> OnMoneyChanged;

		/// <summary>
		/// Цена корабля, с учётом уже купленных и многих других факторов
		/// </summary>
		/// <returns></returns>
		public int GetShipCost()
		{
			return 0;
		}

		public void AddShip(Ship ship) 
		{
			if (ship != null)
				_ships.Add(ship);
		}

		public void Clear() => _ships.Clear();

		public Ship GetFreeShip() =>
			_ships.Where(ship => ship.ShipCommand == ShipCommandEnum.NoCommand).FirstOrDefault();

		public int GetShipsCount() => _ships.Count;

		public IEnumerator<Ship> GetEnumerator()
		{
			return _ships.GetEnumerator();
		}

		internal void CreateShip(Func<ScreenPoint, ScreenPoint, List<ScreenPoint>> getShipRoad)
		{
			var res = GetDefaultCargoCapacity();
			var ship = new Ship(_shipBase, res);
			ship.OnGetRoad += getShipRoad;
			ship.OnRaceEnded += RaceEnd;
			_ships.Add(ship);
		}

		private Resources GetDefaultCargoCapacity()
		{
			var ret = new Resources();
			ret.Add(ResourcesEnum.RawMaterials, 500);
			ret.Add(ResourcesEnum.Consumables, 300);
			ret.Add(ResourcesEnum.Tools, 20);
			return ret;
		}

		internal void Init(Planet shipBase)
		{
			_shipBase = shipBase;
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
				money = order.Reward;
				planet.Order = null;
				planet.Building.BuilingType = BuildingEnum.Nope;
				foreach (var ship in _ships) {
					if (ship.OrderPlanetDestination != planet) continue;
					ship.MoveToBase();
				}
				OnFinishOrder.Invoke();//CreateRandomOrder();
			} else {
				money = order.RewardRace;
				order.Reward -= order.RewardRace;// если корабли получили все деньги за заказ то дальше работают бесплатно
				if (order.Reward <= 0) order.Reward = 0;

				var planetCargo = (Planet)shipEndOrder.OrderPlanetSource;
				var cargo = planetCargo.Building.BuilingType.GetResourceEnum();
				if (order.AmountResources.Value(cargo) <= 0) {
					// один из ресурсов заказа выполнен - отзываем все корабли сваязанные с этим ресурсом
					foreach (var ship in _ships) {
						if (ship.OrderPlanetDestination != planet) continue;
						if (ship.OrderPlanetSource != planetCargo) continue;
						ship.MoveToBase();
					}
				}
			}
			// сигналим сколько денег заработал корабль
			if (money != 0) OnMoneyChanged?.Invoke(money);
		}
	}
}