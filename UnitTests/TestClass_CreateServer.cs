using Engine;
using Engine.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataSupportEF;
using DysonSphere;

namespace UnitTests
{
	[TestClass]
	public class TestClass_CreateServer
	{
		/// <summary>
		/// Создаём сервер
		/// </summary>
		[TestCategory("Server|1")]
		[TestMethod]
		[Description("Соединяемся с БД и запускаем сервер чтоб он брал настройки оттуда. и смотрим с логах сигнал о завершении инициализации")]
		public void CreateServer()
		{
			var ls = new LogSystem();
			var DBContext = new DataSupportEF6();
			DBContext.InitLogSystem(ls);

			var server = new Server(DBContext, ls);
			var a = 1;
			Assert.Fail("получить из базы данные и подумать как они туда должны попасть. скорее всего дополнительное приложение для редактирования настроек");
		}
	}
}
