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
	public class ViewButton : ViewComponent
	{
		/// <summary>
		/// Генерируемое сообщение 
		/// </summary>
		public Action OnClick;

		private string _btnTexture = null;
		private string _btnTextureOver = null;
		private Keys[] _keys;

		/// <summary>
		/// Получили сообщение что нажали - определяем местоположение
		/// </summary>
		private void ClickedOnScreen()
		{
			if (!CursorOver) return;
			if (InRange(Input.CursorX, Input.CursorY))
				KeyPressed();
		}

		private void KeyPressed()
		{
			if (Enabled)
				Press();
		}
		/// <summary>
		/// Заголовок кнопки
		/// </summary>
		protected string Caption;

		/// <summary>
		/// Подсказка кнопки
		/// </summary>
		protected string Hint;

		/// <summary>
		/// Подказка комбинации кнопок
		/// </summary>
		protected string HintKeys;

		/// <summary>
		/// Визуальное состояние
		/// </summary>
		public bool Enabled { get; set; } = true;

		public ViewButton() { }

		/// <summary>
		/// Жмём на кнопку
		/// </summary>
		protected virtual void Press()
		{
			OnClick?.Invoke();
		}

		public void InitButton(Action click, string caption, string hint, params Keys[] keys)
		{
			_keys = keys;
			OnClick += click;
			if (Input == null) throw new NullReferenceException("Input must be initialized first - use AddComponent before Init Button");
			Input.AddKeyActionSticked(KeyPressed, keys);
			Input.AddKeyActionSticked(ClickedOnScreen, Keys.LButton);
			Caption = caption;
			Hint = hint;
			HintKeys = InputHelper.KeyCombinationToString(keys);
			Name = caption;
		}

		protected override void ClearObject()
		{
			base.ClearObject();
			Input.RemoveKeyActionSticked(KeyPressed, _keys);
			Input.RemoveKeyActionSticked(ClickedOnScreen, Keys.LButton);
		}
		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			string txt;
			Color color;
			if (CursorOver) {
				txt = "[" + Caption + "]"; color = Color.Yellow;
			} else {
				txt = " " + Caption + " "; color = Color.White;
			}
			var f = visualizationProvider.FontHeight / 2;

			var texture = CursorOver||!Enabled ? _btnTexture : _btnTextureOver;
			GUIHelper.ViewGUIRectangle(visualizationProvider, this, texture);

			visualizationProvider.SetColor(color);
			visualizationProvider.Print(X + 4, Y + Height / 2 - f - 3, txt);
			if (!Enabled) {
				visualizationProvider.SetColor(Color.Black);
				visualizationProvider.Print(X + 4 - 1, Y + Height / 2 - f - 3 - 1, txt);
			}
			GUIHelper.ShowHint(visualizationProvider, this, Hint, HintKeys);
			/*if (!string.IsNullOrEmpty(Hint) && CursorOver) {
				visualizationProvider.SetColor(GUIHelper.ButtonHintColor);
				visualizationProvider.Print(X + 10, Y + Height + 5 - f, Hint);
				//var s = visualizationProvider.CurTxtX + " " + Hint + " " + visualizationProvider.TextLength(Hint) + " ";
				if (!string.IsNullOrEmpty(HintKeys)) {
					visualizationProvider.SetColor(GUIHelper.ButtonHintKeysColor);
					visualizationProvider.Print(" "+HintKeys);
					//s += " = > " + visualizationProvider.CurTxtX + "  " + HintKeys + " " + visualizationProvider.TextLength(HintKeys) + " ";
				}
				//visualizationProvider.SetColor(Color.White);visualizationProvider.Print(X + 10, Y + Height + 5 + 15, s);
			}*/
		}

		public void InitTexture(string textureName, string textureNameOver)
		{
			_btnTexture = textureName;
			_btnTextureOver = textureNameOver;
			VisualizationProvider.LoadAtlas(_btnTexture);
			VisualizationProvider.LoadAtlas(_btnTextureOver);
		}
	}
}