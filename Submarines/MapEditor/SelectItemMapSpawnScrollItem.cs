using Engine;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using Submarines.Items;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Submarines.MapEditor
{
    internal class SelectItemMapSpawnScrollItem : ScrollItem
    {
        private ItemMap.ItemMapSpawnPoint _spawn;
        private ViewButton _btnSelect;
        public Action<ItemMap.ItemMapSpawnPoint> OnSelect;

        public SelectItemMapSpawnScrollItem(ItemMap.ItemMapSpawnPoint spawn) {
            _spawn = spawn;
        }

        protected override void InitObject(VisualizationProvider visualizationProvider, Input input) {
            base.InitObject(visualizationProvider, input);

            _btnSelect = new ViewButton();
            AddComponent(_btnSelect);
            _btnSelect.InitButton(Select, "Выбрать", "Выбрать", Keys.None);
            _btnSelect.SetParams(10, 10, 200, 40, "Выбрать");
        }

        private void Select() {
            OnSelect?.Invoke(_spawn);
        }

        public override void DrawObject(VisualizationProvider visualizationProvider) {
            visualizationProvider.SetColor(Color.Azure);
            visualizationProvider.Print(X + 250, Y, "тип " + _spawn.Id + " " + _spawn.SpawnType + " " + _spawn.CodeInfo);
            visualizationProvider.Print(X + 250, Y + 10, _spawn.Name);
            visualizationProvider.Print(X + 250, Y + 20, _spawn.Description);
        }
    }
}