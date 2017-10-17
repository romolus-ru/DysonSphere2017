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

namespace DysonSphere
{
	/// <summary>
	/// Сервер
	/// </summary>
	public class Server
	{
		private DataSupportBase _datasupport;
		private LogSystem _logsystem;
		private string LogTag = "Server";
		private ModelMain _model;
		private ViewMain _view;
		private Stopwatch _stopwatch;

		public Server(DataSupportBase dataSupport, LogSystem logSystem)
		{
			_stopwatch = new Stopwatch();
			// сохраняем объект для работы с данными
			_datasupport = dataSupport;
			// сохраняем обработчик логов
			_logsystem = logSystem;

			// коллектор получает необходимые классы из ДЛЛ через базу
			var classesList = _datasupport.GetCollectClasses();			
			var collector = new Collector();
			collector.LoadClasses(classesList);

			// создаётся объект для вывода на экран
			var visualizationId = _datasupport.ServerSettingsGetValue("visualization");
			var visualization = collector.GetObject(visualizationId) as VisualizationProvider;
			visualization.InitVisualization(500, 500, true);

			_model = new ModelMain();
			_view = new ViewMain(visualization);
			// соединяем модели, формируем основные пути передачи информации

			// запуск и обработку перенести в отдельный поток
			visualization.Run();

			// создаётся объект для работы с пользователями
			// создаётся объект для работы с играми
			// создаётся обработчик соединений
			
			
			// TODO запустить сервер и в визуализации вывести меняющийся stopwatch

			Log("Сервер работает");
		}

		private void Log(string msg) {
			_logsystem?.AddLog(LogTag, msg, 1);
		}
	}
}
