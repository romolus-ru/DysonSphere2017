namespace DysonSphereClient.Game.ResourcesNew
{
	/// <summary>
	/// Количество требуемых материалов из группы материалов (требуемый объем)
	/// </summary>
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