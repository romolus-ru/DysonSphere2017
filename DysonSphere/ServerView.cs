using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphere
{
	class ServerView : ViewComponent
	{
		private Stopwatch _serverSW;
		public ServerView() : base()
		{
		}

		public void SetTimerInfo(Stopwatch timeInfo)
		{
			_serverSW = timeInfo;
		}

		protected override void DrawComponents(VisualizationProvider provider)
		{
			var ts = _serverSW.Elapsed;
			provider.Print(100, 100, ts.Ticks.ToString());
		}
	}
}
