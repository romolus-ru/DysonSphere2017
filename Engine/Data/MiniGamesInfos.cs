using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Data
{
	public class MiniGamesInfos: EventBase
	{
		public long IdMiniGamesInfos { get; set; }
		public long MiniGameId { get; set; }
		public string Section { get; set; }
		public string DataType { get; set; }
		public string Values { get; set; }

	}
}