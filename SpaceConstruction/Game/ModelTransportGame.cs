﻿using Engine.Helpers;
using Engine.Models;
using Engine.Visualization;
using Engine.Visualization.Maths;
using SpaceConstruction.Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceConstruction.Game
{
	internal class ModelTransportGame : Model
	{
		public delegate void SetPointsDelegate(List<Planet> points, List<ScreenEdge> mst, Ships ships);
		private Planet _shipDepot { get { return RoadPoints.Count > 0 ? RoadPoints[0] : null; } }
		private List<Planet> RoadPoints = new List<Planet>();
		private List<ScreenEdge> RoadMST = new List<ScreenEdge>();
		private Ships _ships;
		private Orders.Orders _orders;
		private Paths _paths = new Paths();
		public SetPointsDelegate OnSetPoints;
		public Action<int> OnMoneyChanged;
		public Action OnOrdersChanged;
		/// <summary>
		/// Минимальное расстояние на котором точка будет реагировать на курсор
		/// </summary>
		private const int MouseMinimalDistance = 50;

		public ModelTransportGame(Ships ships, Orders.Orders orders)
		{
			_ships = ships;
			_orders = orders;
			_ships.OnFinishOrder = CreateRandomOrders;
		}
		
		public void RecreatePoints()
		{
			_orders.Clear();
			var RoadEdges = new List<ScreenEdge>();
			RoadPoints.Clear();
			RoadPoints.AddRange(_paths.CreateGalaxy(70, 1500, 600, 100));
			RoadPoints[0].IsDepot = true;
			CreateRandomOrders();
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
				var curDist = p.distanceTo(point);
				if (curDist > MouseMinimalDistance) continue;
				if (curDist > minDist) continue;
				minDist = curDist;
				ret = point;
			}
			return ret;
		}

		private void CreateRandomOrders()
		{
			DeleteEmptyOrders();
			var countOrders = _orders.ActualOrdersCount;
			var needOrders = _orders.MaxOrders - countOrders;
			if (needOrders <= 0) return;
			for (int i = 0; i < needOrders; i++) {// создаём нужное количество заказов
				// если есть планеты к которым не подключены заказы
				if (ExistsFreePlanetsForOrder()) {
					CreateRandomOrder();
				}
			}
			OnOrdersChanged?.Invoke();
		}

		private void DeleteEmptyOrders()
		{
			var endedOrders = false;
			foreach (var point in RoadPoints) {
				var order = point.Order;
				if (order == null) continue;
				if (!order.AmountResources.IsEmpty() || !order.AmountResourcesInProgress.IsEmpty())
					continue;

				// всё перевезено и нету перевозимых ресурсов - удаляем заказ
				_ships.CancelOrder(point);
				point.Order = null;
				_orders.EndOrder(order);
				ItemsManager.BuySign("Sign1");
				endedOrders = true;
			}
			if (endedOrders)
				UpdateMoneyInfo();
		}

		public void UpdateMoneyInfo()
		{
			var item = ItemsManager.GetItemByCode("Sign1");
			OnMoneyChanged?.Invoke(item.PlayerCount);
		}

		private void CreateRandomOrder()
		{
			var order = _orders.GetNewOrder(1);
			int num = -1;
			do {
				num = RandomHelper.Random(RoadPoints.Count - 1) + 1;
			} while (RoadPoints[num].Order != null);
			RoadPoints[num].Order = order;
			order.Destination = RoadPoints[num];
			Planet source = null;
			do {
				num = RandomHelper.Random(RoadPoints.Count - 1) + 1;
				if (RoadPoints[num].Order != null)
					continue;
				source = RoadPoints[num];
			} while (source == null);
			order.Source = source;
		}

		private bool ExistsFreePlanetsForOrder()
			=> RoadPoints.Where(r => r.Order == null).Count() >= _orders.MaxOrders - 1;

		/// <summary>
		/// Получаем путь корабля от одной точки до другой
		/// </summary>
		/// <returns></returns>
		private List<ScreenPoint> GetShipRoad(ScreenPoint A, ScreenPoint B)
		{
			return _paths.GetShipRoad(RoadMST, A, B);
		}

		public override void Tick()
		{
			foreach (var ship in _ships) {
				ship.MoveNext();
			}
		}
	}
}