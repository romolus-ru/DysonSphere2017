using Engine.Data;
using Engine.DataPlus;
using Engine.Enums;
using Engine.Enums.Client;
using Engine.Helpers;
using Engine.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
	/// <summary>
	/// Состояния клиента
	/// </summary>
	public class StateClient
	{
		/// <summary>
		/// Состояние соединения с сервером
		/// </summary>
		public static bool ConnectionState = false;
		/// <summary>
		/// Сообщение от системы соединения с сервером
		/// </summary>
		public static string ConnectionMessage = null;

		/// <summary>
		/// Состояние регистрации пользователя
		/// </summary>
		public static RegistrationState RegistrationState = RegistrationState.NotRegistered;
		/// <summary>
		/// Сообщение от сервера в случае какой-либо ошибки
		/// </summary>
		public static string RegistrationMessage = null;
		/// <summary>
		/// Состояние логина
		/// </summary>
		public static LoginState LoginState = LoginState.NotLogedIn;
		/// <summary>
		/// Сообщение от сервера в случае какой-либо ошибки
		/// </summary>
		public static string LoginMessage = null;
		/// <summary>
		/// Для сохранения вспомогательных данных
		/// </summary>
		public static Dictionary<string, string> values = new Dictionary<string, string>();

		private StateClient() { }
		/// <summary>
		/// для сохранения в json
		/// </summary>
		public RegistrationState _RegistrationState { get { return RegistrationState; } set { RegistrationState = value; } }

		public static void ChangeState(ResultOperation result)
		{
			if (result.OperationType == TCPOperations.Registration
				&& result.Result == ErrorType.NoError) RegistrationState = RegistrationState.Registered;
		}

		/// <summary>
		/// Загружаем данные из файла
		/// </summary>
		public static void InitState()
		{
			string data;
			FileUtils.LoadString(DataSupportFileHelper.StateClientFile, DataSupportFileHelper.StateClientData, out data);
			StateClient value = null;
			if (!string.IsNullOrEmpty(data)) {
				value = JsonConvert.DeserializeObject<StateClient>(data);
			}
		}

		/// <summary>
		/// Сохраняем данные в файл
		/// </summary>
		public static void SaveState()
		{
			var data = JsonConvert.SerializeObject(new StateClient());
			FileUtils.SaveString(DataSupportFileHelper.StateClientFile, DataSupportFileHelper.StateClientData, data);
		}
	}
}