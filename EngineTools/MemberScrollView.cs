using Engine;
using Engine.EventSystem.Event;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngineTools
{
	/// <summary>
	/// Основа для редакторов свойств класса
	/// </summary>
	class MemberScrollView<T> : ScrollItem where T : EventBase
	{
		private ViewInput _filter;
		private MemberInfo _memberInfo;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			_filter = new ViewInput();
			AddComponent(_filter);
			_filter.SetParams(20, 30, 500, 40, "value");
			_filter.InputAction("");
		}

		/// <summary>
		/// Инициализация объекта для редактирования
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="memberInfo"></param>
		public virtual void InitValueEditor(T obj, MemberInfo memberInfo)
		{
			_memberInfo = memberInfo;
			var _value = (_memberInfo as PropertyInfo).GetValue(obj);
			if (_value != null)
				_filter.InputAction(_value.ToString());
		}

		/// <summary>
		/// Установить значение поля объекта
		/// </summary>
		/// <param name="obj"></param>
		public virtual void SetValue(T obj) { }

		public override void DrawObject(VisualizationProvider vp)
		{
			vp.SetColor(
				CursorOver
				? Color.YellowGreen
				: Color.Red);
			vp.Rectangle(X, Y, Width, Height);

			vp.SetColor(Color.White);
			vp.Print(X + 10, Y + 10, Name);
		}
	}
}