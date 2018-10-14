using Engine;
using Engine.Utils;

namespace SpaceConstruction
{
	static class Program
	{

		static void Main()
		{
			var ls = new LogSystem();
			var DBContext = new DataSupportFiles();
			DBContext.InitLogSystem(ls);

			var client = new Client(DBContext, ls);
			client.Run();
		}
	}
}
