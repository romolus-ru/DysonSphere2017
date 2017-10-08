using DataSupport;
using Engine.Data;
using Engine.Utils;
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
			var classesList = new List<CollectClass>();
			
			
			// получить из БД список классов
			



			var collector = new Collector();
			collector.LoadClasses(classesList);
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
