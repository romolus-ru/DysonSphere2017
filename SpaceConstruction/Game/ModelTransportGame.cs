using Engine.Helpers;
using Engine.Models;
using Engine.Visualization;
using Engine.Visualization.Maths;
using SpaceConstruction.Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using SpaceConstruction.Game.Orders;

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
		/// Запускаем главный заказ
		/// </summary>
		public Action<DateTime> OnFinalOrderStart;
		/// <summary>
		/// Выполнен главный заказ - прячем всё, выводим приветствие
		/// </summary>
		public Action OnFinalOrderComplete;
		/// <summary>
		/// Останавливаем таймер и показываем кнопку запуска заказа
		/// </summary>
		public Action OnFinalOrderNotComplete;

		/// <summary>
		/// Минимальное расстояние на котором точка будет реагировать на курсор
		/// </summary>
		private const int MouseMinimalDistance = 50;

		private bool _finalOrderStarted;
		private DateTime _finalOrderTimer;
		private Order _finalOrder;
		private bool _stopForRestart;

		public ModelTransportGame(Ships ships, Orders.Orders orders)
		{
			_ships = ships;
			_orders = orders;
			_ships.OnFinishOrder = CreateRandomOrders;
			ItemsManager.CreateItems();
		}

		/// <summary>
		/// Приводим систему к начальному состоянию
		/// </summary>
		private void Restart()
		{
			_openTopOrders = false;
			_stopForRestart = false;
			_ordersLevel = 1;
			_paths.ClearCache();
			_orders.Clear();
			_roadPoints.Clear();
			_ships.Clear();
		}

		public void RecreatePoints()
		{
			ItemsManager.CreateItems();
			Restart();
			var roadEdges = new List<ScreenEdge>();
			_roadPoints.AddRange(_paths.CreateGalaxy(70, 1400, 600, 100));
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
			if (_stopForRestart)
				return;
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
				StopOrder(order);
				if (order == _finalOrder) {
					_finalOrderStarted = false;
					_stopForRestart = true;
					return;
				}
				ItemsManager.GrantSigns("Sign1", order.Reward);
				endedOrders = true;
			}
			if (endedOrders)
				UpdateMoneyInfo();
		}

		private void StopOrder(Order order)
		{
			_ships.CancelOrder(order.Destination);
			order.Destination.Order = null;
			_orders.EndOrder(order);
		}

		public void UpdateMoneyInfo()
		{
			var item = ItemsManager.GetItemByCode("Sign1");
			OnMoneyChanged?.Invoke(item.PlayerCount);
		}

		internal void StartFinalOrder()
		{	
			var order = _orders.GetFinalOrder();
			int num = GetRandomRoadPointWithoutOrder();
			_roadPoints[num].Order = order;
			order.Destination = _roadPoints[num];
			int numSource = GetRandomRoadPointWithoutOrder();
			order.Source = _roadPoints[numSource];
			_finalOrderStarted = true;
			_finalOrderTimer = DateTime.Now + GameConstants.FinalOrderTimer;
			_finalOrder = order;
			OnFinalOrderStart?.Invoke(_finalOrderTimer);
		}

		private void CreateRandomOrder()
		{
			var order = _orders.GetNewOrder(_ordersLevel);
			int num = GetRandomRoadPointWithoutOrder();
			_roadPoints[num].Order = order;
			order.Destination = _roadPoints[num];
			int numSource = GetRandomRoadPointWithoutOrder();
			order.Source = _roadPoints[numSource];
		}

		private int GetRandomRoadPointWithoutOrder()
		{
			int num;
			do {
				num = RandomHelper.Random(_roadPoints.Count - 1) + 1;
			} while (_roadPoints[num].Order != null);

			return num;
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
			if (_finalOrderStarted && DateTime.Now > _finalOrderTimer) {
				_finalOrderStarted = false;
				StopOrder(_finalOrder);
				_finalOrder = null;
				OnFinalOrderNotComplete?.Invoke();
			}

			foreach (var ship in _ships) {
				if (_stopForRestart)
					break;
				if (ship.ShipCommand == ShipCommandsEnum.NoCommand && ship.AutoPilot) {
					var order = _orders.GetRandomOrder();
					ship.MoveToOrder(order.Source, order.Destination);
				}

				ship.MoveNext();
			}

			if (_stopForRestart) {
				_finalOrder = null;
				OnFinalOrderComplete?.Invoke();
				Restart();
			}
		}

		private bool _openTopOrders;
		public void UpdateResearchInfo()
		{
			if (!_openTopOrders && ItemsManager.IsResearchItemBuyed("OpenTopOrders")) {
				_openTopOrders = true;
				_ordersLevel = GameConstants.OpenTopOrdersLevel;
			}
		}
	}
}