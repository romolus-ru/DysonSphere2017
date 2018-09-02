using Engine;
using Engine.Data;
using Engine.Enums;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using Engine.Visualization.Text;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EngineTools
{
	public class CollectorClassScrollViewItem : ScrollItem
	{
		private CollectClass _collectorClass;
		public CollectClass CollectClass { get { return _collectorClass; } }
		public Action<CollectorClassScrollViewItem> OnDelete;
		public Action<CollectorClassScrollViewItem> OnSelect;

		public CollectorClassScrollViewItem(CollectClass collectorClass)
		{
			_collectorClass = collectorClass;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			if (_collectorClass.Id > 0) {
				var btnDelete = new ViewButton();
				AddComponent(btnDelete);
				btnDelete.InitButton(DeleteCollectorClass, "delete", "Удалить", Keys.None);
				btnDelete.SetParams(20, 10, 60, 30, "DeleteCollectorClass");
				btnDelete.InitTexture("textRB", "textRB");
			}

			var btnSelect = new ViewButton();
			AddComponent(btnSelect);
			btnSelect.InitButton(SelectCollectorClass, "Select", "Выбрать", Keys.None);
			btnSelect.SetParams(90, 10, 120, 30, "Выбрать");
			btnSelect.InitTexture("textRB", "textRB");

			var TextClass = new ViewText();
			AddComponent(TextClass);
			TextClass.SetParams(250, 5, 500, 20, "Class");
			TextClass.CreateSplitedTextAuto(Color.White, null, _collectorClass.ClassName, TextAlign.Left);
			TextClass.CalculateTextPositions();

			var TextFile = new ViewText();
			AddComponent(TextFile);
			TextFile.SetParams(250, 25, 500, 20, "File");
			TextFile.CreateSplitedTextAuto(Color.DarkSlateGray, null, _collectorClass.FileName, TextAlign.Left);
			TextFile.CalculateTextPositions();
		}

		private void DeleteCollectorClass()
		{
			OnDelete?.Invoke(this);
		}

		private void SelectCollectorClass()
		{
			OnSelect?.Invoke(this);
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
			if (CollectClass.ClassName.IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) >= 0)
				return true;
			return false;
		}
	}
}