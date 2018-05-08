using Engine.Visualization.Scroll;
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
	/// Скроллируемый объект
	/// </summary>
	public class ViewScroll : ViewPanel
	{
		private List<IScrollItem> _items = new List<IScrollItem>();
		protected bool IsDragMode = false;
		/// <summary>
		/// Ожидаем клик, в режим перемещения ещё не переключились
		/// </summary>
		protected bool IsPressedMode = false;
		private int _oldX;
		private int _oldY;
		private int _startX;
		private int _startY;
		private int _scrollOffsetX = 0;// для ограничения движения и смещения обратно по горизонтали
		private int _scrollWidth = 0;
		private int _scrollOffsetY = 0;// для ограничения движения и смещения обратно по вертикали
		private int _scrollHeight = 0;

		public override void AddComponent(ViewComponent component, bool toTop = false)
		{
			if (component is IScrollItem) {
				_items.Add(component as IScrollItem);
				CalcScrollLength();
			}
			base.AddComponent(component, toTop);
		}

		private void CalcScrollLength()
		{
			var item0 = (_items[0] as ViewComponent);
			if (item0 == null) return;
			var minX = item0.X;
			var maxX = item0.X + item0.Width;
			var minY = item0.Y;
			var maxY = item0.Y + item0.Height;
			foreach (var item in _items) {
				var component = item as ViewComponent;
				if (component == null) continue;
				if (component.X < minX) minX = component.X;
				if (component.X + component.Width > maxX) maxX = component.X + component.Width;
				if (component.Y < minY) minY = component.Y;
				if (component.Y + component.Height > maxY) maxY = component.Y + component.Height;
			}
			_scrollWidth = maxX - minX;
			_scrollHeight = maxY - minY;
		}

		public override void RemoveComponent(ViewComponent component)
		{
			_items.Remove(component as IScrollItem);
			base.RemoveComponent(component);
		}

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
			var cx = Input.CursorX;
			var cy = Input.CursorY;
			if (!IsPressedMode) {
				_startX = cx;
				_startY = cy;
				IsPressedMode = true;
			}

			if (IsDragMode) return;
			if (distance(cx - _startX, cy - _startY) < Constants.DragDistance)
				return;

			if (!InRange(cx, cy)) return;
			IsDragMode = true;
			Input.ModalStateStart();
			Input.AddKeyActionSticked(MouseUnPressed, Keys.LButton);
			Input.AddCursorAction(CursorMove);
			_oldX = _startX;
			_oldY = _startY;
		}

		private double distance(int dx, int dy)
		{
			return Math.Sqrt(dx * dx + dy * dy);
		}

		private void MouseUnPressed()
		{
			if (!IsDragMode) return;
			Input.ModalStateStop();
			IsDragMode = false;
			IsPressedMode = false;
		}

		private void CursorMove(int newX, int newY)
		{
			if (!IsDragMode) return;
			var deltaX = newX - _oldX;
			var deltaY = newY - _oldY;
			if (deltaX == 0 && deltaY == 0) return;
			_oldX = newX;
			_oldY = newY;
			ScrollItems(deltaX, deltaY);
		}

		public void ScrollItems(int deltaX, int deltaY)
		{
			_scrollOffsetX += deltaX;
			_scrollOffsetY += deltaY;
			deltaY = 0;
			foreach (var item in _items) {
				item.ScrollBy(deltaX, deltaY);
			}
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			MoveScrollItems();
			base.DrawObject(visualizationProvider);
			visualizationProvider.SetColor(Color.Azure);
			visualizationProvider.Print(X + 10, Y + 10, "dm=" + IsDragMode + " pm=" + IsPressedMode);
		}

		/// <summary>
		/// Ограничиваем перемещение и скролим элементы обратно
		/// </summary>
		private void MoveScrollItems()
		{
			if (_scrollOffsetX + _scrollWidth < Width) {
				return;
			}
			// TODO ограничить движение скролируемых объектов
		}
	}
}