using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Engine.TCPNet
{
	public class TCPConnector
	{
		/// <summary>
		/// Для клиентского приложения передаём null, иначе при создании сервером он ему передаст настроенного клиента
		/// </summary>
		/// <param name="createClient"></param>
		public void Init(TcpClient client = null)
		{
			IsClient = false;
			if (client == null) {// создаём клиента
				IsClient = true;
				Client = new TcpClient();// создаём клиента. иначе это серверное соединение с клиентом
			}
			if (client != null) {
				counter++;
				Client = client;
				UserName += counter;
			}
		}

		/// <summary>
		/// Клиент или сервер
		/// </summary>
		public bool IsClient = false;

		public TcpClient Client;
		public string UserId = "";
		public string ServerAddress = "localhost";
		public int ServerPort = 5002;

		// переменные для сервера
		public string UserName = "UnnamedUser";
		private static int counter;
		public bool NewUser = true;

		public TCPServer.LOGGING ToLog;
		public void LOG(string msg) { ToLog?.Invoke(msg); }

		/// <summary>
		/// Присланные необработанные данные
		/// </summary>
		protected List<byte[]> DataLoaded = new List<byte[]>();

		public void ConnectToServer(string server, int serverPort)
		{
			var sa = server == "" ? ServerAddress : server;
			var sp = serverPort == -1 ? ServerPort : serverPort;
			IPHostEntry ipHostInfo = Dns.GetHostEntry(sa);
			IPAddress ipAddress = ipHostInfo.AddressList.Where(iphe => iphe.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, sp);

			try {
				Client.Connect(remoteEP);
			}
			catch (SocketException se) {
				var txt = se.Message;
				if (se.ErrorCode == 10013) txt = "Скорее всего сервер не запущен по адресу " + ipAddress.ToString() + " " + txt;
				throw new Exception(txt);
			}
		}

		protected void GetData()
		{
			var st = Client.GetStream();
			if (!st.DataAvailable) return;
			byte[] buffer = new byte[Client.ReceiveBufferSize];
			var readed = st.Read(buffer, 0, buffer.Length);
			if (readed == 0) return;
			byte[] bf = new byte[readed];
			Array.Copy(buffer, bf, readed);

			lock (DataLoaded) DataLoaded.Add(bf);
		}

		/// <summary>
		/// Записать массив байт в поток для передачи
		/// </summary>
		/// <param name="data"></param>
		protected async void SendMsg(byte[] data)
		{
			var st = Client.GetStream();
			await st.WriteAsync(data, 0, data.Length);
			st.Flush();
		}

		public void Disconnect()
		{
			if (Client.Connected) {
				var st = Client.GetStream();
				st.Close();
			}
			Client.Close();
		}

	}
}