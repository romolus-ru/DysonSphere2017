using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.Visualization
{
	/// <summary>
	/// Генерирует событие перемещения
	/// </summary>
	public class ViewDragable : ViewComponent
	{
		/// <summary>
		/// Присылаются координаты относительного перемещения объекта
		/// </summary>
		public Action<int, int> MoveObjectRelative;
		protected bool IsDragMode = false;
		private int _oldX;
		private int _oldY;
		private string _btnTexture = null;
		private int _btnTextureBorder = 0;

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			Input.AddKeyAction(MousePressed, Keys.LButton);
			Input.AddKeyActionSticked(MouseUnPressed, Keys.LButton);
		}

		protected override void ClearObject()
		{
			base.ClearObject();
			Input.RemoveKeyAction(MousePressed, Keys.LButton);
			Input.RemoveKeyActionSticked(MouseUnPressed, Keys.LButton);
		}

		private void MousePressed()
		{
			if (IsDragMode) return;
			if (!InRange(Input.CursorX, Input.CursorY)) return;
			IsDragMode = true;
			Input.ModalStateStart();
			Input.AddKeyActionSticked(MouseUnPressed, Keys.LButton);
			Input.AddCursorAction(CursorMove);
			_oldX = Input.CursorX;
			_oldY = Input.CursorY;
		}

		private void MouseUnPressed()
		{
			if (!IsDragMode) return;
			Input.ModalStateStop();
			//Input.RemoveCursorAction(CursorMove);
			IsDragMode = false;
		}

		private void CursorMove(int newX, int newY)
		{
			if (!IsDragMode) return;
			var deltaX = newX - _oldX;
			var deltaY = newY - _oldY;
			if (deltaX == 0 && deltaY == 0) return;
			_oldX = newX;
			_oldY = newY;
			if (MoveObjectRelative == null) {
				SetCoordinatesRelative(deltaX, deltaY, 0);
			} else {
				MoveObjectRelative?.Invoke(deltaX, deltaY);
			}
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			var texture = _btnTexture;
			if (!string.IsNullOrEmpty(texture)) {
				visualizationProvider.DrawTexturePart(X, Y-4, texture + ".m1", _btnTextureBorder, Height);
				visualizationProvider.DrawTexturePart(X + _btnTextureBorder, Y-4, texture + ".m2", Width - _btnTextureBorder * 2, Height);
				visualizationProvider.DrawTexturePart(X + Width - _btnTextureBorder, Y-4, texture + ".m3", _btnTextureBorder, Height);
			} else {
				var color = System.Drawing.Color.Beige;
				if (CursorOver) color = System.Drawing.Color.Aqua;
				if (IsDragMode) color = System.Drawing.Color.BurlyWood;
				visualizationProvider.SetColor(color);
				visualizationProvider.Rectangle(X, Y, Width, Height);
			}
		}

		public void InitTexture(string textureName, int textureBorder)
		{
			_btnTexture = textureName;
			_btnTextureBorder = textureBorder;
			VisualizationProvider.LoadAtlas(_btnTexture);
		}
	}
}
