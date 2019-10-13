using Engine.Visualization;
using Engine.Visualization.Scroll;
using Submarines.Items;
using System;

namespace Submarines.MapEditor
{
    internal class SelectItemMapSpawnWindow : FilteredScrollViewWindow
    {
        private Action<ItemMap.ItemMapSpawnPoint> _onSelect;
        private Action _onClose;
        private ItemMap _map = null;

        public void InitWindow(ViewManager viewManager, Action<ItemMap.ItemMapSpawnPoint> onSelect, Action onClose, ItemMap map) {
            _onSelect = onSelect;
            _onClose = onClose;
            _map = map;

            InitWindow("Выбор карты для редактирования", viewManager, showOkButton: false, showNewButton: false);
        }

        protected override void InitScrollItems() {
            var i = 1;
            foreach (var item in _map.MapSpawns) {
                var scrollItem = new SelectItemMapSpawnScrollItem(item);
                ViewScroll.AddComponent(scrollItem);
                scrollItem.OnSelect = SelectMapSpawn;
                scrollItem.SetParams(1, 1, 980, 55, "ri" + i + " " + item.Id);
                i++;
            }
        }

        private void SelectMapSpawn(ItemMap.ItemMapSpawnPoint spawn) {
            CloseWindow();
            _onSelect?.Invoke(spawn);
        }

        protected override void InitButtonCancel(ViewButton btnCancel) {
            base.InitButtonCancel(btnCancel);
            btnCancel.SetCoordinatesRelative(-100, 0, 0);
            btnCancel.Caption = "закрыть";
            btnCancel.Hint = "закрыть выбор карты";
        }

        protected override void CloseWindow() {
            base.CloseWindow();
            _onClose?.Invoke();
        }

    }
}