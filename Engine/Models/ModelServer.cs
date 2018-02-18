﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Utils;

namespace Engine
{
	public class ModelServer : ModelMain
	{
		public TCPServerModel TCPServerModel { get; private set; }
		public ModelServer(Collector collector)
		{
			TCPServerModel = new TCPServerModel(collector);
			TCPServerModel.Init();
			AddModel(TCPServerModel);
		}
	}
}
