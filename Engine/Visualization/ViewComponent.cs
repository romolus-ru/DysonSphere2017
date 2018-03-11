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
	public class ViewComponent : ViewObject
	{
		#region Основные переменные

		public ViewComponent Parent { get; protected set; }

		protected VisualizationProvider VisualizationProvider;
		protected Input Input;
		public string Name { get; protected set; }

		private int _x;
		private int _y;
		private int _z;
		protected int _xScreen;// for mouse operations
		protected int _yScreen;// for mouse operations
		protected int _zScreen;// for mouse operations

		/// <summary>
		/// Координата X объекта
		/// </summary>
		public int X { get { return _x; } protected set { _x = value; RecalcScreenX(); } }

		/// <summary>
		/// Координата Y объекта
		/// </summary>
		public int Y { get { return _y; } protected set { _y = value; RecalcScreenY(); } }

		/// <summary>
		/// Координата Z объекта
		/// </summary>
		public int Z { get { return _z; } protected set { _z = value; RecalcScreenZ(); } }

		public bool CanDraw { get; private set; }

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
		protected bool CursorOverOffed;

		#endregion

		#region Размеры объекта

		private int _height;
		/// <summary>
		/// Высота объекта
		/// </summary>
		public int Height { get { return _height; } protected set { _height = value;Resized(); } }

		private int _width;
		/// <summary>
		/// Ширина объекта
		/// </summary>
		public int Width { get { return _width; } protected set { _width = value; Resized();  } }

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
		private bool _disposed = false;

		public virtual void Dispose()
		{
			if (!_disposed) {
				foreach (var component in Components) { component.Dispose(); }
				Components.Clear();
				Components = null;
				_disposed = true;
				VisualizationProvider = null;
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
		public void Init(VisualizationProvider visualizationProvider, Input input)
		{
			VisualizationProvider = visualizationProvider;// сохраняем для будущего использования
			Input = input;
			InitObject(VisualizationProvider, input);
			Show();
		}

		/// <summary>
		/// переопределяемая инициализация объекта для текущей визуализации
		/// </summary>
		protected virtual void InitObject(VisualizationProvider visualizationProvider, Input input) { }

		/// <summary>
		/// подготовка к удалению объекта, удаление вспомогательных объектов, удаление обработчиков событий и т.п.
		/// </summary>
		public void Clear()
		{
			foreach (var component in Components) {
				component.Clear();
			}
			ClearObject();
			VisualizationProvider = null;
			Input = null;
		}

		/// <summary>
		/// переопределяемая подготовка к удалению объекта, удаление вспомогательных объектов, удаление обработчиков событий и т.п.
		/// </summary>
		protected virtual void ClearObject() { }
		
		/// <summary>
		/// /// В данном случае надо показать и компоненты
		/// /// </summary>
		public void Show()
		{
			CanDraw = true;
			foreach (var component in Components) { component.Show(); }
		}

		/// <summary>
		/// В данном случае надо скрыть и компоненты
		/// </summary>
		public void Hide()
		{
			foreach (var component in Components) { component.Hide(); }
			CanDraw = false;
		}

		/// <summary>
		/// Добавить объект к списку компонентов
		/// </summary>
		/// <param name="component"></param>
		public virtual void AddComponent(ViewComponent component, bool toTop = false)
		{
			if (toTop)
				Components.Insert(0, component);
			else
				Components.Add(component);
			component.Parent = this;
			component.Show();
			component.Init(VisualizationProvider, Input);
		}

		/// <summary>
		/// Удалить объект
		/// </summary>
		/// <param name="component"></param>
		public virtual void RemoveComponent(ViewComponent component)
		{
			component.Clear();
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
				foreach (var component in Components) {
					component.BringToFront(topObject);
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
				foreach (var component in Components) {
					component.SendToBack(topObject);
				}
			}
		}

		#region Cursor

		private static bool overedOne = false;
		/// <summary>
		/// Стандартное распределение обработки события курсора
		/// </summary>
		public void CursorHandler(int cursorX, int cursorY)
		{
			overedOne = false;
			CursorHandlerLocal(cursorX, cursorY);
		}

		protected void CursorHandlerLocal(int cursorX, int cursorY)
		{
			if (overedOne) return;// если уже есть объект (с вложенными) у которого выставлено over - у остальных не устанавливаем
			if (!CanDraw) return;
			if (!InRange(cursorX, cursorY)) {
				CursorOverOff(); // сбрасываем выделение, в том числе и у вложенных контролов
				return;
			}
			CursorOver = true;
			Cursor(cursorX, cursorY);
			if (Components.Count > 0) {
				CursorOverOffed = false;
				foreach (var component in Components) {
					component.CursorOverOff();
					var curX = cursorX - _xScreen + X;
					var curY = cursorY - _yScreen + Y;
					if (!component.InRange(curX, curY)) {
						component.CursorOverOff();
						continue; // компонент не в точке нажатия
					}
					// смещаем курсор и передаём контролу смещенные координаты
					component.CursorHandlerLocal(curX, curY);
					if (component.CursorOver) overedOne = true;
				}
			}
		}

		/// <summary>
		/// Переопределяемая обработка события курсора
		/// </summary>
		protected virtual void Cursor(int cursorX, int cursorY) { }

		#endregion

		#region Draw

		/// <summary>
		/// Прорисовка объекта
		/// </summary>
		/// <param name="visualizationProvider">Объект-визуализатор</param>
		public void Draw(VisualizationProvider visualizationProvider)
		{
			if (CanDraw) {
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
		/// Перерисовать подчиненные компоненты
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected virtual void DrawComponents(VisualizationProvider visualizationProvider)
		{
			// можно проверять на предмет правильного восстановления смещения
			visualizationProvider.OffsetAdd(X, Y);// смещаем и рисуем компоненты независимо от их настроек
												  // прорисовываем в обратном порядке, от нижних к верхним - наверху находятся те объекты, которые рисуются последними
			for (int index = Components.Count - 1; index >= 0; index--) {
				var component = Components[index];
				component.Draw(visualizationProvider);
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
		/// <remarks>Можно сделать этот метод частью CursorHandler - он будет сбрасывать 
		/// один раз состояние всех объектов. возможно будет меньше операций чем постоянный вызов
		/// </remarks>
		protected void CursorOverOff()
		{
			CursorOver = false;
			if (CursorOverOffed) return;
			foreach (var component in Components) {
				component.CursorOverOff();
			}
			CursorOverOffed = true;
		}

		/// <summary>
		/// Проверяем, находятся ли переданные координаты внутри объекта
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>Находится координата в пределах области контрола или нет</returns>
		public virtual bool InRange(int x, int y)
		{
			if (!CanDraw) return false; // компонент не рисуется - значит не проверяем дальше
			if ((_xScreen <= x) && (x <= _xScreen + Width)) {
				if ((_yScreen <= y) && (y <= _yScreen + Height)) {
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

		/// <summary>
		/// Отладка. Получить список всех объектов отображения с отступами
		/// </summary>
		/// <returns></returns>
		public List<string> GetObjectsView()
		{
			var ret = new List<string>();
			foreach (var component in Components) {
				GetObjectsView(ret, component, 1);
			}
			return ret;
		}

		private void GetObjectsView(List<string> list, ViewComponent component, int deep)
		{
			if (!component.CanDraw) return;
			var mover = component.CursorOver ? " mouse over" : "";
			list.Add("".PadLeft((deep - 1), '.') + "|" + component.Name + "(" + component.GetType().Name + ")" + mover);
			foreach (var cntrl in component.Components) {
				GetObjectsView(list, cntrl, deep + 1);
			}
		}

		public virtual void SetParams(int x, int y, int width, int height, string name)
		{
			Name = name;
			SetCoordinates(x, y);
			SetSize(width, height);
		}

		protected void RecalcScreenX()
		{
			int x = 0;
			var p = Parent;
			while (p != null) { x += p.X; p = p.Parent; }
			_xScreen = x + X;
			foreach (var component in Components) {
				component.RecalcScreenX();
			}
		}

		protected void RecalcScreenY()
		{
			int y = 0;
			var p = Parent;
			while (p != null) { y += p.Y; p = p.Parent; }
			_yScreen = y + Y;
			foreach (var component in Components) {
				component.RecalcScreenY();
			}
		}

		protected void RecalcScreenZ()
		{
			int z = 0;
			var p = Parent;
			while (p != null) { z += p.Z; p = p.Parent; }
			_zScreen = z + Z;
			foreach (var component in Components) {
				component.RecalcScreenZ();
			}
		}

		/// <summary>
		/// Изменились размеры компонента
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		protected virtual void Resized() { }

	}
}
