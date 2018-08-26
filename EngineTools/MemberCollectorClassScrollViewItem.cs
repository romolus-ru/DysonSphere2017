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
using System.Collections;
using System.Collections.Generic;
using Engine.Helpers;

namespace EngineTools
{
	/// <summary>
	/// Выводит свойство связанное с классом CollectorClass
	/// </summary>
	public class MemberCollectorClassScrollViewItem<T> : MemberBaseScrollView<T> where T : EventBase
	{
		private long _value;
		private Type _collectClassType;
		private ViewText TextClass;
		private ViewText TextFile;
		private MemberInfo _memberInfo;
		private ViewManager _viewManager;
		private DataSupportBase _dataSupport;

		public MemberCollectorClassScrollViewItem(ViewManager viewManager, DataSupportBase dataSupport, Type typeForCollectClass)
		{
			_collectClassType = typeForCollectClass;
			_viewManager = viewManager;
			_dataSupport = dataSupport;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			var btnSelect = new ViewButton();
			AddComponent(btnSelect);
			btnSelect.InitButton(SelectCollectorClass, "Select", "Выбрать", Keys.None);
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

		private void SelectCollectorClass()
		{
			var type1 = _collectClassType;
			var type2 = typeof(CollectorClassSelectWindow<>);
			var windowType = type2.MakeGenericType(new Type[] { type1 });
			var window = Activator.CreateInstance(windowType);
			IEnumerable<MethodInfo> ms = windowType.GetMethods().Where(mv => mv.Name == "InitWindow");
			MethodInfo m = null;
			foreach (var method in ms) {
				var parameters= method.GetParameters();
				if (parameters.Length < 4) continue;
				var p2 = parameters[1];
				if (p2.Name == "dataSupport")
					m = method;
			}
			if (m != null) {
				//.InitWindow(_viewManager, _dataSupport, null, null);
				//ViewManager viewManager, DataSupportBase dataSupport, Action<CollectClass> selectedCollectorClass, Action cancel
				object[] parametersArray = { _viewManager, _dataSupport, (Action<CollectClass>)SelectCollectorClass, null };
				m.Invoke(window, parametersArray);
			} else
				StateEngine.Log.AddLog("InitWindow in CollectorClassSelectWindow not found");
		}

		private void SelectCollectorClass(CollectClass collectClass)
		{
			if (collectClass == null) {
				_value = Constants.UnknownValue;
				SetupViewValue(null, null);
			} else {
				_value = collectClass.Id;
				SetupViewValue(collectClass.ClassName, collectClass.FileName);
			}
		}

		public override void InitValueEditor(T obj, MemberInfo memberInfo)
		{
			_memberInfo = memberInfo;
			var value = (_memberInfo as PropertyInfo).GetValue(obj);
			if (value != null && (value is ValueType)) {
				_value = (long)value;
				if (_value <= 0) {
					SetupViewValue("value not set", null);
					return;
				}

				var type = StateEngine.Collector.GetObjectType((int)_value);
				var appPath = StateEngine.AppPath;
				var shortFileName = type.Assembly.Location.Substring(appPath.Length);
				SetupViewValue(type.FullName, shortFileName);
			} else
				_value = Constants.UnknownValue;
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
			PropertyInfo pi = _memberInfo as PropertyInfo;
			pi.SetValue(obj, _value);
		}
	}
}