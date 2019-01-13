using Engine;
using Engine.Data;
using Engine.EventSystem;
using Engine.Helpers;
using Engine.Utils;
using Engine.Visualization;
using System;
using System.Windows.Forms;

namespace Submarines
{
	/// <summary>
	/// Сервер
	/// </summary>
	public class Client
	{
		private DataSupportBase _datasupport;
		private Input _input;
		private VisualizationProvider _visualization;
		private string LogTag = "Client";
		private ModelMainClient _model;
		private ViewManager _viewManager;
		private Timer _timer;
		private UserRegistration _rplayer;

		public Client(DataSupportBase dataSupport, LogSystem logSystem)
		{
			Settings.Init();
			StateEngine.Log = logSystem;
			StateClient.InitState();
			_datasupport = dataSupport;

			_timer = new Timer();
			_timer.Interval = Constants.TimerInterval;
			_timer.Tick += MainTimerRun;

			// коллектор получает необходимые классы из ДЛЛ через базу
			var classesList = _datasupport.GetCollectClasses();
			var collector = new Collector();
			collector.LoadClasses(classesList);

			// создаётся объект для работы с пользовательским вводом
			var inputId = _datasupport.ServerSettingsGetValue("input");
			_input = collector.GetObject(inputId) as Input;
			_input.OnGetWindowPos += ClientGetWindowPos;

			// создаётся объект для вывода на экран
			var visualizationId = _datasupport.ServerSettingsGetValue("visualization");
			_visualization = collector.GetObject(visualizationId) as VisualizationProvider;
			_visualization.InitVisualization(_datasupport, logSystem, 500, 500, true);

			// создаётся объект для работы с мат моделями
			_rplayer = _datasupport.UserStatus;// загружаем данные игрока (основные)
			_model = new ModelMainClient(_datasupport, collector, _rplayer.UserGUID, _rplayer.NickName);
			_visualization.ExitMessage += _model.Stop;

			_viewManager = new ViewManager(_visualization, _input);
			ViewHelper.SetViewManager(_viewManager);

			var modelClientManager = new ModelViewManager();
			modelClientManager.OnExit += OnExit;
			modelClientManager.Start(_model, _viewManager);

			Log("Клиент работает");
		}

		private void OnExit()
		{
			_datasupport.UserStatus = _rplayer;
		}

		private System.Drawing.Point ClientGetWindowPos()
		{
			return _visualization.WindowLocation;
		}

		private void Log(string msg)
		{
			StateEngine.Log?.AddLog(LogTag, msg, 1);
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
			_model.Tick();
			_viewManager.Draw();
		}
	}
}