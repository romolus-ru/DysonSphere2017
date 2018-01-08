using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Базовый класс для различных видов визуализации
	/// </summary>
	/// <remarks>Умеет только рисовать</remarks>
	public class VisualizationProvider
	{
		public virtual void Exit() { }

		//protected Controller _controller;

		public Version Version = new Version(0, 2);

		/// <summary>
		/// Ширина окна визуализации
		/// </summary>
		public int CanvasWidth { get; protected set; }

		/// <summary>
		/// Высота окна визуализации
		/// </summary>
		public int CanvasHeight { get; protected set; }

		/// <summary>
		/// Конструктор без параметров
		/// </summary>
		/// <remarks>Ничего не должен содержать, всё передаётся при инициализации</remarks>
		public VisualizationProvider()
		{
			Version = new Version(1, 0);
		}

		/// <summary>
		/// Инициализация визуализации. Создание формы и установка нужных размеров. или узнать размеры экрана и использовать в fullscreen
		/// </summary>
		public virtual void InitVisualization(DataSupportBase data, LogSystem log, int width, int height, bool fullScreen)
		{
		}

		/// <summary>
		/// Получаем координаты курсора из других источников
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Заменяется в потомках. Переопределяет координаты курсора из экранных в клиентские</remarks>
		protected virtual void CursorCoordinates(object sender, System.Drawing.Point e)
		{
		}

		#region Установка цвета

		/// <summary>
		/// Текущий цвет
		/// </summary>
		public System.Drawing.Color Color { get; private set; }

		/// <summary>
		/// Фоновый цвет
		/// </summary>
		public System.Drawing.Color BackgroundColor { get; private set; }

		/// <summary>
		/// Установить цвет
		/// </summary>
		/// <param name="color"></param>
		public void SetColor(Color color)
		{
			Color = color;
			SetColor(Color.R, Color.G, Color.B, Color.A);
		}

		/// <summary>
		/// Установить цвет c прозрачностью
		/// </summary>
		/// <param name="color"></param>
		/// <param name="alphaPercent"></param>
		public void SetColor(Color color, byte alphaPercent)
		{
			if (alphaPercent > 100) { alphaPercent = 100; }// обрезаем до 100
			Color = Color.FromArgb(255 * alphaPercent / 100, color);// растягиваем до 255
			SetColor(Color.R, Color.G, Color.B, Color.A);
		}

		public virtual void SetColor(int r, int g, int b, int a)
		{

		}

		/// <summary>
		/// Установить фоновый цвет
		/// </summary>
		/// <param name="color"></param>
		public void SetBackgroundColor(Color color)
		{
			BackgroundColor = color;
			SetBackgroundColor(color.R, color.G, color.B, color.A);
		}

		/// <summary>
		/// Установить фоновый цвет c прозрачностью
		/// </summary>
		/// <param name="color"></param>
		/// <param name="alpha"></param>
		public void SetBackgroundColor(Color color, byte alpha)
		{
			BackgroundColor = Color.FromArgb(alpha, color);
			SetBackgroundColor(color.R, color.G, color.B, color.A);
		}

		public virtual void SetBackgroundColor(int r, int g, int b, int a)
		{

		}

		#endregion

		#region Линия, прямоугольник и круг


		public static Dictionary<int, PointF> RoundPoints = InitPoints();

		public static Dictionary<int, PointF> InitPoints()
		{
			var ret = new Dictionary<int, PointF>();
			int radius = 1;

			int num_segments = 360;
			double theta = 2 * Math.PI / num_segments;
			double tangetialFactor = Math.Tan(theta);
			double radialFactor = Math.Cos(theta);
			double xa = 0;
			double ya = -radius;
			ret.Add(0, new PointF((float)xa, (float)ya));

			for (int ii = 0; ii < num_segments; ii++) {
				double tx = -ya;
				double ty = xa;
				xa += tx * tangetialFactor;
				ya += ty * tangetialFactor;
				xa *= radialFactor;
				ya *= radialFactor;
				ret.Add(ii + 1, new PointF((float)xa, (float)ya));
			}
			return ret;
		}


		/// <summary>
		/// Рисовать линию
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		public void Line(int x1, int y1, int x2, int y2)
		{
			_Line(x1 + curOffsetX, y1 + curOffsetY, x2 + curOffsetX, y2 + curOffsetY);
		}

		protected virtual void _Line(int x1, int y1, int x2, int y2) { }

		/// <summary>
		/// Нарисовать прямоугольник
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void Rectangle(int x, int y, int width, int height)
		{
			_Rectangle(x, y, width, height);// TODO заменить на вызов Line напрямую
		}

		protected virtual void _Rectangle(int x, int y, int width, int height)
		{

		}

		/// <summary>
		/// Нарисовать прямоугольник со скругленными углами
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="radius"></param>
		public void Rectangle(int x, int y, int width, int height, int radius)
		{
			if (radius == 0) {// радиус =0 значит вызываем обычный метод
				_Rectangle(x, y, width, height);
				return;
			}
			int r = radius * 2;
			int ri = radius;
			if (width < r) ri = width / 2;// если радиус больше чем нужно - уменьшаем
			if (height < r) ri = height / 2;
			_Rectangle(x, y, width, height, ri);
		}

		protected virtual void _Rectangle(int x, int y, int width, int height, int radius)
		{

		}

		/// <summary>
		/// Нарисовать закрашенный прямоугольник
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void Box(int x, int y, int width, int height)
		{ _Box(x + curOffsetX, y + curOffsetY, width, height); }

		protected virtual void _Box(int x, int y, int width, int height)
		{

		}

		/// <summary>
		/// Нарисовать закрашенный прямоугольник со скругленными углами
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="radius"></param>
		public void Box(int x, int y, int width, int height, int radius)
		{ _Box(x + curOffsetX, y + curOffsetY, width, height, radius); }

		protected virtual void _Box(int x, int y, int width, int height, int radius)
		{

		}

		/// <summary>
		/// Нарисовать круг
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="radius"></param>
		public virtual void Circle(int x, int y, int radius)
		{ _Circle(x + curOffsetX, y + curOffsetY, radius); }

		protected virtual void _Circle(int x, int y, int radius) { }

		/// <summary>
		/// Полигон по 4м точкам
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="x3"></param>
		/// <param name="y3"></param>
		/// <param name="x4"></param>
		/// <param name="y4"></param>
		public virtual void Quad(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
		{
			_Quad(x1 + curOffsetX, y1 + curOffsetY, x2 + curOffsetX, y2 + curOffsetY, x3 + curOffsetX, y3 + curOffsetY,
				x4 + curOffsetX, y4 + curOffsetY);
		}

		protected virtual void _Quad(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4) { }

		/// <summary>
		/// Нарисовать круговой прогресс бар
		/// </summary>
		/// <param name="vp"></param>
		/// <param name="cx"></param>
		/// <param name="cy"></param>
		/// <param name="radius"></param>
		/// <param name="cur"></param>
		/// <param name="max"></param>
		public void DrawRound(int cx, int cy, int radius, int cur, int max)
		{
			//return;
			{
				const int numSegments = 360;

				var curValue = (int)(1f * cur / max * numSegments);
				var color = Color; // потом этот цвет будет меняться
				OffsetAdd(cx, cy);
				for (int i = 360; i > 0; i--) {
					var c1 = (curValue - i) * 1;
					if (c1 < 0) c1 += 360;
					var c2 = c1 - 1;
					if (c2 < 0) c2 += 360;

					var p1 = RoundPoints[c1];
					var p2 = RoundPoints[c2];
					var mx1 = (int)(radius * p1.X);
					var my1 = (int)(radius * p1.Y);
					var nx1 = (int)((radius + 20) * p1.X);
					var ny1 = (int)((radius + 20) * p1.Y);
					var mx2 = (int)(radius * p2.X);
					var my2 = (int)(radius * p2.Y);
					var nx2 = (int)((radius + 20) * p2.X);
					var ny2 = (int)((radius + 20) * p2.Y);

					SetColor(color, (byte)(100 - i * 10 / 36f));
					Quad(mx1, my1, mx2, my2, nx2, ny2, nx1, ny1);
				}
				OffsetRemove();
			}

			//{const int numSegments = 36;radius += 35;var mx1 = (int) (radius*RoundPoints[0].X + cx);var my1 = (int) (radius*RoundPoints[0].Y + cy);
			//	var nx1 = (int) ((radius + 20)*RoundPoints[0].X + cx);var ny1 = (int) ((radius + 20)*RoundPoints[0].Y + cy);
			//	var numSegmentsDraw = (int) (1f*cur/max*numSegments);
			//	SetColor(Color.Peru);for (var ii = 0; ii < numSegmentsDraw + 1; ii++){
			//		var i1 = ii*10;if (i1 > 360) i1 -= 360;

			//		var mx2 = (int) (radius*RoundPoints[i1].X + cx);var my2 = (int) (radius*RoundPoints[i1].Y + cy);
			//		var nx2 = (int) ((radius + 20)*RoundPoints[i1].X + cx);var ny2 = (int) ((radius + 20)*RoundPoints[i1].Y + cy);

			//		Quad(mx1, my1, mx2, my2, nx2, ny2, nx1, ny1);mx1 = mx2;my1 = my2;nx1 = nx2;ny1 = ny2;}}
		}

		#endregion

		/// <summary>
		/// Запустить объект визуализации (окно) в модальном режиме
		/// </summary>
		public virtual void Run() { }

		/// <summary>
		/// Загрузка текстуры и создание в ней альфа-канала на основе серого цвета(сумма RGBсоставляющих цвета каждой отдельной точки)
		/// </summary>
		/// <param name="textureName">Имя текстуры</param>
		/// <param name="fileName">Имя файла</param>
		/// <param name="mode">режим. для будущего использования, чтоб можно было грузить альфу из других файлов (например *Alpha.jpg)</param>
		/// <returns></returns>
		[Obsolete]
		public virtual bool LoadTextureAlpha(string textureName, string fileName, int mode = 0)
		{ return false; }

		/// <summary>
		/// Загрузить текстуру из файла. Фактически текстуры грузится 24х битная, без альфа-канала
		/// </summary>
		/// <param name="textureName">Имя текстуры</param>
		/// <param name="fileName">Имя файла</param>
		/// <returns></returns>
		[Obsolete]
		public virtual bool LoadTexture(string textureName, string fileName)
		{ return false; }

		/// <summary>
		/// Загрузить атлас
		/// </summary>
		/// <param name="atlasName">Имя атласа</param>
		/// <returns></returns>
		public virtual bool LoadAtlas(string atlasName)
		{ return false; }

		/// <summary>
		/// Вывести на экран текстуру
		/// </summary>
		/// <param name="x">Координата Х</param>
		/// <param name="y">Координата У</param>
		/// <param name="textureName">Имя текстуры</param>
		/// <param name="scale">Увеличение размера текстуры</param>
		public void DrawTexture(int x, int y, string textureName, float scale = 1)
		{ _DrawTexture(x + curOffsetX, y + curOffsetY, textureName, scale); }

		protected virtual void _DrawTexture(int x, int y, String textureName, float scale = 1) { }

		/// <summary>
		/// Вывести на экран часть разбитой на блоки текстуры
		/// </summary>
		/// <param name="x">Координата X</param>
		/// <param name="y">Координата Y</param>
		/// <param name="textureName">Имя текстуры</param>
		/// <param name="blockWidth">Ширина одного блока в текстуре</param>
		/// <param name="blockHeight">Высота одного блока в текстуре</param>
		/// <param name="num">Номер с нуля блока, выводимого на экран</param>
		public void DrawTexturePart(int x, int y, string textureName, int blockWidth, int blockHeight, int num)
		{ _DrawTexturePart(x + curOffsetX, y + curOffsetY, textureName, blockWidth, blockHeight, num); }

		protected virtual void _DrawTexturePart(int x, int y, String textureName, int blockWidth, int blockHeight, int num) { }


		/// <summary>
		/// Вывести на экран произвольную часть текстуры, например в качестве прогрессбара
		/// </summary>
		/// <param name="x">Координата вывода на экран</param>
		/// <param name="y"></param>
		/// <param name="textureName">Имя текстуры</param>
		/// <param name="placeWidth">Координата на текстуре откуда выводить</param>
		/// <param name="placeHeight">Координата на текстуре откуда выводить</param>
		/// <param name="width">Ширина выводимой области</param>
		/// <param name="height">Высота выводимой области</param>
		public void DrawTexturePart(int x, int y, string textureName, int placeWidth, int placeHeight)
		{ _DrawTexturePart(x + curOffsetX, y + curOffsetY, textureName, placeWidth, placeHeight); }

		protected virtual void _DrawTexturePart(int x, int y, String textureName, int placeWidth, int placeHeight) { }

		/// <summary>
		/// Вывести на экран текстуру с маской (почти аналог LoadTextureAlpha)
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="textureName"></param>
		/// <param name="textureMaskName"></param>
		public void DrawTextureMasked(int x, int y, String textureName, String textureMaskName)
		{ _DrawTextureMasked(x + curOffsetX, y + curOffsetY, textureName, textureMaskName); }

		protected virtual void _DrawTextureMasked(int x, int y, String textureName, String textureMaskName) { }

		/// <summary>
		/// скопировать изображение с экрана в текстуру
		/// </summary>
		/// <param name="textureName"></param>
		public void CopyToTexture(String textureName)
		{ _CopyToTexture(textureName); }

		protected virtual void _CopyToTexture(String textureName) { }

		[Obsolete]
		public void DeleteTexture(String textureName)
		{ _DeleteTexture(textureName); }
		[Obsolete]
		protected virtual void _DeleteTexture(String textureName) { }


		///// <summary>
		///// Вывести на экран повёрнутую текстуру
		///// </summary>
		///// <param name="x"></param>
		///// <param name="y"></param>
		///// <param name="textureName"></param>
		//public virtual void DrawTextureRotated(int x, int y, String textureName){}

		#region Операции с текстом

		/// <summary>
		/// Имя текстуры
		/// </summary>
		/// <remarks>Вне зависимости от способа загрузки шрифта должна появиться текстура
		/// Хотя может и не появиться текстура, тогда будет выводиться без неё
		/// (зависит от конкретной реализации объекта визуализации)</remarks>
		protected string FontTexture = "";

		/// <summary>
		/// Высота шрифта. по умолчанию = 16
		/// </summary>
		protected int FontHeight = 16;

		/// <summary>
		/// Получить высоту шрифта. в C#5 можно будет объединить с объявлением переменной, сейчас нельзя из-за инициализации переменной сделать обычный get set
		/// </summary>
		/// <returns></returns>
		public int FontHeightGet() { return FontHeight; }

		/// <summary>
		/// Загрузить шрифт по имени 
		/// </summary>
		/// <param name="fontCodeName">строковой код шрифта, по которому к нему потом можно обращаться</param>
		/// <param name="fontName">Системное имя шрифта</param>
		/// <param name="fontHeight">Высота шрифта</param>
		public virtual void LoadFont(string fontCodeName, string fontName, int fontHeight = 12) { }

		/// <summary>
		/// Загрузить текстуру-шрифт
		/// </summary>
		/// <param name="textureName"></param>
		public virtual void LoadFontTexture(String textureName) { }

		/// <summary>
		/// Установить 
		/// </summary>
		/// <param name="fontCodeName"></param>
		public virtual void SetFont(String fontCodeName) { }
		/// <summary>
		/// Вычисление длины текста
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public virtual int TextLength(String text) { return 0; }

		/// <summary>
		/// Координата текста Х
		/// </summary>
		public int CurTxtX { get; protected set; }

		/// <summary>
		/// Координата текста У
		/// </summary>
		public int CurTxtY { get; protected set; }

		/// <summary>
		/// Вывод текста на экран в координатах ХУ
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		public void Print(int x, int y, String text)
		{
			CurTxtX = x;
			CurTxtY = y;
			Print(text);
		}

		/// <summary>
		/// Вывод текста на экран в текущих координатах
		/// </summary>
		/// <param name="text"></param>
		public void Print(string text)
		{
			PrintOnly(CurTxtX + curOffsetX, CurTxtY + curOffsetY, text);
			CurTxtX += TextLength(text);
		}

		/// <summary>
		/// Именно эта функция и  выводит текст
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		/// <remarks>
		/// Переопределено должно быть в потомках
		/// выводит текст в указанных координатах
		/// цвет уже установлен в других функциях, тут это не должно требоваться</remarks>
		protected virtual void PrintOnly(int x, int y, String text) { }

		#endregion

		/// <summary>
		/// Подготовка к началу рисования
		/// </summary>
		public virtual void BeginDraw() { }

		/// <summary>
		/// Окончательный вывод на экран, наподобие swapBuffers
		/// </summary>
		public virtual void FlushDrawing() { }

		/// <summary>
		/// Повернуть
		/// </summary>
		/// <param name="angle"></param>
		public virtual void Rotate(int angle) { }

		/// <summary>
		/// Восстановить повернутое
		/// </summary>
		public virtual void RotateReset() { }

		/// <summary>
		/// Смещения для прорисовки компонентов
		/// </summary>
		private Stack<int> offsets = new Stack<int>();

		public int curOffsetX = 0;
		public int curOffsetY = 0;

		/// <summary>
		/// Добавить смещение
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		public void OffsetAdd(int dx, int dy)
		{
			offsets.Push(curOffsetX);
			offsets.Push(curOffsetY);
			curOffsetX += dx;
			curOffsetY += dy;
		}

		/// <summary>
		/// Удалить смещение, восстановив предыдущее состояние
		/// </summary>
		public void OffsetRemove()
		{
			curOffsetY = offsets.Pop();
			curOffsetX = offsets.Pop();
		}

		public void SetClipPlane(int x1, int y1, int x2, int y2)
		{ _SetClipPlane(x1, y1, x2, y2); }

		protected virtual void _SetClipPlane(int x1, int y1, int x2, int y2) { }

		public void ClipPlaneOff()
		{ _ClipPlaneOff(); }

		protected virtual void _ClipPlaneOff() { }

		public void SetStencilArea(int x1, int y1, int x2, int y2)
		{ _SetStencilArea(x1, y1, x2, y2); }

		protected virtual void _SetStencilArea(int x1, int y1, int x2, int y2) { }

		public void StensilAreaOff()
		{ _StencilAreaOff(); }

		protected virtual void _StencilAreaOff() { }
	}
}
