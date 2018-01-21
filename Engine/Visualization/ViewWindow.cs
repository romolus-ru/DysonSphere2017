﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Окно
	/// </summary>
	public class ViewWindow : ViewPanel
	{
		private string _btnTexture = null;
		private int _btnTextureBorder = 0;
		private ViewDragable _dragHeader = null;
		private ViewDragable _resizer = null;
		protected List<ViewComponent> _inputs = null;

		/// <summary>
		/// Заголовок окна
		/// </summary>
		protected string Caption;

		public ViewWindow() { }

		public override void AddComponent(ViewComponent component, bool toTop = false)
		{
			base.AddComponent(component, toTop);
			if (component is ViewInput) {
				if (_inputs == null) _inputs = new List<ViewComponent>();
				_inputs.Add(component);
			}
		}

		public override void RemoveComponent(ViewComponent component)
		{
			base.RemoveComponent(component);
			if (_inputs != null) {
				_inputs.Remove(component);
				if (_inputs.Count == 0) _inputs = null;
			}
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			_dragHeader = new ViewDragable();
			this.AddComponent(_dragHeader);
			_dragHeader.SetParams(10, 0, 30, 10,"dragHeader");
			_dragHeader.MoveObjectRelative += HeadMove;
			_resizer = new ViewDragable();
			this.AddComponent(_resizer);
			_resizer.SetParams(50, 50, 15, 10, "resizer");
			_resizer.MoveObjectRelative += Resize;
		}

		private void HeadMove(int rx, int ry)
		{
			this.SetCoordinatesRelative(rx, ry, 0);
		}

		private void Resize(int rx, int ry)
		{
			// TODO менять надо только когда курсор ниже и правее начальной точки
			if (rx != 0) {
				Width += rx;
				if (Width < 10) Width = 10;
			}
			if (ry != 0) {
				Height += ry;
				if (Height < 10) Height = 10;
			}
		}

		protected override void Resized()
		{
			_resizer.SetCoordinates(Width, Height);
		}
		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			var texture = _btnTexture;
			if (!string.IsNullOrEmpty(texture)) {
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
				var color = CursorOver ? Color.DarkOliveGreen : Color.Green;
				visualizationProvider.SetColor(color, 75);
				visualizationProvider.Box(X, Y, Width, Height);
				visualizationProvider.SetColor(Color.Red);
				visualizationProvider.Rectangle(X, Y, Width, Height);
			}
		}

		public void InitTexture(string textureName, int textureBorder)
		{
			if (VisualizationProvider == null) throw new NullReferenceException("need start init before init texture");
			_btnTexture = textureName;
			_btnTextureBorder = textureBorder;
			VisualizationProvider.LoadAtlas(_btnTexture);
			_dragHeader.InitTexture(textureName, 6);
			_resizer.InitTexture(textureName, 6);
		}
	}
}
