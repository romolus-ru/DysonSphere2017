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
		public ShipCommandEnum TimeToWaitState = ShipCommandEnum.NoCommand;
		public int ShipNum;

		public Ship(ScreenPoint shipBase, Resources cargoMax)
		{
			TimeToWaitMax = 10;
			TimeToWaitCurrent = 0;
			Base = shipBase;
			CurrentTarget = Base;
			_cargoMax = cargoMax;
		}

		/// <summary>
		/// Двигаться к выполнению заказа
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns>Успешно ли запустили</returns>
		public bool MoveToOrder(Planet start, Planet end)
		{
			if (ShipCommand == ShipCommandEnum.NoCommand) {
				//CurrentTarget = start;// начинаем движение от начальной точки
				CurrentRoad?.Clear();
			}
			if (end != null) {
				var cargo = start.Building.BuilingType.GetResourceEnum();
				var needCount = end.Order.AmountResources.Value(cargo);
				if (needCount <= 0) return false;
			}
			ShipCommand = ShipCommandEnum.MoveToOrder;
			OrderPlanetSource = start;
			OrderPlanetDestination = end;
			return true;
		}

		public void MoveToBase()
		{
			ShipCommand = ShipCommandEnum.ToBasePrepare;
		}
		
		/// <summary>
		/// Двигаемся дальше по пути
		/// </summary>
		public void MoveNext()
		{
			if (ShipCommand == ShipCommandEnum.NoCommand) return;

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

			if (ShipCommand == ShipCommandEnum.ToBasePrepare)
				ShipCommand = ShipCommandEnum.ToBase;

			CurrentRoadPointNum++;
			if (CurrentRoadPointNum < (CurrentRoad?.Count ?? 0)) return;
			if (OrderEmpty()) { ShipCommand = ShipCommandEnum.ToBase; }
			if (ShipCommand== ShipCommandEnum.ToBasePrepare) {
				ShipCommand = ShipCommandEnum.ToBase;
			}

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
			if (CurrentTarget!= Base) {
				TimeToWaitState = ShipCommandEnum.CargoUnload;
				CurrentRoad = OnGetRoad?.Invoke(CurrentTarget, Base);
				CurrentTarget = Base;
				OrderPlanetSource = null;
				OrderPlanetDestination = null;
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
			}
		}

		/// <summary>
		/// Двигаемся к заказу и переходим в режим перевозки заказа
		/// </summary>
		private void ProcessMoveToOrder()
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
		}
	}
}