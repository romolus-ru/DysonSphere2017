using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine
{
	/// <summary>
	/// Класс для получения координат курсора и нажатых клавиш
	/// </summary>
	/// <remarks>
	/// Предоставляет возможность узнать координаты курсора и нажатых клавиш
	/// Сложный как оказалось класс. Использует VisualizationProvider для 
	/// получения координат мыши и вращения колёсика
	/// </remarks> 
	public class Input
	{
		public Dictionary<List<Keys>, Action> _keyAction = new Dictionary<List<Keys>, Action>();
		public Action<int, int> _cursorMoved = (a, b) => { };

		public void AddKeyAction(Action action, params Keys[] keyCombination)
		{
			var keyComb = new List<Keys>(keyCombination);
			List<Keys> dictKey = null;
			foreach (var k1 in _keyAction.Keys) {
				if (k1.SequenceEqual(keyComb)) {
					dictKey = k1;break;
				}
			}
			if (dictKey==null)
				_keyAction.Add(keyComb, action);
			else
				_keyAction[dictKey] += action;
		}

		public void AddCursorAction(Action<int,int> action)
		{
			_cursorMoved += action;
		}

		/// <summary>
		/// Обработать пользовательский ввод и разослать нужные события
		/// </summary>
		public void GetInput()
		{
			var keyNew = SetKeyboard();
			var curNew = SetCursor();
			if (curNew) _cursorMoved(CursorX, CursorY);
			foreach (var keyComb in _keyAction.Keys) {
				var keyFounded = true;
				foreach (var k1 in keyComb) {
					if (IsKeyPressed(k1)) continue;
					keyFounded = false;
					break;
				}
				// кнопки есть - запускаем действие
				if (keyFounded) _keyAction[keyComb]();
			}
		}


		/// <summary>
		/// Проверить, нажата ли данная клавиша клавиатуры или мышки
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public virtual bool IsKeyPressed(Keys key) { return false; }

		/// <summary>
		/// Установить состояние кнопок клавиатуры и мыши
		/// </summary>
		/// <returns></returns>
		protected virtual bool SetKeyboard() { return false; }

		/// <summary>
		/// Установить положение курсора
		/// </summary>
		/// <returns></returns>
		protected virtual bool SetCursor() { return false; }

		public void ModalStart(ViewComponent view) {
			// TODO запускаем модальный режим для компонента
		}
		public void ModalStop(ViewComponent view) {
			// TODO останавливаем модальный режим для компонента
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

		public Input() { }

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
			var keyNew = SetKeyboard(); // устанавливаем состояния устройств ввода
			if (!keyNew) {// если кнопка не нажата
				if (!_lastKeyPressed) {// если до этого что то нажимали
					_lastKeyPressed = true; // запоминаем что теперь это было последнее нажатие, пользователь отпустил все кнопки
					keyNew = true; // устанавливаем флаг принудительно, что бы отправить событие без нажатий
				}
			}
			else {
				_lastKeyPressed = false;
			} // фиксируем что пользователь ещё нажимает какую то кнопку

			var curNew = SetCursor();

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
		public virtual String KeysToUnicode()
		{
			return String.Empty;
		}
	}
}