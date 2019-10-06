using Engine;
using Engine.Helpers;
using Engine.Visualization;
using Engine.Visualization.Text;
using Submarines.Editors;
using Submarines.Items;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Submarines.MapEditor
{
    public class MapCodeScrollItem<T> : MemberBaseScrollView<T> where T : class
    {
        private ViewText _viewValue;
        private string _value;
        private PropertyInfo _valueProperty;
        internal List<string> Filter = null;

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
            new SelectItemMapWindow().InitWindow(ViewHelper.ViewManager, GetItemMap, onClose: null, filter: Filter);
        }

        private void GetItemMap(ItemMap map) {
            _value = map.MapCode;
            SetupViewValue(_value);
        }

        public override void InitValueEditor(T obj, MemberInfo memberInfo) {
            _valueProperty = (memberInfo as PropertyInfo);
            _value = _valueProperty.GetValue(obj)?.ToString();
            SetupViewValue(_value);
        }

        private void SetupViewValue(string name) {
            _viewValue.ClearTexts();
            if (string.IsNullOrEmpty(name)) {
                _viewValue.CreateSplitedTextAuto(Color.Red, null, "value not set");
            } else {
                _viewValue.CreateSplitedTextAuto(Color.White, null, name);
            }
            _viewValue.CalculateTextPositions();
        }

        /// <summary>
        /// Установить значение поля объекта
        /// </summary>
        /// <param name="obj"></param>
        public override void SetValue(T obj) {
            if (!string.IsNullOrEmpty(_value))
                _valueProperty.SetValue(obj, _value);
        }
    }
}