using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Utils;
using Engine.Enums;
using Engine.EventSystem.Event;
using Engine.DataPlus;
using System.Threading;
using System.Net.Sockets;
using Engine.Exceptions;

namespace Engine
{
	public class ModelMainClient : ModelMain
	{
		protected TCPClientModel TCPClientModel { get; private set; }
		protected ModelPlayersClient Players { get; private set; }
		protected ModelPlayerClient Player { get; private set; }
		public Action<ResultOperation> OnRegistrationResult;
		public Action<ResultOperation> OnLoginResult;
		/// <summary>
		/// Признак попытки соединиться
		/// </summary>
		private bool IsAttemptConnection = false;
		private Thread _threadConnect = null;

		public ModelMainClient(DataSupportBase db, Collector collector, string playerGUID, string nickName)
		{
			TCPClientModel = new TCPClientModel(collector);
			TCPClientModel.Init();
			AddModel(TCPClientModel);
			Player = new ModelPlayerClient(playerGUID, nickName);
			AddModel(Player);
			Players = new ModelPlayersClient(Player, db);
			AddModel(Players);

			TCPClientModel.OnProcessPlayer += Players.ProcessMessagesClient;
			Players.OnRegistrationResult += RegistrationResultRequest;
			Players.OnLoginResult += LoginResultRequest;
		}

		private void LoginResultRequest(ResultOperation result)
		{
			OnLoginResult?.Invoke(result);
		}

		private void RegistrationResultRequest(ResultOperation result)
		{
			OnRegistrationResult?.Invoke(result);
		}

		public void SendMessage(TCPOperations opCode, EventBase msg)
		{
			TCPClientModel.SendMessage(opCode, msg);
		}

		public void ConnectAsync(Action<bool> connectionResult, string server, int serverPort)
		{
			_threadConnect = new Thread(() =>
			{
				try {
					IsAttemptConnection = true;
					StateClient.ConnectionState = false;
					TCPClientModel.Connect(server, serverPort);
					StateClient.ConnectionState = true;
					connectionResult?.Invoke(true);
				}
				catch (Exception ex) {
					if (ex is NoConnectionException)
						connectionResult?.Invoke(false);
					// но если поток прервали то никаких сообщений
				}
			}
			);
			_threadConnect.Start();
		}

		/// <summary>
		/// Отменить попытку соединения с сервером (вызовется ConnectionResult(false))
		/// </summary>
		public void ConnectionCancel()
		{
			_threadConnect.Abort();
		}
	}
}