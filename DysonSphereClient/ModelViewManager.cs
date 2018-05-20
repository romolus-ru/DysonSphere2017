using DysonSphereClient.Game;
using Engine;
using Engine.Data;
using Engine.DataPlus;
using Engine.Enums;
using Engine.Enums.Client;
using Engine.Models;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient
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
		private ModelMenu _mm;
		private ViewTransportGame _vtg;
		private ModelTransportGame _mtg;
		private GameAchievements _achievements;

		public Action OnExit;


		public void Start(ModelMainClient modelMainClient, ViewManager viewManager, UserRegistration userRegistration)
		{
			_stopwatch = Stopwatch.StartNew();
			_achievements = new GameAchievements();
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

			_mm = new ModelMenu();

			_vm = new ViewMenu(_viewManager, _stopwatch);
			_viewManager.AddView(_vm);

			_vm.OnConnect += Connect;
			_vm.OnSendLogin += Login;
			_vm.OnRegistration += RegistrationWindow;
			_vm.OnExitPressed += Close;
			_vm.OnStartGame += StartGame;
			StartGame();
		}

		private void Close()
		{
			StateClient.SaveState();
			_viewManager.Provider.Exit();
		}

		private WaitWindow _waitWindow = null;

		private void Connect()
		{
			_waitWindow = new WaitWindow();
			_modelMainClient.ConnectAsync(ConnectionResult, server: "", serverPort: -1);
			_waitWindow.InitWindow(_viewManager, "MESSAGE", ConnectionCancel, "bigFont");
		}

		private void ConnectionCancel()
		{
			_modelMainClient.ConnectionCancel();
		}
		private void ConnectionResult(bool result)
		{
			_waitWindow.CloseWindow();
			_waitWindow = null;
		}


		private void Login()
		{
			StateClient.LoginState = LoginState.LogInRequest;
			var login = new LoginData();
			login.UserGUID = _rplayer.UserGUID;
			login.HSPassword = _rplayer.HSPassword;
			_modelMainClient.SendMessage(TCPOperations.Login, login);
		}


		private void Register()
		{
			StateClient.RegistrationState = RegistrationState.RegistrationRequest;
			_rplayer.NickName = "nick";
			_rplayer.OfficialName = "oname";
			_rplayer.Mail = "a@a.a";
			_rplayer.HSPassword = Engine.Helpers.CryptoHelper.CalculateHash("111");
			_modelMainClient.SendMessage(TCPOperations.Registration, _rplayer);
		}



		private RegistrationWindow rwin;
		private void RegistrationWindow()
		{
			if (StateClient.RegistrationState == RegistrationState.Registered) return;
			new RegistrationWindow().InitWindow(_viewManager, _rplayer, null, null);
			/*rwin = new RegistrationWindow(_rplayer);
			_viewManager.AddViewModal(rwin);
			rwin.SetParams(350, 200, 500, 150, "Регистрация игрока");
			rwin.InitTexture("WindowSample", 10);
			//rwin.OnRegistration += RegistrationWindowClose;
			rwin.OnClose += RegistrationWindowClose;
			*/
		}

		private void StartGame()
		{
			_modelMainClient.DelModel(_mm);
			_mm = null;
			_viewManager.RemoveView(_vm);
			_vm = null;

			var ships = new Ships();
			_mtg = new ModelTransportGame(ships);
			_modelMainClient.AddModel(_mtg);

			_vtg = new ViewTransportGame();
			_vtg.InitTransportGame(_viewManager);
			_viewManager.AddView(_vtg);

			_achievements.SetupAvievementsActions(_vtg);

			_vtg.OnRecreatePoints += _mtg.RecreatePoints;
			_mtg.OnSetPoints += _vtg.SetPoints;
			_vtg.OnExitPressed += Close;
			_vtg.OnFindNearest += _mtg.FindNearest;
			_vtg.OnBuyShip += _mtg.BuyShip;

			//_vtg.OnGetPath += _mtg.GetPath;
			_mtg.OnMoneyChanged += _vtg.MoneyChanged;
			_mtg.OnOrdersChanged += _vtg.OrdersChanged;
		}
	}
}