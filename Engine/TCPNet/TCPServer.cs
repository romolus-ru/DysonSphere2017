using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Engine.TCPNet
{
    public class TCPServer
    {
        // пример вывода логов
        //private void LOG(string msg){Action<string> action = LOG;if (InvokeRequired) Invoke(action, msg);else lbLog.Items.Insert(0, msg);}

        /// <summary>
        /// Создаём модель игрока с которым установлено соединение
        /// </summary>
        public Action<TCPEngineConnector> OnClientConnected;

        public Collector collector;
		private static int _counterPlayer = 0;// 1, 2, 3 and so on
		public HashSet<int> PlayersWithMessages = new HashSet<int>();

        public delegate void LOGGING(string LogMsg);
        public LOGGING ToLog;
        public void LOG(string msg) { ToLog?.Invoke(msg); }

        private TcpListener listener;
        public List<TCPEngineConnector> _clientsInfo = new List<TCPEngineConnector>();
        public string ServerAddress = "localhost";
        public int ServerPort = 5002;

        // Accept one client connection asynchronously.
        public void DoBeginAcceptTcpClient(TcpListener listener)
        {
            // Start to listen for connections from a client.
            LOG("Waiting for a connection...");

            // Accept the connection. BeginAcceptSocket() creates the accepted socket.
            listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), listener);
        }

        // Process the client connection.
        public void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            // Get the listener that handles the client request.
            TcpListener localListener = (TcpListener)ar.AsyncState;

            // End the operation and display the received data on the console.
            TcpClient client = localListener.EndAcceptTcpClient(ar);
			if (!IsAcceptConnection(client)) return;

			_counterPlayer++;
            var client1 = new TCPEngineConnector();
			client1.playerId = _counterPlayer;
            client1.Init(client);
            client1.SetCollector(collector);
            lock (_clientsInfo) _clientsInfo.Add(client1);
			OnClientConnected?.Invoke(client1);
			// Process the connection here. (Add the client to a server table, read data, etc.)
			//client1.SendMsg(client1.MSGHello());
			//LOG("Client connected completed");
		}

		/// <summary>
		/// Проверяем разрешено ли соединение
		/// </summary>
		/// <param name="client"></param>
		/// <returns></returns>
		private bool IsAcceptConnection(TcpClient client)
		{
			// Например проверяем не в чёрном ли списке, количество и время соединений, частота соединений, есть ли возможность соединиться и т.п.
			return true;
		}

		public void StartServer(string server= "", int serverPort= -1)
        {
            var sa = server == "" ? ServerAddress : server;
            var sp = serverPort == -1 ? ServerPort : serverPort;
            IPHostEntry ipHostInfo = Dns.GetHostEntry(sa);
            IPAddress ipAddress = ipHostInfo.AddressList.Where(iphe => iphe.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, sp);
            LOG("Адрес " + ipAddress.ToString() + ", порт " + ServerPort);
            listener = new TcpListener(remoteEP);
            listener.Start(100);
            DoBeginAcceptTcpClient(listener);
        }

        /// <summary>
        /// Обрабатываем каждого клиента и сохраняем что ему прислали в RecievedMessages
        /// </summary>
        public void ProcessData()
        {
			HashSet<int> playersId = null;
            foreach (var client1 in _clientsInfo)
            {
                var client = client1.Client;
                if (client.Available == 0) continue;// нету доступных данных
				var playerId = client1.ProcessData();
				if (playerId == -1) continue;
				if (playersId == null) playersId = new HashSet<int>();
				if (playersId.Contains(playerId)) continue;// уже есть
				playersId.Add(playerId);
            }
			if (playersId != null)
				lock (PlayersWithMessages)
					PlayersWithMessages.UnionWith(playersId);

            // принимаем соединение
            if (listener.Pending()) DoBeginAcceptTcpClient(listener);
        }

    }
}