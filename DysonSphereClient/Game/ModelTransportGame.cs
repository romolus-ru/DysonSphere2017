using DysonSphereClient.Game.Resource;
using Engine.Helpers;
using Engine.Models;
using Engine.Visualization;
using Engine.Visualization.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	internal class ModelTransportGame : Model
	{
		public delegate void SetPointsDelegate(List<Planet> points, List<ScreenEdge> mst, Ships ships);
		private List<Planet> RoadPoints = new List<Planet>();
		private List<ScreenEdge> RoadMST = new List<ScreenEdge>();
		private Ships _ships;
		private Orders _orders = new Orders();
		private Paths _paths = new Paths();
		public SetPointsDelegate OnSetPoints;
		public Action<int> OnMoneyChanged;
		public Action OnOrdersChanged;

		public ModelTransportGame(Ships ships)
		{
			_ships = ships;
			_ships.OnFinishOrder = CreateRandomOrders;
			_ships.OnRaceEnded = MoneyChanged;
		}

		/// <summary>
		/// Минимальное расстояние на котором точка будет реагировать на курсор
		/// </summary>
		private const int MouseMinimalDistance = 50;
		private int Money = 0;

		public void RecreatePoints()
		{
			var RoadEdges = new List<ScreenEdge>();
			RoadPoints.Clear();
			RoadPoints.AddRange(_paths.CreateGalaxy(70, 1200, 700, 100));
			InitFillResources(RoadPoints);
			var tmpPoints = new List<Vertex>();
			foreach (var point in RoadPoints) {
				var p = new Vertex(point);
				tmpPoints.Add(p);
			}
			var triangulator = new Triangulator();
			var edges = triangulator.Triangulation(tmpPoints, true);
			foreach (var item in edges) {
				var pa = tmpPoints[item.a].LinkPoint;
				var pb = tmpPoints[item.b].LinkPoint;
				var pc = tmpPoints[item.c].LinkPoint;
				var ab = new ScreenEdge(pa, pb);
				var ac = new ScreenEdge(pa, pc);
				var bc = new ScreenEdge(pb, pc);
				if (!ContainsScreenEdge(RoadEdges, ab)) RoadEdges.Add(ab);
				if (!ContainsScreenEdge(RoadEdges, ac)) RoadEdges.Add(ac);
				if (!ContainsScreenEdge(RoadEdges, bc)) RoadEdges.Add(bc);
			}

			_ships.Clear();
			_ships.Init(RoadPoints[0], GetShipRoad);
			RoadMST = _paths.AlgorithmByPrim(RoadEdges, RoadPoints);
			OnSetPoints?.Invoke(RoadPoints, RoadMST, _ships);
		}

		/// <summary>
		/// Заполняем три разные планеты ресурсами - с них надо будет выполнять все заказы
		/// </summary>
		/// <param name="roadPoints"></param>
		private void InitFillResources(List<Planet> roadPoints)
		{
			//заполнить вспомогательный массив нужным количеством зданий (гараж, 3 ресурса и заказы. дополнительно 3 заказа должны быть очень большими)
			//и при выполнении каждого заказа обновлять информацию по выбранному заказу
			foreach (var point in roadPoints) point.Building = new Building() { BuilingType = BuildingEnum.Nope };
			roadPoints[0].Building = new Building() { BuilingType = BuildingEnum.ShipDepot };

			CreateRandomOrders();
			// добавляем ресурсные базы
			for (int i = 0; i < 3; i++) {
				var rp = roadPoints[roadPoints.Count - 3 + i];
				rp.Source = new ResourcesHolder();
				rp.Source.Add((ResourcesEnum)(i + 1), 50000000);
				var be = ((ResourcesEnum)(i + 1)).GetBuildingEnum();
				rp.Building = new Building() { BuilingType = be };
			}
		}

		private bool ContainsScreenEdge(IEnumerable<ScreenEdge> roadEdges, ScreenEdge newedge)
		{
			foreach (var edge in roadEdges) {
				if (edge.IsEqual(newedge)) return true;
			}
			return false;
		}

		public ScreenPoint FindNearest(int x, int y)
		{
			var p = new ScreenPoint(x, y);
			ScreenPoint ret = null;
			var minDist = p.distanceTo(RoadPoints[0]);// первоначальное значение, чтоб не с потолка брать
			foreach (var point in RoadPoints) {
				if (point.Building == null
					|| point.Building.BuilingType == BuildingEnum.Nope
					|| point.Building.BuilingType == BuildingEnum.ShipDepot
					) continue;
				var curDist = p.distanceTo(point);
				if (curDist > MouseMinimalDistance) continue;
				if (curDist > minDist) continue;
				minDist = curDist;
				ret = point;
			}
			return ret;
		}

		/// <summary>
		/// Изменение денег, обычно вследствие завершения рейса
		/// </summary>
		/// <param name="deltaMoney"></param>
		private void MoneyChanged(int deltaMoney)
		{
			if (deltaMoney == 0) return;
			Money += deltaMoney;
			OnMoneyChanged.Invoke(Money);
			_ships.ProcessMoney(Money);
		}

		private void CreateRandomOrders()
		{
			var countOrders = RoadPoints.Where(p => p.Order != null).Count();
			var needOrders = _orders.MaxOrders - countOrders;
			if (needOrders > 0) {
				for (int i = 0; i < needOrders; i++) {// создаём нужное количество заказов
					var num = RandomHelper.Random(RoadPoints.Count - 4) + 1;
					var order = _orders.GetRandomOrder(100, 0);
					if (RoadPoints[num].Order == null)
						RoadPoints[num].Order = order;
					else {// добавляем значение заказа к текущему
						RoadPoints[num].Order.AddOrder(order);
					}
					RoadPoints[num].Building = new Building() { BuilingType = BuildingEnum.QuestBuilding };
				}
				OnOrdersChanged?.Invoke();
			}
		}

		/// <summary>
		/// Получаем путь корабля от одной точки до другой
		/// </summary>
		/// <returns></returns>
		private List<ScreenPoint> GetShipRoad(ScreenPoint A, ScreenPoint B)
		{
			return _paths.GetShipRoad(RoadMST, A, B);
		}

		public void BuyShip()
		{
			_ships.BuyShip();
		}

		public override void Tick()
		{
			foreach (var ship in _ships) {
				ship.MoveNext();
			}
		}
	}
}