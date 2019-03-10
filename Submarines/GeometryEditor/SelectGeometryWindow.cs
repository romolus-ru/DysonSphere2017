using System;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using Submarines.Geometry;
using Submarines.Items;

namespace Submarines.GeometryEditor
{
	internal class SelectGeometryWindow : FilteredScrollViewWindow
	{
		private Action<GeometryBase> _onSelect;
		private Action _onClose;

		public void InitWindow(ViewManager viewManager, Action<GeometryBase> onSelect, Action onClose)
		{
			_onSelect = onSelect;
			_onClose = onClose;

			InitWindow("Выбор геометрии для редактирования", viewManager, showOkButton: false, showNewButton: false);
		}

		protected override void InitScrollItems()
		{
			var items = ItemsManager.GetAllGeometries();
			var i = 1;
			foreach (var item in items) {
				var scrollItem = new SelectGeometryScrollItem(item);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.OnSelect = SelectGeometry;
				scrollItem.SetParams(1, 1, 980, 55, "ri" + i + " " + item.Name);
				i++;
			}
		}

		private void SelectGeometry(GeometryBase geometry)
		{
			_onSelect?.Invoke(geometry);
			CloseWindow();
		}

		protected override void InitButtonCancel(ViewButton btnCancel)
		{
			base.InitButtonCancel(btnCancel);
			btnCancel.SetCoordinatesRelative(-100, 0, 0);
			btnCancel.Caption = "закрыть";
			btnCancel.Hint = "закрыть выбор геометрий";
		}

		protected override void CloseWindow()
		{
			base.CloseWindow();
			_onClose?.Invoke();
		}

	}
}