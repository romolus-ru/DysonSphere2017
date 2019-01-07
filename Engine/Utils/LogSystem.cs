using Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Engine.Utils
{
	/// <summary>
	/// Сохранение логов
	/// </summary>
	public class LogSystem
	{
		private static int _logCounter;
		private List<LogData> _logData = new List<LogData>();
		public Action<LogData> OnNewLogRecieved = null;

		public void AddLog(string message, int level = 0)
		{
			AddLog("", message, level);
		}

		public void AddLog(string tag, string message, int level = 0)
		{
			_logCounter++;
			var ld = new LogData {
				Id = _logCounter,
				Level = level,
				Tag = tag,
				Time = DateTime.Now,
				Message = message
			};
			_logData.Add(ld);
			Debug.WriteLine(tag + " " + message);
			OnNewLogRecieved?.Invoke(ld);
		}

		public List<string> ScanMsg(string partMsg)
		{
			return ScanMsg(null, partMsg);
		}

		public List<string> ScanMsg(string tag, string partMsg)
		{
			var res = new List<string>();
			IEnumerable<LogData> values = null;
			values = tag.IsNullOrEmpty() 
				? _logData 
				: _logData.Where(ld => ld.Tag == tag);
			foreach (var ld in values) {
				if (ld.Message.IndexOf(partMsg, StringComparison.InvariantCultureIgnoreCase) >= 0)
					res.Add(ld.ToString());
			}
			return res;
		}
	}
}