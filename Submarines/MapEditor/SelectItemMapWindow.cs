using Engine.Visualization;
using Engine.Visualization.Scroll;
using Submarines.Items;
using System;
using System.Collections.Generic;

namespace Submarines.MapEditor
{
	internal class SelectItemMapWindow : FilteredScrollViewWindow
	{
		private Action<ItemMap> _onSelect;
		private Action _onClose;
        private List<string> _filter = null;

		public void InitWindow(ViewManager viewManager, Action<ItemMap> onSelect, Action onClose, List<string> filter)
		{
			_onSelect = onSelect;
			_onClose = onClose;
            _filter = filter;

			InitWindow("Выбор карты для редактирования", viewManager, showOkButton: false, showNewButton: false);
		}

		protected override void InitScrollItems()
		{
			var items = ItemsManager.GetAllMaps();
			var i = 1;
			foreach (var item in items) {
                if (_filter != null && _filter.Contains(item.MapCode))
                    continue;

				var scrollItem = new SelectItemMapScrollItem(item);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.OnSelect = SelectMap;
				scrollItem.SetParams(1, 1, 980, 55, "ri" + i + " " + item.MapName);
				i++;
			}
		}

		private void SelectMap(ItemMap map)
		{
			CloseWindow();
            _onSelect?.Invoke(map);
        }

        protected override void InitButtonCancel(ViewButton btnCancel)
		{
			base.InitButtonCancel(btnCancel);
			btnCancel.SetCoordinatesRelative(-100, 0, 0);
			btnCancel.Caption = "закрыть";
			btnCancel.Hint = "закрыть выбор карты";
		}

		protected override void CloseWindow()
		{
			base.CloseWindow();
			_onClose?.Invoke();
		}

	}
}