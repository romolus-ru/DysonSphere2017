﻿using Engine;
using Engine.Data;
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
		public CollectClass CollectClass { get; }
		public Action<CollectClass> OnDelete;
		public Action<CollectClass> OnSelect;

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
			TextClass.CreateSplitedTextAuto(Color.White, null, _collectorClass.ClassName);
			TextClass.CalculateTextPositions();

			var TextFile = new ViewText();
			AddComponent(TextFile);
			TextFile.SetParams(250, 25, 500, 20, "File");
			TextFile.CreateSplitedTextAuto(Color.DarkSlateGray, null, _collectorClass.FileName);
			TextFile.CalculateTextPositions();
		}

		private void DeleteCollectorClass()
		{
			OnDelete?.Invoke(_collectorClass);
		}

		private void SelectCollectorClass()
		{
			OnSelect?.Invoke(_collectorClass);
		}

		public override void DrawObject(VisualizationProvider vp)
		{
			vp.SetColor(
				CursorOver
				? Color.YellowGreen
				: Color.Red);
			vp.Rectangle(X, Y, Width, Height);
		}
	}
}