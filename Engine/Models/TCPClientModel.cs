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
	public class TCPClientModel : Model
	{
		public Action<int, UserRegistration> RegisterPlayer;
		private TCPClient _tcpClient;
		private bool _stopTCPClient = false;
		private Dictionary<TCPOperations, Action<TCPMessage>> _operate;
		private List<TCPMessage> RecievedMEssages = new List<TCPMessage>();

		public TCPClientModel(Collector collector)
		{
			_operate = new Dictionary<TCPOperations, Action<TCPMessage>>();
			_operate[TCPOperations.Registration] = RegistrationMessage;
			_tcpClient = new TCPClient();
			_tcpClient.SetCollector(collector);
		}

		public void Connect(string server, int serverPort)
		{
			_tcpClient.Init();
			_tcpClient.ConnectToServer(server, serverPort);
			new Thread(() => ProcessData()).Start();
		}

		public override void Tick()
		{
			List<TCPMessage> messages = null;
			lock (_tcpClient.RecievedMessages) {
				if (_tcpClient.RecievedMessages.Count == 0) return;
				messages = new List<TCPMessage>(_tcpClient.RecievedMessages);
				_tcpClient.RecievedMessages.Clear();
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
			_stopTCPClient = true;
		}

		private void ProcessData()
		{
			while (!_stopTCPClient)
				_tcpClient.ProcessData();
		}
	}
}
