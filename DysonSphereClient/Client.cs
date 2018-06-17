using DysonSphereClient.Game;
using Engine;
using Engine.Data;
using Engine.DataPlus;
using Engine.Enums;
using Engine.Enums.Client;
using Engine.EventSystem;
using Engine.Helpers;
using Engine.Models;
using Engine.TCPNet;
using Engine.Utils;
using Engine.Visualization;
using Engine.Visualization.Debug;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace DysonSphereClient
{
	/// <summary>
	/// Сервер
	/// </summary>
	public class Client
	{
		private DataSupportBase _datasupport;
		private LogSystem _logsystem;
		private Collector _collector;
		private Input _input;
		private VisualizationProvider _visualization;
		private string LogTag = "Client";
		private ModelMainClient _model;
		private ViewManager _viewManager;
		private Timer _timer;
		private UserRegistration _rplayer;
		private ModelViewManager _modelClientManager;

		/// <summary>
		/// Значение времени для рассчёта количества кадров в секунду
		/// </summary>
		public static int TimerInterval = 1000 / 60;

		public Client(DataSupportBase dataSupport, LogSystem logSystem)
		{
			// сохраняем объект для работы с данными
			_datasupport = dataSupport;
			// инициализируем настройки
			Settings.Init();
			// сохраняем обработчик логов
			_logsystem = logSystem;
			StateClient.InitState();

			_timer = new Timer();
			_timer.Interval = TimerInterval;
			_timer.Tick += MainTimerRun;

			//отсюда всё перенести в ModelClientManage и/или в ModelMenu ViewMenu
			// создать игрока и добавить его к игрокам
			// сформировать соединение и прописать метод для его запуска
			// у игрока сделать 2 метода - регистрация и логин

			// коллектор получает необходимые классы из ДЛЛ через базу
			var classesList = _datasupport.GetCollectClasses();
			_collector = new Collector();
			_collector.LoadClasses(classesList);

			// создаётся объект для работы с пользовательским вводом
			var inputId = _datasupport.ServerSettingsGetValue("input");
			_input = _collector.GetObject(inputId) as Input;
			_input.OnGetWindowPos += ClientGetWindowPos;

			// 3 создаётся объект для вывода на экран
			var visualizationId = _datasupport.ServerSettingsGetValue("visualization");
			_visualization = _collector.GetObject(visualizationId) as VisualizationProvider;
			_visualization.InitVisualization(_datasupport, _logsystem, 500, 500, true);

			// 1 создаётся объект для работы с пользователями (мат модель работы с пользователями)
			_rplayer = _datasupport.UserStatus;// загружаем данные игрока (основные)
			_model = new ModelMainClient(_datasupport, _collector, _rplayer.UserGUID, _rplayer.NickName);
			_visualization.ExitMessage += _model.Stop;
			_model.OnRegistrationResult += RegisterResult;
			_model.OnLoginResult += LoginResult;

			_viewManager = new ViewManager(_visualization, _input);
			ViewHelper.SetViewManager(_viewManager);
			// соединяем модели, формируем основные пути передачи информации
			// вынести в отдельный метод. делать что то наподобие serverInitializer нету смысла - надо будет передавать много параметров, а они уникальные

			/*var debugView = new DebugView();// keep - example
			_viewManager.AddView(debugView);
			debugView.SetParams(1100, 0, debugView.Width, debugView.Height, "DebugView");*/

			var dragable = new ViewDragable();
			dragable.SetParams(800, 250, 30, 30, "dragableObject");
			_viewManager.AddView(dragable, true);

			/*var gv = new GameView();
			_viewManager.AddView(gv);
			gv.SetParams(0, 0, _visualization.CanvasWidth, _visualization.CanvasHeight, "game view");*/

			_modelClientManager = new ModelViewManager();
			_modelClientManager.OnExit += OnExit;
			_modelClientManager.Start(_model, _viewManager, _rplayer);

			// 2 создаётся объект для работы с играми (мат модель запуска серверов игр)
			// 4 создаётся обработчик соединений

			Log("Клиент работает");
		}

		private void OnExit()
		{
			_datasupport.UserStatus = _rplayer;
		}

		private void RegisterResult(ResultOperation result)
		{
			if (result.Result == ErrorType.NoError) {
				StateClient.RegistrationState = RegistrationState.Registered;
				return;
			}
			if (result.Result == ErrorType.UserAlreadyRegistered) {
				StateClient.RegistrationState = RegistrationState.RegistrationRejected;
				StateClient.RegistrationMessage = result.Message;
				return;
			}
			StateClient.RegistrationState = RegistrationState.NotRegistered;
		}

		private void LoginResult(ResultOperation result)
		{
			if (result.Result == ErrorType.NoError) {
				StateClient.LoginState = LoginState.LogIn;
				return;
			}
			if (result.Result == ErrorType.LoginFailed) {
				StateClient.LoginState = LoginState.NotLogedIn;
				StateClient.RegistrationMessage = result.Message;
				return;
			}
			StateClient.RegistrationState = RegistrationState.NotRegistered;
		}

		private System.Drawing.Point ClientGetWindowPos()
		{
			return _visualization.WindowLocation;
		}

		/// <summary>
		/// Инициализируем и запускаем игру по коду
		/// </summary>
		/// <param name="gameId"></param>
		private void InitGame(int gameId)
		{
			// получаем инициализатор игры
			var gi = _collector.GetObject(gameId) as GameInitializer;
			gi.InitGame(_model, _viewManager, _visualization, _logsystem, _input);
		}

		private void Log(string msg)
		{
			_logsystem?.AddLog(LogTag, msg, 1);
		}

		/// <summary>
		/// Запуск обработки по таймеру и визуализации
		/// </summary>
		public void Run()
		{
			_timer.Start();
			_visualization.Run();
		}

		/// <summary>
		/// Основной цикл. по таймеру
		/// </summary>
		private void MainTimerRun(object sender, EventArgs eventArgs)
		{
			Checkers.CheckOnce();
			_input.ProcessInput();
			// обработка сетевого взаимодействия (получение обновления)
			_model.Tick();
			_viewManager.Draw();
		}
	}
}
