using DataSupportEF;
using Engine;
using Engine.Utils;

namespace EngineTools
{
	class Program
	{
		static void Main()
		{
			var ls = new LogSystem();
			StateEngine.Log = ls;
			var DBContext = new DataSupportEF6();
			DBContext.InitLogSystem(ls);

			var tools = new Tools(DBContext, ls);
			tools.Run();
		}
	}
}
