using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Helpers
{
	public static class DataSupportFileHelper
	{
		/// <summary>
		/// Расширение дляфайлов с данными
		/// </summary>
		public const string DataFileExtension = ".data";
		/// <summary>
		/// Каталог где лежат данные
		/// </summary>
		public const string DataFileDirectory = "data/";
		/// <summary>
		/// имя файла в архиве с данными
		/// </summary>
		public const string MainData = "main";

		public const string CollectClassesFile = DataFileDirectory + "CollectClasses" + DataFileExtension;
		public const string CollectClassesData = MainData;
		public const string SettingsFile = DataFileDirectory + "Settings" + DataFileExtension;
		public const string SettingsData = MainData;
		public const string UserRegistrationFile = DataFileDirectory + "UserRegistration" + DataFileExtension;
		public const string UserRegistrationData = MainData;

	}
}
