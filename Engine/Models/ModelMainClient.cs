using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Utils;

namespace Engine
{
	public class ModelMainClient : ModelMain
	{
		public TCPClientModel TCPClientModel { get; private set; }
		public ModelMainClient(Collector collector)
		{
			TCPClientModel = new TCPClientModel(collector);
			TCPClientModel.Init();
			AddModel(TCPClientModel);
		}
	}
}
