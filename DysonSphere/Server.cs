using DataSupport;
using Engine;
using Engine.Data;
using Engine.Utils;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			// сохраняем обработчик логов
			_logsystem = logSystem;

			_timer = new Timer();
			_timer.Interval = TimerInterval;
			_timer.Tick += MainTimerRun;

			// коллектор получает необходимые классы из ДЛЛ через базу
			var classesList = _datasupport.GetCollectClasses();			
			_collector = new Collector();
			_collector.LoadClasses(classesList);

			// создаётся объект для вывода на экран
			var visualizationId = _datasupport.ServerSettingsGetValue("visualization");
			_visualization = _collector.GetObject(visualizationId) as VisualizationProvider;
			_visualization.InitVisualization(500, 500, true);

			_model = new ModelMain();
			_viewManager = new ViewManager(_visualization);
			// соединяем модели, формируем основные пути передачи информации
			// вынести в отдельный метод. делать что то наподобие serverInitializer нету смысла - надо будет передавать много параметров, а они уникальные
			var serverView = new ServerView(_visualization);
			serverView.SetTimerInfo(GetTime);
			_viewManager.AddView(serverView);

			// создаётся объект для работы с пользователями (мат модель работы с пользователями)
			// создаётся объект для работы с играми (мат модель запуска серверов игр)
			// создаётся обработчик соединений
			
			Log("Сервер работает");
		}

		/// <summary>
		/// Инициализируем и запускаем игру по коду
		/// </summary>
		/// <param name="gameId"></param>
		private void InitGame(int gameId)
		{
			// получаем инициализатор игры
			var gi = _collector.GetObject(gameId) as GameInitializer;
			gi.InitGame(_model, _viewManager, _visualization);
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

		public TimeSpan GetTime()
		{
			return _stopwatch.Elapsed;
		}
		/// <summary>
		/// Основной цикл. по таймеру
		/// </summary>
		private void MainTimerRun(object sender, EventArgs eventArgs)
		{
			//_input.GetInput();// обработка устройств ввода
			_model.Tick();
			_viewManager.Draw();
		}
	}
}
