﻿using Engine;
using Engine.Data;
using Engine.Utils;
using Engine.Visualization;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace DysonSphere
{
	/// <summary>
	/// Сервер
	/// </summary>
	public class Server
	{
		private Stopwatch _stopwatch;
		private DataSupportBase _datasupport;
		private LogSystem _logsystem;
		private Collector _collector;
		private Input _input;
		private VisualizationProvider _visualization;
		private string LogTag = "Server";
		private ModelMain _model;
		private ViewManager _viewManager;
		private Timer _timer;

		/// <summary>
		/// Значение времени для рассчёта количества кадров в секунду
		/// </summary>
		public static int TimerInterval = 1000 / 60;

		public Server(DataSupportBase dataSupport, LogSystem logSystem)
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

			// коллектор получает необходимые классы из ДЛЛ через базу
			var classesList = _datasupport.GetCollectClasses();
			_collector = new Collector();
			_collector.LoadClasses(classesList);

			// создаётся объект для работы с пользовательским вводом
			var inputId = _datasupport.ServerSettingsGetValue("input");
			_input = _collector.GetObject(inputId) as Input;

			// 3 создаётся объект для вывода на экран
			var visualizationId = _datasupport.ServerSettingsGetValue("visualization");
			_visualization = _collector.GetObject(visualizationId) as VisualizationProvider;
			_visualization.InitVisualization(_datasupport, _logsystem, 500, 500, true);

			_model = new ModelMain();
			_viewManager = new ViewManager(_visualization, _input);
			// соединяем модели, формируем основные пути передачи информации
			// вынести в отдельный метод. делать что то наподобие serverInitializer нету смысла - надо будет передавать много параметров, а они уникальные
			var serverView = new ServerView();
			serverView.SetTimerInfo(_stopwatch);
			_viewManager.AddView(serverView);

			var pnl = new ViewPanel();
			_viewManager.AddView(pnl);
			pnl.SetParams(100, 100, 600, 400, "Pnl");

			var btn1 = new ViewButton();
			pnl.AddComponent(btn1);
			btn1.InitButton(MsgToModel, "c", "hint", Keys.Y);
			btn1.SetParams(20, 20, 40, 40, "btn1");
			//btn1.InitTexture("btn0");

			var btn2 = new ViewButton();
			pnl.AddComponent(btn2);
			btn2.InitButton(RunModalWindow, "c", "hint", Keys.U);
			btn2.SetParams(70, 20, 40, 40, "btn2");
			btn2.InitTexture("testbutton2", "testbutton2a", 10);

			var btnClose = new ViewButton();
			_viewManager.AddView(btnClose);
			btnClose.InitButton(Close, "exit", "hint", Keys.LMenu, Keys.X);
			btnClose.SetParams(1651, 0, 20, 20, "btn2");

			var debugView = new DebugView();
			_viewManager.AddView(debugView);

			var dragable = new ViewDragable();
			dragable.SetParams(800, 250, 30, 30, "dragableObject");
			_viewManager.AddView(dragable, true);

			// 1 создаётся объект для работы с пользователями (мат модель работы с пользователями)
			// 2 создаётся объект для работы с играми (мат модель запуска серверов игр)
			// 4 создаётся обработчик соединений

			Log("Сервер работает");
		}

		private void Close()
		{
			_visualization.Exit();
		}

		private void MsgToModel()
		{
			var a = 1;
		}

		private ViewModalWindow win;
		private void RunModalWindow()
		{
			win = new ViewModalWindow();
			_viewManager.AddViewModal(win);
			win.SetParams(150, 150, 500, 150, "win");
			win.InitTexture("WindowSample", 10);

			var btn1 = new ViewButton();
			win.AddComponent(btn1);
			btn1.InitButton(Entered, "ok", "Согласен", Keys.Enter);
			btn1.SetParams(20, 110, 40, 25, "btn1");
			btn1.InitTexture("WindowSample", "WindowSample", 10);

			var btn2 = new ViewButton();
			win.AddComponent(btn2);
			btn2.InitButton(CloseModalWindow, "Cancel", "Отмена", Keys.Escape);
			btn2.SetParams(70, 110, 40, 25, "btn2");
			btn2.InitTexture("WindowSample", "WindowSample", 10);

			var field = new ViewInput();
			win.AddComponent(field);
			field.SetParams(30, 30, 200, 40, "inputField");
			win.InitInputDialog(field);
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

		private void Log(string msg) {
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
