using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Data
{
	/// <summary>
	/// Значения ачивки для игры и для пользователя
	/// </summary>
	public class AchieveValues
	{
		public long Id { get; set; }
		public long MiniGameId { get; set; }
		public long AchieveId { get; set; }
		public long PlayerId { get; set; }
		public bool Achieved { get; set; }
		public float CurrentValue { get; set; }

	}
}