using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.TCPNet;

namespace Engine.Models
{
	/// <summary>
	/// Клиентская модель игрока
	/// </summary>
	public class ModelPlayerClient : ModelPlayer
	{
		public ModelPlayerClient(string playerGUID, string nickName, TCPEngineConnector connection) : base(playerGUID, nickName, connection)
		{
		}

		/// <summary>
		/// Залогирован ли на сервере игрок
		/// </summary>
		public bool LoggedIn { get; private set; }

	}
}