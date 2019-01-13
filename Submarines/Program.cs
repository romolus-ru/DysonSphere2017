using Engine.Utils;
using Engine;

namespace Submarines
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