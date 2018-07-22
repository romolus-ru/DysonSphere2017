using Engine.Visualization.Scroll;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// TODO переделать. нужно что бы скрол принимал айтемы-генерики и мог выдать тот айтем который счас выбран
// так же айтем-генерик должен легко переопределяться для вывода нужной информации
// например что бы он мог получить строку и другую информацию для вывода на экран
namespace Engine.Visualization
{
	/// <summary>
	/// Скроллируемый объект
	/// </summary>
	public class ViewScroll : ViewPanel
	{
		private List<IScrollItem> _items = new List<IScrollItem>();
		protected bool IsDragMode = false;
		private bool ModalStateActive = false;
		/// <summary>
		/// Ожидаем клик, в режим перемещения ещё не переключились
		/// </summary>
		protected bool IsPressedMode = false;
		private int _oldX;
		private int _oldY;
		private int _startX;
		private int _startY;
		private DateTime _startTime;
		private int _autoX = 0;
		private int _autoY = 0;
		private int _scrollOffsetX = 0;// для ограничения движения и смещения обратно по горизонтали
		private int _scrollWidth = 0;
		private int _scrollOffsetY = 0;// для ограничения движения и смещения обратно по вертикали
		private int _scrollHeight = 0;

		public override void AddComponent(ViewComponent component, bool toTop = false)
		{
			if (component is IScrollItem) {
				_items.Add(component as IScrollItem);
				CalcScrollSize();
			}
			base.AddComponent(component, toTop);
		}

		/// <summary>
		/// Получить ссылки на элементы скрола
		/// </summary>
		/// <returns></returns>
		public List<IScrollItem> GetItems() => new List<IScrollItem>(_items);

		public void CalcScrollSize()
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
			if (ModalStateActive)
				Input.ModalStateStop();
			Input.RemoveKeyAction(MousePressed, Keys.LButton);
			Input.RemoveKeyActionSticked(MouseUnPressed, Keys.LButton);
			base.ClearObject();
		}

		private void MousePressed()
		{
			var cx = Input.CursorX;
			var cy = Input.CursorY;
			if (!IsPressedMode) {
				_startX = cx;
				_startY = cy;
				_startTime = DateTime.Now;
				IsPressedMode = true;
			}

			if (IsDragMode) return;
			if (distance(cx - _startX, cy - _startY) < Constants.DragDistance)
				return;

			if (!InRange(cx, cy)) return;
			IsDragMode = true;
			Input.ModalStateStart();
			ModalStateActive = true;
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
			if (IsDestroyed) return;// это проще. можно сделать интерфейс IDestroyed - и с его помощью образатывать события в классе Input GetSticked
			if (IsDragMode) {
				Input.ModalStateStop();
				ModalStateActive = false;
				IsDragMode = false;
				IsPressedMode = false;
				CalcAutoMove();
			} else {
				// определяем какой айтем под курсором и выделяем его
				var x = Input.CursorX;
				var y = Input.CursorY;
				if (!InRange(x, y)) return;
				_items.ForEach(item => item.SetSelected(x, y));
			}
		}

		private void CursorMove(int newX, int newY)
		{
			if (!IsDragMode) return;
			var deltaX = newX - _oldX;
			var deltaY = newY - _oldY;
			//if (_scrollHeight < Height) deltaY = 0;
			//if (_scrollWidth < Width) deltaX = 0;
			if (deltaX == 0 && deltaY == 0) return;
			_oldX = newX;
			_oldY = newY;
			ScrollItems(deltaX, deltaY);
		}

		public void ScrollItems(int deltaX, int deltaY)
		{
			if (_scrollHeight < Height) deltaY = 0;
			if (_scrollWidth < Width) deltaX = 0;
			_scrollOffsetX += deltaX;
			_scrollOffsetY += deltaY;
			//deltaY = 0;
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
			visualizationProvider.Print(X + 10, Y + 25, "ox=" + _scrollOffsetX + " b=" + Constants.ScrollMoveBorder);
			visualizationProvider.Print(X + 10, Y + 40, "dxr=" + (X + Width - _scrollOffsetX - _scrollWidth - 2 * Constants.ScrollMoveBorder)
				//- _scrollOffsetX - Width + Constants.ScrollMoveBorder)
				+ " W=" + Width + " sw=" + _scrollWidth
				+ " H=" + Height + " sh=" + _scrollHeight);
			visualizationProvider.Print(X + 10, Y + 55, "ax=" + _autoX + " ay=" + _autoY);
		}

		/// <summary>
		/// Ограничиваем перемещение и скролим элементы обратно
		/// </summary>
		private void MoveScrollItems()
		{
			if (_scrollWidth + 2 * Constants.ScrollMoveBorder < Width) return;

			var dxLeft = _scrollOffsetX - Constants.ScrollMoveBorder;
			if (dxLeft > 0) ScrollItems(-dxLeft, 0);

			var dxRight = X + Width - _scrollOffsetX - _scrollWidth - 3 * Constants.ScrollMoveBorder;
			//X+_scrollWidth - _scrollOffsetX - Width;// + Constants.ScrollMoveBorder;
			if (dxRight > 0) ScrollItems(dxRight, 0);

			var dxTop = _scrollOffsetY - Constants.ScrollMoveBorder;
			if (dxTop > 0) ScrollItems(0, -dxTop);

			var dxBottom = Y + Height - _scrollOffsetY - _scrollHeight - 3 * Constants.ScrollMoveBorder;
			//X+_scrollWidth - _scrollOffsetX - Width;// + Constants.ScrollMoveBorder;
			if (dxBottom > 0) ScrollItems(0, dxBottom);

			if (IsDragMode) return;

			var dxLeftBack = _scrollOffsetX;
			if (dxLeftBack > 0) ScrollItems(-dxLeftBack / 10, 0);
			var dxRightBack = X + Width - _scrollOffsetX - _scrollWidth - 2 * Constants.ScrollMoveBorder;
			//X - _scrollOffsetX - Width + Constants.ScrollMoveBorder;
			if (dxRightBack > 0) ScrollItems(dxRightBack / 10, 0);

			var dxTopBack = _scrollOffsetY;
			if (dxTopBack > 0) ScrollItems(0, -dxTopBack / 10);
			var dxBottomBack = Y + Height - _scrollOffsetY - _scrollHeight - 2 * Constants.ScrollMoveBorder;
			//X - _scrollOffsetX - Width + Constants.ScrollMoveBorder;
			if (dxBottomBack > 0) ScrollItems(0, dxBottomBack / 10);

			var dx = _autoX / 4;
			_autoX -= dx;
			var dy = _autoY / 4;
			_autoY -= dy;
			ScrollItems(dx, dy);
		}

		private void CalcAutoMove()
		{
			int dtime = (DateTime.Now - _startTime).Milliseconds / 100;
			if (dtime == 0) {
				_autoX = 0;
				_autoY = 0;
				return;
			}
			var dx = Input.CursorX - _startX;
			var dy = Input.CursorY - _startY;
			_autoX = dx / dtime;
			_autoY = dy / dtime;
		}
	}
}