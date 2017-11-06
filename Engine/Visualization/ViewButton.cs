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

		/// <summary>
		/// Получили сообщение что нажали - определяем местоположение
		/// </summary>
		public void ClickedOnScreen()
		{
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
			Input.AddKeyAction(KeyPressed, keys);
			Input.AddKeyAction(ClickedOnScreen, Keys.LButton);
			Input.AddCursorAction(this.CursorHandler);
			Caption = caption;
			Hint = hint;
			Name = caption;
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.RotateReset();
			string txt;
			Color color;
			if (CursorOver) {
				txt = "[" + Caption + "]"; color = Color.Red;
			} else {
				txt = " " + Caption + " "; color = Color.White;
			}
			var f = visualizationProvider.FontHeightGet() / 2;

			visualizationProvider.SetColor(color);
			visualizationProvider.Rectangle(X, Y, Width, Height);// если текстуры будут то они замаскируют этот прямоугольник

			visualizationProvider.SetColor(color);
			visualizationProvider.Print(X + 4, Y + Height / 2 - f - 3, txt);
			if (Hint != "" && CursorOver) {
				visualizationProvider.Print(X + 10, Y + Height + 5 - f, Hint);
			}
			if (_btnTexture != null) {
				var tw = 30;
				var th = 30;

				visualizationProvider.DrawTexture(X + 100, Y-50, _btnTexture);

				// углы
				visualizationProvider.DrawTexturePart(X, Y, _btnTexture, 0, 0, 10, 10);
				visualizationProvider.DrawTexturePart(X + Width - 10, Y, _btnTexture, tw - 10, 0, 10, 10);
				visualizationProvider.DrawTexturePart(X + Width - 10, Y + Height - 10, _btnTexture, tw - 10, th - 10, 10, 10);
				visualizationProvider.DrawTexturePart(X, Y + Height - 10, _btnTexture, 0, th - 10, 10, 10);
				// стороны
				visualizationProvider.DrawTexturePart(X + 10, Y, _btnTexture, 10, 0, 10, 10);
				visualizationProvider.DrawTexturePart(X + 10, Y + Height - 10, _btnTexture, 10, th - 10, 10, 10);
				visualizationProvider.DrawTexturePart(X, Y + 10, _btnTexture, 0, 10, 10, 10);
				visualizationProvider.DrawTexturePart(X + Width - 10, Y + 10, _btnTexture, tw - 10, 10, 10, 10);
				// центр
				visualizationProvider.DrawTexturePart(X + 10, Y+10, _btnTexture, 10, 10, 10, 10);
			}
		}

		public void InitTexture(string textureName)
		{
			_btnTexture = textureName;
			VisualizationProvider.LoadTexture(_btnTexture, @"..\_files\_graph\btn00a.png");
		}


	}
}
