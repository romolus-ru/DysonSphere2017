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
	public class ModelClient : ModelMain
	{
		private List<Model> _models = new List<Model>();
		public TCPClientModel TCPClient { get; private set; }
		public ModelClient(Collector collector)
		{
			TCPServerModel TCPServer = new TCPServerModel(collector);
			TCPServer.Init();
			AddModel(TCPServer);
		}
	}
}
