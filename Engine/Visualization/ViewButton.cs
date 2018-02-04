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

		/// <summary>
		/// Получили сообщение что нажали - определяем местоположение
		/// </summary>
		public void ClickedOnScreen()
		{
			if (!CursorOver) return;
			if (InRange(Input.CursorX, Input.CursorY))
				Press();
		}

		public void KeyPressed()
		{
			Press();
		}
		/// <summary>
		/// Заголовок кнопки
		/// </summary>
		protected string Caption;

		/// <summary>
		/// Заголовок кнопки
		/// </summary>
		protected string Hint;

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
			OnClick += click;
			if (Input == null) throw new NullReferenceException("Input must be initialized first - use AddComponent before Init Button");
			Input.AddKeyActionSticked(KeyPressed, keys);
			Input.AddKeyActionSticked(ClickedOnScreen, Keys.LButton);
			Caption = caption;
			Hint = hint;
			Name = caption;
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			string txt;
			Color color;
			if (CursorOver) {
				txt = "[" + Caption + "]"; color = Color.Red;
			} else {
				txt = " " + Caption + " "; color = Color.White;
			}
			var f = visualizationProvider.FontHeightGet() / 2;

			visualizationProvider.SetColor(color);
			visualizationProvider.Print(X + 4, Y + Height / 2 - f - 3, txt);
			var texture = _btnTexture;
			if (!string.IsNullOrEmpty(Hint) && CursorOver) {
				visualizationProvider.Print(X + 10, Y + Height + 5 - f, Hint);
				texture = _btnTextureOver;
			}
			GUIHelper.ViewGUIRectangle(visualizationProvider, this, texture);
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
