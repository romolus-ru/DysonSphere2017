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
			var classesList = _datasupport.GetCollectClasses();			
			var collector = new Collector();
			collector.LoadClasses(classesList);

			// таблица settings и там указать какие классы для чего использовать
			var visualizationId = _datasupport.ServerSettingsSetValue("visualization");
			получить нужные данные и закэшировать их. что бы что бы потом уже без запроса всё получать
			// надо какие то настройки сервера получить. тоже из базы
			// основные настройки - объект визуализации. остальное пока по умолчанию

			// создаётся объект для вывода на экран
			var visualization = collector.GetObject(visualizationId);
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
