using Engine.Visualization;
using System;
using System.Collections.Generic;
using Engine;
using Engine.Visualization.Debug;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace DysonSphereClient.Game
{
	public class ViewTransportGame : ViewComponent
	{
		public delegate void SendShipDelegate(Planet start, Planet end);
		public Action OnRecreatePoints;
		public Action OnExitPressed;
		public Func<int, int, ScreenPoint> OnFindNearest;
		public Func<ScreenPoint, ScreenPoint, List<ScreenEdge>> OnGetRoadShort;
		public Func<List<ScreenEdge>, ScreenPoint, List<ScreenPoint>> OnGetPath;
		public SendShipDelegate OnSendShip;

		private ViewManager _viewManager;
		private List<Planet> _RoadPoints = new List<Planet>();
		private List<ScreenEdge> _RoadEdges = new List<ScreenEdge>();
		private List<ScreenEdge> _RoadEdgesMST = new List<ScreenEdge>();
		private List<ScreenEdge> _RoadShort = null;
		private List<ScreenPoint> _RoadPath = null;
		private List<Ship> _ships = null;
		private int _mapX = 100;
		private int _mapY = 250;
		private int _curX;
		private int _curY;
		private int _oldCurX;
		private int _oldCurY;
		private ScreenPoint _nearest = null;
		private ScreenPoint _selected = null;
		private int Money = 0;
		private ViewLabel _showMoney = null;
		private ViewShipsPanel _shipsPanel = null;
		private ViewShipPanel _shipPanel = null;

		public void InitTransportGame(ViewManager viewManager)
		{
			_viewManager = viewManager;
		}

		public void SetPoints(List<Planet> points, List<ScreenEdge> edges, List<ScreenEdge> MST, List<Ship> ships)
		{
			_RoadPoints = points;
			_RoadEdges = edges;
			_RoadEdgesMST = MST;
			_ships = ships;
			if (_ships?.Count>0)
				_shipsPanel.SetShips(_ships);
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			visualizationProvider.LoadAtlas("RoundDigits");
			SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewTransportGame");
			var debugView = new DebugView();
			AddComponent(debugView, true);
			debugView.SetParams(1100, 0, debugView.Width, debugView.Height, "DebugView");

			var btnRecreatePoints = new ViewButton();
			AddComponent(btnRecreatePoints);
			btnRecreatePoints.InitButton(RecreatePoints, "RecreatePoints", "hint", Keys.Y);
			btnRecreatePoints.SetParams(20, 120, 140, 30, "RecreatePoints");
			btnRecreatePoints.InitTexture("textRB", "textRB");

			_shipsPanel = new ViewShipsPanel();
			AddComponent(_shipsPanel);

			_shipPanel = new ViewShipPanel();
			AddComponent(_shipPanel);

			var btnClose = new ViewButton();
			AddComponent(btnClose);
			btnClose.InitButton(Close, "exit", "hint", Keys.LMenu, Keys.X);
			btnClose.SetParams(1659, 0, 20, 20, "btnE");

			_showMoney = ViewLabel.Create(300, 20, Color.Red, "Money", "bigFont");
			AddComponent(_showMoney);

			Input.AddKeyActionSticked(SelectPoint, Keys.LButton);
			visualizationProvider.InitShader();
		}

		private void RecreatePoints()
		{
			_selected = null;
			_nearest = null;
			_RoadShort = null;
			OnRecreatePoints?.Invoke();
		}
		private void SelectPoint()
		{
			if (_selected == null) {
				_selected = _nearest;
				return;
			}
			if (_nearest == null) return;
			
			if (_selected != null) {
				if (_selected == _nearest) { _selected = null; return; }
				Planet fromPlanet, toPlanet;
				if ((_selected as Planet).Building.BuilingType.IsSource()) {
					fromPlanet = (Planet)_selected;
					toPlanet = (Planet)_nearest;
				} else {
					toPlanet = (Planet)_selected;
					fromPlanet = (Planet)_nearest;
				}
				if (!fromPlanet.Building.BuilingType.IsSource()) return;
				if (toPlanet.Building.BuilingType != BuildingEnum.QuestBuilding) return;

				// формируем путь
				_RoadShort = OnGetRoadShort?.Invoke(fromPlanet, toPlanet);
				_RoadPath = OnGetPath?.Invoke(_RoadShort, fromPlanet);
				OnSendShip?.Invoke((Planet)fromPlanet, (Planet)toPlanet);
				_nearest = null;
				_selected = null;
			}
		}

		public void MoneyChanged(int amount)
		{
			_showMoney.Text = "Money " + amount;
		}

		protected override void Cursor(int cursorX, int cursorY)
		{
			if (cursorX == _curX && cursorY == _curY) return;
			if (_RoadPoints.Count == 0) return;
			_oldCurX = _curX;
			_oldCurY = _curY;
			_curX = cursorX;
			_curY = cursorY;
			_nearest = OnFindNearest(_curX - _mapX, _curY - _mapY);
		}

		private void Close()
		{
			OnExitPressed();
		}
		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.UseShader();
			visualizationProvider.SetColor(Color.DeepSkyBlue);
			//visualizationProvider.Box(10, 10, 1400, 800);
			visualizationProvider.Print(100, 100, "Текст");
			visualizationProvider.DrawTexturePart(70, 70, "RoundDigits.1", 20, 20);

			visualizationProvider.StopShader();
			visualizationProvider.DrawTexturePart(100, 70, "RoundDigits.1", 20, 20);

			visualizationProvider.SetColor(Color.White);
			visualizationProvider.OffsetAdd(_mapX, _mapY);
			foreach (var p in _RoadPoints) {
				DrawPlanetInfo(visualizationProvider, p);
			}

			visualizationProvider.SetColor(Color.DarkSeaGreen, 10);
			foreach (var e in _RoadEdges) {
				visualizationProvider.Line(e.A.X, e.A.Y, e.B.X, e.B.Y);
			}
			visualizationProvider.SetColor(Color.White);
			foreach (var e in _RoadEdgesMST) {
				visualizationProvider.Line(e.A.X, e.A.Y, e.B.X, e.B.Y);
			}
			/*for (int i = 0; i < _RoadEdges.Count / 3; i++) {
				var a = _RoadEdges[i * 3];
				var b = _RoadEdges[i * 3 + 1];
				var c = _RoadEdges[i * 3 + 2];
				visualizationProvider.Line(a.X, a.Y, b.X, b.Y);
				visualizationProvider.Line(a.X, a.Y, c.X, c.Y);
				visualizationProvider.Line(b.X, b.Y, c.X, c.Y);
			}*/

			if (_nearest != null) {
				visualizationProvider.SetColor(Color.OrangeRed);
				visualizationProvider.Circle(_nearest.X, _nearest.Y, 10);
			}
			if (_selected != null) {
				visualizationProvider.SetColor(Color.Red);
				visualizationProvider.Circle(_selected.X, _selected.Y, 15);
			}

			if (_RoadShort != null) {
				foreach (var e in _RoadShort) {
					visualizationProvider.SetColor(Color.GreenYellow, 10);
					visualizationProvider.Print((e.A.X + e.B.X) / 2, (e.A.Y + e.B.Y) / 2, e.Weight.ToString());
					double dy = (e.B.X - e.A.X);
					double dx = -(e.B.Y - e.A.Y);
					var sq = Math.Sqrt(dx * dx + dy * dy);
					dx = 5 * dx / sq;
					dy = 5 * dy / sq;
					//visualizationProvider.Line(e.A.X, e.A.Y, e.B.X, e.B.Y);
					visualizationProvider.SetColor(Color.GreenYellow, 30);
					visualizationProvider.Line(
						(int)(e.A.X + dx),
						(int)(e.A.Y + dy),
						(int)(e.B.X + dx),
						(int)(e.B.Y + dy));
					visualizationProvider.Line(
						(int)(e.A.X - dx),
						(int)(e.A.Y - dy),
						(int)(e.B.X - dx),
						(int)(e.B.Y - dy));
				}
			}

			if (_RoadPath != null && _RoadShort != null) {
				visualizationProvider.SetColor(Color.GreenYellow, 20);
				var len = (int)(_RoadShort/*.Count * 20);//*/ .Sum(e => e.Weight) / 5 / _RoadShort.Count);
				visualizationProvider.Print(_RoadPath[0].X, _RoadPath[0].Y, "rpcount=" + _RoadPath.Count.ToString() + " rcount=" + _RoadShort.Count.ToString() + " len=" + len);
				visualizationProvider.SetColor(Color.LightGoldenrodYellow, 10);
				ScreenPoint p1 = null;
				foreach (var p2 in _RoadPath) {
					if (p1 == null) { p1 = p2; continue; }
					visualizationProvider.Rectangle(p1.X, p1.Y, 1, 1);//, p2.X, p2.Y);
					p1 = p2;
				}
			}

			if (_ships != null) {
				visualizationProvider.SetColor(Color.LightCoral);
				foreach (var ship in _ships) {
					if (ship.CurrentRoadPointNum <= 0) continue;
					var p = ship.CurrentRoad[ship.CurrentRoadPointNum];
					visualizationProvider.Rectangle(p.X, p.Y, 3, 3);
					if (ship.CurrentRoad != null) {
						visualizationProvider.SetColor(Color.DeepSkyBlue, 90);
						foreach (var p1 in ship.CurrentRoad) {
							visualizationProvider.Rectangle(p1.X, p1.Y, 1, 1);
						}
					}
					ScreenPoint sp;

					sp = ship.CurrentTarget;
					visualizationProvider.SetColor(Color.Red);
					visualizationProvider.Circle(sp.X, sp.Y, 20);
					visualizationProvider.Print(sp.X, sp.Y, "        current");

					sp = ship.OrderPlanetSource;
					visualizationProvider.SetColor(Color.Green);
					visualizationProvider.Circle(sp.X, sp.Y, 15);
					visualizationProvider.Print(sp.X, sp.Y, "  s");

					sp = ship.OrderPlanetDestination;
					visualizationProvider.SetColor(Color.Blue);
					visualizationProvider.Circle(sp.X, sp.Y, 15);
					visualizationProvider.Print(sp.X, sp.Y, "  d");
					
					sp = ship.Base;
					visualizationProvider.SetColor(Color.Yellow);
					visualizationProvider.Circle(sp.X, sp.Y, 15);
					visualizationProvider.Print(sp.X, sp.Y, "  b");

				}
			}

			visualizationProvider.OffsetRemove();
		}

		private void DrawPlanetInfo(VisualizationProvider visualizationProvider, Planet p)
		{
			if (p == _nearest)
				visualizationProvider.SetColor(Color.White);
			else
				visualizationProvider.SetColor(Color.White, 75);
			visualizationProvider.Rectangle(p.X - 1, p.Y - 1, 3, 3);
			var infoStr = "";
			if (p.Building != null && p.Building.BuilingType != BuildingEnum.Nope) {
				infoStr = p.Building.BuilingType.ToString();
			}

			visualizationProvider.Print(p.X + 10, p.Y, infoStr);
			if (p.Order != null) {
				var s = p.Order.GetInfo();
				var strNum = 0;
				foreach (var str in s) {
					strNum++;
					visualizationProvider.Print(p.X, p.Y + strNum * 16, str);
				}
			}
		}
	}
}