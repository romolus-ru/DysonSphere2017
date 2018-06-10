using Engine.Visualization;
using System;
using System.Collections.Generic;
using Engine;
using Engine.Visualization.Debug;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using DysonSphereClient.Game.Upgrades;
using DysonSphereClient.Game.Achievements;
using Engine.Visualization.Text;

namespace DysonSphereClient.Game
{
	public class ViewTransportGame : ViewComponent
	{
		public delegate bool SendShipDelegate(Planet start, Planet end);
		public Action OnRecreatePoints;
		public Action OnExitPressed;
		public Func<int, int, ScreenPoint> OnFindNearest;
		public Action OnBuyShip;
		[AchievementInfo(Name = GameAchievementsConstants.SelectPlanet)]
		public Action OnSelectPlanet;// = () => { };

		private ViewManager _viewManager;
		private List<Planet> _RoadPoints = new List<Planet>();
		private List<ScreenEdge> _RoadEdgesMST = new List<ScreenEdge>();
		private Ships _ships = null;
		private int _mapX = 100;
		private int _mapY = 250;
		private int _curX;
		private int _curY;
		private int _oldCurX;
		private int _oldCurY;
		private ScreenPoint _nearest = null;
		private ScreenPoint _selected = null;
		private ViewLabelIcon _showMoney = null;
		private ViewShipsPanel _shipsPanel = null;
		/// <summary>
		/// Позиция для вывода информации о загрузке
		/// </summary>
		private int _shipLoadRow = 0;

		public void InitTransportGame(ViewManager viewManager)
		{
			_viewManager = viewManager;
		}

		public void SetPoints(List<Planet> points, List<ScreenEdge> MST, Ships ships)
		{
			_RoadPoints = points;
			_RoadEdgesMST = MST;
			_ships = ships;
			_shipsPanel.SetShips(_ships);
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			visualizationProvider.LoadAtlas("RoundDigits");
			visualizationProvider.LoadAtlas("Planets");
			visualizationProvider.LoadAtlas("Money");
			visualizationProvider.LoadAtlas("Resources");
			SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewTransportGame");
			var debugView = new DebugView();
			AddComponent(debugView, true);
			debugView.SetParams(1100, 0, debugView.Width, debugView.Height, "DebugView");

			var btnRecreatePoints = new ViewButton();
			AddComponent(btnRecreatePoints);
			btnRecreatePoints.InitButton(RecreatePoints, "RecreatePoints", "hint", Keys.Y);
			btnRecreatePoints.SetParams(20, 120, 140, 30, "RecreatePoints");
			btnRecreatePoints.InitTexture("textRB", "textRB");

			var btnBuyShip = new ViewButton();
			AddComponent(btnBuyShip);
			btnBuyShip.InitButton(null, "Купить корабль", "hint", Keys.S);
			btnBuyShip.SetParams(250, 70, 140, 30, "btnBuyShip");
			btnBuyShip.InitTexture("textRB", "textRB");

			var btnShop = new ViewButton();
			AddComponent(btnShop);
			btnShop.InitButton(ShowShop, "ShowShop", "hint", Keys.S);
			btnShop.SetParams(250, 105, 140, 30, "btnShop");
			btnShop.InitTexture("textRB", "textRB");

			_shipsPanel = new ViewShipsPanel();
			AddComponent(_shipsPanel);
			_shipsPanel.OnBuyShip += () => OnBuyShip?.Invoke();// отправляем запрос выше

			var btnClose = new ViewButton();
			AddComponent(btnClose);
			btnClose.InitButton(Close, "exit", "hint", Keys.LMenu, Keys.X);
			btnClose.SetParams(1659, 0, 20, 20, "btnE");

			_showMoney = ViewLabelIcon.Create(300, 20, Color.Red, "0", "bigFont", "Money.MR");
			AddComponent(_showMoney);

			var viewText = new ViewText();
			AddComponent(viewText);
			viewText.SetParams(250, 140, 180, 50, "ViewText");
			var tr = viewText.CreateTextRow();
			viewText.AddText(tr, Color.Red, null, "Red");
			viewText.AddTexture(tr, "Resources.Tools");
			viewText.AddText(tr, Color.Yellow, null, "Yellow");
			viewText.AddText(tr, Color.Green, null, "Green");
			var tr2 = viewText.CreateTextRow();
			viewText.AddText(tr2, Color.Red, null, "color = Red 2 ");
			viewText.CalculateTextPositions();

			Input.AddKeyActionSticked(SelectPoint, Keys.LButton);
			visualizationProvider.InitShader();
		}

		private void ShowShop()
		{
			new ShopWindow().InitWindow(_viewManager);
		}

		private void RecreatePoints()
		{
			_selected = null;
			_nearest = null;
			OnRecreatePoints?.Invoke();
		}
		private void SelectPoint()
		{
			if (_selected == null) {
				if (_nearest == null) return;
				_selected = _nearest;
				OnSelectPlanet?.Invoke();
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

				var res = _ships.SendShip((Planet)fromPlanet, (Planet)toPlanet);
				if (!res) {
					//TODO добавить крупное сообщение на экран что не получилось отправить корабль

				}
				_nearest = null;
				_selected = null;
			}
		}

		public void MoneyChanged(int amount)
		{
			_showMoney.Text = amount.ToString();
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

			foreach (var e in _RoadEdgesMST) {
				visualizationProvider.Line(e.A.X, e.A.Y, e.B.X, e.B.Y);
			}

			if (_nearest != null) {
				visualizationProvider.SetColor(Color.OrangeRed);
				visualizationProvider.Circle(_nearest.X, _nearest.Y, 10);
			}
			if (_selected != null) {
				visualizationProvider.SetColor(Color.Red);
				visualizationProvider.Circle(_selected.X, _selected.Y, 15);
			}

			if (_ships != null) {
				visualizationProvider.SetColor(Color.LightCoral);
				_shipLoadRow = 0;
				foreach (var ship in _ships) {
					DrawShipMapInfo(visualizationProvider, ship);
				}
			}

			foreach (var p in _RoadPoints) {
				if (p == _nearest) continue;
				DrawPlanetInfo(visualizationProvider, p);
			}
			if (_nearest != null) {
				visualizationProvider.SetColor(Color.Black, 70);
				visualizationProvider.Box(_nearest.X - 10, _nearest.Y - 10, 200, 100);
				visualizationProvider.SetColor(Color.Green, 40);
				visualizationProvider.Rectangle(_nearest.X - 10, _nearest.Y - 10, 200, 100);
				DrawPlanetInfo(visualizationProvider, _nearest as Planet, isBright: true);
			}

			visualizationProvider.OffsetRemove();
		}

		private void DrawShipMapInfo(VisualizationProvider visualizationProvider, Ship ship)
		{
			var waitState = ship.TimeToWaitState;
			if (waitState != ShipCommandEnum.NoCommand) {
				// рисуем прогресс бар
				var planet = ship.CurrentRoad[0];
				const int progressBarLenght = 50;
				int cur = progressBarLenght * ship.TimeToWaitCurrent / ship.TimeToWaitMax;
				visualizationProvider.SetColor(Color.Green);
				visualizationProvider.Box(planet.X + 10, planet.Y - 10 + _shipLoadRow * progressBarLenght, cur, 10);
				visualizationProvider.SetColor(Color.Red);
				visualizationProvider.Box(planet.X + 10 + cur, planet.Y - 10 + _shipLoadRow * progressBarLenght, progressBarLenght - cur, 10);
				_shipLoadRow++;
				return;
			}
			if (ship.CurrentRoadPointNum <= 0) return;
			if (ship.CurrentRoadPointNum >= ship.CurrentRoad.Count) {
				System.Diagnostics.Debug.WriteLine("количество точек в пути меньше чем текущее положение корабля");
				return;
			}
			var p = ship.CurrentRoad[ship.CurrentRoadPointNum];
			visualizationProvider.Rectangle(p.X, p.Y, 3, 3);
			if (ship.CurrentRoad != null) {
				visualizationProvider.SetColor(Color.DeepSkyBlue, 90);
				for (int i = ship.CurrentRoadPointNum; i < ship.CurrentRoad.Count; i++) {
					var p1 = ship.CurrentRoad[i];
					visualizationProvider.Rectangle(p1.X, p1.Y, 1, 1);
				}
				//foreach (var p1 in ship.CurrentRoad) { visualizationProvider.Rectangle(p1.X, p1.Y, 1, 1); }
			}
		}

		private void DrawPlanetInfo(VisualizationProvider visualizationProvider, Planet p, bool isBright = false)
		{
			if (p == _nearest)
				visualizationProvider.SetColor(Color.White);
			else
				visualizationProvider.SetColor(Color.White, 75);
			visualizationProvider.Rectangle(p.X - 1, p.Y - 1, 3, 3);
			const int offsetPlanet = 7;
			const int offsetResources = 16;
			if (p.Building.BuilingType.IsSource()) {
				var texture = ResourcesHelper.GetTexture(
					p.Building.BuilingType.GetResourceEnum());
				if (!string.IsNullOrEmpty(texture)) {
					if (isBright)
						visualizationProvider.DrawTexturePart(p.X + 10, p.Y + 30, texture, 40, 40);
					visualizationProvider.DrawTexturePart(p.X - offsetResources, p.Y - offsetResources, texture, offsetResources * 2, offsetResources * 2);
					visualizationProvider.SetColor(Color.BlanchedAlmond);
					visualizationProvider.Circle(p.X, p.Y, 20);
				}
			} //else visualizationProvider.DrawTexturePart(p.X - offsetPlanet, p.Y - offsetPlanet, "Planets.P2", offsetPlanet * 2, offsetPlanet * 2);
			if (p.Building.BuilingType == BuildingEnum.ShipDepot)
				visualizationProvider.DrawTexturePart(p.X - offsetResources, p.Y - offsetResources, "Resources.ShipDepot", offsetResources * 2, offsetResources * 2);
			var infoStr = "";
			if (p.Building != null && p.Building.BuilingType != BuildingEnum.Nope) {
				infoStr = p.Building.BuilingType.ToString();
			}
			visualizationProvider.Print(p.X + 10, p.Y, infoStr);

			if (p.Order != null) {
				var s = p.Order.GetInfo();
				visualizationProvider.Print(p.X, p.Y + 16, " +" + p.Order.Reward);
				//visualizationProvider.DrawTextLineArtifacts();
				visualizationProvider.PrintTexture("Money.MR", null);
				//visualizationProvider.DrawTextLineArtifacts();
				visualizationProvider.Print(" (+" + p.Order.RewardRace);
				//visualizationProvider.DrawTextLineArtifacts();
				visualizationProvider.PrintTexture("Money.MR", null);
				//visualizationProvider.DrawTextLineArtifacts();
				visualizationProvider.Print(" за рейс)");
				//visualizationProvider.DrawTextLineArtifacts();
				ResourcesHelper.PrintResources(visualizationProvider, p.X, p.Y + 32, p.Order.AmountResources);
				//var strNum = 0;
				//foreach (var str in s) {
				//	strNum++;
				//	visualizationProvider.Print(p.X, p.Y + strNum * 16, str);
				//}


			}
		}

		/// <summary>
		/// При изменении заказов проверить, не выделена ли теперь планета без заказа
		/// </summary>
		public void OrdersChanged()
		{
			if (_selected != null && (_selected as Planet).Order == null)
				_selected = null;
			if (_nearest != null && (_nearest as Planet).Order == null)
				_nearest = null;
		}
	}
}