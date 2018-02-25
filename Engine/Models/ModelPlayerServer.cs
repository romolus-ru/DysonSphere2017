using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.TCPNet;
using Engine.Data;
using Engine.DataPlus;
using Engine.Enums;
using Engine.Helpers;
using Engine.EventSystem.Event;

namespace Engine.Models
{
	/// <summary>
	/// Серверная модель игрока
	/// </summary>
	public class ModelPlayerServer : ModelPlayer
	{		
		/// <summary>
		/// Авторизован ли игрок
		/// </summary>
		public bool Authorized { get; private set; }

		public ModelPlayerServer(string playerGUID, string nickName, TCPEngineConnector connection) : base(playerGUID, nickName, connection)
		{
		}

		public void PrepareLoginData(LoginData loginData)
		{
			loginData.HSPassword = RecalcPass(loginData.HSPassword, loginData.UserGUID);
		}

		public void AcceptLoginData(UserRegistration ur)
		{
			this.UserRole = ur.UserRole;
			this.PlayerGUID = ur.UserGUID;
			this.NickName = ur.NickName;// по идее ник всё равно нужен. но если не нужен будет
										//  - то достаточно из базы получить роль - остальная информация есть
			this.Authorized = true;
		}
		public UserRegistration PrepareUserRegistrationInfo(UserRegistration incoming)
		{
			var pass1 = incoming.HSPassword;
			var guid = incoming.UserGUID;

			var reg = new UserRegistration();
			reg.Mail = incoming.Mail;
			reg.NickName = incoming.NickName;
			reg.OfficialName = incoming.OfficialName;
			reg.UserGUID = guid;
			reg.HSPassword = RecalcPass(pass1, guid);
			reg.UserRole = Role.Player;
			return reg;
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

		public void SendMSG(TCPOperations operation, EventBase message)
		{
			_connection.SendMSGData(operation, message);
		}
	}
}