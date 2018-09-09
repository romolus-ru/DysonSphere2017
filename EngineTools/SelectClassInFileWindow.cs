using Engine.Visualization;
using Engine.Visualization.Scroll;
using Engine.Visualization.Text;
using System;
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
			InitWindow("Select class in file", viewManager, showOkButton: true, showNewButton: true);
			_classFile = classFile;
			_className = className;
			OnClassSelected = result;

			_textFile = new ViewText();
			AddComponent(_textFile);
			_textFile.SetParams(200, 5, 500, 20, "File");
			_textFile.CreateSplitedTextAuto(Color.Gray, null, "Unknown");
			_textFile.CalculateTextPositions();
		}

		protected override void InitButtonNew(ViewButton btnNew)
		{
			base.InitButtonNew(btnNew);
			btnNew.Caption = "SelectFile";
			btnNew.Hint = "Выберите файл";
		}

		protected override void NewCommand()
		{
			тут. получить список файлов и вызвать SelectStringWindow
				потом получить список классов (всех) и вернуть класс и файл
		}
	}
}