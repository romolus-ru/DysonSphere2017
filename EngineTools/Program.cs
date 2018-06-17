using DataSupportEF;
using Engine.Utils;

namespace EngineTools
{
	class Program
	{
		static void Main()
		{
			var ls = new LogSystem();
			var DBContext = new DataSupportEF6();
			DBContext.InitLogSystem(ls);

			var tools = new Tools(DBContext, ls);
			tools.Run();
		}
	}
}
