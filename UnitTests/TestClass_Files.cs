using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Utils;

namespace UnitTests
{
	[TestClass]
	public class TestClass_Files
	{
		[TestMethod]
		public void TestFileArchive()
		{
			var dataMain = "{}";
			var a = "arch.topsec";
			var f = "data.1";
			var data = dataMain;
			FileUtils.SaveString(a, f, data);

			f = "data.2";
			data = "1vrevb4n4n4en5";
			FileUtils.SaveString(a, f, data);

			f = "data.1";
			FileUtils.LoadString(a, f, out data);
			Assert.AreEqual(dataMain, data);
		}
	}
}
