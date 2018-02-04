using Engine.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.Visualization
{
	/// <summary>
	/// Ввод строкового значения
	/// </summary>
	public class ViewInput : ViewComponent
	{
		public string Txt { get; protected set; } = "";
		private int _cursorPos = 0;
		private int _cursorPosX = 0;
		private bool _isFocused = false;
		public bool IsFocused {
			get { return _isFocused; }
			set {
				if (value == _isFocused) return;
				_isFocused = value;
				ChangeFocus(_isFocused);
			}
		}

		private void ChangeFocus(bool _isFocused)
		{
			if (_isFocused) {
				Input.AddKeyActionPaused(CursorLeft, Keys.Left);
				Input.AddKeyActionPaused(CursorRight, Keys.Right);
				Input.AddKeyActionPaused(CursorBackDeleteChar, Keys.Back);
				Input.AddKeyActionPaused(CursorDeleteChar, Keys.Delete);
				Input.AddInputStringAction(InputAction);
			} else {
				Input.RemoveKeyActionPaused(CursorLeft, Keys.Left);
				Input.RemoveKeyActionPaused(CursorRight, Keys.Right);
				Input.RemoveKeyActionPaused(CursorBackDeleteChar, Keys.Back);
				Input.RemoveKeyActionPaused(CursorDeleteChar, Keys.Delete);
				Input.RemoveInputStringAction(InputAction);
			}
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
			if (_cursorPos >= Txt.Length) return;
			Txt = Txt.Remove(_cursorPos, 1);
		}

		private void RecalcCursorPosX()
		{
			_cursorPosX = VisualizationProvider.TextLength(Txt.Substring(0, _cursorPos));
		}
		private void CursorRight()
		{
			_cursorPos++;
			if (_cursorPos > Txt.Length) _cursorPos = Txt.Length;
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
			Txt = Txt.Insert(_cursorPos, str);
			_cursorPos += str.Length;
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
			visualizationProvider.Line(x, Y + 14, x, Y + 22 + 10);
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			var color = CursorOver ? Color.DarkOliveGreen : Color.Green;
			visualizationProvider.SetColor(color, 75);
			visualizationProvider.Box(X, Y, Width, Height);
			color = IsFocused ? Color.Red : Color.LightYellow;
			visualizationProvider.SetColor(color);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			color = Color.White;
			visualizationProvider.Print(X + 10, Y + 10, Txt);
			ShowCursor(visualizationProvider);
		}
	}
}
