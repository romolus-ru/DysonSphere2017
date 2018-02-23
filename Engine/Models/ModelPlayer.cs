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
		public string PlayerGUID { get; protected set; }
		/// <summary>
		/// Ник игрока
		/// </summary>
		public string NickName { get; protected set; }
		/// <summary>
		/// Роль игрока в системе, определяет набор разрешенных команд
		/// </summary>
		public Role UserRole { get; protected set; }

		protected TCPEngineConnector _connection = null;

		/// <summary>
		/// Для логирования ошибок
		/// </summary>
		public Action<ModelPlayer, ErrorType, string> LogErrors;

		public ModelPlayer(string playerGUID, string nickName, TCPEngineConnector connection)
		{
			PlayerGUID = playerGUID;
			NickName = nickName;
			_connection = connection;
		}

		/// <summary>
		/// Получить список сетевых сообщений
		/// </summary>
		public List<TCPMessage> GetMessages()
		{
			List<TCPMessage> messages = null;
			lock (_connection.Messages) {
				if (_connection.Messages.Count == 0) return null;
				messages = new List<TCPMessage>(_connection.Messages);
				_connection.Messages.Clear();
			}
			return messages;
		}

	}
}