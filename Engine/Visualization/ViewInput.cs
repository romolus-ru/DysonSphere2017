using Engine.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Engine.Visualization
{
	/// <summary>
	/// Ввод строкового значения
	/// </summary>
	public class ViewInput : ViewComponent
	{
		public string Text { get; protected set; } = "";
		/// <summary>
		/// Событие изменения вводимого текста
		/// </summary>
		public Action<string> OnTextChanged = null;
		/// <summary>
		/// Фильтруем символы
		/// </summary>
		public Func<string, string> Filter = null;
		private int _offsetTxtY = 0;
		private int _offsetCursorY = 0;
		private int _offsetCursorH = 0;
		private int _cursorPos = 0;
		private int _cursorPosX = 0;
		private bool _isFocused = false;
		public bool IsFocused {
			get { return _isFocused; }
			set {
				if (value == _isFocused) return;
				_isFocused = Enabled ? value : false;
				ChangeFocus(_isFocused);
			}
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			Input.AddKeyActionSticked(ViewInputActivate, Keys.LButton);
		}

		protected override void ClearObject()
		{
			IsFocused = false;
			Input.RemoveKeyActionSticked(ViewInputActivate, Keys.LButton);
			base.ClearObject();
		}

		private void ViewInputActivate()
		{
			IsFocused = false;
			if (!CursorOver) return;
			if (!Enabled) return;
			if (InRange(Input.CursorX, Input.CursorY))
				IsFocused = true;
		}

		private void ChangeFocus(bool isFocused)
		{
			if (isFocused) {
				Input.AddKeyActionPaused(CursorLeft, Keys.Left);
				Input.AddKeyActionPaused(CursorRight, Keys.Right);
				Input.AddKeyActionPaused(CursorBackDeleteChar, Keys.Back);
				Input.AddKeyActionPaused(CursorDeleteChar, Keys.Delete);
				Input.AddKeyActionPaused(CursorHome, Keys.Home);
				Input.AddKeyActionPaused(CursorEnd, Keys.End);
				Input.AddInputStringAction(InputAction);
			} else {
				Input.RemoveKeyActionPaused(CursorLeft, Keys.Left);
				Input.RemoveKeyActionPaused(CursorRight, Keys.Right);
				Input.RemoveKeyActionPaused(CursorBackDeleteChar, Keys.Back);
				Input.RemoveKeyActionPaused(CursorDeleteChar, Keys.Delete);
				Input.RemoveKeyActionPaused(CursorHome, Keys.Home);
				Input.RemoveKeyActionPaused(CursorEnd, Keys.End);
				Input.RemoveInputStringAction(InputAction);
			}
			RecalcOfffsets();
		}

		/// <summary>
		/// Пересчитываем смещения и т.п. 
		/// </summary>
		private void RecalcOfffsets()
		{
			_offsetCursorH = VisualizationProvider.FontHeight * 2;
			_offsetTxtY = (Height - _offsetCursorH) / 2;
			if (_offsetCursorH > Height) _offsetCursorH = Height;
			_offsetCursorY = (Height - _offsetCursorH) / 2;
		}

		private void CursorBackDeleteChar()
		{
			if (_cursorPos <= 0) return;
			_cursorPos--;
			CursorDeleteChar();
			RecalcCursorPosX();
		}

		private void CursorDeleteChar()
		{
			if (_cursorPos >= Text.Length) return;
			Text = Text.Remove(_cursorPos, 1);
			OnTextChanged?.Invoke(Text);
		}

		private void CursorHome()
		{
			_cursorPos = 0;
			RecalcCursorPosX();
		}

		private void CursorEnd()
		{
			_cursorPos = Text.Length;
			RecalcCursorPosX();
		}


		private void RecalcCursorPosX()
		{
			_cursorPosX = VisualizationProvider.TextLength(Text.Substring(0, _cursorPos));
		}
		private void CursorRight()
		{
			_cursorPos++;
			if (_cursorPos > Text.Length) _cursorPos = Text.Length;
			else RecalcCursorPosX();
		}

		private void CursorLeft()
		{
			_cursorPos--;
			if (_cursorPos < 0) _cursorPos = 0;
			else RecalcCursorPosX();
		}

		public void InputAction(string str)
		{
			if (Filter != null)
				str = Filter(str);
			Text = Text.Insert(_cursorPos, str);
			_cursorPos += str.Length;
			OnTextChanged?.Invoke(Text);
			RecalcCursorPosX();
		}

		private bool _cursorFlashState = false;
		private int _cursorFlashCounter = 0;
		private void ShowCursor(VisualizationProvider visualizationProvider)
		{
			if (!_isFocused) return;
			_cursorFlashCounter--;
			if (_cursorFlashCounter < 0) {
				_cursorFlashCounter = Constants.CursorFrequency;
				_cursorFlashState = !_cursorFlashState;
			}
			var color = _cursorFlashState ? GUIHelper.CursorLightColor : GUIHelper.CursorDarkColor;
			visualizationProvider.SetColor(color);
			var x = _cursorPosX + X + 10 + 2;
			visualizationProvider.Line(x, Y + _offsetCursorY, x, Y + _offsetCursorY + _offsetCursorH);
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			var color = 
				Enabled 
					? CursorOver 
						? Color.DarkOliveGreen 
						: Color.Green 
					: Color.DarkSlateGray;
			visualizationProvider.SetColor(color, 75);
			visualizationProvider.Box(X, Y, Width, Height);
			color = IsFocused ? Color.YellowGreen : Color.LightYellow;
			visualizationProvider.SetColor(color);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			color = Color.White;
			visualizationProvider.Print(X + 10, Y + _offsetTxtY, Text);
			ShowCursor(visualizationProvider);
		}
	}
}
