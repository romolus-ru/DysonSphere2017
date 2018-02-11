using Engine.Data;
using Engine.Enums;
using Engine.TCPNet;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Engine.Models
{
	public class TCPServerModel : Model
	{
		public Action<int, UserRegistration> RegisterPlayer;
		private TCPServer _tcpServer;
		private bool _stopTCPServer = false;
		private Dictionary<TCPOperations, Action<TCPMessage>> _operate;

		public TCPServerModel(Collector collector)
		{
			_operate = new Dictionary<TCPOperations, Action<TCPMessage>>();
			_operate[TCPOperations.Registration] = RegistrationMessage;
			_tcpServer = new TCPServer();
			_tcpServer.collector = collector;
			new Thread(() => ProcessData()).Start();
		}

		public override void Tick()
		{
			List<TCPMessage> messages = null;
			lock (_tcpServer.RecievedMessages) {
				if (_tcpServer.RecievedMessages.Count == 0) return;
				messages = new List<TCPMessage>(_tcpServer.RecievedMessages);
				_tcpServer.RecievedMessages.Clear();
			}
			// обрабатываем
			foreach (var msg in messages) {
				ProcessMessage(msg);
			}
		}

		private void ProcessMessage(TCPMessage msg)
		{
			var opcode = msg.opCode;
			if (_operate.ContainsKey(opcode))
				_operate[opcode](msg);
		}

		private void RegistrationMessage(TCPMessage msg)
		{
			if (msg._msg is UserRegistration)
				RegisterPlayer?.Invoke(msg.PlayerId, msg._msg as UserRegistration);
		}

		public override void Stop()
		{
			_stopTCPServer = true;
		}

		private void ProcessData()
		{
			_tcpServer.StartServer();
			while (!_stopTCPServer)
				_tcpServer.ProcessData();
		}
	}
}
