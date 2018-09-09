using System;

namespace Engine.DataPlus
{
	/// <summary>
	/// Атрибут определения редактора для работы с идентификатором от CollectorClassId
	/// </summary>
	/// <remarks>Определяет тип основного класса для которого будет вызван редактор MemberCollectorClassScrollViewItem</remarks>
	[AttributeUsage(AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
	public class MemberCollectorClassEditorAttribute : Attribute
	{
		/// <summary>
		/// Тип основного класса для редактора
		/// </summary>
		public Type Type { get; set; }
	}
}