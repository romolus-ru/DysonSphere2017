using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Helpers;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace UnitTests
{
	[TestClass]
	public class TestClass_Crypt
	{
		[TestMethod]
		public void TestCryptBase()
		{
			foreach (var item in Enumerable.Range(1, 30)) {
				var s = CryptoHelper.Generate(6);
				Debug.WriteLine(s);
				Thread.Sleep(200);
			}
		}
	}
}
