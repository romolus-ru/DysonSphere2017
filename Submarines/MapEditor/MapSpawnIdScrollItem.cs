using Engine;
using Engine.Helpers;
using Engine.Visualization;
using Engine.Visualization.Text;
using Submarines.Editors;
using Submarines.Items;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Submarines.MapEditor
{
    internal class MapSpawnIdScrollItem<T> : MemberBaseScrollView<T> where T : class
    {
        private ViewText _viewValue;
        private int _value;
        private PropertyInfo _valueProperty;
        internal ItemMap Map = null;

        protected override void InitObject(VisualizationProvider visualizationProvider, Input input) {
            base.InitObject(visualizationProvider, input);

            var btnChange = new ViewButton();
            AddComponent(btnChange);
            btnChange.InitButton(SelectGeometry, "Change", "Поменять", Keys.None);
            btnChange.SetParams(90, 10, 120, 30, "Поменять");
            btnChange.InitTexture("textRB", "textRB");

            _viewValue = new ViewText();
            AddComponent(_viewValue);
            _viewValue.SetParams(250, 5, 500, 20, "Class");
            _viewValue.CreateSplitedTextAuto(Color.Gray, null, "Unknown");
            _viewValue.CalculateTextPositions();
        }

        private void SelectGeometry() {
            new SelectItemMapSpawnWindow().InitWindow(ViewHelper.ViewManager, GetItemMap, onClose: null, Map);
        }

        private void GetItemMap(ItemMap.ItemMapSpawnPoint spawn) {
            _value = spawn.Id;
            SetupViewValue(_value);
        }

        public override void InitValueEditor(T obj, MemberInfo memberInfo) {
            _valueProperty = (memberInfo as PropertyInfo);
            var value = _valueProperty.GetValue(obj);
            _value = (int)value;
            SetupViewValue(_value);
        }

        private void SetupViewValue(int value) {
            _viewValue.ClearTexts();
            var valueExists = false;
            if (value != 0) {
                var spawn = Map.MapSpawns.FirstOrDefault(s => s.Id == value);
                if (spawn != null) {
                    _viewValue.CreateSplitedTextAuto(Color.White, null, "map " + Map.MapCode + " " + spawn.Id + " " + spawn.Name + " " + spawn.SpawnType);
                    valueExists = true;
                }
            }
            if (!valueExists)
                _viewValue.CreateSplitedTextAuto(Color.Red, null, "value not set");
            _viewValue.CalculateTextPositions();
        }

        /// <summary>
        /// Установить значение поля объекта
        /// </summary>
        /// <param name="obj"></param>
        public override void SetValue(T obj) {
            _valueProperty.SetValue(obj, _value);
        }
    }
}