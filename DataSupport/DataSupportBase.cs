using DataSupport.Data;
using Engine.Data;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
	/// <summary>
	/// Основной класс. Организует доступ к данным
	/// </summary>
	public class DataSupportBase
	{
		protected virtual string LogTag { get { return "dataSupport"; } }
		private LogSystem _logSystem;

		public void InitLogSystem(LogSystem logSystem) { _logSystem = logSystem; }

		/// <summary>Логирование информации </summary>
		/// <param name="msg"></param>
		protected void Log(string msg) { _logSystem?.AddLog(LogTag, msg, 0); }

		public virtual List<AtlasFiles> AtlasFilesGetAll()
		{
			return null;
		}

		public virtual void SetLog(Action<string> log1)
		{

		}

		public virtual List<CollectClass> GetCollectClasses()
		{
			return null;
		}

		public virtual int ServerSettingsGetValue(string valueName)
		{
			return -1;
		}

		/// <summary>
		/// Save or Update CollectClass
		/// </summary>
		/// <param name="collectClass">entity</param>
		/// <param name="save">save now</param>
		public virtual void SaveCollectClasses(CollectClass collectClass, bool save = true)
		{

		}
	}
}