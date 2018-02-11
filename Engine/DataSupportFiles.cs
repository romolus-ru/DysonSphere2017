using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Data;
using Engine.Utils;
using Newtonsoft.Json;
using Engine.Enums;
using Engine.Helpers;

namespace Engine
{
	/// <summary>
	/// Реализация работы с данными на основе файлов
	/// </summary>
	public class DataSupportFiles : DataSupportBase
	{
		public override UserRegistration UserStatus {
			get {
				string data;
				FileUtils.LoadString(DataSupportFileHelper.UserRegistrationFile, DataSupportFileHelper.UserRegistrationData, out data);
				UserRegistration value = null;
				if (string.IsNullOrEmpty(data)) {
					// создаём запись по умолчанию и заполняем её
					value = new UserRegistration();
					value.UserRole = Role.Player;
					value.UserGUID = CryptoHelper.Generate();
				} else {
					// десериализуем данные
					value = JsonConvert.DeserializeObject<UserRegistration>(data);
				}
				return value;
			}

			set {
				var data = JsonConvert.SerializeObject(value);
				FileUtils.SaveString(DataSupportFileHelper.UserRegistrationFile, DataSupportFileHelper.UserRegistrationData, data);
			}
		}

		public override List<CollectClass> GetCollectClasses()
		{
			string data;
			FileUtils.LoadString(DataSupportFileHelper.CollectClassesFile, DataSupportFileHelper.CollectClassesData, out data);
			List<CollectClass> value = null;
			if (string.IsNullOrEmpty(data)) {
				throw new NullReferenceException("нету информации о классах " +
					DataSupportFileHelper.CollectClassesFile + " " +
					DataSupportFileHelper.CollectClassesData);
			} else {
				// десериализуем данные
				value = JsonConvert.DeserializeObject<List<CollectClass>>(data);
			}
			return value;
		}

		private Dictionary<string, int> _settings = null;

		private void ServerSettingsGetAll()
		{
			string data;
			FileUtils.LoadString(DataSupportFileHelper.SettingsFile, DataSupportFileHelper.SettingsData, out data);
			_settings = null;
			if (string.IsNullOrEmpty(data)) {
				throw new NullReferenceException("нету информации о настройках " +
					DataSupportFileHelper.SettingsFile + " " +
					DataSupportFileHelper.SettingsData);
			} else {
				// десериализуем данные
				_settings = JsonConvert.DeserializeObject<Dictionary<string, int>>(data);
			}
		}

		public override int ServerSettingsGetValue(string valueName)
		{
			if (_settings == null) {
				ServerSettingsGetAll();
			}
			if (!_settings.ContainsKey(valueName)) {
				Log("значение " + valueName + " не обнаружено в настройках");
				return -1;
			}
			return _settings[valueName];
		}
	}
}