using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Utils;
using Engine.Enums;
using Engine.TCPNet;

namespace Engine
{
	public class ModelServer : ModelMain
	{
		public TCPServerModel TCPServer { get; private set; }
		public ModelServer(Collector collector)
		{
			TCPServer = new TCPServerModel(collector);
			TCPServer.Init();
			AddModel(TCPServer);
		}
	}
}
