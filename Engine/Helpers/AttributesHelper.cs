using Engine.DataPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Helpers
{
	/// <summary>
	/// вспомогательный класс для получения информации об атрибутах имеющихся у класса/поля и т.п.
	/// </summary>
	public static class AttributesHelper
	{
		public static bool IsHasAttribute<T>(PropertyInfo prop) where T : Attribute
			=> prop.GetCustomAttribute<T>() != null;

		public static Type GetMemberEditorType(PropertyInfo prop)
		{
			var attr = prop.GetCustomAttribute<MemberEditorAttribute>();
			if (attr == null) return null;
			return attr.Type;
		}
	}
}