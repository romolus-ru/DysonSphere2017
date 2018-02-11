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
        /// Аутентификация пользователя. там создаётся объект player с идентификацией и соответственно заполняются при необходимости поля для авторизации разрешенных действий
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public delegate int AuthPlayerDelegate(TCPEngineConnector conn);
        /// <summary>
        /// Авторизовываем игрока с которым установлено соединение
        /// </summary>
        public AuthPlayerDelegate AuthPlayer;

        public Collector collector;
		private static int _counterPlayer = -1;
		public List<TCPMessage> RecievedMessages = new List<TCPMessage>();

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
            TcpListener listener = (TcpListener)ar.AsyncState;

            // End the operation and display the received data on the console.
            TcpClient client = listener.EndAcceptTcpClient(ar);

			_counterPlayer++;
            var client1 = new TCPEngineConnector();
			client1.playerId = _counterPlayer;
            client1.Init(client);
            client1.SetCollector(collector);
            AuthPlayer?.Invoke(client1);
            lock (_clientsInfo) _clientsInfo.Add(client1);
            // Process the connection here. (Add the client to a server table, read data, etc.)
            //client1.SendMsg(client1.MSGHello());
            //LOG("Client connected completed");
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
			List<TCPMessage> messages = null;
            foreach (var client1 in _clientsInfo)
            {
                var client = client1.Client;
                if (client.Available == 0) continue;// нету доступных данных
				var msgs = client1.ProcessData();
				if (msgs == null) continue;
				if (messages == null) messages = new List<TCPMessage>();
				messages.AddRange(msgs);
            }
			if (messages != null)
				lock (RecievedMessages)
					RecievedMessages.AddRange(messages);

            // принимаем соединение
            if (listener.Pending()) DoBeginAcceptTcpClient(listener);
        }

    }
}