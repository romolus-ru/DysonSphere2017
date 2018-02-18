using Engine.Data;
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
	/// Данные об игроках
	/// </summary>
	public class ModelPlayers:Model
	{
		private Dictionary<int, ModelPlayer> _players = new Dictionary<int, ModelPlayer>();
		private DataSupportBase _db;

		public ModelPlayers(DataSupportBase db) : base()
		{
			_db = db;
		}

		public void CreatePlayer(TCPEngineConnector playerConnection)
		{
			var playerId = playerConnection.playerId;
			var player = new ModelPlayer(null, null, playerConnection);
			_players.Add(playerId, player);
			player.OnRegistrationUser += _db.RegisterUser;
			player.OnLogin += _db.LoginUser;
		}

		/// <summary>
		/// Обработать присланные сообщения (сервер)
		/// </summary>
		/// <param name="playersIds"></param>
		public void ProcessMessagesServer(HashSet<int> playersIds)
		{
			foreach (var playerId in playersIds) {
				if (!_players.ContainsKey(playerId)) continue;
				_players[playerId].ProcessMessages();
			}
		}

		// TODO Наверно всё таки разделить надо будет на клиента и сервера - а то вызовут добавление клиента
		private bool _playerAdded = false;
		/// <summary>
		/// Добавить клиента в список игроков. при запуске игры
		/// </summary>
		public void AddClient(ModelPlayer player)
		{
			if (_playerAdded) return;
			_playerAdded = true;
			_players.Add(0, player);
		}

		/// <summary>
		/// Обработать присланные сообщения (клиент)
		/// </summary>
		/// <remarks>Идентификатор у локального клиента должен быть 0</remarks>
		public void ProcessMessagesClient()
		{
			_players[0].ProcessMessages();
		}
	}
}
