using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Engine
{
	/// <summary>
	/// Класс для получения координат курсора и нажатых клавиш
	/// </summary>
	/// <remarks>
	/// Предоставляет возможность узнать координаты курсора и нажатых клавиш
	/// режимы получения нужной информации
	/// 1 Постоянная обработка - обычный игровой режим, пока кнопка нажата генерируется событие нажатия
	/// 2 Обработка с паузой между нажатиями - ввод текста
	/// 3 Залипание - отправка события только после отпускания кнопки (если будут нажаты ещё другие кнопки, то событие не отправится)
	/// Так же будут обработчики дополнительных режимов - перемещение и модальный
	/// двойной клик можно сделать как helper. возможно перемещение тоже будет перенесено в хелпер
	/// </remarks> 
	public class Input
	{
		private Dictionary<List<Keys>, Action> _keyAction = new Dictionary<List<Keys>, Action>();
		private Stack<Dictionary<List<Keys>, Action>> _keyActionStack = new Stack<Dictionary<List<Keys>, Action>>();

		private Dictionary<List<Keys>, Action> _keyActionDouble = new Dictionary<List<Keys>, Action>();
		private Stack<Dictionary<List<Keys>, Action>> _keyActionDoubleStack = new Stack<Dictionary<List<Keys>, Action>>();

		private Dictionary<List<Keys>, Action> _keyActionSticked = new Dictionary<List<Keys>, Action>();
		private Stack<Dictionary<List<Keys>, Action>> _keyActionStickedStack = new Stack<Dictionary<List<Keys>, Action>>();
		private List<List<Keys>> _listKeySticked = new List<List<Keys>>();

		private Action<string> _setStringInput;
		private Stack<Action<string>> _setStringInputStack = new Stack<Action<string>>();
		private Dictionary<List<Keys>, Action> _keyActionPaused = new Dictionary<List<Keys>, Action>();
		private Stack<Dictionary<List<Keys>, Action>> _keyActionPausedStack = new Stack<Dictionary<List<Keys>, Action>>();
		private List<InputKeyStatePause> _keyPausedStates = new List<InputKeyStatePause>();

		private Action<int, int> _cursorMovedSystem;
		private Action<int, int> _cursorMoved;
		private Stack<Action<int, int>> _cursorMovedStack = new Stack<Action<int, int>>();

		private DateTime _pause = DateTime.Now;
		private int _pauseState = Constants.PauseStateNone;
		private string _oldInputValue = null;
		private bool _isAnyKeyPressed;
		private bool ModalStateChanged;
		private int _cursorX;
		private int _cursorY;
		private int _windowPosX;
		private int _windowPosY;
		private bool _initWinPos = true;
		public Func<Point> OnGetWindowPos;

		/// <summary>
		/// Кнопки в комбинациях, которые можно держать нажатыми, но комбинация будет считаться сработавшей
		/// </summary>
		private List<Keys> shiftKeys = new List<Keys> {
			Keys.ControlKey, Keys.LControlKey, Keys.LMenu, Keys.LShiftKey, Keys.LWin,
			Keys.Menu, Keys.RControlKey, Keys.RMenu, Keys.RShiftKey, Keys.RWin, Keys.ShiftKey
		};

		/// <summary>
		/// Кнопки для которых разрешено событие двойного нажатия
		/// </summary>
		private List<Keys> clickKeys = new List<Keys> {
			Keys.LButton, Keys.RButton, Keys.MButton,
		};

		public Input() { }

		public void ProcessInput()
		{
			if (_initWinPos) InitWindowPos();
			ModalStateChanged = false;
			_isAnyKeyPressed = UpdateKeyboardState();
			var curNew = UpdateCursorState();
			if (curNew && _cursorMoved != null) _cursorMoved(CursorX, CursorY);
			if (curNew && _cursorMovedSystem != null) _cursorMovedSystem(CursorX, CursorY);

			GetInput();// обработка обычных (игровых) нажатий
			GetInputPaused();// обработка управляющих кнопок и разных комбинаций при вводе текста
			GetInputStringPaused();// обработка ввода строки
			GetInputSticked();// обработка отпускания кнопок
		}

		private void InitWindowPos()
		{
			if (OnGetWindowPos != null) {
				var pos = OnGetWindowPos();
				_windowPosX = pos.X;
				_windowPosY = pos.Y;
			}
			_initWinPos = false;
		}

		/// <summary>
		/// Добавить обычный получатель события
		/// </summary>
		/// <param name="action"></param>
		/// <param name="keyCombination"></param>
		public void AddKeyAction(Action action, params Keys[] keyCombination)
		{
			AddKeyActionDict(_keyAction, action, keyCombination);
		}

		/// <summary>
		/// Удалить обычный получатель события
		/// </summary>
		/// <param name="action"></param>
		/// <param name="keyCombination"></param>
		public void RemoveKeyAction(Action action, params Keys[] keyCombination)
		{
			RemoveKeyActionDict(_keyAction, action, keyCombination);
		}

		/// <summary>
		/// Добавить получатель события двойного клика
		/// </summary>
		/// <param name="action"></param>
		/// <param name="keyCombination"></param>
		public void AddKeyActionDouble(Action action, params Keys[] keyCombination)
		{
			if (keyCombination.Length != 1 && !clickKeys.Contains(keyCombination[0])) return;
			AddKeyActionDict(_keyActionDouble, action, keyCombination);
		}

		/// <summary>
		/// Удалить получатель события двойного клика
		/// </summary>
		/// <param name="action"></param>
		/// <param name="keyCombination"></param>
		public void RemoveKeyActionDouble(Action action, params Keys[] keyCombination)
		{
			RemoveKeyActionDict(_keyActionDouble, action, keyCombination);
		}

		/// <summary>
		/// Добавить обычный получатель события курсора
		/// </summary>
		/// <param name="action"></param>
		/// <param name="isSystem"></param>
		public void AddCursorAction(Action<int, int> action, bool isSystem = false)
		{
			if (isSystem)
				_cursorMovedSystem += action;
			else
				_cursorMoved += action;
		}

		/// <summary>
		/// Удалить обычный получатель события курсора
		/// </summary>
		/// <param name="action"></param>
		public void RemoveCursorAction(Action<int, int> action)
		{
			_cursorMoved -= action;
			_cursorMovedSystem -= action;
		}

		/// <summary>
		/// Обработать пользовательский ввод и разослать нужные события
		/// </summary>
		private void GetInput()
		{
			foreach (var keyComb in _keyAction.Keys) {
				if (ModalStateChanged) return;
				var keyFounded = true;
				foreach (var k1 in keyComb) {
					if (IsKeyPressed(k1)) continue;
					keyFounded = false;
					break;
				}
				if (keyFounded)
					RunEachAction(_keyAction[keyComb]);
			}
		}

		/// <summary>
		/// Добавить получатель события с паузой
		/// </summary>
		/// <param name="action"></param>
		/// <param name="keyCombination"></param>
		public void AddKeyActionPaused(Action action, params Keys[] keyCombination)
		{
			AddKeyActionDict(_keyActionPaused, action, keyCombination);
		}

		/// <summary>
		/// Удалить получатель события с паузой
		/// </summary>
		/// <param name="action"></param>
		/// <param name="keyCombination"></param>
		public void RemoveKeyActionPaused(Action action, params Keys[] keyCombination)
		{
			RemoveKeyActionDict(_keyActionPaused, action, keyCombination);
		}

		/// <summary>
		/// Обработать пользовательский ввод с паузой между повторениями и разослать нужные события
		/// </summary>
		private void GetInputPaused()
		{
			var listPressedCombs = new List<List<Keys>>();
			foreach (var keyComb in _keyActionPaused.Keys) {
				if (ModalStateChanged) return;
				// комбинация кнопок нажата - сохраняем
				if (IsСombPressed(keyComb)) listPressedCombs.Add(keyComb);
			}

			// очищаем - никакие кнопки не нажаты
			if (listPressedCombs.Count == 0) {
				_keyPausedStates.Clear();
				return;
			}

			// ищем те которые есть в списке _keyPausedStates и которых нету в списке listPressedCombs и удаляем
			var unPressed = _keyPausedStates.Where(a => !listPressedCombs.Contains(a.KeyCombination));
			unPressed.ToList().ForEach((del) => { _keyPausedStates.Remove(del); });

			// добавляем те которых еще нету и запускаем их первый раз
			var now = DateTime.Now;
			foreach (var key in listPressedCombs) {
				var keyState = _keyPausedStates.Where(a => a.KeyCombination == key).FirstOrDefault();
				if (keyState == null) {
					_keyPausedStates.Add(new InputKeyStatePause
					{
						KeyCombination = key,
						PauseState = Constants.PauseStatePauseFirst,
						StateLimit = DateTime.Now.AddMilliseconds(Settings.KeyBoardRepeatPauseFirst)
					}
					);
					_keyActionPaused[key]();
				}
			}

			// меняем статус, обновляем таймер и запускаем
			var startAction = false;
			foreach (var keyState in _keyPausedStates) {
				if (keyState.PauseState == Constants.PauseStatePauseFirst) {
					if (keyState.StateLimit > now) continue;
					keyState.PauseState = Constants.PauseStatePause;
					startAction = true;
				}
				if (keyState.PauseState == Constants.PauseStatePause) {
					if (keyState.StateLimit < now) {
						keyState.StateLimit = now.AddMilliseconds(Settings.KeyBoardRepeatPause);
						startAction = true;
					}
				}
				if (startAction) {
					_keyActionPaused[keyState.KeyCombination]();
					startAction = false;
				}
			}
		}

		/// <summary>
		/// Добавить получатель строки нажатых кнопок
		/// </summary>
		/// <param name="action"></param>
		public void AddInputStringAction(Action<string> action)
		{
			_setStringInput += action;
		}

		/// <summary>
		/// Удалить получатель строки нажатых кнопок
		/// </summary>
		/// <param name="action"></param>
		public void RemoveInputStringAction(Action<string> action)
		{
			_setStringInput -= action;
		}

		/// <summary>
		/// Обработать пользовательский ввод и разослать нужные события с паузой
		/// </summary>
		private void GetInputStringPaused()
		{
			if (ModalStateChanged) return;
			if (_setStringInput == null) return;
			if (_keyPausedStates.Count > 0) return;// есть нажатые управляющие кнопки (например ctrl+C) - значит уже вводим не текст
			var keys = KeysToUnicode();
			if (keys == string.Empty) {
				_oldInputValue = keys;
				return;
			}
			DateTime now = DateTime.Now;
			if (_oldInputValue != keys) {
				_pauseState = Constants.PauseStatePauseFirst;
				_setStringInput(keys);
				_oldInputValue = keys;
				_pause = now.AddMilliseconds(Settings.KeyBoardRepeatPauseFirst);
				return;
			}
			if (now < _pause) return;
			if (_pauseState == Constants.PauseStatePauseFirst) {
				_pauseState = Constants.PauseStatePause;
			}

			if (_pauseState == Constants.PauseStatePause) {
				_setStringInput(keys);
				_pause = now.AddMilliseconds(Settings.KeyBoardRepeatPause);
			}
		}



		/// <summary>
		/// Добавить получатель события отпускания кнопки
		/// </summary>
		/// <param name="action"></param>
		/// <param name="keyCombination"></param>
		public void AddKeyActionSticked(Action action, params Keys[] keyCombination)
		{
			AddKeyActionDict(_keyActionSticked, action, keyCombination);
		}

		/// <summary>
		/// Удалить получатель события отпускания кнопки
		/// </summary>
		/// <param name="action"></param>
		/// <param name="keyCombination"></param>
		public void RemoveKeyActionSticked(Action action, params Keys[] keyCombination)
		{
			RemoveKeyActionDict(_keyActionSticked, action, keyCombination);
		}

		/// <summary>
		/// Обработать пользовательский ввод и разослать нужные события после отпускания кнопок
		/// </summary>
		private void GetInputSticked()
		{
			Action start = null;
			foreach (var keyComb in _keyActionSticked.Keys) {
				if (ModalStateChanged) return;
				if (IsСombPressed(keyComb)) {
					if (!_listKeySticked.Contains(keyComb))
						_listKeySticked.Add(keyComb);// save
				} else {
					if (_listKeySticked.Contains(keyComb)) {
						_listKeySticked.Remove(keyComb);// remove
						if (!_isAnyKeyPressed || IsMainKeysUnpressed(keyComb))
							start += _keyActionSticked[keyComb];// запустить событие если ничего больше не нажато
					}
				}
			}
			// TODO иногда могут запускаться события уже удаленных объектов
			start?.Invoke();
		}

		/// <summary>
		/// Проверяем нажата ли комбинация кнопок
		/// </summary>
		/// <param name="keyComb"></param>
		/// <returns></returns>
		private bool IsСombPressed(List<Keys> keyComb)
		{
			foreach (var k1 in keyComb) {
				if (!IsKeyPressed(k1)) return false;
			}
			return true;
		}

		/// <summary>
		/// Основные кнопки отпущены, а кнопки смещения могут остаться нажатыми
		/// </summary>
		private bool IsMainKeysUnpressed(List<Keys> keyComb)
		{
			var noShiftedKeysPressed = false;
			var shiftedKeysFound = false;
			foreach (var k1 in keyComb) {
				if (shiftKeys.Contains(k1)) { shiftedKeysFound = true; }
				else if (IsKeyPressed(k1)) noShiftedKeysPressed = true;
			}
			return (shiftedKeysFound && !noShiftedKeysPressed);
		}

		/// <summary>
		/// Проверить, нажата ли данная клавиша клавиатуры или мышки
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected virtual bool IsKeyPressed(Keys key) { return false; }

		/// <summary>
		/// Установить состояние кнопок клавиатуры и мыши
		/// </summary>
		/// <returns></returns>
		protected virtual bool UpdateKeyboardState() { return false; }

		/// <summary>
		/// Установить положение курсора
		/// </summary>
		/// <returns></returns>
		protected virtual bool UpdateCursorState() { return false; }

		/// <summary>
		/// Перемещаем все существующие обработчики в стек
		/// </summary>
		public void ModalStateStart()
		{
			ModalStateChanged = true;
			_keyActionStack.Push(_keyAction);
			_keyAction = new Dictionary<List<Keys>, Action>();

			_keyActionStickedStack.Push(_keyActionSticked);
			_keyActionSticked = new Dictionary<List<Keys>, Action>();
			_listKeySticked.Clear();

			_setStringInputStack.Push(_setStringInput);
			_setStringInput = null;
			_keyActionPausedStack.Push(_keyActionPaused);
			_keyActionPaused = new Dictionary<List<Keys>, Action>();
			_keyPausedStates.Clear();

			_cursorMovedStack.Push(_cursorMoved);
			_cursorMoved = null;
		}

		/// <summary>
		/// Восстанавливаем предыдущие обработчики
		/// </summary>
		public void ModalStateStop()
		{
			ModalStateChanged = true;
			if (_keyActionStack.Count == 0) throw new Exception("Модальный режим не запускался");
			_keyAction = _keyActionStack.Pop();

			if (_keyActionStickedStack.Count == 0) throw new Exception("Модальный режим не запускался");
			_keyActionSticked = _keyActionStickedStack.Pop();
			_listKeySticked.Clear();

			if (_setStringInputStack.Count == 0) throw new Exception("Модальный режим не запускался");
			_setStringInput = _setStringInputStack.Pop();
			if (_keyActionPausedStack.Count == 0) throw new Exception("Модальный режим не запускался");
			_keyActionPaused = _keyActionPausedStack.Pop();
			_keyPausedStates.Clear();

			if (_cursorMovedStack.Count == 0) throw new Exception("Модальный режим не запускался");
			_cursorMoved = _cursorMovedStack.Pop();

			CursorX = -1;
			CursorY = -1;
			ProcessInput();
		}

		private void AddKeyActionDict(Dictionary<List<Keys>, Action> dict, Action action, params Keys[] keyCombination)
		{
			var keyComb = new List<Keys>(keyCombination);
			var dictKey = GetDictKey(dict, keyComb);
			if (dictKey == null)
				dict.Add(keyComb, action);
			else
				dict[dictKey] += action;
		}

		private void RemoveKeyActionDict(Dictionary<List<Keys>, Action> dict, Action action, params Keys[] keyCombination)
		{
			var keyComb = new List<Keys>(keyCombination);
			var dictKey = GetDictKey(dict, keyComb);
			if (dictKey != null) {
				var dictValue = dict[dictKey];
				var countBefore = dictValue.GetInvocationList().Length;
				dict[dictKey] -= action;
				var countAfter = dict[dictKey]?.GetInvocationList()?.Length ?? 0;
				if (countAfter == countBefore) {
					// LOG "в словаре по ключу нету такого метода. возможно из-за модального режима"
				}
			} else {
				// LOG "в словаре не обнаружен ключ. возможно из-за модального режима"
			}
		}

		private List<Keys> GetDictKey(Dictionary<List<Keys>, Action> dict, List<Keys> keyComb)
		{
			List<Keys> dictKey = null;
			foreach (var k1 in dict.Keys) {
				if (k1.SequenceEqual(keyComb)) {
					dictKey = k1; break;
				}
			}
			return dictKey;
		}

		private void RunEachAction(MulticastDelegate actions)
		{
			foreach (var action in actions.GetInvocationList()) {
				if (ModalStateChanged) break;
				action.DynamicInvoke();
			}
		}

		private string GetKeyList(List<Keys> keys)
		{
			return string.Join(";", keys);
		}

		private void GetActionList(MulticastDelegate actions, List<string> result)
		{
			if (actions == null) return;
			foreach (var action in actions.GetInvocationList()) {
				var obj = action.Target as ViewComponent;
				var name = obj == null 
					? "(none)" 
					: obj.Name;
				result.Add("  " + name + "." + action.Method.Name);
			}
		}

		public List<string> GetActionsLists()
		{
			var ret = new List<string>();
			foreach (var item in _keyAction) {
				ret.Add(" keys = " + GetKeyList(item.Key));
				GetActionList(item.Value, ret);
			}
			ret.Add("KA stack " + _keyActionStack.Count);

			foreach (var item in _keyActionSticked) {
				ret.Add(" keys = " + GetKeyList(item.Key));
				GetActionList(item.Value, ret);
			}
			ret.Add("KASticked stack " + _keyActionStickedStack.Count);

			GetActionList(_setStringInput, ret);
			ret.Add("KAStringInput stack " + _setStringInputStack.Count);

			foreach (var item in _keyActionPaused) {
				ret.Add(" keys = " + GetKeyList(item.Key));
				GetActionList(item.Value, ret);
			}
			ret.Add("KAPaused stack " + _keyActionPausedStack.Count);

			GetActionList(_cursorMoved, ret);
			ret.Add("KACursorMoved stack " + _cursorMovedStack.Count);

			GetActionList(_cursorMovedSystem, ret);
			ret.Add("KACursorMovedSystem stack nope");

			return ret;
		}

		public string GetCurrentKeysPressed()
		{
			var list = new List<Keys>();
			var keyComb = new List<Keys> { Keys.LMenu, Keys.X };
			var keyFounded = true;
			var str1 = "";
			/*foreach (var k1 in keyComb) {
				str1 += " k " + k1 + " p=" + IsKeyPressed(k1) + " s=" + shiftKeys.Contains(k1) + " l=" + _listKeySticked.Contains(keyComb) 
					+ " r=" + (shiftKeys.Contains(k1) && _listKeySticked.Contains(keyComb));
				if (IsKeyPressed(k1)) continue;
				if (shiftKeys.Contains(k1) && _listKeySticked.Contains(keyComb)) continue;
				keyFounded = false;
			}*/

			var onlyShiftedKeysPressed = true;
			var shiftedKeysPresentInCombination = false;
			foreach (var k1 in keyComb) {
				if (shiftKeys.Contains(k1)) {
					shiftedKeysPresentInCombination = true; continue;
				}
				if (IsKeyPressed(k1)) onlyShiftedKeysPressed = false;
			}
			if (shiftedKeysPresentInCombination)
				keyFounded = onlyShiftedKeysPressed;
			str1 += " sk=" + shiftedKeysPresentInCombination + " onlys=" + onlyShiftedKeysPressed;



			for (int i = 0; i < 255; i++) {
				var key = (Keys)i;
				if (IsKeyPressed(key)) list.Add(key);
			}
			var ret = str1 + " " + string.Join(" ", list);
			return ret;
		}

		/// <summary>
		/// Координата курсора X. Клиентская
		/// </summary>
		public int CursorX { get { return _cursorX - _windowPosX; } protected set { _cursorX = value; } }

		/// <summary>
		/// Координата курсора Y. Клиентская
		/// </summary>
		public int CursorY { get { return _cursorY - _windowPosY; } protected set { _cursorY = value; } }

		/// <summary>
		/// Преобразовывает код нажатой клавиши на клавиатуре в код с учётом текущей раскладки клавиатуры
		/// </summary>
		/// <returns></returns>
		public virtual string KeysToUnicode()
		{
			return string.Empty;
		}

	}
}