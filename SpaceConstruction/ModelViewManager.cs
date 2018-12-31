using Engine;
using Engine.Data;
using Engine.Models;
using Engine.Visualization;
using SpaceConstruction.Game;
using SpaceConstruction.Game.Orders;
using System;
using System.Diagnostics;

namespace SpaceConstruction
{
	/// <summary>
	/// Заставка, загрузка, начальное меню и запуск игр
	/// </summary>
	public class ModelViewManager
	{
		private Stopwatch _stopwatch;

		private ViewManager _viewManager;
		private ModelMainClient _modelMainClient;
		private UserRegistration _rplayer;

		private GameView _gv;
		private ViewProgressBar _vp;
		private ViewMenu _vm;
		private ViewTransportGame _vtg;
		private ModelTransportGame _mtg;

		public Action OnExit;

		public void Start(ModelMainClient modelMainClient, ViewManager viewManager, UserRegistration userRegistration)
		{
			_stopwatch = Stopwatch.StartNew();
			_modelMainClient = modelMainClient;
			_viewManager = viewManager;
			_rplayer = userRegistration;

			ModelIntro mi = new ModelIntro();
			mi.OnComplete += IntroEnded;
			_modelMainClient.AddModel(mi);

			_gv = new GameView();
			_viewManager.AddView(_gv);
			_gv.SetParams(0, 0, _viewManager.Provider.CanvasWidth, _viewManager.Provider.CanvasHeight, "Заставка");
		}

		private void IntroEnded(Model introModel)
		{
			_modelMainClient.DelModel(introModel);
			_viewManager.RemoveView(_gv);
			_gv = null;

			ModelLoadResources ml = new ModelLoadResources();
			ml.OnComplete += ResourcesLoaded;
			ml.OnProgress += ResourcesProgress;
			_modelMainClient.AddModel(ml);

			_vp = new ViewProgressBar();
			_viewManager.AddView(_vp);
			_vp.SetParams(0, 0, _viewManager.Provider.CanvasWidth, _viewManager.Provider.CanvasHeight, "Загрузка");
		}

		private void ResourcesProgress(int progress)
		{
			_vp.Percent = progress;
		}

		private void ResourcesLoaded(Model loadModel)
		{
			_modelMainClient.DelModel(loadModel);
			_viewManager.RemoveView(_vp);
			_vp = null;
			
			_vm = new ViewMenu(_stopwatch);
			_viewManager.AddView(_vm);

			_vm.OnExitPressed += Close;
			_vm.OnStartGame += StartGame;
			StartGame();
		}

		private void Close()
		{
			StateClient.SaveState();
			_viewManager.Provider.Exit();
		}

		private void StartGame()
		{
			_viewManager.RemoveView(_vm);
			_vm = null;

			var orders = new Orders();
			var ships = new Ships(orders);
			_mtg = new ModelTransportGame(ships, orders);
			_modelMainClient.AddModel(_mtg);

			_vtg = new ViewTransportGame();
			_vtg.InitTransportGame(_viewManager);
			_viewManager.AddView(_vtg);

			_vtg.OnRecreatePoints += _mtg.RecreatePoints;
			_mtg.OnSetPoints += _vtg.SetPoints;
			_vtg.OnExitPressed += Close;
			_vtg.OnFindNearest += _mtg.FindNearest;
			_vtg.ResourceInfos = orders.ResourceInfos;
			_vtg.OrderInfos = orders.OrderInfos;
			_vtg.OnUpdateMoneyInfo = _mtg.UpdateMoneyInfo;
			_vtg.OnUpdateMoneyInfo += ships.UpdateResearchInfo;
			_vtg.OnUpdateMoneyInfo += orders.UpdateResearchInfo;
			_vtg.OnUpdateMoneyInfo += _vtg.UpdateResearchInfo;

			//_vtg.OnGetPath += _mtg.GetPath;
			_mtg.OnMoneyChanged += _vtg.MoneyChanged;
			_mtg.OnOrdersChanged += _vtg.OrdersChanged;
		}
	}
}