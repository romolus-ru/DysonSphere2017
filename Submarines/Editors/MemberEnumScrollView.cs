using System;
using System.Drawing;
using System.Reflection;
using Engine;
using Engine.Visualization;
using System.Windows.Forms;

namespace Submarines.Editors
{
	internal class MemberEnumScrollView<T> : MemberBaseScrollView<T> where T : class
	{
		private MemberInfo _memberInfo;
		private Enum _value;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			var nextEnumValue = new ViewButton();
			AddComponent(nextEnumValue);
			nextEnumValue.InitButton(NextEnumValue, "+", "Поменять значение", Keys.Right);
			nextEnumValue.SetParams(280, 10, 30, 25, "+");
			nextEnumValue.InitTexture("textRB", "textRB");

			var prevEnumValue = new ViewButton();
			AddComponent(prevEnumValue);
			prevEnumValue.InitButton(PrevEnumValue, "-", "Поменять значение", Keys.Left);
			prevEnumValue.SetParams(240, 10, 30, 25, "-");
			prevEnumValue.InitTexture("textRB", "textRB");
		}

		private void NextEnumValue()
		{
			var arr = Enum.GetValues(_value.GetType());
			int j = Array.IndexOf(arr, _value) + 1;
			j = j >= arr.Length ? 0 : j;
			_value = (Enum)arr.GetValue(j);
		}

		private void PrevEnumValue()
		{
			var arr = Enum.GetValues(_value.GetType());
			int j = Array.IndexOf(arr, _value) - 1;
			j = j < 0 ? arr.Length - 1 : j;
			_value = (Enum)arr.GetValue(j);
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
			_value = (Enum) value;
		}

		/// <summary>
		/// Установить значение поля объекта
		/// </summary>
		/// <param name="obj"></param>
		public override void SetValue(T obj)
		{
			//string str = _inputView.Text;
			PropertyInfo pi = _memberInfo as PropertyInfo;
			//object value = _getValue == null
			//	? Convert.ChangeType(str, pi.PropertyType)
			//	: _getValue(str);

			pi.SetValue(obj, _value);
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
			vp.Print(X + 330, Y + 10, _value.ToString());
		}
	}
}