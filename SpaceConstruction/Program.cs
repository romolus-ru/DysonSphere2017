﻿using Engine;
using Engine.Utils;

namespace SpaceConstruction
{
	static class Program
	{

		static void Main()
		{
			var ls = new LogSystem();
			var dbContext = new DataSupportFiles();
			dbContext.InitLogSystem(ls);

			var client = new Client(dbContext, ls);
			client.Run();
		}
	}
}