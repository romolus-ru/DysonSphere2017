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

namespace Engine
{
	public class ModelMainClient : ModelMain
	{
		protected TCPClientModel TCPClientModel { get; private set; }
		protected ModelPlayersClient Players { get; private set; }
		protected ModelPlayerClient Player { get; private set; }
		public Action<ResultOperation> OnRegistrationResult;
		public Action<ResultOperation> OnLoginResult;

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

		public void Connect(string server, int serverPort)
		{
			TCPClientModel.Connect(server, serverPort);
		}

		public void SendMessage(TCPOperations opCode, EventBase msg)
		{
			TCPClientModel.SendMessage(opCode, msg);
		}
	}
}
