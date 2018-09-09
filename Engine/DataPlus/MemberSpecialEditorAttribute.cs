using System;

namespace Engine.DataPlus
{
	/// <summary>
	/// Атрибут определения специального редактора для члена класса
	/// </summary>
	/// <remarks>Определяет тип ScrollItem который будет создан что бы редактировать объект</remarks>
	[AttributeUsage(AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
	public class MemberSpecialEditorAttribute : Attribute
	{
		/// <summary>
		/// Определение типа редактора в виде строки
		/// </summary>
		public string EditorType { get; set; }
	}
}