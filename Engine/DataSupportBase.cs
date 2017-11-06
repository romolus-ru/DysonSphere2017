using Engine.Data;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
	/// <summary>
	/// Основной класс. Организует доступ к данным
	/// </summary>
	public class DataSupportBase
	{
		protected virtual string LogTag { get { return "dataSupport"; } }
		private LogSystem _logSystem;

		public void InitLogSystem(LogSystem logSystem) { _logSystem = logSystem; }

		/// <summary>Логирование информации</summary>
		/// <param name="msg"></param>
		protected void Log(string msg) { _logSystem?.AddLog(LogTag, msg, 0); }

		/// <summary>
		/// Сохранить все изменения
		/// </summary>
		public virtual void SaveChanges() { }

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

		public virtual void ServerSettingsSetValue(string valueName, int classId)
		{

		}

		/// <summary>
		/// Save or Update CollectClass
		/// </summary>
		/// <param name="collectClass">entity</param>
		/// <param name="save">save now</param>
		public virtual void SaveCollectClasses(CollectClass collectClass, bool save = true)
		{

		}

		/// <summary>
		/// Удалить класс из базы
		/// </summary>
		/// <param name="collectClass"></param>
		/// <param name="save"></param>
		public virtual void DeleteCollectClasses(CollectClass collectClass, bool save = true)
		{

		}
	}
}