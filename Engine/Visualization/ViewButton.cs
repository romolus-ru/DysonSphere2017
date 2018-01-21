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
		private int _btnTextureBorder = 0;

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
			if (texture != null) {
				// углы
				visualizationProvider.DrawTexturePart(X, Y, texture + ".t1", _btnTextureBorder, _btnTextureBorder);
				visualizationProvider.DrawTexturePart(X + Width - 10, Y, texture + ".t3", _btnTextureBorder, _btnTextureBorder);
				visualizationProvider.DrawTexturePart(X, Y + Height - 10, texture + ".t7", _btnTextureBorder, _btnTextureBorder);
				visualizationProvider.DrawTexturePart(X + Width - 10, Y + Height - 10, texture + ".t9", _btnTextureBorder, _btnTextureBorder);
				// стороны
				visualizationProvider.DrawTexturePart(X + 10, Y, texture + ".t2", Width - _btnTextureBorder * 2, _btnTextureBorder);
				visualizationProvider.DrawTexturePart(X, Y + 10, texture + ".t4", _btnTextureBorder, Height - _btnTextureBorder * 2);
				visualizationProvider.DrawTexturePart(X + Width - 10, Y + 10, texture + ".t6", _btnTextureBorder, Height - _btnTextureBorder * 2);
				visualizationProvider.DrawTexturePart(X + 10, Y + Height - 10, texture + ".t8", Width - _btnTextureBorder * 2, _btnTextureBorder);
				// центр
				visualizationProvider.DrawTexturePart(X + 10, Y + 10, texture + ".t5", Width - _btnTextureBorder * 2, Height - _btnTextureBorder * 2);
			} else {
				visualizationProvider.SetColor(color);
				visualizationProvider.Rectangle(X, Y, Width, Height);
			}
		}

		public void InitTexture(string textureName, string textureNameOver, int textureBorder)
		{
			_btnTexture = textureName;
			_btnTextureOver = textureNameOver;
			_btnTextureBorder = textureBorder;
			VisualizationProvider.LoadAtlas(_btnTexture);
			VisualizationProvider.LoadAtlas(_btnTextureOver);
		}

	}
}
