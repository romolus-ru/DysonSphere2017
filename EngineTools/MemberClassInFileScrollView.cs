using Engine.EventSystem.Event;
using System;
using System.Reflection;
using Engine.Visualization.Text;
using Engine.Visualization;
using Engine;
using System.Drawing;
using System.Windows.Forms;
using Engine.Data;
using System.Linq;

namespace EngineTools
{
	/// <summary>
	/// Выводит свойство для просмотра и редактирования имени файла и класса
	/// </summary>
	public class MemberClassInFileScrollView<T> : MemberBaseScrollView<T> where T : EventBase
	{
		private ViewText TextClass;
		private ViewText TextFile;
		private ViewManager _viewManager;
		private string _className;
		private string _classFile;
		private PropertyInfo _classNameProperty;
		private PropertyInfo _classFileProperty;

		public MemberClassInFileScrollView(ViewManager viewManager)
		{
			_viewManager = viewManager;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			var btnSelect = new ViewButton();
			AddComponent(btnSelect);
			btnSelect.InitButton(SelectClassInFile, "Select", "Выбрать", Keys.None);
			btnSelect.SetParams(90, 10, 120, 30, "Выбрать");
			btnSelect.InitTexture("textRB", "textRB");

			TextClass = new ViewText();
			AddComponent(TextClass);
			TextClass.SetParams(250, 5, 500, 20, "Class");
			TextClass.CreateSplitedTextAuto(Color.Gray, null, "Unknown");
			TextClass.CalculateTextPositions();

			TextFile = new ViewText();
			AddComponent(TextFile);
			TextFile.SetParams(250, 25, 500, 20, "File");
			TextFile.CreateSplitedTextAuto(Color.Gray, null, "Unknown");
			TextFile.CalculateTextPositions();
		}

		private void SelectClassInFile()
		{
			new SelectClassInFileWindow().InitWindow(_viewManager, _classFile, _className, SelectClassInFileResult);
		}

		private void SelectClassInFileResult(string classFile, string className)
		{
			_classFile = classFile;
			_className = className;
			SetupViewValue(_className, _classFile);
		}

		const string classFilePropName = "ClassFile";
		const string classNamePropName = "ClassName";

		public override void InitValueEditor(T obj, MemberInfo memberInfo)
		{
			var mis = obj.GetType().GetMembers().Where(
				mi => mi.MemberType == MemberTypes.Property &&
				(mi.Name == classFilePropName || mi.Name == classNamePropName));
			foreach (PropertyInfo mi in mis) {
				if (mi.Name == classNamePropName) {
					_className = mi.GetValue(obj) as string;
					_classNameProperty = mi;
				}
				if (mi.Name == classFilePropName) {
					_classFile = mi.GetValue(obj) as string;
					_classFileProperty = mi;
				}
			}
			SetupViewValue(_className, _classFile);
		}

		private void SetupViewValue(string name, string file)
		{
			TextClass.ClearTexts();
			TextFile.ClearTexts();
			if (string.IsNullOrEmpty(name)) {
				TextClass.CreateSplitedTextAuto(Color.Red, null, "value not set");
			} else {
				TextClass.CreateSplitedTextAuto(Color.White, null, name);
				TextFile.CreateSplitedTextAuto(Color.Yellow, null, file);
			}
			TextClass.CalculateTextPositions();
			TextFile.CalculateTextPositions();
		}

		/// <summary>
		/// Установить значение поля объекта
		/// </summary>
		/// <param name="obj"></param>
		public override void SetValue(T obj)
		{
			if (!string.IsNullOrEmpty(_className))
				_classNameProperty.SetValue(obj, _className);
			if (!string.IsNullOrEmpty(_classFile))
				_classFileProperty.SetValue(obj, _classFile);
		}
	}
}