using System;
using System.Collections.Generic;
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
        private List<GeometryType> _filter;

		public void InitWindow(ViewManager viewManager, Action<GeometryBase> onSelect, Action onClose, List<GeometryType> filter = null)
		{
			_onSelect = onSelect;
			_onClose = onClose;
            _filter = filter;

			InitWindow("Выбор геометрии для редактирования", viewManager, showOkButton: false, showNewButton: false);
		}

		protected override void InitScrollItems()
		{
			var items = ItemsManager.GetAllGeometries();
			var i = 1;
			foreach (var item in items) {
                if (_filter != null && !_filter.Contains(item.GeometryType))
                    continue;
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