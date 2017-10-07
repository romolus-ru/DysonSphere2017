using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace UnitTests
{
	[TestClass]
	public class TestClass02
	{
		/// <summary>
		/// Простой тест с использованием сериализации/десериазизации
		/// </summary>
		[TestCategory("DB|1")]
		[TestMethod]
		[Description("Соединяемся с БД и получаем данные из таблицы")]
		public void DBConnectTest()
		{
			var db = new DataSupportEF.DataSupportEF();
			var a = db.AtlasFilesGetAll();
			Debug.WriteLine(a.Count+" count rows in table");
		}
	}
}
