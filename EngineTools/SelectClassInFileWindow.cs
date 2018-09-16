using Engine;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using Engine.Visualization.Text;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EngineTools
{
	public class SelectClassInFileWindow : FilteredScrollViewWindow
	{
		public Action<string, string> OnClassSelected;
		private string _classFile;
		private string _className;
		private ViewText _textFile;

		public void InitWindow(ViewManager viewManager, string classFile, string className, Action<string,string> result)
		{
			_classFile = classFile;
			_className = className;
			OnClassSelected = result;

			InitWindow("Select class in file", viewManager, showOkButton: true, showNewButton: true);
		}

		protected override void InitScrollItems()
		{
			_textFile = new ViewText();
			AddComponent(_textFile);
			_textFile.SetParams(200, 5, 500, 20, "File");
			_textFile.CreateSplitedTextAuto(Color.Gray, null, "Unknown");
			_textFile.CalculateTextPositions();

			UpdateScroll();
		}

		protected override void InitButtonNew(ViewButton btnNew)
		{
			base.InitButtonNew(btnNew);
			btnNew.Caption = "SelectFile";
			btnNew.Hint = "Выберите файл";
		}

		protected override void NewCommand()
		{
			var filesWithPath = ToolsCollectorHelper.GetFiles();
			var files = new List<string>();
			var appPath = StateEngine.AppPath;
			foreach (var fileName in filesWithPath) {
				var shortFileName = fileName.Substring(appPath.Length);
				files.Add(shortFileName);
			}
			new SelectStringWindow().InitWindow(ViewManager, files, GetClasses, null);
		}
		
		private void GetClasses(string fileName)
		{
			_classFile = fileName;
			if (_textFile != null) {
				_textFile.ClearTexts();
				_textFile.CreateSplitedTextAuto(Color.White, null, fileName);
				_textFile.CalculateTextPositions();
			}
			UpdateScroll();
		}

		private void UpdateScroll()
		{
			ViewScroll.ClearItems();
			if (string.IsNullOrEmpty(_classFile))
				return;
			var i = 2;
			var classes = ToolsCollectorHelper.GetClassesInFile(StateEngine.AppPath + _classFile);
			foreach (var cls in classes) {
				var scrollItem = new SelectStringScrollItem(cls);
				ViewScroll.AddComponent(scrollItem);
				scrollItem.SetParams(10, (i - 1) * 50 + 10, 950, 50, cls);
				scrollItem.OnSelect += SelectClass;
				i++;
			}
			ViewScroll.CalcScrollSize();
		}

		private void SelectClass(string className)
		{
			_className = className;
			OnClassSelected?.Invoke(_classFile, _className);
			CloseWindow();
		}
	}
}