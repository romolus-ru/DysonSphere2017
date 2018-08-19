using System;

namespace Engine.DataPlus
{
	/// <summary>
	/// Атрибут определения редактора для члена класса
	/// </summary>
	[AttributeUsage(AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
	public class MemberEditorAttribute:Attribute
	{
		/// <summary>
		/// Тип основного класса для редактора
		/// </summary>
		public Type Type { get; set; }
	}
}