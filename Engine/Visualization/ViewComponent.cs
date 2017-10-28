using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Компонент управления
	/// </summary>
	public class ViewComponent :ViewObject
	{
		#region Основные переменные

		public ViewComponent Parent { get; protected set; }

		protected VisualizationProvider VisualizationProvider;
		
		/// <summary>
		/// Координата X объекта
		/// </summary>
		public int X { get; protected set; }

		/// <summary>
		/// Координата Y объекта
		/// </summary>
		public int Y { get; protected set; }

		/// <summary>
		/// Координата Z объекта
		/// </summary>
		public int Z { get; protected set; }

		public bool CanDraw { get; private set; }
		protected bool IsModal;
		protected bool IsDraging;

		/// <summary>
		/// Флаг, находится ли курсор над компонентом
		/// </summary>
		public bool CursorOver {
			get;
			set;
		}

		/// <summary>
		/// Вложенные компоненты
		/// </summary>
		protected List<ViewComponent> Components = new List<ViewComponent>();

		/// <summary>
		/// Покинул ли курсор пределы объекта (что бы лишний раз не сбрасывать состояние)
		/// </summary>
		protected Boolean CursorOverOffed;

		#endregion

		#region Размеры объекта

		/// <summary>
		/// Высота объекта
		/// </summary>
		public int Height { get; protected set; }

		/// <summary>
		/// Ширина объекта
		/// </summary>
		public int Width { get; protected set; }

		/// <summary>
		/// Установить размеры объекта
		/// </summary>
		/// <param name="width">Ширина</param>
		/// <param name="height">Высота</param>
		public void SetSize(int width, int height)
		{
			Height = height;
			Width = width;
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Для блокировки дополнительных вызовов dispose
		/// </summary>
		private bool _disposed = !true;

		public virtual void Dispose()
		{
			if (!_disposed) {
				foreach (var component in Components) { component.Dispose(); }
				Components.Clear();
				Components = null;
				_disposed = true;
				VisualizationProvider = null;
				X = -99000;
				Y = -99000;
				Width = 0;
				Height = 0;
			}
		}

		/// <summary>
		/// Деструктор
		/// </summary>
		~ViewComponent()
		{
			Dispose();
		}

		#endregion

		public ViewComponent() { }

		/// <summary>
		/// Инициализация объекта для текущей визуализации (размер экрана и т.п.)
		/// </summary>
		/// <param name="visualizationProvider"></param>
		/// <remarks>Отдельно потому что инициализация происходит при добавлении объекта к вышестоящему компоненту</remarks>
		public void Init(VisualizationProvider visualizationProvider)
		{
			VisualizationProvider = visualizationProvider;// сохраняем для будущего использования
			InitObject(VisualizationProvider);
		}

		/// <summary>
		/// переопределяемая инициализация объекта для текущей визуализации
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected virtual void InitObject(VisualizationProvider visualizationProvider) { }

		/// <summary>
		/// В данном случае надо показать и компоненты
		/// </summary>
		public void Show()
		{
			CanDraw = true;
			foreach (var control in Components) { control.Show(); }
		}

		/// <summary>
		/// В данном случае надо скрыть и компоненты
		/// </summary>
		public void Hide()
		{
			foreach (var control in Components) { control.Hide(); }
			CanDraw = false;
		}

		/// <summary>
		/// Добавить объект к списку компонентов
		/// </summary>
		/// <param name="component"></param>
		public void AddComponent(ViewComponent component)
		{
			Components.Add(component);
			component.Parent = this;
			component.Show();
			component.Init(VisualizationProvider);
		}

		/// <summary>
		/// Удалить объект
		/// </summary>
		/// <param name="component"></param>
		public void RemoveComponent(ViewComponent component)
		{
			component.Hide();
			component.Parent = null;
			Components.Remove(component);
		}

		/// <summary>
		/// Переместить объект на передний план
		/// </summary>
		/// <param name="topObject">Если объект не задан то перемещается текущий объект</param>
		public void BringToFront(ViewComponent topObject = null)
		{
			if (topObject == null) { Parent.BringToFront(this); return; }
			if (Components.Contains(topObject)) {
				Components.Remove(topObject);
				Components.Insert(0, topObject);
			} else {
				foreach (var control in Components) {
					control.BringToFront(topObject);
				}
			}
		}

		/// <summary>
		/// Переместить объект на задний план
		/// </summary>
		/// <param name="topObject">Если объект не задан то перемещается текущий объект</param>
		public void SendToBack(ViewComponent topObject)
		{
			if (topObject == null) { Parent.SendToBack(this); return; }
			if (Components.Contains(topObject)) {
				Components.Remove(topObject);
				Components.Add(topObject);
			} else {
				foreach (var control in Components) {
					control.SendToBack(topObject);
				}
			}
		}

		#region Cursor

		/// <summary>
		/// Стандартное распределение обработки события курсора
		/// </summary>
		public void CursorHandler(int cursorX, int cursorY)
		{
			if (!CanDraw) return;
			if (!InRange(cursorX,cursorY)) {
				CursorOverOff(); // сбрасываем выделение, в том числе и у вложенных контролов
				return;
			}
			CursorOver = true;
			Cursor(cursorX, cursorY);
			if (Components.Count > 0) {
				CursorOverOffed = false;
				foreach (var control in Components) {
					control.CursorOverOff();
					if (!control.InRange(cursorX - X, cursorY - Y)) {
						control.CursorOverOff();
						continue; // компонент не в точке нажатия
					}
					// смещаем курсор и передаём контролу смещенные координаты
					control.CursorHandler(cursorX - X, cursorY - Y);
				}
			}
		}

		/// <summary>
		/// Переопределяемая обработка события курсора
		/// </summary>
		/// <param name="o"></param>
		/// <param name="args"></param>
		protected virtual void Cursor(int cursorX, int cursorY) { }

		#endregion

		#region Keyboard

		клавиатура подключается для каждого отдельного действия
		для событий кликов надо что то делать.

		#endregion

		#region Draw

		/// <summary>
		/// Прорисовка объекта
		/// </summary>
		/// <param name="visualizationProvider">Объект-визуализатор</param>
		public void Draw(VisualizationProvider visualizationProvider)
		{
			if (CanDraw) {
				DrawComponentBackground(visualizationProvider);
				DrawObject(visualizationProvider);
				DrawComponents(visualizationProvider);
			}
		}

		/// <summary>
		/// Прорисовка объекта переопределяемая
		/// </summary>
		/// <param name="visualizationProvider"></param>
		public override void DrawObject(VisualizationProvider visualizationProvider) { }

		/// <summary>
		/// Прорисовать фон компонентов, если нужно
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected virtual void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{
			//if (CursorOver)	visualizationProvider.SetColor(Color.DodgerBlue, 20);
			//else			visualizationProvider.SetColor(Color.DimGray, 50);
			//visualizationProvider.Box(X, Y, Width, Height);
		}

		/// <summary>
		/// Перерисовать подчиненные компоненты
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected virtual void DrawComponents(VisualizationProvider visualizationProvider)
		{
			// можно проверять на предмет правильного восстановления смещения
			visualizationProvider.OffsetAdd(X, Y);// смещаем и рисуем компоненты независимо от их настроек
												  // прорисовываем в обратном порядке, от нижних к верхним - наверху находятся те объекты, которые рисуются последними
			for (int index = Components.Count - 1; index >= 0; index--) {
				var control = Components[index];
				control.Draw(visualizationProvider);
			}
			visualizationProvider.OffsetRemove();// восстанавливаем смещение			
		}

		/// <summary>
		/// Прорисовка объекта для текстуры
		/// </summary>
		/// <param name="visualizationProvider">Объект-визуализатор</param>
		public void DrawToTexture(VisualizationProvider visualizationProvider)
		{
			if (CanDraw) {
				DrawObjectToTexture(visualizationProvider);
			}
		}

		/// <summary>
		/// Прорисовка объекта для текстуры. Без проверки на необходимость вывода на экран
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected virtual void DrawObjectToTexture(VisualizationProvider visualizationProvider)
		{

		}

		#endregion

		/// <summary>
		/// Сбрасываем CursorOver, в том числе и у всех вложенных компонентов
		/// </summary>
		protected void CursorOverOff()
		{
			CursorOver = false;
			if (CursorOverOffed) return;
			foreach (var controls in Components) {
				controls.CursorOverOff();
			}
			CursorOverOffed = true;
		}

		/// <summary>
		/// Проверяем, находятся ли переданные координаты внутри объекта
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>Находится координата в пределах области контрола или нет</returns>
		public virtual Boolean InRange(int x, int y)
		{
			if (!CanDraw) return false; // компонент не рисуется - значит не проверяем дальше
			if ((X < x) && (x < X + Width)) {
				if ((Y < y) && (y < Y + Height)) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Установить координаты объекта
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void SetCoordinates(int x, int y, int z = 0)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Установить относительные координаты объекта
		/// </summary>
		/// <param name="rx"></param>
		/// <param name="ry"></param>
		/// <param name="rz"></param>
		public void SetCoordinatesRelative(int rx, int ry, int rz)
		{
			X += rx;
			Y += ry;
			Z += rz;
		}

		public void SetName(string name)
		{
			Name = name;
		}

		public Boolean ModalStart()
		{
			var args = ViewControlEventArgs.Send(this);
			Controller.StartEvent("ViewSystem.ModalStart", this, args);
			if (args.Result) {
				IsModal = true;
				_isModalStoreCanDraw = CanDraw;// сохраняем состояние, при выводе модального объекта извне оно не обрабатывается, но сильно меняется
			}
			return args.Result;
		}

		public void ModalStop()
		{
			IsModal = false;
			CanDraw = _isModalStoreCanDraw;// восстанавливаем старое состояние, которое было до модального вызова
			Controller.StartEvent("ViewSystem.ModalStop", this, ViewControlEventArgs.Send(this));
		}

		/// <summary>
		/// Отладка. Получить список всех объектов отображения с отступами
		/// </summary>
		/// <returns></returns>
		public List<string> GetObjectsView()
		{
			var ret = new List<string>();
			foreach (var control in Components) {
				GetObjectsView(ret, control, 1);
			}
			return ret;
		}

		private void GetObjectsView(List<string> list, ViewControl control, int deep)
		{
			if (!control.CanDraw) return;
			list.Add("".PadLeft((deep - 1), ' ') + ":" + control.Name + "(" + control.GetType().Name + ")");
			foreach (var cntrl in control.Controls) {
				GetObjectsView(list, cntrl, deep + 1);
			}
		}

		public void SetParams(int x, int y, int width, int height, string name)
		{
			Name = name;
			SetCoordinates(x, y);
			SetSize(width, height);
		}
	}
}
