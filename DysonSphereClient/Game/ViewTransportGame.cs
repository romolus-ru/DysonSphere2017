using Engine.Visualization;
using System;
using System.Collections.Generic;
using Engine;
using Engine.Visualization.Debug;
using System.Windows.Forms;
using System.Drawing;

namespace DysonSphereClient.Game
{
	public class ViewTransportGame : ViewComponent
	{
		public Action OnRecreatePoints;
		public Action OnExitPressed;
		public Func<int, int, ScreenPoint> OnFindNearest;
		public Func<ScreenPoint, ScreenPoint, List<ScreenEdge>> OnGetRoadShort;
		public Func<List<ScreenEdge>, ScreenPoint, List<ScreenPoint>> OnGetPath;

		private ViewManager _viewManager;
		private List<Planet> _RoadPoints = new List<Planet>();
		private List<ScreenEdge> _RoadEdges = new List<ScreenEdge>();
		private List<ScreenEdge> _RoadEdgesMST = new List<ScreenEdge>();
		private List<ScreenEdge> _RoadShort = null;
		private List<ScreenPoint> _RoadPath = null;
		private int _mapX = 100;
		private int _mapY = 100;
		private int _curX;
		private int _curY;
		private int _oldCurX;
		private int _oldCurY;
		private ScreenPoint _nearest = null;
		private ScreenPoint _selected = null;
		private int Money = 0;
		private ViewLabel _showMoney = null;

		public void InitTransportGame(ViewManager viewManager)
		{
			_viewManager = viewManager;
		}

		public void SetPoints(List<Planet> points, List<ScreenEdge> edges, List<ScreenEdge> MST)
		{
			_RoadPoints = points;
			_RoadEdges = edges;
			_RoadEdgesMST = MST;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewTransportGame");
			var debugView = new DebugView();
			AddComponent(debugView, true);
			debugView.SetParams(1100, 0, debugView.Width, debugView.Height, "DebugView");

			var btnRecreatePoints = new ViewButton();
			AddComponent(btnRecreatePoints);
			btnRecreatePoints.InitButton(RecreatePoints, "RecreatePoints", "hint", Keys.Y);
			btnRecreatePoints.SetParams(20, 120, 140, 30, "RecreatePoints");
			btnRecreatePoints.InitTexture("textRB", "textRB");

			var btnClose = new ViewButton();
			AddComponent(btnClose);
			btnClose.InitButton(Close, "exit", "hint", Keys.LMenu, Keys.X);
			btnClose.SetParams(1659, 0, 20, 20, "btnE");

			_showMoney = ViewLabel.Create(300, 20, Color.Red, "Money", "bigFont");
			AddComponent(_showMoney);

			Input.AddKeyActionSticked(SelectPoint, Keys.LButton);
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
			if (_selected != null) {
				if (_selected == _nearest) { _selected = null; return; }
				// иначе формируем путь
				_RoadShort = OnGetRoadShort?.Invoke(_selected, _nearest);
				_RoadPath = OnGetPath?.Invoke(_RoadShort, _selected);
				_nearest = null;
				_selected = null;
			}
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
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.OffsetAdd(_mapX, _mapY);
			foreach (var p in _RoadPoints) {
				visualizationProvider.Rectangle(p.X - 1, p.Y - 1, 3, 3);
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
					visualizationProvider.SetColor(Color.GreenYellow, 100);
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

			if (_RoadPath != null) {
				visualizationProvider.SetColor(Color.LightGoldenrodYellow , 40);
				ScreenPoint p1 = null;
				foreach (var p2 in _RoadPath) {
					if (p1 == null) { p1 = p2; continue; }
					visualizationProvider.Rectangle(p1.X, p1.Y, 1, 1);//, p2.X, p2.Y);
					p1 = p2;
				}
			}

			visualizationProvider.OffsetRemove();
		}

	}
}