using Engine.Data;
using Engine.Helpers;
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
		private List<UserRegistration> _players = new List<UserRegistration>();
		private DataSupportBase _db;
		/// <summary>
		/// Соль для паролей. а название в честь фильма где ангелина джоли играла роль тома круза
		/// </summary>
		private string _solt;
		public ModelPlayers(DataSupportBase db, string solt) : base()
		{
			_db = db;
		}

		public override void Tick()
		{
			base.Tick();
		}

		public void RegisterUser(int playerTCPId, UserRegistration user)
		{
			var pass1 = user.HSPassword;
			pass1 = CryptoHelper.CalculateHash(pass1 + _solt + user.UserGUID);
			user.HSPassword = pass1;
			user.UserRole = Enums.Role.Player;
			// проверяем есть ли в базе такой и сохраняем его там

		}
	}
}
