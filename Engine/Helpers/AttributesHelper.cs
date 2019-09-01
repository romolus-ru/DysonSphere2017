using Engine.DataPlus;
using System;
using System.Reflection;

namespace Engine.Helpers
{
	/// <summary>
	/// вспомогательный класс для получения информации об атрибутах имеющихся у класса/поля и т.п.
	/// </summary>
	public static class AttributesHelper
	{
		public static bool IsHasAttribute<T>(MemberInfo prop) where T : Attribute
			=> prop.GetCustomAttribute<T>() != null;

		public static T GetAttribute<T>(PropertyInfo prop) where T : Attribute
			=> prop.GetCustomAttribute<T>();

		public static Type GetMemberCollectorClassEditorType(PropertyInfo prop)
		{
			var attr = prop.GetCustomAttribute<MemberCollectorClassEditorAttribute>();
			if (attr == null) return null;
			return attr.Type;
		}
	}
}