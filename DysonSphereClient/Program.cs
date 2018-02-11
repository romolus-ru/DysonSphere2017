using Engine;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient
{
	class Program
	{
		static void Main(string[] args)
		{
			var ls = new LogSystem();
			var DBContext = new DataSupportFiles();
			DBContext.InitLogSystem(ls);

			var client = new Client(DBContext, ls);
			client.Run();
		}
	}
}
