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
		private TCPServer _tcpServer;
		private bool _stopTCPServer = false;
		public Action<TCPEngineConnector> OnPlayerConnected;
		/// <summary>
		/// Обработать клиентов, у которых есть необработанные данные
		/// </summary>
		public Action<HashSet<int>> OnServerProcessPlayers;

		public TCPServerModel(Collector collector)
		{
			_tcpServer = new TCPServer();
			_tcpServer.collector = collector;
			_tcpServer.OnClientConnected += OnClientConnected;
			new Thread(() => ProcessData()).Start();
		}

		public override void Tick()
		{
			HashSet<int> playersId = null;
			lock (_tcpServer.PlayersWithMessages) {
				if (_tcpServer.PlayersWithMessages.Count == 0) return;
				playersId = new HashSet<int>(_tcpServer.PlayersWithMessages);
				_tcpServer.PlayersWithMessages.Clear();
			}
			// отправляем на обработку
			OnServerProcessPlayers?.Invoke(playersId);
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

		private void OnClientConnected(TCPEngineConnector client)
		{
			OnPlayerConnected?.Invoke(client);
		}
	}
}
