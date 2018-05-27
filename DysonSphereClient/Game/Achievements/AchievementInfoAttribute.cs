using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game.Achievements
{
	/// <summary>
	/// Атрибут добавления ачивки 
	/// </summary>
	[AttributeUsage(AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Property|AttributeTargets.Method)]
	public class AchievementInfoAttribute : Attribute
	{
		/// <summary>
		/// Имя ачивки которая будет получать событие
		/// </summary>
		public string Name { get; set; }
	}
}