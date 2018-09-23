using Engine;
using Engine.Data;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EngineTools
{
	/// <summary>
	/// Просмотр джисон строки, содержащей записи класса переданного типа
	/// </summary>
	public class ViewJsonWindow<T> : FilteredScrollViewWindow where T : class, new()
	{
		private Action<MiniGamesInfos, string> _saveData;
		private Action _cancel;
		private MiniGamesInfos _miniGameInfo;
		private List<T> _values = new List<T>();

		public void InitWindow(ViewManager viewManager, MiniGames miniGame, MiniGamesInfos miniGameInfo, Action<MiniGamesInfos, string> saveData, Action cancel)
		{
			_saveData = saveData;
			_cancel = cancel;
			_miniGameInfo = miniGameInfo;

			InitData(_miniGameInfo);
			InitWindow("Редактирование " + miniGame.Name + " - " + miniGameInfo.Section, viewManager, true);
		}

		private void InitData(MiniGamesInfos miniGameInfo)
		{
			var values = JsonConvert.DeserializeObject<List<T>>(miniGameInfo.Values);
			if (values != null)
				_values = values;
			else
				StateEngine.Log.AddLog("section " + miniGameInfo.Section + " empty");
		}

		protected override void InitScrollItems()
		{
			ViewScroll.ClearItems();
			foreach (var value in _values) {
				AddScrollItem(_values.IndexOf(value), value);
			}
			//var type1 = typeof(EventBase);
			//var type2 = typeof(EventBaseRowScrollView<>);
			//var type3 = type2.MakeGenericType(new Type[] { type1 });
			//var scrollItem = Activator.CreateInstance(type3);
		}

		private void AddScrollItem(int i, T value)
		{
			var type1 = typeof(T);
			var type2 = typeof(ViewJsonScrollViewItem<>);
			var type3 = type2.MakeGenericType(new Type[] { type1 });
			var scrollItem = Activator.CreateInstance(type3, ViewManager, value, null, (Action<ScrollItem, T>)DeleteItem) as ScrollItem;
			if (scrollItem == null) {
				StateEngine.Log.AddLog("ViewJsonWindow scroll item is null");
				return;
			}

			ViewScroll.AddComponent(scrollItem);
			scrollItem.SetParams(10, i * 200 + 10, 950, 200, value.ToString());
			//scrollItem.OnDelete += EditMiniGame;
			//scrollItem.OnItemChanged += SelectSection;
		}

		private void DeleteItem(ScrollItem si, T obj)
		{
			_values.Remove(obj);
			InitScrollItems();
		}

		protected override void NewCommand()
		{
			var obj = new T();
			ViewJsonScrollViewItem<T>.EditRow(ViewManager, obj, AddNewItem);
		}

		private void AddNewItem(T obj)
		{
			_values.Add(obj);
			AddScrollItem(_values.IndexOf(obj), obj);
		}

		protected override void OkCommand()
		{
			// сохранить в джисон, сохранить в объекте и отправить объект на сохранение в базу
			//var values = JsonConvert.DeserializeObject<List<T>>(miniGameInfo.Values);
			string str = JsonConvert.SerializeObject(_values);
			_saveData?.Invoke(_miniGameInfo, str);
		}

		protected override void CloseWindow()
		{
			base.CloseWindow();
			_saveData = null;
			_cancel = null;
			_miniGameInfo = null;
		}

		protected override void CancelCommand()
		{
			_cancel?.Invoke();
		}
	}
}