using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.TCPNet;
using Engine.EventSystem.Event;
using Engine.Data;
using System.Collections.Generic;
using Engine.Utils;

namespace UnitTests
{
	[TestClass]
	public class TestClass01
	{
		/// <summary>
		/// Простой тест с использованием сериализации/десериазизации
		/// </summary>
		[TestCategory("Base|1")]
		[TestMethod]
		[Description("Простой тест с использованием сериализации/десериализации")]
		public void BytesSerializeTest()
		{
			// тестируем возможности сериализации
			var conn = new TCPEngineConnector();
			var m = new MessageString();
			m.Message = "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789";
			m.num = 3457;
			m.dt = DateTime.Now;
			ushort classId = 99;

			var bts = conn.ConvertToBytes(0, classId, m);
			Console.WriteLine(bts.ToString());
			Console.WriteLine(bts.Length);

			MessageString mr = null;
			conn.ConvertFromBytes(out mr, classId, bts);

			Assert.AreEqual(m.Message, mr.Message);

		}

		[TestCategory("Base|2")]
		[TestMethod]
		[Description("Тест сериализации с коллектором")]
		public void BytesSerializeTestWithCollector()
		{
			var classesList = new List<CollectClass>();
			classesList.Add(new CollectClass(10, "engine.dll", "MessageString"));
			classesList.Add(new CollectClass(11, "engine.dll", "EventBus"));
			classesList.Add(new CollectClass(18, "engine.dll", "Player"));

			var collector = new Collector();
			collector.LoadClasses(classesList);

			var mobj = collector.GetObject(10);

			Assert.AreEqual(mobj.GetType().Name, "MessageString");

			// тестируем возможности сериализации
			var conn = new TCPEngineConnector();
			var m = new MessageString();
			m.Message = "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789";
			m.num = 3457;
			m.dt = DateTime.Now;
			ushort classId = 10;

			var bts = conn.ConvertToBytes(0, classId, m);
			Console.WriteLine(bts.ToString());
			Console.WriteLine(bts.Length);

			MessageString mr = null;
			conn.ConvertFromBytes(out mr, classId, bts);

			Assert.AreEqual(m.Message, mr.Message);

		}

	}
}
