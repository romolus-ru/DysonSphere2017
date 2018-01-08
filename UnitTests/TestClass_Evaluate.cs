using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicExpression = Engine.Utils.Query.DynamicExpression;
using System.Linq.Expressions;

namespace UnitTests
{
	[TestClass]
	public class TestClass_Evaluate
	{
		[TestMethod]
		public void TestEvaluationExpression()
		{
			var xExpression = Expression.Parameter(typeof(int), "x");
			var objExpession = Expression.Parameter(typeof(TestObj), "a");
			var parsedExpression =
				(Expression<Func<int,TestObj, int>>)DynamicExpression.ParseLambda(
				new[] { xExpression, objExpession }, null, "(x+1)*3+a.f1*a.GetRandom+a.f2");
			var func = parsedExpression.Compile();
			var a = new TestObj { f1 = 1, f2 = 6 };
			var x = 3;
			var result = func(x, a);
			var original = (x + 1) * 3 + a.f1 * a.GetRandom + a.f2;

			Assert.AreEqual(result, original);
		}

		public class TestObj
		{
			public int f1;
			public int f2;
			public int GetRandom { get { return 4; } }
		}

	}
}
