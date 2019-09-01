using Engine;
using Engine.Visualization;
using System;
using System.Drawing;
using System.Reflection;

namespace Submarines.Editors
{
	/// <summary>
	/// Основа для редакторов свойств класса
	/// </summary>
	/// <remarks>
	/// Выводит обязательно InputView
	/// по умолчанию InputView заблокирован
	/// Требует внешнее управление для установки фильтра и для получения значения переменной из строки
	/// </remarks>
	public class MemberScrollView<T> : MemberBaseScrollView<T> where T : class
	{
		private ViewInput _inputView;
		private MemberInfo _memberInfo;
		private bool _selected = false;
		private Func<string, object> _getValue;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			_inputView = new ViewInput();
			AddComponent(_inputView);
			_inputView.SetParams(240, 10, 500, 20, "value");
			_inputView.InputAction("");
			_inputView.Enabled = false;
		}

		/// <summary>
		/// Настроить Filter для работы с конкретным типом данных
		/// </summary>
		public virtual void SetupMemberEditor(Func<string, object> getValue, Func<string, string> filter = null)
		{
			_getValue = getValue;
			_inputView.Enabled = true;
			if (filter != null) {
				_inputView.Filter = filter;
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
		public override void InitValueEditor(T obj, MemberInfo memberInfo)
		{
			_memberInfo = memberInfo;
            var value = (_memberInfo as PropertyInfo).GetValue(obj);
			if (value != null)
				_inputView.InputAction(value.ToString());
		}

        /// <summary>
        /// Установить значение поля объекта
        /// </summary>
        /// <param name="obj"></param>
        public override void SetValue(T obj) {
            if (!_inputView.Enabled)
                return;

            string str = _inputView.Text;

            PropertyInfo pi = _memberInfo as PropertyInfo;
            object value = _getValue == null
                ? Convert.ChangeType(str, pi.PropertyType)
                : _getValue(str);

            pi.SetValue(obj, value);
        }

		public override void DrawObject(VisualizationProvider vp)
		{
			vp.SetColor(
				CursorOver
				? Color.YellowGreen
				: Color.Red);
			vp.Rectangle(X, Y, Width, Height);
			if (Selected)
				vp.Rectangle(X + 1, Y + 1, Width - 2, Height - 2);

			vp.SetColor(Color.White);
			vp.Print(X + 10, Y + 10, Name);
		}
	}
}