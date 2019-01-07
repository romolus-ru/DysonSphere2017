using Engine;
using Engine.Helpers;
using Engine.Visualization;
using SpaceConstruction.Game.Items;
using SpaceConstruction.Game.Orders;
using SpaceConstruction.Game.Resources;
using SpaceConstruction.Game.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SpaceConstruction.Game
{
	public class ViewTransportGame : ViewComponent
	{
		public delegate bool SendShipDelegate(Planet start, Planet end);
		public Action OnRecreatePoints;
		public Action OnExitPressed;
		public Func<int, int, ScreenPoint> OnFindNearest;
		public List<ResourceInfo> ResourceInfos;
		public List<OrderInfo> OrderInfos;
		public Action OnUpdateMoneyInfo;

		private ViewManager _viewManager;
		private List<Planet> _roadPoints = new List<Planet>();
		private List<ScreenEdge> _roadEdgesMST = new List<ScreenEdge>();
		private Ships _ships = null;
		private int _mapX = 50;
		private int _mapY = 300;
		private int _curX;
		private int _curY;
		private ScreenPoint _nearest;
		private ViewLabelIcon _showMoney;
		private ViewShipsPanel _shipsPanel;
		private ViewButton _btnResearches;
		private ViewButton _btnShop;
		private ViewButton _btnRecreatePoints;
		private bool _availableResearchesToBuy;
		private ResearchesBuyWindow _researchesBuyWindow;
		private ShopUpgradesBuyWindow _shopUpgradesBuyWindow;

		public void InitTransportGame(ViewManager viewManager)
		{
			_viewManager = viewManager;
		}

		public void SetPoints(List<Planet> points, List<ScreenEdge> edgesMST, Ships ships)
		{
			_roadPoints = points;
			_roadEdgesMST = edgesMST;
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
			visualizationProvider.LoadAtlas("Result");
			visualizationProvider.LoadAtlas("ResearchBuyButton");
			SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewTransportGame");

			/*var debugView = new DebugView();
			AddComponent(debugView, true);
			debugView.SetParams(1100, 0, debugView.Width, debugView.Height, "DebugView");*/

			// сделать основной кнопкой - будет запускать игру
			_btnRecreatePoints = new ViewButton();
			AddComponent(_btnRecreatePoints);
			_btnRecreatePoints.InitButton(RecreatePoints, "RecreatePoints", "hint", Keys.Y);
			_btnRecreatePoints.SetParams(110, 15, 140, 25, "RecreatePoints");
			_btnRecreatePoints.InitTexture("textRB", "textRB");

			// нужно что бы сравнивать ресурсы заказов
			/*var btnRIView = new ViewButton();
			AddComponent(btnRIView);
			btnRIView.InitButton(RIView, "btnRIView", "hint", Keys.S);
			btnRIView.SetParams(250, 135, 60, 30, "btnRIView");
			btnRIView.InitTexture("textRB", "textRB");*/

			var btnShipsEdit = new ViewButton();
			AddComponent(btnShipsEdit);
			btnShipsEdit.InitButton(OIView, "Установка улучшений", "Установка улучшений", Keys.E);
			btnShipsEdit.SetParams(720, 15, 110, 25, "btnShipEdit");
			btnShipsEdit.InitTexture("textRB", "textRB");

			// нужно что бы смотреть имеющиеся заказы
			var btnOIView = new ViewButton();
			AddComponent(btnOIView);
			btnOIView.InitButton(OIView, "Заказы", "Просмотр информации о заказах", Keys.Q);
			btnOIView.SetParams(1460, 15, 110, 25, "btnOIView");
			btnOIView.InitTexture("textRB", "textRB");
			
			_btnShop = new ViewButton();
			AddComponent(_btnShop);
			_btnShop.InitButton(ShowShop, "Магазин", "hint", Keys.S);
			_btnShop.SetParams(550, 15, 80, 25, "btnShop");
			_btnShop.InitTexture("textRB", "textRB");
			_btnShop.Enabled = false;

			_btnResearches = new ViewButton();
			AddComponent(_btnResearches);
			_btnResearches.InitButton(ResearchesView, "Исследования", "hint", Keys.S);
			_btnResearches.SetParams(390, 15, 110, 25, "btnResearches");
			_btnResearches.InitTexture("textRB", "textRB");
			_btnResearches.Enabled = false;

			_shipsPanel = new ViewShipsPanel();
			AddComponent(_shipsPanel);
			_shipsPanel.SetParams(30, 50, 1620, 220, "ShipsPanel");
			_shipsPanel.OnUpgradeShip = SUEView;

			var btnClose = new ViewButton();
			AddComponent(btnClose);
			btnClose.InitButton(Close, "exit", "hint", Keys.LMenu, Keys.X);
			btnClose.SetParams(1659, 0, 20, 20, "btnE");

			_showMoney = ViewLabelIcon.Create(280, 10, Color.Red, "0", "bigFont", "Resources.Sign1");
			AddComponent(_showMoney);

			Input.AddKeyActionSticked(StartShip, Keys.LButton);
		}

		private void ShowShop()
		{
			var upgrades = ItemsManager.GetUpgrades(_canBuyNormalUpgrades, _canBuyExtraUpgrades, _canBuyAutopilot);
			_shopUpgradesBuyWindow = new ShopUpgradesBuyWindow();
			_shopUpgradesBuyWindow.InitWindow(_viewManager, upgrades, OnUpdateMoneyInfo, ShowShopClose);
		}

		private void ShowShopClose()
		{
			_shopUpgradesBuyWindow = null;
		}

		private void RIView()
		{
			new ResourcesInfosViewWindow().InitWindow(_viewManager, ResourceInfos);
		}

		private void OIView()
		{
			new OrdersInfosViewWindow().InitWindow(_viewManager, OrderInfos);
		}

		private void SUEView(Ship ship)
		{
			new ShipUpgradesEditWindow().InitWindow(_viewManager, _ships, ship);
		}

		private void ResearchesView()
		{
			var researches = ItemsManager.GetResearches().ToList();
			_researchesBuyWindow = new ResearchesBuyWindow();
			_researchesBuyWindow.InitWindow(_viewManager, researches, OnUpdateMoneyInfo, ResearchesViewClosed);
		}

		private void ResearchesViewClosed()
		{
			// из-за модального режима нельзя создавать графические объекты - их события будут потом удалены при выключении модального режима
			_shipsPanel.CreateNewShipPanels();
			_researchesBuyWindow = null;
		}

		private void RecreatePoints()
		{
			_nearest = null;
			OnRecreatePoints?.Invoke();
			_btnResearches.Enabled = true;
		}

		private void StartShip()
		{
			var planet = _nearest as Planet;
			if (planet?.Order == null) return;
			var res = _ships.SendShip(planet.Order.Source, planet.Order.Destination);
			if (!res)
				ViewHelper.ShowBigMessage("корабль не запустился");
		}

		public void MoneyChanged(int amount)
		{
			_showMoney.Text = amount.ToString();
			_availableResearchesToBuy = ItemsManager.IsAvailableResearchesToBuy();
			_researchesBuyWindow?.UpdateBuyButtons();
			_shopUpgradesBuyWindow?.UpdateBuyButtons();
		}

		protected override void Cursor(int cursorX, int cursorY)
		{
			if (cursorX == _curX && cursorY == _curY) return;
			if (_roadPoints.Count == 0) return;
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
			if (_availableResearchesToBuy && _btnResearches.Enabled) {
				visualizationProvider.SetColor(Color.Red);
				var x = _btnResearches.X + _btnResearches.Width;
				var y = _btnResearches.Y;
				visualizationProvider.DrawTexture(x - 8, y - 8, "RoundDigits.red");
			}

			visualizationProvider.SetColor(Color.White);
			visualizationProvider.OffsetAdd(_mapX, _mapY);

			foreach (var e in _roadEdgesMST) {
				visualizationProvider.Line(e.A.X, e.A.Y, e.B.X, e.B.Y);
			}

			if (_nearest != null) {
				visualizationProvider.SetColor(Color.OrangeRed);
				visualizationProvider.Circle(_nearest.X, _nearest.Y, 10);
			}

			if (_ships != null) {
				visualizationProvider.SetColor(Color.LightCoral);
				foreach (var ship in _ships) {
					DrawShipMapInfo(visualizationProvider, ship);
				}
			}

			foreach (var p in _roadPoints) {
				DrawPlanetInfo(visualizationProvider, p);
			}

			visualizationProvider.OffsetRemove();
		}

		private void DrawShipMapInfo(VisualizationProvider visualizationProvider, Ship ship)
		{
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

		private void DrawPlanetInfo(VisualizationProvider visualizationProvider, Planet p)
		{
			if (p == _nearest)
				visualizationProvider.SetColor(Color.White);
			else
				visualizationProvider.SetColor(Color.White, 75);

			const int offsetResources = 16;
			if (p.IsDepot)
				visualizationProvider.DrawTexturePart(p.X - offsetResources, p.Y - offsetResources, "Resources.ShipDepot", offsetResources * 2, offsetResources * 2);
			if (p.Order != null) {
				visualizationProvider.Rectangle(p.X - 1, p.Y - 1, 3, 3);
				DrawPlanetOrderInfo(visualizationProvider, p, p == _nearest);
			}

		}

		private void DrawPlanetOrderInfo(VisualizationProvider visualizationProvider, Planet p, bool fullView)
		{
			var o = p.Order;
			if (fullView) {
				visualizationProvider.SetColor(Color.Black, 70);
				visualizationProvider.Box(_nearest.X - 10, _nearest.Y - 10, 250, 150);
				visualizationProvider.SetColor(Color.Green, 40);
				visualizationProvider.Rectangle(_nearest.X - 10, _nearest.Y - 10, 250, 150);

				visualizationProvider.SetColor(Color.DarkSeaGreen);
				visualizationProvider.Line(o.Destination.X, o.Destination.Y, o.Source.X, o.Source.Y);

				var i = 0;
				var oi = o.GetInfo();
				foreach (var item in oi) {
					i++;
					visualizationProvider.Print(p.X, p.Y + 20 + i * 15, item);
				}
			}
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Print(p.X, p.Y, o.OrderName);
			visualizationProvider.Print(p.X, p.Y + 20, o.OrderDescription);
			var k = 200 / o.ProgressMax;
			int ttl = (int)(o.ProgressMax * k);
			int inp = (int)(o.ProgressInMove * k);
			int mvd = (int)(o.ProgressMoved * k);
			visualizationProvider.SetColor(Color.Red);
			visualizationProvider.Box(p.X + inp + mvd, p.Y - 30, ttl - inp - mvd, 15);
			visualizationProvider.SetColor(Color.Yellow);
			visualizationProvider.Box(p.X + mvd, p.Y - 30, inp, 15);
			visualizationProvider.SetColor(Color.Green);
			visualizationProvider.Box(p.X, p.Y - 30, mvd, 15);
		}

		/// <summary>
		/// При изменении заказов проверить, не выделена ли теперь планета без заказа
		/// </summary>
		public void OrdersChanged()
		{
			if (_nearest != null && (_nearest as Planet)?.Order == null)
				_nearest = null;
		}

		private bool _openShop;
		private bool _canBuyNormalUpgrades;
		private bool _canBuyExtraUpgrades;
		private bool _canBuyAutopilot;
		public void UpdateResearchInfo()
		{
			if (!_openShop && ItemsManager.IsResearchItemBuyed("OpenShop")) {
				_openShop = true;
				_btnShop.Enabled = true;
			}
			if (!_canBuyNormalUpgrades && ItemsManager.IsResearchItemBuyed("CanBuyNormalUpgrades")) {
				_canBuyNormalUpgrades = true;
			}
			if (!_canBuyExtraUpgrades && ItemsManager.IsResearchItemBuyed("CanBuyExtraUpgrades")) {
				_canBuyExtraUpgrades = true;
			}
			if (!_canBuyAutopilot && ItemsManager.IsResearchItemBuyed("CanBuyAutopilot")) {
				_canBuyAutopilot = true;
			}
		}
	}
}