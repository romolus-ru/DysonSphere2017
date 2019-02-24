﻿using System;
using System.Windows.Forms;
using Engine.Helpers;

namespace Engine.Visualization
{
	/// <summary>
	/// Генерирует событие перемещения
	/// </summary>
	public class ViewDraggable : ViewComponent
	{
		public delegate void MoveObjectRelativeDelegate(int dx, int dy);
		
		/// <summary>
		/// Присылаются координаты относительного перемещения объекта
		/// </summary>
		public MoveObjectRelativeDelegate OnMoveObjectRelative;

		public Action OnDragModeStart;
		public Action OnDragModeEnd;

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
			OnDragModeStart?.Invoke();
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
			OnDragModeEnd?.Invoke();
		}

		private void CursorMove(int newX, int newY)
		{
			if (!IsDragMode) return;
			var deltaX = newX - _oldX;
			var deltaY = newY - _oldY;
			if (deltaX == 0 && deltaY == 0) return;
			_oldX = newX;
			_oldY = newY;
			if (OnMoveObjectRelative == null) {
				SetCoordinatesRelative(deltaX, deltaY, 0);
			} else {
				OnMoveObjectRelative?.Invoke(deltaX, deltaY);
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
				var color = GUIHelper.DraggableDefaultColor;
				if (CursorOver) color = GUIHelper.DraggableCursorOverColor;
				if (IsDragMode) color = GUIHelper.DraggableDragModeColor;
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
