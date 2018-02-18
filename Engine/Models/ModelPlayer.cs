using Engine.Data;
using Engine.DataPlus;
using Engine.Enums;
using Engine.Helpers;
using Engine.TCPNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
	/// <summary>
	/// Модель игрока. содержит основные данные - код TCP, код игрока и его ник
	/// </summary>
	/// <remarks>Что бы информация об игроках была уницифирована для клиента и сервера</remarks>
	public class ModelPlayer : Model
	{
		private const string RegistrationFailed = "Registration Failed";
		public int PlayerTCPId {
			get {
				return _connection?.playerId ?? -1;
			}
		}
		/// <summary>
		/// Строковой уникальный идентификатор пользователя
		/// </summary>
		public string PlayerGUID { get; private set; }
		/// <summary>
		/// Ник игрока
		/// </summary>
		public string NickName { get; private set; }
		/// <summary>
		/// Авторизован ли игрок
		/// </summary>
		public bool Authorized { get; private set; }
		/// <summary>
		/// Роль игрока в системе, определяет набор разрешенных команд
		/// </summary>
		public Role UserRole { get; private set; }

		private TCPEngineConnector _connection = null;
		private Dictionary<TCPOperations, Action<TCPMessage>> _operate;

		/// <summary>
		/// Для логирования ошибок
		/// </summary>
		public Action<ModelPlayer, ErrorType, string> LogErrors;
		public Func<UserRegistration, ErrorType> OnRegistrationUser;
		public Func<LoginData, UserRegistration> OnLogin;

		public ModelPlayer(string playerGUID, string nickName, TCPEngineConnector connection)
		{
			PlayerGUID = playerGUID;
			NickName = nickName;
			_connection = connection;
			_operate = new Dictionary<TCPOperations, Action<TCPMessage>>();
			_operate[TCPOperations.Registration] = RegistrationUser;
			_operate[TCPOperations.Login] = LoginUser;
		}

		public void ProcessMessages()
		{
			var messages = GetMessages();
			foreach (var message in messages) {
				ProcessMessage(message);
			}
		}

		/// <summary>
		/// Получить список сетевых сообщений
		/// </summary>
		private List<TCPMessage> GetMessages()
		{
			List<TCPMessage> messages = null;
			lock (_connection.Messages) {
				if (_connection.Messages.Count == 0) return null;
				messages = new List<TCPMessage>(_connection.Messages);
				_connection.Messages.Clear();
			}
			return messages;
		}

		private void ProcessMessage(TCPMessage msg)
		{
			var opcode = msg.opCode;
			if (_operate.ContainsKey(opcode))
				_operate[opcode](msg);
		}

		private void LoginUser(TCPMessage message)
		{
			var login = message._msg as LoginData;
			var result = new ResultOperation() { OperationType = TCPOperations.Login, Result = ErrorType.LoginFailed };
			if (login == null) {
				LogErrors?.Invoke(this, ErrorType.LoginFailed, null);
				_connection.SendMSGData(TCPOperations.Login, result);
				return;
			}
			login.HSPassword = RecalcPass(login.HSPassword, login.UserGUID);
			var ur = OnLogin?.Invoke(login);
			if (ur.UserRole == Role.Intruder) {
				LogErrors?.Invoke(this, ErrorType.LoginFailed, "intruder");
				return;
			}
			this.UserRole = ur.UserRole;
			this.PlayerGUID = ur.UserGUID;
			this.NickName = ur.NickName;// по идее ник всё равно нужен. но если не нужен будет
										//  - то достаточно из базы получить роль - остальная информация есть
			this.Authorized = true;
			result.Result = ErrorType.NoError;
			_connection.SendMSGData(TCPOperations.Login, result);
			LogErrors?.Invoke(this, ErrorType.NoError, "login");
		}

		private void RegistrationUser(TCPMessage message)
		{
			var userRegistration = message._msg as UserRegistration;
			if (userRegistration == null) {
				LogErrors?.Invoke(this, ErrorType.RegistrationUserIsNull, null);
				// Клиента не оповещаем, это что то внутреннее
				return;
			}
			var pass1 = userRegistration.HSPassword;
			var guid = userRegistration.UserGUID;
			userRegistration.HSPassword = RecalcPass(pass1, guid);
			userRegistration.UserRole = Role.Player;
			// проверяем есть ли в базе такой и сохраняем его там
			var res = OnRegistrationUser.Invoke(userRegistration);
			var result = new ResultOperation() { OperationType = TCPOperations.Registration, Result = res, Message = null };
			if (res != ErrorType.NoError) {// если произошла ошибка то пишем клиенту об ошибке и логируем её
				result.Message = RegistrationFailed;
				LogErrors?.Invoke(this, res, RegistrationFailed);
			}
			_connection.SendMSGData(TCPOperations.Registration, result);
		}

		/// <summary>
		/// Преобразовываем пароль до неузнаваемости
		/// </summary>
		/// <param name="hspassword">Присланный уже неузнаваемый пароль</param>
		/// <returns></returns>
		private string RecalcPass(string hspassword, string guid)
		{
			var p = hspassword + Constants.Solt + "_" + guid;
			return CryptoHelper.CalculateHash(p);
		}

	}
}