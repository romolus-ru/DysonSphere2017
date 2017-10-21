using DataSupportEF;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphere
{
    class Program
    {
        static void Main(string[] args)
        {
			var ls = new LogSystem();
			var DBContext = new DataSupportEF6();
			DBContext.InitLogSystem(ls);

			var server = new Server(DBContext, ls);
			server.Run();

		}
	}
}
