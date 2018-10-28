using System.Diagnostics;

namespace SpaceConstruction.Game.Resources
{
	/// <summary>
	/// Количество требуемых материалов из группы материалов (требуемый объем)
	/// </summary>
	[DebuggerDisplay("{Value} {Group} {GetType()}")]
	public class ResourceGroupValue
	{
		public ResourcesGroupEnum Group;
		public int Value;
		public ResourceGroupValue(ResourcesGroupEnum group, int value)
		{
			Group = group;
			Value = value;
		}
	}
}