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
	/// <remarks>Что бы информация об игроках была уницифирована дляклиента и сервера</remarks>
	public class ModelPlayer : Model
	{
		public int PlayerTCPId { get; private set; }
		public string PlayerGUID { get; private set; }
		public string NickName { get; private set; }

		public ModelPlayer(int playerTCPId, string playerGUID, string nickName)
		{
			PlayerTCPId = playerTCPId;
			PlayerGUID = playerGUID;
			NickName = nickName;
		}
	}
}
