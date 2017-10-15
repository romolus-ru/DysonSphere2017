using DataSupport;
using Engine.Data;
using Engine.Utils;
using Engine.Visualization;
using System;
using System.Collections.Generic;
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

		public Server(DataSupportBase dataSupport, LogSystem logSystem)
		{
			// сохраняем объект для работы с данными
			_datasupport = dataSupport;
			// сохраняем обработчик логов
			_logsystem = logSystem;

			// коллектор получает необходимые классы из ДЛЛ через базу
			var classesList = _datasupport.GetCollectClasses();			
			var collector = new Collector();
			collector.LoadClasses(classesList);

			var visualizationId = _datasupport.ServerSettingsGetValue("visualization");
			// создаётся объект для вывода на экран
			var visualization = collector.GetObject(visualizationId) as VisualizationProvider;
			visualization.InitVisualization(500, 500, true);
			// запуск и обработку перенести в отдельный поток
			visualization.Run();

			// создаётся объект для работы с пользователями
			// создаётся объект для работы с играми
			// создаётся обработчик соединений
			Log("Сервер работает");
		}

		private void Log(string msg) {
			_logsystem?.AddLog(LogTag, msg, 1);
		}
	}
}
