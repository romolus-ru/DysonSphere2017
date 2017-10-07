using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EventSystem.Event
{
	[Serializable]
	/// <summary>
	/// Строковое сообщение
	/// </summary>
	public class MessageString : EventBase
	{
		public string Message = string.Empty;
		public int num;
		public DateTime dt;
		public static MessageString Create(string msg)
		{
			var r = new MessageString();
			r.Message = msg;
			return r;
		}
	}
}