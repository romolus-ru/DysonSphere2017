using Engine;
using Engine.EventSystem;
using Engine.Helpers;
using Engine.Utils;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngineTools
{
	/// <summary>
	/// Сервер
	/// </summary>
	public class Tools
	{
		private DataSupportBase _datasupport;
		private LogSystem _logsystem;
		private Collector _collector;
		private Input _input;
		private VisualizationProvider _visualization;
		private string LogTag = "Tools";
		private ModelMain _model;
		private ViewManager _viewManager;
		private Timer _timer;
		private ModelViewManager _modelClientManager;

		/// <summary>
		/// Значение времени для рассчёта количества кадров в секунду
		/// </summary>
		public static int TimerInterval = 1000 / 60;

		public Tools(DataSupportBase dataSupport, LogSystem logSystem)
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

			_model = new ModelMain();
			_visualization.ExitMessage += _model.Stop;

			_viewManager = new ViewManager(_visualization, _input);
			ViewHelper.SetViewManager(_viewManager);
			// соединяем модели, формируем основные пути передачи информации"DebugView");*/

			_modelClientManager = new ModelViewManager();
			_modelClientManager.Start(_model, _viewManager);

			// 2 создаётся объект для работы с играми (мат модель запуска серверов игр)
			// 4 создаётся обработчик соединений

			Log("Клиент работает");
		}

		private System.Drawing.Point ClientGetWindowPos()
		{
			return _visualization.WindowLocation;
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
