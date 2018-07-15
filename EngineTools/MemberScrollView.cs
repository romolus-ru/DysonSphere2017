using Engine;
using Engine.EventSystem.Event;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using Engine.Visualization.Text;
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
	/// <remarks>
	/// Выводит обязательно InputView
	/// по умолчанию InputView заблокирован
	/// Требует внешнее управление для установки фильтра и для получения значения переменной из строки
	/// </remarks>
	public class MemberScrollView<T> : ScrollItem where T : EventBase
	{
		private ViewInput inputView;
		private MemberInfo _memberInfo;
		private bool _selected = false;
		private Func<string, object> _getValue;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			inputView = new ViewInput();
			AddComponent(inputView);
			inputView.SetParams(240, 10, 500, 20, "value");
			inputView.InputAction("");
			inputView.Enabled = false;
		}

		/// <summary>
		/// Настроить Filter для работы с конкретным типом данных
		/// </summary>
		public virtual void SetupMemberEditor(Func<string, object> getValue, Func<string, string> filter=null)
		{
			_getValue = getValue;
			inputView.Enabled = true;
			if (filter != null) {
				inputView.Filter = filter;
			}
		}

		public void SetInputFocus()
		{
			//_filter.IsFocused = true;
			_selected = false;
			//поле input необходимо фокусировать кликом
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
				inputView.InputAction(_value.ToString());
		}

		/// <summary>
		/// Установить значение поля объекта
		/// </summary>
		/// <param name="obj"></param>
		public virtual void SetValue(T obj)
		{
			if (_getValue == null) return;
			var str = inputView.Text;
			var value = _getValue(str);
			var pi = _memberInfo as PropertyInfo;
			pi.SetValue(obj, value);
		}

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