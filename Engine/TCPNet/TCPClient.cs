using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.TCPNet
{
	public class TCPClient : TCPEngineConnector
	{
		public List<TCPMessage> RecievedMessages = new List<TCPMessage>();

		/// <summary>
		/// Получаем данные от сервера и сохраняем их в RecievedMessages
		/// </summary>
		public new void ProcessData()
		{
			var messages = (this as TCPEngineConnector).ProcessData();
			if (messages != null)
				lock (RecievedMessages)
					RecievedMessages.AddRange(messages);
		}
	}
}