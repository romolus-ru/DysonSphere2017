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
		public delegate void SetPointsDelegate(List<Planet> points, List<ScreenEdge> edges, List<ScreenEdge> mst, List<Ship> ships);
		private List<Planet> RoadPoints = new List<Planet>();
		private List<ScreenEdge> RoadEdges = new List<ScreenEdge>();
		private List<ScreenEdge> RoadMST = new List<ScreenEdge>();
		private List<Ship> _Ships = new List<Ship>();
		private List<Order> _Orders = new List<Order>();
		public SetPointsDelegate OnSetPoints;
		public Action<int> OnMoneyChanged;

		/// <summary>
		/// Минимальное расстояние на котором точка будет реагировать на курсор
		/// </summary>
		private const int MouseMinimalDistance = 50;
		private int Money = 0;

		public void RecreatePoints()
		{
			RoadPoints.Clear();
			RoadEdges.Clear();
			RoadPoints = CreateGalaxy(70, 1200, 700);
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

			RoadMST = AlgorithmByPrim(RoadEdges, RoadPoints);
			OnSetPoints?.Invoke(RoadPoints, RoadEdges, RoadMST, _Ships);
		}

		/// <summary>
		/// Заполняем три разные планеты ресурсами - с них надо будет выполнять все заказы
		/// </summary>
		/// <param name="roadPoints"></param>
		private void InitFillResources(List<Planet> roadPoints)
		{
			//заполнить вспомогательный массив нужным количеством зданий (гараж, 3 ресурса и заказы. дополнительно 3 заказа должны быть очень большими)
			//и при выполнении каждого заказа обновлять информацию по выбранному заказу
			roadPoints[0].Building = new Building() { BuilingType = BuildingEnum.ShipDepot };
			
			// TODO генерируем все возможные виды строек (пока одна будет)
			// и вывод информации сделать двух видов - краткий и полный
			var order = new Order();
			order.Value = new Resources();
			order.Value.Add(ResourcesEnum.Materials, 2000);
			order.RewardRace = 3;
			order.Reward = 100;
			_Orders.Add(order);
			
			// добавляем начальные ордера
			for (int i = 1; i < roadPoints.Count-3; i++) {
				roadPoints[i].Order = _Orders[0];
			}

			// добавляем ресурсные базы
			for (int i = 0; i < 3; i++) {
				var rp = roadPoints[roadPoints.Count - 3 + i];
				rp.Source = new Resources();
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

		public List<ScreenEdge> AlgorithmByPrim(List<ScreenEdge> E, IEnumerable<ScreenPoint> points)
		{
			List<ScreenEdge> MST = new List<ScreenEdge>();
			//неиспользованные ребра
			List<ScreenEdge> notUsedE = new List<ScreenEdge>(E);
			//использованные вершины
			List<ScreenPoint> usedV = new List<ScreenPoint>();
			//неиспользованные вершины
			List<ScreenPoint> notUsedV = new List<ScreenPoint>(points);
			//выбираем случайную начальную вершину
			var num = RandomHelper.Random(notUsedV.Count);
			usedV.Add(notUsedV[num]);
			notUsedV.RemoveAt(num);
			while (notUsedV.Count > 0) {
				int minE = -1; //номер наименьшего ребра
							   //поиск наименьшего ребра
				for (int i = 0; i < notUsedE.Count; i++) {
					if ((usedV.IndexOf(notUsedE[i].A) != -1) && (notUsedV.IndexOf(notUsedE[i].B) != -1) ||
						(usedV.IndexOf(notUsedE[i].B) != -1) && (notUsedV.IndexOf(notUsedE[i].A) != -1)) {
						if (minE != -1) {
							if (notUsedE[i].Weight < notUsedE[minE].Weight)
								minE = i;
						} else
							minE = i;
					}
				}
				//заносим новую вершину в список использованных и удаляем ее из списка неиспользованных
				if (usedV.IndexOf(notUsedE[minE].A) != -1) {
					usedV.Add(notUsedE[minE].B);
					notUsedV.Remove(notUsedE[minE].B);
				} else {
					usedV.Add(notUsedE[minE].A);
					notUsedV.Remove(notUsedE[minE].A);
				}
				//заносим новое ребро в дерево и удаляем его из списка неиспользованных
				MST.Add(notUsedE[minE]);
				notUsedE.RemoveAt(minE);
			}
			return MST;
		}

		private List<Planet> CreateGalaxy(int count, int width, int height)
		{
			List<Planet> resultPoints = new List<Planet>();
			var minDist = width / 30;
			var counter = 0;

			var first = new Planet()
			{
				X = RandomHelper.Random(width),
				Y = RandomHelper.Random(height),
			};
			resultPoints.Add(first);

			while (resultPoints.Count != count) {
				counter++;
				if (counter > 50) {
					counter = 0;
					minDist = minDist / 2;
					if (minDist == 0) break;
				}

				var p = new Planet()
				{
					X = RandomHelper.Random(width),
					Y = RandomHelper.Random(height)
				};
				var foundedMin = false;
				foreach (var pt in resultPoints) {
					var dist = p.distanceTo(pt);
					if (dist < minDist) {
						foundedMin = true;
						break;
					}
				}
				if (foundedMin) continue;// начинаем цикл по новой - нашлась точка которая близко расположена к новой										 
				resultPoints.Add(p);
				counter = 0;
			}
			return resultPoints;
		}

		public ScreenPoint FindNearest(int x, int y)
		{
			var p = new ScreenPoint(x, y);
			ScreenPoint ret = null;
			var minDist = p.distanceTo(RoadPoints[0]);
			if (minDist < MouseMinimalDistance) {
				ret = RoadPoints[0];
			}
			foreach (var point in RoadPoints) {
				var curDist = p.distanceTo(point);
				if (curDist > MouseMinimalDistance) continue;
				if (curDist > minDist) continue;
				minDist = curDist;
				ret = point;
			}
			return ret;
		}

		/// <summary>
		/// Запускаем корабль 
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		internal void SendShip(Planet start, Planet end)
		{
			Ship ship = null;
			if (_Ships.Count == 0) {
				var res = GetDefaultCargoCapacity();
				ship = new Ship(RoadPoints[0], res);
				ship.OnGetRoad += GetShipRoad;
				ship.OnShipEndOrder += ShipEndOrder;
				_Ships.Add(ship);
			}
			if (ship != null) {
				foreach (var fship in _Ships) {
					if (fship.ShipCommand == ShipCommandEnum.NoCommand) {
						ship = fship;
						break;
					}
				}
			}
			if (ship == null) ship = _Ships[0];
			if (ship == null) return;
			ship.MoveToOrder(start, end);
		}

		private void ShipEndOrder(Ship ship)
		{
			var order = ((Planet)ship.OrderPlanetDestination).Order;
			if (order == null) return;
			Money += order.RewardRace;
			OnMoneyChanged?.Invoke(Money);
		}

		/// <summary>
		/// Получаем путь корабля от одной точки до другой
		/// </summary>
		/// <returns></returns>
		private List<ScreenPoint> GetShipRoad(ScreenPoint A, ScreenPoint B)
		{
			var shortRoad = GetShortRoad(A, B);
			return GetPath(shortRoad, A);
		}

		/// <summary>
		/// Почти по дейкстре. 
		/// распространяем сигнал по всем направлениям. как только достигли целевой точки - 
		/// двигаемся назад по граням с меньшим весом
		/// </summary>
		/// <param name="A"></param>
		/// <param name="B"></param>
		/// <returns></returns>
		public List<ScreenEdge> GetShortRoad(ScreenPoint A, ScreenPoint B)
		{
			var edges = new List<ScreenEdge>();
			var pointsSearched = new List<ScreenPoint>();

			foreach (var edgeRoad in RoadMST) {
				var ne = new ScreenEdge(edgeRoad.A, edgeRoad.B);
				edges.Add(ne);
			}

			var path = new List<ScreenEdge>();
			var beginEdges = edges.Where(e => (e.A == A || e.B == A)).DefaultIfEmpty().ToList();
			if (beginEdges == null || beginEdges.Count == 0) return null;
			pointsSearched.Add(A);
			path.AddRange(beginEdges);
			beginEdges.ForEach(e =>
				{
					edges.Remove(e);// удаляем 
					if (e == null) throw new Exception("e = null");
					e.Weight = e.A.distanceTo(e.B);// расчитываем начальный вес
				}
			);
			
			var founded = false;
			while (!founded) {
				ScreenPoint search = null;
				ScreenEdge searchEdge = null;
				// ищем точку для которой будем искать следующие ребра
				foreach (var pathEdge in path) {
					if (!pointsSearched.Contains(pathEdge.A)) {
						search = pathEdge.A;
						searchEdge = pathEdge;
						break;
					}
					if (!pointsSearched.Contains(pathEdge.B)) {
						search = pathEdge.B;
						searchEdge = pathEdge;
						break;
					}
				}
				if (search == null) { founded = true; continue; }

				// ищем ребра которые связаны с новой вершиной, за исключением самого найденного ребра
				var edgesSearch = edges.Where(
					e => (/*e != searchEdge &&*/ (e.A == search || e.B == search))
				).ToList();
				foreach (var e in edgesSearch) {
					e.Weight = searchEdge.Weight + e.A.distanceTo(e.B);
					if (e.A == B || e.B == B) { founded = true; }
				}

				pointsSearched.Add(search);// запоминаем
				path.AddRange(edgesSearch);// добавляем
				edgesSearch.ForEach(e => edges.Remove(e));// удаляем
			}

			// выбираем только нужные вершины
			var ret = new List<ScreenEdge>();
			ScreenPoint searchPoint = B;
			var edge = path.Where(e => e.A == searchPoint || e.B == searchPoint).OrderBy(e => e.Weight).First();
			ret.Add(edge);
			searchPoint = edge.A == searchPoint ? edge.B : edge.A;
			while (searchPoint != A) {// ищем вершины по точке и берём из них только с меньшим весом. из нее берём следующую точку
				edge = path.Where(e => e.A == searchPoint || e.B == searchPoint)
					.OrderBy(e => e.Weight).First();
				ret.Add(edge);
				searchPoint = edge.A == searchPoint ? edge.B : edge.A;// выбираем другую точку
			}
			ret.Reverse();

			return ret;
		}

		/// <summary>
		/// По присланным граням 
		/// </summary>
		/// <param name="basePath"></param>
		/// <param name="firstPoint"></param>
		/// <returns></returns>
		public List<ScreenPoint> GetPath(List<ScreenEdge> basePath, ScreenPoint firstPoint)
		{
			var bc = new BezierCurve();
			var pathlength = (int)(basePath/*.Count * 20);//*/ .Sum(e => e.Weight) / 5 / basePath.Count);
			var basePoints = GetPathFromEdges(basePath, firstPoint);
			var ret = new List<ScreenPoint>();
			bc.Bezier2D(basePoints, pathlength, ret);
			return ret;
		}

		private List<ScreenPoint> GetPathFromEdges(List<ScreenEdge> basePath, ScreenPoint firstPoint)
		{
			var ret = new List<ScreenPoint>();
			var p = firstPoint;
			ret.Add(p);// сохраняем первую точку
			foreach (var edge in basePath) {
				p = edge.A == p ? edge.B : edge.A;
				ret.Add(p);// сохраняем другую точку 
			}
			return ret;
		}

		private Resources GetDefaultResources()
		{
			var ret = new Resources();
			ret.Add(ResourcesEnum.Materials, 50000000);
			ret.Add(ResourcesEnum.Tools, 50000000);
			ret.Add(ResourcesEnum.Personal, 50000000);
			return ret;
		}

		private Resources GetDefaultReward()
		{
			var ret = new Resources();
			ret.Add(ResourcesEnum.Materials, 5);
			ret.Add(ResourcesEnum.Tools, 7);
			ret.Add(ResourcesEnum.Personal, 10);
			return ret;
		}

		private Resources GetDefaultCargoCapacity()
		{
			var ret = new Resources();
			ret.Add(ResourcesEnum.Materials, 500);
			ret.Add(ResourcesEnum.Tools, 300);
			ret.Add(ResourcesEnum.Personal, 20);
			return ret;
		}
		public override void Tick()
		{
			if (_Ships.Count == 0) return;
			_Ships[0].MoveNext();
		}
	}
}