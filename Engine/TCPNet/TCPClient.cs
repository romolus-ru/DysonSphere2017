using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.TCPNet
{
	public class TCPClient : TCPEngineConnector
	{
		public bool HasNewMessages = false;

		/// <summary>
		/// Получаем данные от сервера и сохраняем их в RecievedMessages
		/// </summary>
		public new void ProcessData()
		{
			var messages = base.ProcessData();
			if (messages == -1) return;
			lock (this)
				HasNewMessages = true;
		}
	}
}