using Engine.Visualization.Scroll;
using System.Reflection;

namespace Submarines.Editors
{
	public class MemberBaseScrollView<T> : ScrollItem where T : class
	{
		/// <summary>
		/// Инициализация объекта для редактирования
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="memberInfo"></param>
		public virtual void InitValueEditor(T obj, MemberInfo memberInfo) { }

		/// <summary>
		/// Установить значение поля объекта
		/// </summary>
		/// <param name="obj"></param>
		public virtual void SetValue(T obj) { }
	}
}