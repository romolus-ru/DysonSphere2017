using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.TCPNet;
using System.Threading.Tasks;
using Engine.Enums;
using Engine.Data;
using Engine.Helpers;
using Engine.DataPlus;

namespace Engine.Models
{
	public class ModelPlayersServer:ModelPlayers
	{
		private const string RegistrationFailed = "Registration Failed";        
		/// <summary>
		/// Обработка операций
		/// </summary>
		protected Dictionary<TCPOperations, Action<ModelPlayerServer, TCPMessage>> Operate;

		private Dictionary<int, ModelPlayerServer> _players = new Dictionary<int, ModelPlayerServer>();
		public Func<LoginData, UserRegistration> OnLogin;

		public ModelPlayersServer(DataSupportBase db) : base(db)
		{
			Operate = new Dictionary<TCPOperations, Action<ModelPlayerServer, TCPMessage>>();
			Operate[TCPOperations.Registration] = RegistrationUser;
			Operate[TCPOperations.Login] = LoginUser;
		}

		public void CreatePlayer(TCPEngineConnector playerConnection)
		{
			var playerId = playerConnection.playerId;
			var player = new ModelPlayerServer(null, null, playerConnection);
			_players.Add(playerId, player);
		}

		/// <summary>
		/// Обработать присланные сообщения (сервер)
		/// </summary>
		/// <param name="playersIds"></param>
		public void ProcessMessagesServer(HashSet<int> playersIds)
		{
			foreach (var playerId in playersIds) {
				if (!_players.ContainsKey(playerId)) continue;
				ProcessMessages(_players[playerId]);
			}
		}

		private void ProcessMessages(ModelPlayerServer modelPlayer)
		{
			var messages = modelPlayer.GetMessages();
			foreach (var msg in messages) {
				var opcode = msg.opCode;
				if (Operate.ContainsKey(opcode))
					Operate[opcode](modelPlayer, msg);
			}
		}

		private void RegistrationUser(ModelPlayerServer player, TCPMessage message)
		{
			var userRegistration = message._msg as UserRegistration;
			if (userRegistration == null) {
				//LogErrors?.Invoke(this, ErrorType.RegistrationUserIsNull, null);
				// Клиента не оповещаем, это что то внутреннее
				return;
			}
			var reg = player.PrepareUserRegistrationInfo(userRegistration);
			var guid = userRegistration.UserGUID;
			// проверяем есть ли в базе такой и сохраняем его там
			var res = DB.RegisterUser(reg);
			var result = new ResultOperation() { OperationType = TCPOperations.Registration, Result = res, Message = null };
			if (res != ErrorType.NoError) {// если произошла ошибка то пишем клиенту об ошибке и логируем её
				result.Message = RegistrationFailed;
				//LogErrors?.Invoke(this, res, RegistrationFailed);
			}
			player.SendMSG(TCPOperations.Registration, result);
		}

		private void LoginUser(ModelPlayerServer player, TCPMessage message)
		{
			var login = message._msg as LoginData;
			var result = new ResultOperation() { OperationType = TCPOperations.Login, Result = ErrorType.LoginFailed };
			if (login == null) {
				//LogErrors?.Invoke(this, ErrorType.LoginFailed, null);
				player.SendMSG(TCPOperations.Login, result);
				return;
			}
			player.PrepareLoginData(login);
			var ur = DB.LoginUser(login);
			if (ur.UserRole == Role.Intruder) {
				//LogErrors?.Invoke(this, ErrorType.LoginFailed, "intruder");
				return;
			}
			player.AceptLoginData(ur);
			result.Result = ErrorType.NoError;
			player.SendMSG(TCPOperations.Login, result);
			//LogErrors?.Invoke(this, ErrorType.NoError, "login");
		}
	}
}
