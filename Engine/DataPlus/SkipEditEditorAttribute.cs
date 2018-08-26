using System;

namespace Engine.DataPlus
{
	/// <summary>
	/// Не редактировать это поле
	/// </summary>
	[AttributeUsage(AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Property)]
	public class SkipEditEditorAttribute : Attribute
	{
	}
}