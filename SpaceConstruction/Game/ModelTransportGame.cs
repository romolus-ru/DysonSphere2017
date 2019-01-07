using Engine.Helpers;
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
		private List<Planet> _roadPoints = new List<Planet>();
		private List<ScreenEdge> _roadMST = new List<ScreenEdge>();
		private Ships _ships;
		private Orders.Orders _orders;
		private Paths _paths = new Paths();
		public SetPointsDelegate OnSetPoints;
		public Action<int> OnMoneyChanged;
		public Action OnOrdersChanged;
		private int _ordersLevel = 1;
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
			_paths.ClearCache();
			_orders.Clear();
			var roadEdges = new List<ScreenEdge>();
			_roadPoints.Clear();
			_roadPoints.AddRange(_paths.CreateGalaxy(70, 1500, 600, 100));
			_roadPoints[0].IsDepot = true;
			CreateRandomOrders();
			var tmpPoints = new List<Vertex>();
			foreach (var point in _roadPoints) {
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
				if (!ContainsScreenEdge(roadEdges, ab)) roadEdges.Add(ab);
				if (!ContainsScreenEdge(roadEdges, ac)) roadEdges.Add(ac);
				if (!ContainsScreenEdge(roadEdges, bc)) roadEdges.Add(bc);
			}

			_ships.Clear();
			_ships.Init(_roadPoints[0], GetShipRoad);
			_roadMST = _paths.AlgorithmByPrim(roadEdges, _roadPoints);
			OnSetPoints?.Invoke(_roadPoints, _roadMST, _ships);
		}

		private bool ContainsScreenEdge(IEnumerable<ScreenEdge> roadEdges, ScreenEdge newEdge)
		{
			foreach (var edge in roadEdges) {
				if (edge.IsEqual(newEdge)) return true;
			}
			return false;
		}

		public ScreenPoint FindNearest(int x, int y)
		{
			var p = new ScreenPoint(x, y);
			ScreenPoint ret = null;
			var minDist = p.distanceTo(_roadPoints[0]);// первоначальное значение, чтоб не с потолка брать
			foreach (var point in _roadPoints) {
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
			for (var i = 0; i < needOrders; i++) {
				if (ExistsFreePlanetsForOrder()) {
					CreateRandomOrder();
				}
			}
			OnOrdersChanged?.Invoke();
		}

		private void DeleteEmptyOrders()
		{
			var endedOrders = false;
			foreach (var point in _roadPoints) {
				var order = point.Order;
				if (order == null) continue;
				if (!order.AmountResources.IsEmpty() || !order.AmountResourcesInProgress.IsEmpty())
					continue;

				// всё перевезено и нету перевозимых ресурсов - удаляем заказ
				_ships.CancelOrder(point);
				point.Order = null;
				_orders.EndOrder(order);
				ItemsManager.GrantSigns("Sign1", order.Level);
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
			var order = _orders.GetNewOrder(_ordersLevel);
			int num;
			do {
				num = RandomHelper.Random(_roadPoints.Count - 1) + 1;
			} while (_roadPoints[num].Order != null);
			_roadPoints[num].Order = order;
			order.Destination = _roadPoints[num];
			Planet source = null;
			do {
				num = RandomHelper.Random(_roadPoints.Count - 1) + 1;
				if (_roadPoints[num].Order != null)
					continue;
				source = _roadPoints[num];
			} while (source == null);
			order.Source = source;
		}

		private bool ExistsFreePlanetsForOrder()
			=> _roadPoints.Count(r => r.Order == null) >= _orders.MaxOrders - 1;

		/// <summary>
		/// Получаем путь корабля от одной точки до другой
		/// </summary>
		/// <returns></returns>
		private List<ScreenPoint> GetShipRoad(ScreenPoint a, ScreenPoint b)
		{
			return _paths.GetShipRoad(_roadMST, a, b);
		}

		public override void Tick()
		{
			foreach (var ship in _ships) {
				if (ship.ShipCommand == ShipCommandsEnum.NoCommand && ship.AutoPilot) {
					var order = _orders.GetRandomOrder();
					ship.MoveToOrder(order.Source, order.Destination);
				}

				ship.MoveNext();
			}
		}

		private bool _finalOrderActive;
		private bool _openTopOrders;
		public void UpdateResearchInfo()
		{
			if (!_finalOrderActive && ItemsManager.IsResearchItemBuyed("StartFinalOrder")) {
				_finalOrderActive = true;
			}
			if (!_openTopOrders && ItemsManager.IsResearchItemBuyed("OpenTopOrders")) {
				_openTopOrders = true;
				_ordersLevel = GameConstants.OpenTopOrdersLevel;
			}
		}
	}
}