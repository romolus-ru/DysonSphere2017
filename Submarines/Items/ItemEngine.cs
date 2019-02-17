using System.Collections.Generic;
using Submarines.Utils;

namespace Submarines.Items
{
	/// <summary>
	/// Информация о двигателе
	/// </summary>
	internal class ItemEngine : ItemBase
	{
		public float EnginePower { get; private set; }
		public int EnginePercentMin { get; private set; }
		public int EnginePercentMax { get; private set; }
		public string EngineType { get; private set; }

		internal override void Init(Dictionary<string, string> values)
		{
			base.Init(values);
			EnginePower = values.GetString("EnginePower").ToFloat(0);
			EnginePercentMin = values.GetString("EnginePercentMin").ToInt(0);
			EnginePercentMax = values.GetString("EnginePercentMax").ToInt(0);
			EngineType = values.GetString("EngineType");
		}
	}
}