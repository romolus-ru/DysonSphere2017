using Engine.Enums;
using Engine.Visualization.Text;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Engine.Visualization.Scroll
{
	public class SelectStringScrollItem : ScrollItem
	{
		private string _value;
		public Action<string> OnSelect;

		public SelectStringScrollItem(string value)
		{
			_value = value;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			var btnSelect = new ViewButton();
			AddComponent(btnSelect);
			btnSelect.InitButton(SelectValue, "Select", "Выбрать", Keys.None);
			btnSelect.SetParams(90, 10, 120, 30, "Выбрать");
			btnSelect.InitTexture("textRB", "textRB");

			var TextClass = new ViewText();
			AddComponent(TextClass);
			TextClass.SetParams(250, 5, 500, 20, "SelectStringScrollItem");
			TextClass.CreateSplitedTextAuto(Color.White, null, _value, TextAlign.Left);
			TextClass.CalculateTextPositions();
		}

		private void SelectValue()
		{
			OnSelect?.Invoke(_value);
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(
				CursorOver
				? Color.YellowGreen
				: Color.Red);
			visualizationProvider.Rectangle(X, Y, Width, Height);
		}

		public override bool Filtrate(string filter = null)
		{
			if (string.IsNullOrEmpty(filter))
				return true;
			return _value.IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) >= 0;
		}
	}
}