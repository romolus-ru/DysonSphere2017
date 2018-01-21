//тут 
//onmouseOver должна корректно проверять кто over - скорее всего надо возвращать есть ли уже over и если есть - дальше уже не смотреть у внешних объектов
//добавить окну 2 кнопки, добавить элемент ввода и заблокировать остальные события (скорее всего сделать метод у ViewMAnager что бы активировать модальный режим
//	событие focus - элемент может быть неактивен, но при клике на нем он получает фокус и принимает/подписывается на события ввода текста
//	или при клике вне его области оно теряет фокус (но наверно не теряет если не выбран другой элемент ввода)
//	добавить флаг focused - и чтоб компоненты могли сами следить за своим  состоянием

	//сделать чтоб при нажатии кнопки все остальные события блокировались, запускалось отдельное модальное окно с двумя кнопками
	//и после закрытия кнопка получала ту надпись которую ввели

using Engine.Visualization;
using System;
using System.Collections.Generic;
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
	/// </remarks> 
	public class Input
	{
		private Dictionary<List<Keys>, Action> _keyAction = new Dictionary<List<Keys>, Action>();
		private Stack<Dictionary<List<Keys>, Action>> _keyActionStack = new Stack<Dictionary<List<Keys>, Action>>();

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

		public Input() { }

		public void ProcessInput()
		{
			_isAnyKeyPressed = UpdateKeyboardState();
			var curNew = UpdateCursorState();
			if (curNew && _cursorMoved != null) _cursorMoved(CursorX, CursorY);
			if (curNew && _cursorMovedSystem != null) _cursorMovedSystem(CursorX, CursorY);

			GetInput();// обработка обычных (игровых) нажатий
			GetInputPaused();// обработка управляющих кнопок и разных комбинаций при вводе текста
			GetInputStringPaused();// обработка ввода строки
			GetInputSticked();// обработка отпускания кнопок
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
				var keyFounded = true;
				foreach (var k1 in keyComb) {
					if (IsKeyPressed(k1)) continue;
					keyFounded = false;
					break;
				}
				// кнопки есть - запускаем действие
				if (keyFounded)
					_keyAction[keyComb]();
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
				var keyFounded = true;
				foreach (var k1 in keyComb) {
					if (IsKeyPressed(k1)) continue;
					keyFounded = false;
					break;
				}
				// кнопки есть - сохраняем
				if (keyFounded) listPressedCombs.Add(keyComb);
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
				var keyFounded = true;
				foreach (var k1 in keyComb) {
					if (IsKeyPressed(k1)) continue;
					keyFounded = false;
					break;
				}
				if (keyFounded) {
					if (!_listKeySticked.Contains(keyComb))
						_listKeySticked.Add(keyComb);// save
				} else {
					if (_listKeySticked.Contains(keyComb)) {
						_listKeySticked.Remove(keyComb);// remove														
						if (!_isAnyKeyPressed)
							start += _keyActionSticked[keyComb];// запустить событие если ничего больше не нажато
					}
				}
			}
			start?.Invoke();
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
				var countAfter= dictValue.GetInvocationList().Length;
				if (countAfter == countBefore) {
					// LOG "в словаре по ключу нету такого метода. возомжно из-за модального режима"
				}
			} else {
				// LOG "в словаре не обнаружен ключ. возможно из-за модального режима"
			}
			ShrinkDict(dict);
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

		private void ShrinkDict(Dictionary<List<Keys>, Action> dict)
		{
			var keys = dict.Keys.ToList();
			foreach (var k1 in keys) {
				if (dict[k1].GetInvocationList().Length == 0)
					dict.Remove(k1);
			}
		}

		/////////// всё что ниже - старое. будет переноситься выше по мере необходимости. и/или переделываться


		/// <summary>
		/// Координата курсора X. Клиентская
		/// </summary>
		public int CursorX { get; protected set; }

		/// <summary>
		/// Координата курсора Y. Клиентская
		/// </summary>
		public int CursorY { get; protected set; }

		/// <summary>
		/// Смещение колеса мыши
		/// </summary>
		public int CursorDelta { get; protected set; }

		private int _cursorDeltaState = 0;
		/// <summary>
		/// флаг очистки событий клавиатуры. работает некоторое время, потом отменяется
		/// </summary>
		public bool KeyboardCleared { get; protected set; }

		/// <summary>
		/// Для определения отпускания кнопки. кнопки не нажаты, но перед этим были нажаты и генерируется событие, что что то нажато
		/// </summary>
		/// <remarks></remarks>
		private bool _lastKeyPressed = false;

		/// <summary>
		/// Завершение блокировки клавиатуры
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void KeyboardClearEndEH(object sender, EventArgs e)
		{
			KeyboardCleared = false;
		}

		/// <summary>
		/// Заблокировать на небольшое время события от клавиатуры
		/// </summary>
		/// <remarks>Нужно часто при нажатии на кнопки мыши, что бы событие дальше не распространялось некоторое время</remarks>
		public void KeyboardClear(object sender, EventArgs e)
		{
			KeyboardCleared = true;
		}

		/// <summary>
		/// Получаем вращение колеса мыши
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="integerEventArgs"></param>
		private void CursorDeltaEH(object sender, EventArgs integerEventArgs)
		{
			// сохраняем значение вращения колеса
			//CursorDelta = integerEventArgs.Value;
			_cursorDeltaState = 1;
		}

		/// <summary>
		/// Передаём координаты курсора по запросу
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="pointEventArgs"></param>
		private void CursorGetEH(object sender, EventArgs pointEventArgs)
		{
			// меняем точку на текущую, независимо от того что передали
			//pointEventArgs.SetCoord(new Point(CursorX, CursorY));
		}

		/// <summary>
		/// получить информацию о клавишах и мышке
		/// </summary>
		public virtual void GetInputOld()
		{
			var keyNew = UpdateKeyboardState(); // устанавливаем состояния устройств ввода
			if (!keyNew) {// если кнопка не нажата
				if (!_lastKeyPressed) {// если до этого что то нажимали
					_lastKeyPressed = true; // запоминаем что теперь это было последнее нажатие, пользователь отпустил все кнопки
					keyNew = true; // устанавливаем флаг принудительно, что бы отправить событие без нажатий
				}
			} else {
				_lastKeyPressed = false;
			} // фиксируем что пользователь ещё нажимает какую то кнопку

			var curNew = UpdateCursorState();

			if (_cursorDeltaState != 0) {
				if (_cursorDeltaState == 2) {// состояние 2
					_cursorDeltaState = 0;
					CursorDelta = 0; // сбрасываем всё
				}
				if (_cursorDeltaState == 1) {// ничего не делаем, но переходим в состояние 2
					_cursorDeltaState = 2;
					// активируем событие от клавиатуры, 
					//чтоб обработчики получили вращение колеса
					keyNew = true;
				}
			}

			if (curNew) {// запускаем событие обработки изменения положения курсора
						 //Controller.StartEvent("Cursor", this, PointEventArgs.Set(CursorX, CursorY));
			}

			if (keyNew) {// запускаем событие обработки клавиатуры и мышки
				if (!KeyboardCleared) {
					//Controller.StartEvent("Keyboard", this, InputEventArgs.SetInput(this));
				}
			}
		}






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