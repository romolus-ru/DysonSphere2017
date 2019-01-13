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
		public Action OnRecreatePoints;
		public Action OnExitPressed;
		public Func<int, int, ScreenPoint> OnFindNearest;
		public List<ResourceInfo> ResourceInfos;
		public List<OrderInfo> OrderInfos;
		public Action OnUpdateMoneyInfo;
		public Action OnStartFinalOrderPressed;
		public Action OnRestart;

		private ViewManager _viewManager;
		private List<Planet> _roadPoints = new List<Planet>();
		private List<ScreenEdge> _roadEdgesMST = new List<ScreenEdge>();
		private Ships _ships;
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
		private ViewButton _btnStartFinalOrder;
		private ViewButton _btnShipsEdit;
		private bool _availableResearchesToBuy;
		private ResearchesBuyWindow _researchesBuyWindow;
		private ShopUpgradesBuyWindow _shopUpgradesBuyWindow;
		private bool _finalOrderStarted;
		private DateTime _finalOrderTimer;
		private bool _showIntro;
		private string _introTexture = "Result.Welcome";

		public void InitTransportGame(ViewManager viewManager)
		{
			_viewManager = viewManager;
			_showIntro = true;
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

			_btnShipsEdit = new ViewButton();
			AddComponent(_btnShipsEdit);
			_btnShipsEdit.InitButton(ShipsEdit, "Установка улучшений", "Установка улучшений", Keys.E);
			_btnShipsEdit.SetParams(630, 15, 140, 25, "btnShipEdit");
			_btnShipsEdit.InitTexture("textRB", "textRB");
			_btnShipsEdit.Enabled = false;
			
			_btnStartFinalOrder = new ViewButton();
			AddComponent(_btnStartFinalOrder);
			_btnStartFinalOrder.InitButton(StartFinalOrderPressed, "Запустить Главный Заказ", "Запустить финальный главный заказ", Keys.O);
			_btnStartFinalOrder.SetParams(790, 15, 200, 25, "btnStartFinalOrder");
			_btnStartFinalOrder.InitTexture("textRB", "textRB");
			_btnStartFinalOrder.Enabled = false;

			// нужно что бы смотреть имеющиеся заказы
			var btnOIView = new ViewButton();
			AddComponent(btnOIView);
			btnOIView.InitButton(OIView, "Заказы", "Просмотр информации о заказах", Keys.Q);
			btnOIView.SetParams(1460, 15, 90, 25, "btnOIView");
			btnOIView.InitTexture("textRB", "textRB");
			
			_btnShop = new ViewButton();
			AddComponent(_btnShop);
			_btnShop.InitButton(ShowShop, "Магазин", "hint", Keys.S);
			_btnShop.SetParams(530, 15, 80, 25, "btnShop");
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
			btnClose.InitButton(Close, "exit", "Выход", Keys.LMenu, Keys.X);
			btnClose.SetParams(1659, 0, 20, 20, "btnE");

			_showMoney = ViewLabelIcon.Create(280, 10, Color.Red, "0", "bigFont", "Resources.Sign1");
			AddComponent(_showMoney);

			Input.AddKeyActionSticked(StartShip, Keys.LButton);
		}

		private void Restart()
		{
			_btnShipsEdit.Enabled = false;
			_btnStartFinalOrder.Enabled = false;
			_btnShop.Enabled = false;
			_btnResearches.Enabled = false;
			_shipsPanel.ClearShipsPanel();
			_showMoney.Text = "0";

			_openShop = false;
			_canBuyGoodUpgrades = false;
			_canBuyExtraUpgrades = false;
			_canBuyAutopilot = false;
			_finalOrderGranted = false;
			_roadPoints = null;
			_roadEdgesMST = null;
		}

		private void StartFinalOrderPressed()
		{
			OnStartFinalOrderPressed?.Invoke();
		}

		private void ShowShop()
		{
			var upgrades = ItemsManager.GetUpgrades(_canBuyGoodUpgrades, _canBuyExtraUpgrades, _canBuyAutopilot);
			_shopUpgradesBuyWindow = new ShopUpgradesBuyWindow();
			_shopUpgradesBuyWindow.InitWindow(_viewManager, upgrades, OnUpdateMoneyInfo, ShowShopClose);
		}

		private void ShowShopClose()
		{
			_shopUpgradesBuyWindow = null;
		}

		private void ShipsEdit()
		{
			new ShipUpgradesEditWindow().InitWindow(_viewManager, _ships, null);
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
			_showIntro = false;
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
			if ((_roadPoints?.Count ?? 0) == 0) return;
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
			if (_showIntro) {
				DrawIntro(visualizationProvider);
			}
			if (_finalOrderStarted) {
				visualizationProvider.SetFont("bigFont");
				visualizationProvider.SetColor(Color.Red);
				var delta = _finalOrderTimer - DateTime.Now;
				string time = delta.Minutes + ":" + delta.Seconds + ":" + (delta.Milliseconds / 10);
				visualizationProvider.Print(790, 15, time);
				visualizationProvider.SetFont("default");
			}

			if (_availableResearchesToBuy && _btnResearches.Enabled) {
				visualizationProvider.SetColor(Color.Red);
				var x = _btnResearches.X + _btnResearches.Width;
				var y = _btnResearches.Y;
				visualizationProvider.DrawTexture(x - 8, y - 8, "RoundDigits.red");
			}

			visualizationProvider.SetColor(Color.White);
			visualizationProvider.OffsetAdd(_mapX, _mapY);

			if (_roadEdgesMST != null) {
				foreach (var e in _roadEdgesMST) {
					visualizationProvider.Line(e.A.X, e.A.Y, e.B.X, e.B.Y);
				}
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

			if (_roadPoints != null) {
				foreach (var p in _roadPoints) {
					if (p == _nearest)
						continue;
					DrawPlanetInfo(visualizationProvider, p);
				}
			}

			if (_nearest != null)
				DrawPlanetInfo(visualizationProvider, _nearest as Planet);

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

			visualizationProvider.SetColor(o.Level == 1 ? Color.White : Color.DarkSeaGreen);
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
		private bool _canBuyGoodUpgrades;
		private bool _canBuyExtraUpgrades;
		private bool _canBuyAutopilot;
		private bool _finalOrderGranted;
		public void UpdateResearchInfo()
		{
			if (!_finalOrderGranted && ItemsManager.IsResearchItemBuyed("StartFinalOrder")) {
				_finalOrderGranted = true;
				_btnStartFinalOrder.Show();
				_btnStartFinalOrder.Enabled = true;
			}

			if (!_openShop && ItemsManager.IsResearchItemBuyed("OpenShop")) {
				_openShop = true;
				_btnShop.Enabled = true;
				_btnShipsEdit.Enabled = true;
			}
			if (!_canBuyGoodUpgrades && ItemsManager.IsResearchItemBuyed("CanBuyGoodUpgrades")) {
				_canBuyGoodUpgrades = true;
			}
			if (!_canBuyExtraUpgrades && ItemsManager.IsResearchItemBuyed("CanBuyExtraUpgrades")) {
				_canBuyExtraUpgrades = true;
			}
			if (!_canBuyAutopilot && ItemsManager.IsResearchItemBuyed("CanBuyAutopilot")) {
				_canBuyAutopilot = true;
			}
		}
		
		internal void FinalOrderStart(DateTime finalTime)
		{
			_finalOrderStarted = true;
			_finalOrderTimer = finalTime;
			_btnStartFinalOrder.SetVisible(false);
		}

		internal void FinalOrderComplete()
		{
			_introTexture = "Result.Victory";
			_finalOrderStarted = false;
			_showIntro = true;
			Restart();
		}

		internal void FinalOrderNotComplete()
		{
			_finalOrderStarted = false;
			_btnStartFinalOrder.SetVisible(true);
		}

		private void DrawIntro(VisualizationProvider visualizationProvider)
		{
			var strs = new List<string>() {
				"Игра запускается нажатием на кнопку RecreatePoints",
				"На экране появится карта с точками-планетами",
				"На точки с заказами надо кликнуть - к ним полетит свободный корабль",
				"Для выхода из игры есть кнопка справа вверху",
				"",
				"Цель игры - выполнить финальный заказ",
				"Для этого надо запустить первый корабль и покупать в магазине исследования",
				"На начальных этапах игры старайтесь выбирать более легкие заказы",
				"и заказы с небольшим расстоянием перевозки",
				"После открытия магазина надо покупать там улучшения для кораблей",
				"и устанавливать их на корабли (или кнопка <установка улучшений>",
				"или кнопка с инструментами у каждого корабля)",
				"Обязательно надо купить и поставить автопилоты",
				"После открытия улучшений третьего уровня и установке их на корабли",
				"и замены автопилотов нужными модулями",
				"можно будет запустить финальный заказ",
				"",
				"",
			};

			visualizationProvider.DrawTexture(500, 300, _introTexture);
			var row = 0;
			foreach (string str in strs) {
				row++;
				visualizationProvider.Print(450, 350 + row * 15, str);
			}

		}

	}
}