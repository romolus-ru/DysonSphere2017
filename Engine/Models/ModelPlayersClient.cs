using Engine.DataPlus;
using Engine.Enums;
using Engine.TCPNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
	public class ModelPlayersClient : ModelPlayers
	{
		private ModelPlayerClient _player = null;
		public Action<ResultOperation> OnRegistrationResult;
		public Action<ResultOperation> OnLoginResult;
		/// <summary>
		/// Обработка операций
		/// </summary>
		protected Dictionary<TCPOperations, Action<ModelPlayerClient, TCPMessage>> Operate;
		public ModelPlayersClient(ModelPlayerClient player, DataSupportBase db) : base(db)
		{
			_player = player;
			Operate = new Dictionary<TCPOperations, Action<ModelPlayerClient, TCPMessage>>();
			Operate[TCPOperations.Registration] = RegistrationResultRequest;
			Operate[TCPOperations.Login] = LoginResultRequest;
		}

		/// <summary>
		/// Обработать присланные сообщения (клиент)
		/// </summary>
		public void ProcessMessagesClient(List<TCPMessage> messages)
		{
			if (messages == null) return;
			foreach (var msg in messages) {
				var opcode = msg.opCode;
				if (Operate.ContainsKey(opcode))
					Operate[opcode](_player, msg);
			}
		}

		private void RegistrationResultRequest(ModelPlayerClient player, TCPMessage msg)
		{
			OnRegistrationResult?.Invoke(msg._msg as ResultOperation);
		}

		private void LoginResultRequest(ModelPlayerClient player, TCPMessage msg)
		{
			OnLoginResult?.Invoke(msg._msg as ResultOperation);
		}

	}
}