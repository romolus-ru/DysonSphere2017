using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphere
{
	class ServerView : View
	{
		private Stopwatch _serverSW;
		public ServerView(VisualizationProvider provider) : base(provider)
		{
		}

		public void SetTimerInfo(Stopwatch timeInfo)
		{
			_serverSW = timeInfo;
		}

		protected override void DrawObject(VisualizationProvider provider)
		{
			var ts = _serverSW.Elapsed;
			provider.Print(100, 100, ts.Ticks.ToString());
		}
	}
}
