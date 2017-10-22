using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphere
{
	class ServerView : View
	{
		private Func<TimeSpan> GetTime;
		public ServerView(VisualizationProvider provider) : base(provider)
		{
		}

		public void SetTimerInfo(Func<TimeSpan> timeInfo)
		{
			GetTime += timeInfo;
		}

		protected override void DrawObject(VisualizationProvider provider)
		{
			var ts = GetTime();
			provider.Print(100, 100, ts.Ticks.ToString());
		}
	}
}
