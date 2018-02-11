using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.DataPlus
{
	public class LoginData:EventBase
	{
		public string UserGUID;
		public string HSPassword;
	}
}
