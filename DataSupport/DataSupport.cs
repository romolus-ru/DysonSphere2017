﻿using DataSupport.Data;
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
		protected virtual string LogTag { get { return "dataSupportED6"; } }
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

    }
}
