using Engine;
using Engine.Data;
using Engine.Enums;
using Engine.Models;
using Engine.TCPNet;
using Engine.Utils;
using Engine.Visualization;
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
		private string _solt = "";
		private Stopwatch _stopwatch;
		private DataSupportBase _datasupport;
		private LogSystem _logsystem;
		private Collector _collector;
		private Input _input;
		private VisualizationProvider _visualization;
		private string LogTag = "Client";
		private ModelClient _model;
		private ViewManager _viewManager;
		private Timer _timer;
		private ModelPlayer _mplayer;
		private UserRegistration _rplayer;

		/// <summary>
		/// Значение времени для рассчёта количества кадров в секунду
		/// </summary>
		public static int TimerInterval = 1000 / 60;

		public Client(DataSupportBase dataSupport, LogSystem logSystem)
		{
			_stopwatch = Stopwatch.StartNew();
			// сохраняем объект для работы с данными
			_datasupport = dataSupport;
			// инициализируем настройки
			Settings.Init();
			// сохраняем обработчик логов
			_logsystem = logSystem;

			_timer = new Timer();
			_timer.Interval = TimerInterval;
			_timer.Tick += MainTimerRun;

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
			_model = new ModelClient(_collector);
			_visualization.ExitMessage += _model.Stop;
			var modelPlayers = new ModelPlayers(_datasupport);
			_rplayer = _datasupport.UserStatus;// загружаем данные игрока (основные)
			var clientConnection = new TCPClient();
			_mplayer = new ModelPlayer(_rplayer.UserGUID, _rplayer.NickName, clientConnection);
			modelPlayers.AddClient(_mplayer);// добавляем игрока для обработки в матмоделях
			_model.AddModel(modelPlayers);


			_viewManager = new ViewManager(_visualization, _input);
			// соединяем модели, формируем основные пути передачи информации
			// вынести в отдельный метод. делать что то наподобие serverInitializer нету смысла - надо будет передавать много параметров, а они уникальные
			var clientView = new ClientView();
			clientView.SetTimerInfo(_stopwatch);
			_viewManager.AddView(clientView);

			var pnl = new ViewPanel();
			_viewManager.AddView(pnl);
			pnl.SetParams(100, 100, 600, 400, "Pnl");

			var btn1 = new ViewButton();
			pnl.AddComponent(btn1);
			btn1.InitButton(MsgToModel, "c", "hint", Keys.Y);
			btn1.SetParams(20, 20, 40, 40, "btn1");
			btn1.InitTexture("textRB", "textRB");

			var btn2 = new ViewButton();
			pnl.AddComponent(btn2);
			btn2.InitButton(RunModalWindow, "c", "hint", Keys.U);
			btn2.SetParams(70, 20, 240, 40, "btn2");
			btn2.InitTexture("textRB", "textRB");

			var btn3 = new ViewButton();
			pnl.AddComponent(btn3);
			btn3.InitButton(Connect, "y", "hint", Keys.U);
			btn3.SetParams(70, 35, 240, 40, "btn2");
			btn3.InitTexture("textRB", "textRB");

			var btnClose = new ViewButton();
			_viewManager.AddView(btnClose);
			btnClose.InitButton(Close, "exit", "hint", Keys.LMenu, Keys.X);
			btnClose.SetParams(1659, 0, 20, 20, "btnE");

			//var debugView = new DebugView();
			//_viewManager.AddView(debugView);
			//debugView.SetParams(1100, 0, debugView.Width, debugView.Height, "DebugView");

			var dragable = new ViewDragable();
			dragable.SetParams(800, 250, 30, 30, "dragableObject");
			_viewManager.AddView(dragable, true);

			// 2 создаётся объект для работы с играми (мат модель запуска серверов игр)
			// 4 создаётся обработчик соединений

			Log("Сервер работает");
		}

		private void Connect()
		{
			_model.TCPClientModel.Connect("", -1);
		}

		private Point ClientGetWindowPos()
		{
			return _visualization.WindowLocation;
		}

		private void Close()
		{
			_visualization.Exit();
		}

		private void MsgToModel()
		{
			new LoginWindow().InitWindow(_viewManager, null, null);
		}

		private ViewModalWindow win;
		private void RunModalWindow()
		{
			win = new ViewModalWindow();
			_viewManager.AddViewModal(win);
			win.SetParams(150, 150, 500, 150, "Окно");
			win.InitTexture("WindowSample", 10);

			var btn1 = new ViewButton();
			win.AddComponent(btn1);
			btn1.InitButton(Entered, "ok", "Согласен", Keys.Enter);
			btn1.SetParams(20, 110, 40, 25, "btn1");
			btn1.InitTexture("WindowSample", "WindowSample");

			var btn2 = new ViewButton();
			win.AddComponent(btn2);
			btn2.InitButton(CloseModalWindow, "Cancel", "Отмена", Keys.Escape);
			btn2.SetParams(70, 110, 40, 25, "btn2");
			btn2.InitTexture("WindowSample", "WindowSample");

			var field = new ViewInput();
			win.AddComponent(field);
			field.SetParams(30, 30, 200, 40, "inputField");
			field.IsFocused = true;
		}

		private void Entered()
		{
			CloseModalWindow();
		}

		private void CloseModalWindow()
		{
			if (win == null) return;
			_viewManager.RemoveViewModal(win);
			win = null;
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
			_input.ProcessInput();
			// обработка сетевого взаимодействия (получение обновления)
			_model.Tick();
			_viewManager.Draw();
		}
	}
}
