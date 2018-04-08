using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	public class Ship
	{
		private Resources _cargo = new Resources();
		private Resources _cargoMax = null;
		public ShipCommandEnum ShipCommand;
		public Func<ScreenPoint, ScreenPoint, List<ScreenPoint>> OnGetRoad;
		public Action<Ship> OnShipEndOrder;

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

		public Ship(ScreenPoint shipBase, Resources cargoMax)
		{
			Base = shipBase;
			CurrentTarget = Base;
			_cargoMax = cargoMax;
		}

		/// <summary>
		/// Двигаться к выполнению заказа
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public void MoveToOrder(Planet start, Planet end)
		{
			if (ShipCommand == ShipCommandEnum.NoCommand) {
				//CurrentTarget = start;// начинаем движение от начальной точки
				CurrentRoad?.Clear();
			}
			ShipCommand = ShipCommandEnum.MoveToOrder;
			OrderPlanetSource = start;
			OrderPlanetDestination = end;
		}

		public void MoveToBase()
		{
			ProcessMoveToBase();
		}

		/// <summary>
		/// Двигаемся дальше по пути
		/// </summary>
		public void MoveNext()
		{
			if (ShipCommand == ShipCommandEnum.NoCommand) return;

			CurrentRoadPointNum++;
			if (CurrentRoadPointNum < (CurrentRoad?.Count ?? 0)) return;
			if (OrderEmpty()) { ShipCommand = ShipCommandEnum.ToBase; }

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
			}
		}

		private bool OrderEmpty()
		{
			var dest = (Planet)OrderPlanetDestination;
			if (dest.Order == null || dest.Order.AmountResources.IsEmpty()) return true;
			return false;
		}

		/// <summary>
		/// Вернуть корабль на базу для апгрейда
		/// </summary>
		private void ProcessMoveToBase()
		{
			if (CurrentTarget!= Base) {
				CurrentRoad = OnGetRoad?.Invoke(CurrentTarget, Base);
				CurrentTarget = Base;
				return;
			}
			ShipCommand = ShipCommandEnum.NoCommand;
		}

		/// <summary>
		/// Перевозим заказ
		/// </summary>
		private void ProcessMoveOrder()
		{
			if (CurrentTarget != OrderPlanetDestination) {
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
				OnShipEndOrder?.Invoke(this);
			} else {
				planet.Order.AmountResources.Add(cargo, -cargoCount);
				OnShipEndOrder?.Invoke(this);
				ShipCommand = ShipCommandEnum.MoveToOrder;
				ProcessMoveToOrder();
			}
		}

		/// <summary>
		/// Двигаемся к заказу и переходим на режим перевозки заказа
		/// </summary>
		private void ProcessMoveToOrder()
		{
			if (CurrentTarget != OrderPlanetSource) {
				CurrentRoad = OnGetRoad?.Invoke(CurrentTarget, OrderPlanetSource);
				CurrentTarget = OrderPlanetSource;
				return;
			}
			// корабль на планете откуда перевозят ресурсы - запускаем заказ
			ShipCommand = ShipCommandEnum.Ordered;
			ProcessMoveOrder();
		}
	}
}