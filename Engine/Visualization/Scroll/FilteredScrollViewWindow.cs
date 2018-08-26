using System;
using System.Windows.Forms;

namespace Engine.Visualization.Scroll
{
	/// <summary>
	/// Выбрать из коллекции коллектора нужный тип класса и/или добавить новый
	/// </summary>
	public class FilteredScrollViewWindow : ViewModalWindow
	{
		protected ViewManager ViewManager;
		protected ViewScroll ViewScroll;

		private ViewInput _filter;
		private ViewButton _btnOk;
		private ViewButton _btnCancel;
		private ViewButton _btnNew;

		public void InitWindow(string header, ViewManager viewManager, bool showOkButton, bool showCancelButton = true, bool showNewButton = true)
		{
			ViewManager = viewManager;
			ViewManager.AddViewModal(this);

			SetParams(150, 150, 1200, 700, header);
			InitTexture("textRB", 10);

			if (showOkButton) {
				_btnOk = new ViewButton();
				AddComponent(_btnOk);
				_btnOk.InitButton(Ok, "Ok", "Ок", Keys.Enter);
				InitButtonOk(_btnOk);
			}

			if (showCancelButton) {
				_btnCancel = new ViewButton();
				AddComponent(_btnCancel);
				_btnCancel.InitButton(Cancel, "Cancel", "Отмена", Keys.Escape);
				InitButtonCancel(_btnCancel);
			}

			if (showNewButton) {
				_btnNew = new ViewButton();
				AddComponent(_btnNew);
				_btnNew.InitButton(NewCommand, "New", "New", Keys.N);
				InitButtonNew(_btnNew);
			}

			_filter = new ViewInput();
			AddComponent(_filter);
			_filter.InputAction("");
			_filter.OnTextChanged += FiltrateViewScroll;
			InitFilter(_filter);

			ViewScroll = new ViewScroll();
			AddComponent(ViewScroll);
			ViewScroll.SetParams(10, 80, 1000, 560, "ViewScroll for FilteredScrollViewWindow");
			InitScrollItems();
			UpdateScrollViewSize();
		}

		protected virtual void InitButtonOk(ViewButton btnOk)
		{
			btnOk.SetParams(20, 670, 80, 25, "btnOk");
			btnOk.InitTexture("textRB", "textRB");
		}

		protected virtual void InitButtonCancel(ViewButton btnCancel)
		{
			btnCancel.SetParams(150, 670, 80, 25, "btnCancel");
			btnCancel.InitTexture("textRB", "textRB");
		}

		protected virtual void InitButtonNew(ViewButton btnNew)
		{
			btnNew.SetParams(20, 20, 80, 25, "btnNew");
			btnNew.InitTexture("textRB", "textRB");
		}

		protected virtual void InitFilter(ViewInput filter)
		{
			filter.SetParams(140, 30, 500, 40, "filter");
		}

		private void FiltrateViewScroll(string filter)
		{
			var items = ViewScroll.GetItems();
			foreach (var item in items) {
				var f = item.Filtrate(filter);
				item.Visible = f;
			}
			UpdateScrollViewSize();
		}

		/// <summary>
		/// Обработка кнопки New (она же иногда scan)
		/// </summary>
		protected virtual void NewCommand()
		{

		}

		/// <summary>
		/// Расстановка элементов и пересчёт размеров скрола
		/// </summary>
		protected virtual void UpdateScrollViewSize()
		{
			var pos = 5;
			var items = ViewScroll.GetItems();
			foreach (var item in items) {
				if (!item.Visible) continue;
				item.SetCoordinates(10, pos);
				pos += item.Height;
			}
			ViewScroll.CalcScrollSize();
		}

		/// <summary>
		/// Инициализация списка
		/// </summary>
		protected virtual void InitScrollItems()
		{
		}

		private void Select(ScrollItem item)
		{
			SelectCommand(item);
			CloseWindow();
		}

		/// <summary>
		/// Выбрали элемент из списка
		/// </summary>
		/// <param name="item"></param>
		protected virtual void SelectCommand(ScrollItem item) { }

		protected virtual void CloseWindow()
		{
			ViewManager.RemoveViewModal(this);
			ViewManager = null;
		}

		private void Cancel()
		{
			CancelCommand();
			CloseWindow();
		}

		/// <summary>
		/// Нажали отмену
		/// </summary>
		protected virtual void CancelCommand() { }

		private void Ok()
		{
			OkCommand();
			CloseWindow();
		}

		/// <summary>
		/// Нажали ok
		/// </summary>
		protected virtual void OkCommand() { }

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(System.Drawing.Color.Black, 50);
			visualizationProvider.Box(X, Y, Width, Height);
			base.DrawObject(visualizationProvider);
		}
	}
}