using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Tao.DevIl;
using OpenGL4NET;
using Engine.Visualization;
using Engine.Utils;
using VisualizationOpenGL.Fonts;

namespace VisualizationOpenGL
{
	class VisualizationOpenGL4 : VisualizationProvider
	{
		private FormOpenGL4 _formOpenGl;
		private AtlasManager _atlasManager;

		public override void InitVisualization(DataSupportBase data, LogSystem log, int width, int height, bool fullScreen)
		{
			// пригодится в дальнейшем.
			//if (w <= h) {
			//    glOrtho(-nRange, nRange, -nRange * h / w, nRange * h / w, -nRange, nRange);
			//} else {
			//    glOrtho(-nRange * w / h, nRange * w / h, -nRange, nRange, -nRange, nRange);
			//}

			_formOpenGl = new FormOpenGL4();
			_formOpenGl.Size = fullScreen ?
				Screen.PrimaryScreen.Bounds.Size :
				_formOpenGl.Size = new Size(width, height);
			_formOpenGl.WindowState = fullScreen ?
				FormWindowState.Maximized :
				FormWindowState.Normal;
			_formOpenGl.KeyPreview = true;

			_formOpenGl.Text = @"OpenGL4";

			_formOpenGl.MouseWheel += MouseWheel;
			_formOpenGl.Focus();
			_formOpenGl.BringToFront();
			_formOpenGl.FormClosed += FormClosed;

			_atlasManager = new AtlasManager(data, log);

			Il.ilInit();
			Il.ilEnable(Il.IL_ORIGIN_SET);
			// очиcтка окна
			gl.ClearDepth(1.0f);        // Depth Buffer Setup
			gl.Disable(GL.DEPTH_TEST);  // Disable Depth Buffer было enable, но почему то из-за этого не выводились текстуры
			gl.DepthFunc(GL.LESS);      // The Type Of Depth Test To Do
			gl.Enable(GL.ALPHA_TEST);
			ResizeGlScene(_formOpenGl.Width, _formOpenGl.Height);
			LoadFont("bigFont", "Segoe UI", 24);
			LoadFont("default", "Consolas", 12);
			SetFont("default");
			//LoadFont("default", "Book Antiqua", 14);
			//LoadFont("default2", "Book Antiqua", 10);
			//LoadFont("default", "Impact обычный", 12);
			//LoadFont("default2", "Segoe UI", 15);
			//LoadFont("default2", "Segoe Print", 15);
			//LoadFont("default2", "Calibri", 14);
			//_controller.AddEventHandler("setHeader", (o, args) => SetHeader(o, args as MessageEventArgs));
			//_controller.AddEventHandler("systemExit", Exit);
			var c = _formOpenGl.PointToClient(Point.Empty);
			//LoadTextureModify("clear", "Resources/clear256x256.tga", new TPTRounded(), Color.Empty, Color.Empty);
		}

		public override Point WindowLocation => _formOpenGl.DesktopLocation;

		private void FormClosed(object sender, FormClosedEventArgs e)
		{
			ExitMessage?.Invoke();
		}

		public override void Exit()
		{
			ExitMessage?.Invoke();
			_formOpenGl.Close();
		}

		/// <summary>
		/// Отлавливаем колёсико мыши
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MouseWheel(object sender, MouseEventArgs e)
		{
            OnMouseWheel?.Invoke(e.Delta);
			//_controller.StartEvent("ViewStringListAdd", this, MessageEventArgs.Msg("Колесо мыши " + e.Delta));
			//_controller.StartEvent("CursorDelta", this, EngineGenericEventArgs<int>.Send(e.Delta));
		}

		/*/// <summary>
		/// Получаем координаты курсора из других источников
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void CursorCoordinates(object sender, PointEventArgs e)
		{
			var p = _formOpenGl.PointToClient(e.Pt);
			e.SetCoord(p);// меняем переданные координаты на новые
		}*/

		private void ResizeGlScene(int width, int height)
		{
			var aspect = 1f * width / height;// TODO возможно лучше упростить - всё равно WxH
			// задаётся размер экрана, влияет на искажение вида, поэтому надо пересчитать размеры
			gl.Viewport(0, 0, width, height);
			gl.MatrixMode(GL.PROJECTION);
			gl.LoadIdentity();

			if (width >= height) {
				// aspect >= 1, set the height from -1 to 1, with larger width
				gl.Ortho(0, width, width / aspect, 0, -1, 1);
			} else {
				// aspect < 1, set the width to -1 to 1, with larger height
				gl.Ortho(0, height * aspect, height, 0, -1, 1);
			}

			//Glu.gluOrtho2D(0, width, height, 0);
			//gl.Ortho(0, width-1, height-1, 0, -1, 1);
			gl.MatrixMode(GL.MODELVIEW);
			CanvasHeight = height;
			CanvasWidth = width;
		}

		/*private void SetHeader(object sender, MessageEventArgs e)
		{
			if (e != null){
				_formOpenGl.Text = e.Message;
			}
		}*/

		private float _colorR = 0;
		private float _colorG = 0;
		private float _colorB = 0;
		private float _colorA = 0;

		/// <summary>
		/// OpenGL установка цвета
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public override void SetColor(int r, int g, int b, int a)
		{
			_colorR = (float)r / 255;
			_colorG = (float)g / 255;
			_colorB = (float)b / 255;
			_colorA = (float)a / 255;
			gl.Color4f(_colorR, _colorG, _colorB, _colorA);
		}

		private float _backgroundColorR;
		private float _backgroundColorG;
		private float _backgroundColorB;
		private float _backgroundColorA;

		/// <summary>
		/// OpenGL установка фонового цвета
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		/// <param name="a"></param>
		public override void SetBackgroundColor(int r, int g, int b, int a)
		{
			_backgroundColorR = (float)r / 255;
			_backgroundColorG = (float)g / 255;
			_backgroundColorB = (float)b / 255;
			_backgroundColorA = (float)a / 255;
		}

		protected override void _Line(int x1, int y1, int x2, int y2, int lineType)
		{
			gl.Enable(GL.LINE_SMOOTH);
			gl.Disable(GL.TEXTURE_2D); // Turn off textures
			gl.Enable(GL.BLEND);
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);

			if (lineType == 0) {
				gl.Begin(GL.LINES);
				gl.Vertex2f(x1, y1);
				gl.Vertex2f(x2, y2);
				gl.End();
			} else {
				var dx = y1 - y2;
				var dy = x2 - x1;
				if (dx != 0) dx /= Math.Abs(dx);
				if (dy != 0) dy /= Math.Abs(dy);
				gl.Begin(GL.QUADS);
				gl.Vertex2f(x1, y1);
				gl.Vertex2f(x2, y2);
				gl.Vertex2f(x2 + dx, y2 + dy);
				gl.Vertex2f(x1 + dx, y1 + dy);
				gl.End();
			}
		}

		protected override void _Rectangle(int x, int y, int width, int height)
		{
			Line(x, y, x + width, y, 1);
			Line(x + width, y, x + width, y + height, 1);
			Line(x + width, y + height, x, y + height, 1);
			Line(x, y + height, x, y, 1);
		}

		protected override void _Rectangle(int x, int y, int width, int height, int radius)
		{
			Line(x + radius, y, x + width - radius, y, 1);
			Line(x + width, y + radius, x + width, y + height - radius, 1);
			Line(x + width - radius, y + height, x + radius, y + height, 1);
			Line(x, y + height - radius, x, y + radius, 1);
			_drawArc(x + radius, y + radius, radius, 270, 360, 10);
			_drawArc(x + width - radius, y + radius, radius, 0, 90, 10);
			_drawArc(x + width - radius, y + height - radius, radius, 90, 180, 10);
			_drawArc(x + radius, y + height - radius, radius, 180, 270, 10);
		}

		// рисуем скругление для точки с заданным радиусом для углов в диапазоне от 1 до 2 с заданным шагом
		private void _drawArc(int x, int y, int radius, int a1, int a2, int stepA)
		{
			var cx = x;
			var cy = y;
			var rd = radius;
			var st = stepA;
			if (radius < 50) st = 30;
			if (radius < 200) st = 10;
			if (radius < 400) st = 3;

			var p1 = RoundPoints[a1];
			var x1 = (int)(p1.X * rd + cx);
			var y1 = (int)(p1.Y * rd + cy);

			int i;
			int x2;
			int y2;

			for (i = a1 + stepA; i <= a2; i += st) {
				p1 = RoundPoints[i];
				x2 = (int)(p1.X * rd + cx);
				y2 = (int)(p1.Y * rd + cy);
				Line(x1, y1, x2, y2);
				x1 = x2;
				y1 = y2;
			}
			i += st;// дорисовываем дополнительно
			if (i > 360) i -= 360;
			p1 = RoundPoints[i];
			x2 = (int)(p1.X * rd + cx);
			y2 = (int)(p1.Y * rd + cy);
			Line(x1, y1, x2, y2);


		}

		protected override void _Box(int x, int y, int width, int height)
		{
			// TODO повторяет quads - возможно надо удалить box
			gl.Enable(GL.LINE_SMOOTH);
			gl.Disable(GL.TEXTURE_2D); // Turn off textures
			gl.Enable(GL.BLEND);
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);

			gl.Begin(GL.QUADS);
			gl.Vertex2f(x, y);
			gl.Vertex2f(x + width, y);
			gl.Vertex2f(x + width, y + height);
			gl.Vertex2f(x, y + height);
			gl.End();
		}

		protected override void _Box(int x, int y, int width, int height, int radius)
		{
			_Line(x + radius, y, x + width - radius, y, 1);
			_Line(x + width, y + radius, x + width, y + height - radius, 1);
			_Line(x + width - radius, y + height, x + radius, y + height, 1);
			_Line(x, y + height - radius, x, y + radius, 1);
			var p1 = _getArcPoints(x + radius, y + radius, radius, 270, 360, 10);
			var p2 = _getArcPoints(x + width - radius, y + radius, radius, 0, 90, 10);
			var p3 = _getArcPoints(x + width - radius, y + height - radius, radius, 90, 180, 10);
			var p4 = _getArcPoints(x + radius, y + height - radius, radius, 180, 270, 10);

			gl.Disable(GL.BLEND);
			gl.Enable(GL.LINE_SMOOTH);
			gl.Disable(GL.TEXTURE_2D); // Turn off textures
			gl.Enable(GL.BLEND);
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);

			gl.Begin(GL.POLYGON);
			foreach (var f in p1) { gl.Vertex2f(f.X, f.Y); }
			gl.Vertex2f(x + radius, y);
			gl.Vertex2f(x + width - radius, y);
			foreach (var f in p2) { gl.Vertex2f(f.X, f.Y); }

			gl.Vertex2f(x + width, y + radius);
			gl.Vertex2f(x + width, y + height - radius);
			foreach (var f in p3) { gl.Vertex2f(f.X, f.Y); }

			gl.Vertex2f(x + width - radius, y + height);
			gl.Vertex2f(x + radius, y + height);
			foreach (var f in p4) { gl.Vertex2f(f.X, f.Y); }

			gl.Vertex2f(x, y + height - radius);
			gl.Vertex2f(x, y + radius);

			gl.End();
		}

		// рисуем скругление для точки с заданным радиусом для углов в диапазоне от 1 до 2 с заданным шагом
		// почти копия _drawArc
		private List<PointF> _getArcPoints(int x, int y, int radius, int a1, int a2, int stepA)
		{
			var ret = new List<PointF>();
			var cx = x;
			var cy = y;
			var rd = radius;
			var st = stepA;
			if (radius < 50) st = 30;
			if (radius < 200) st = 10;
			if (radius < 400) st = 3;
			int i;
			PointF p1;
			int x2;
			int y2;

			for (i = a1 + stepA; i <= a2; i += st) {
				p1 = RoundPoints[i];
				x2 = (int)(p1.X * rd + cx);
				y2 = (int)(p1.Y * rd + cy);
				ret.Add(new PointF(x2, y2));
			}
			i += st;// дорисовываем дополнительно
			if (i > 360) i -= 360;
			p1 = RoundPoints[i];
			x2 = (int)(p1.X * rd + cx);
			y2 = (int)(p1.Y * rd + cy);
			ret.Add(new PointF(x2, y2));

			return ret;
		}

		protected override void _Quad(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
		{
			gl.Enable(GL.LINE_SMOOTH);
			gl.Disable(GL.TEXTURE_2D); // Turn off textures
			gl.Enable(GL.BLEND);
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);

			gl.Begin(GL.QUADS);
			gl.Vertex2f(x1, y1);
			gl.Vertex2f(x2, y2);
			gl.Vertex2f(x3, y3);
			gl.Vertex2f(x4, y4);
			gl.End();
		}

		private int num_segments = 36;

		protected override void _Circle(int cx, int cy, int radius)
		{
			double theta = 2 * Math.PI / num_segments;
			double tangentialFactor = Math.Tan(theta);
			double radialFactor = Math.Cos(theta);
			double x = radius;//we start at angle = 0 
			double y = 0;

			gl.Enable(GL.LINE_SMOOTH);
			gl.Disable(GL.TEXTURE_2D); // Turn off textures
			gl.Enable(GL.BLEND);
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);

			gl.Begin(GL.LINE_LOOP);
			for (int ii = 0; ii < num_segments; ii++) {
				gl.Vertex2d(x + cx, y + cy); //output vertex 

				//calculate the tangential vector 
				//remember, the radial vector is (x, y) 
				//to get the tangential vector we flip those coordinates and negate one of them 

				double tx = -y;
				double ty = x;

				//add the tangential vector 

				x += tx * tangentialFactor;
				y += ty * tangentialFactor;

				//correct using the radial factor 

				x *= radialFactor;
				y *= radialFactor;
			}
			gl.End();
		}

		public override void Run()
		{
			base.Run();
			//_formOpenGl.WindowState = FormWindowState.Maximized;
			_formOpenGl.BringToFront();
			_formOpenGl.Focus();
			_formOpenGl.ShowDialog();
		}

		public override bool LoadAtlas(string atlasName)
		{
			var atlasFile = _atlasManager.GetAtlasFile(atlasName);
			if (atlasFile == null) return false;

			// идентификатор текстуры
			int imageId = 0;

			bool r = false;
			// создаем изображение с идентификатором imageId
			Il.ilGenImages(1, out imageId);
			// делаем изображение текущим
			Il.ilBindImage(imageId);

			// пробуем загрузить изображение
			var fileName = AtlasUtils.GetAtlasFileFullPath(atlasFile.AtlasFile);
			if (Il.ilLoadImage(fileName)) {
				// если загрузка прошла успешно
				// сохраняем размеры изображения
				int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
				int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);

				// определяем число бит на пиксель
				int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);

				uint textureCode = 0;
				switch (bitspp) // в зависимости от полученного результата
				{
					// создаем текстуру используя режим GL_RGB или GL_RGBA
					case 24:
						//case 8:
						//case 16:
						textureCode = MakeGlTexture(GL.RGB, Il.ilGetData(), width, height);
						break;
					case 32:
						textureCode = MakeGlTexture(GL.RGBA, Il.ilGetData(), width, height);
						break;
				}
				var blendParam = /*opacity ?*/ GL.SRC_ALPHA;// : GL.ONE;
				_atlasManager.InitAtlasTextures(atlasFile, textureCode, blendParam);

				// активируем флаг, сигнализирующий успешную загрузку текстуры
				r = true;
				// очищаем память
				Il.ilDeleteImages(1, ref imageId);

			}
			return r;
		}

		protected override void _DrawTexture(int x, int y, string textureName, float scale = 1)
		{
			var texInfo = _atlasManager.GetTextureInfo(textureName);
			if (texInfo == null) return;
			gl.LoadIdentity();
			int z = 0;
			gl.PushAttrib(GL.ALPHA_TEST);       // Save the current GL_ALPHA_TEST
			gl.Enable(GL.ALPHA_TEST);
			gl.PushAttrib(GL.TEXTURE_2D);
			// включаем режим текстурирования 
			gl.Enable(GL.TEXTURE_2D);
			gl.PushAttrib(GL.BLEND);
			gl.Enable(GL.BLEND);
			//gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);// непрозрачно
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE);// прозрачно, как в DrawTexturePart
			gl.BindTexture(GL.TEXTURE_2D, texInfo.TextureCode);
			float h = scale * texInfo.Height;
			float w = scale * texInfo.Width;

			float x1 = texInfo.X;
			float x2 = texInfo.X + texInfo.Width;
			float y1 = texInfo.AtlasHeight - texInfo.Y;
			float y2 = texInfo.AtlasHeight - texInfo.Y - texInfo.Height;

			x1 = x1 / texInfo.AtlasWidth;
			x2 = x2 / texInfo.AtlasWidth;
			y1 = y1 / texInfo.AtlasHeight;
			y2 = y2 / texInfo.AtlasHeight;

			// сохраняем состояние матрицы 
			gl.PushMatrix();
			gl.Translated(x, y, 0);
			gl.Rotated(_angle, 0.0f, 0.0f, 1.0f);
			//Gl.glTranslated(0, 0, 0);

			gl.Begin(GL.QUADS);
			// указываем поочередно вершины и текстурные координаты
			//gl.TexCoord2f(x1, y2); gl.Vertex3d(0, h, z);
			//gl.TexCoord2f(x1, y1); gl.Vertex3d(0, 0, z);
			//gl.TexCoord2f(x2, y1); gl.Vertex3d(w, 0, z);
			//gl.TexCoord2f(x2, y2); gl.Vertex3d(w, h, z);
			gl.TexCoord2f(x1, y2); gl.Vertex3d(-w / 2, h / 2, z);
			gl.TexCoord2f(x1, y1); gl.Vertex3d(-w / 2, -h / 2, z);
			gl.TexCoord2f(x2, y1); gl.Vertex3d(w / 2, -h / 2, z);
			gl.TexCoord2f(x2, y2); gl.Vertex3d(w / 2, h / 2, z);

			gl.End();

			gl.Rotated(-_angle, 0.0f, 0.0f, 1.0f);// вращаем всё назад
												  // возвращаем матрицу 
			gl.PopMatrix();
			// возвращаем всё в исходное состояние
			gl.PopAttrib();//Gl.GL_BLEND
			gl.PopAttrib();//Gl.GL_TEXTURE_2D
			gl.PopAttrib();//Gl.GL_alpha_test
			gl.BlendFunc(GL.ONE, GL.ONE);
		}

		protected override void _DrawTexturePart(int x, int y, string textureName, int blockWidth, int blockHeight, int num)
		{
			if (blockWidth == 0) return;
			if (blockHeight == 0) return;

			var texInfo = _atlasManager.GetTextureInfo(textureName);
			if (texInfo == null) return;
			gl.LoadIdentity();
			int z = 0;
			gl.PushAttrib(GL.TEXTURE_2D);
			// включаем режим текстурирования 
			gl.Enable(GL.TEXTURE_2D);
			gl.PushAttrib(GL.BLEND);
			gl.Enable(GL.BLEND);
			gl.BlendFunc(texInfo.BlendParam, GL.ONE);
			gl.BindTexture(GL.TEXTURE_2D, texInfo.TextureCode);
			int textureHeight = texInfo.AtlasHeight;// по идее это можно узнать с помощью GL_TEXTURE_WIDTH и HEIGHT
			int textureWidth = texInfo.AtlasWidth;// но наврядли быстрее - счас без обращения к видеокарте

			// сохраняем состояние матрицы 
			gl.PushMatrix();
			gl.Translated(x, y, 0);
			gl.Rotated(_angle, 0.0f, 0.0f, 1.0f);

			// вычисляем координаты блока
			int cx = textureWidth / blockWidth;// количество блоков по X
			int cy = textureHeight / blockHeight;// количество блоков по Y
			if (cx < 1) return;// если меньше нуля получили 
			if (cy < 1) return;// значит где то что то не то

			int posX = num / cy;
			int posY = num % cy;
			if (posX >= cx) return;// проверяем выходит ли за рамки
			if (posY >= cy) return;// выходит - выходим отсюда

			float x1 = blockWidth * posX;
			float x2 = blockWidth * (posX + 1);
			float y1 = blockHeight * posY;
			float y2 = blockHeight * (posY + 1);

			x1 = x1 / textureWidth;
			x2 = x2 / textureWidth;
			y1 = y1 / textureHeight;
			y2 = y2 / textureHeight;

			gl.Begin(GL.QUADS);
			// указываем поочередно вершины и текстурные координаты
			var blWd2 = blockWidth / 2;
			var blHd2 = blockHeight / 2;
			gl.TexCoord2f(x2, y1); gl.Vertex3d(blWd2, blHd2, z);
			gl.TexCoord2f(x2, y2); gl.Vertex3d(blWd2, -blHd2, z);
			gl.TexCoord2f(x1, y2); gl.Vertex3d(-blWd2, -blHd2, z);
			gl.TexCoord2f(x1, y1); gl.Vertex3d(-blWd2, blHd2, z);

			gl.End();

			gl.Rotated(-_angle, 0.0f, 0.0f, 1.0f);// вращаем всё назад
												  // возвращаем матрицу 
			gl.PopMatrix();
			// возвращаем всё в исходное состояние
			gl.PopAttrib();//Gl.GL_BLEND
			gl.PopAttrib();//Gl.GL_TEXTURE_2D
			gl.BlendFunc(GL.ONE, GL.ONE);
		}

		protected override void _DrawTexturePart(int x, int y, string textureName, int placeWidth, int placeHeight, bool useColorModification)
		{
			var texInfo = _atlasManager.GetTextureInfo(textureName);
			if (texInfo == null) return;
			int xtex = texInfo.X;
			int ytex = texInfo.Y;
			int width = texInfo.Width;
			int height = texInfo.Height;
			if (width < 1) return;
			if (height < 1) return;

			gl.LoadIdentity();
			int z = 0;
			int textureHeight = texInfo.AtlasHeight;
			int textureWidth = texInfo.AtlasWidth;
			if (textureHeight < ytex + height) return;// выходим за рамки текстуры
			if (textureWidth < xtex + width) return;// выходим за рамки текстуры

			gl.PushAttrib(GL.TEXTURE_2D);
			// включаем режим текстурирования 
			gl.Enable(GL.TEXTURE_2D);
			gl.PushAttrib(GL.BLEND);
			gl.Enable(GL.BLEND);

			if (useColorModification)
				gl.TexEnvi(GL.TEXTURE_ENV, GL.TEXTURE_ENV_MODE, GL.MODULATE);// что бы текстура была прозрачной, сама текстура
																			 //gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
			gl.BlendFunc(texInfo.BlendParam, GL.ONE);
			gl.BindTexture(GL.TEXTURE_2D, texInfo.TextureCode);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, GL.REPEAT);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, GL.REPEAT);


			// сохраняем состояние матрицы 
			gl.PushMatrix();
			gl.Translated(x, y, 0);
			gl.Rotated(_angle, 0.0f, 0.0f, 1.0f);

			float x1a = xtex;
			float x2a = xtex + width;
			float y1a = textureHeight - ytex;
			float y2a = textureHeight - ytex - height;

			float x1 = x1a / textureWidth;
			float x2 = x2a / textureWidth;
			float y1 = y1a / textureHeight;
			float y2 = y2a / textureHeight;

			gl.Begin(GL.QUADS);
			// указываем поочередно вершины и текстурные координаты
			gl.TexCoord2f(x1, y1); gl.Vertex3d(0, 0, z);
			gl.TexCoord2f(x2, y1); gl.Vertex3d(placeWidth, 0, z);
			gl.TexCoord2f(x2, y2); gl.Vertex3d(placeWidth, placeHeight, z);
			gl.TexCoord2f(x1, y2); gl.Vertex3d(0, placeHeight, z);

			gl.End();

			gl.Rotated(-_angle, 0.0f, 0.0f, 1.0f);// вращаем всё назад
												  // возвращаем матрицу 
			gl.PopMatrix();
			// возвращаем всё в исходное состояние
			gl.PopAttrib();//Gl.GL_BLEND
			gl.PopAttrib();//Gl.GL_TEXTURE_2D
			gl.BlendFunc(GL.ONE, GL.ONE);
			if (useColorModification)
				gl.TexEnvi(GL.TEXTURE_ENV, GL.TEXTURE_ENV_MODE, GL.REPLACE);
		}

		protected override void _DrawTextureMasked(int x, int y, string textureName, string textureMaskName)
		{
			var texInfoMask = _atlasManager.GetTextureInfo(textureMaskName);
			if (texInfoMask == null) return;
			var texInfoNormal = _atlasManager.GetTextureInfo(textureName);
			if (texInfoNormal == null) return;
			int h = texInfoMask.AtlasHeight;
			int w = texInfoMask.AtlasWidth;

			gl.PushAttrib(GL.BLEND);
			gl.PushAttrib(GL.DEPTH_TEST);
			gl.Disable(GL.DEPTH_TEST);
			gl.Enable(GL.BLEND);
			// http://www.opengl.org/archives/resources/faq/technical/transparency.htm
			//Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);		// Blend Screen Color With Zero (Black)
			gl.BlendFunc(GL.DST_COLOR, GL.ZERO);        // Blend Screen Color With Zero (Black)
			gl.BindTexture(GL.TEXTURE_2D, texInfoMask.TextureCode); // Select The First Mask Texture
			gl.Begin(GL.QUADS);                         // Start Drawing A Textured Quad
			gl.TexCoord2f(1, 0); gl.Vertex3d(x + h, y + w, 0);
			gl.TexCoord2f(1, 1); gl.Vertex3d(x + h, y, 0);
			gl.TexCoord2f(0, 1); gl.Vertex3d(x, y, 0);
			gl.TexCoord2f(0, 0); gl.Vertex3d(x, y + w, 0);
			gl.End();                                           // Done Drawing The Quad

			h = texInfoNormal.AtlasHeight;
			w = texInfoNormal.AtlasWidth;

			gl.BlendFunc(GL.ONE, GL.ONE);               // Copy Image 1 Color To The Screen
														//Gl.glBlendFunc(Gl.GL_ONE, Gl.GL_ONE);				// Copy Image 1 Color To The Screen
			gl.BindTexture(GL.TEXTURE_2D, texInfoNormal.TextureCode);   // Select The First Image Texture
			gl.Begin(GL.QUADS);                         // Start Drawing A Textured Quad
			gl.TexCoord2f(1, 0); gl.Vertex3d(x + h, y + w, 0);
			gl.TexCoord2f(1, 1); gl.Vertex3d(x + h, y, 0);
			gl.TexCoord2f(0, 1); gl.Vertex3d(x, y, 0);
			gl.TexCoord2f(0, 0); gl.Vertex3d(x, y + w, 0);
			gl.End();
			gl.PopAttrib();// GL_DEPTH_TEST
			gl.PopAttrib();// GL_BLEND

			/*
			// мультитекстурирование
			glActiveTextureARB(GL_TEXTURE0_ARB);
			glEnable(GL_TEXTURE_2D);
			glBindTexture(GL_TEXTURE_2D,texNames[0]);
			glTexEnvi(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_REPLACE);
			glMatrixMode(GL_TEXTURE);
			glLoadIdentity();
			glTranslatef(0.5,0.5,0.0);
			glRotatef(45.0,0.0,0.0,1.0);
			glTranslatef(-0.5,-0.5,0.0);
			glMatrixMode(GL_MODELVIEW);
			glActiveTextureARB(GL_TEXTURE1_ARB);
			glEnable(GL_TEXTURE_2D);
			glBindTexture(GL_TEXTURE_2D,texNames[1]);
			glTexEnvi(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
			 
			*/


		}

		public override void LoadFont(string fontCodeName, string fontName, int fontHeight = 12)
		{
			if (_fonts.ContainsKey(fontCodeName))
				return;
			var font = new FontGdi(fontCodeName, fontName, fontHeight, _formOpenGl.Height);
			_fonts.Add(fontCodeName, font);
		}

		public override void LoadFontTexture(string textureName)
		{
			// или надо переделать или сохранить в архивах и удалить отсюда
			//LoadTexture(TextureFont, textureName);
			//BuildFont();
		}



		public override int TextLength(string text)
		{
			return _currentFont.TextLength(text);
		}

		public override int TextLength(string font, string text)
		{
            if (font == null || !_fonts.ContainsKey(font))
                return 0;
			return _fonts[font].TextLength(text);
		}

		public override int GetFontSize(string font)
		{
			if (string.IsNullOrEmpty(font) || !_fonts.ContainsKey(font))
				return FontHeight;
			var fi = _fonts[font];
			return fi.FontHeight;
		}

	
		protected override void PrintOnly(int x, int y, string text)
		{
			_currentFont.PrintOnly(x, y, text);
		}

		public override void BeginDraw()
		{
			gl.ClearColor(_backgroundColorR, _backgroundColorG, _backgroundColorB, _backgroundColorA);
			gl.Clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT);
			gl.LoadIdentity();
		}

		public override void FlushDrawing()
		{
			gl.Flush();
			_formOpenGl.rc.SwapBuffers();
		}

		private int _angle = 0;

		/// <summary>
		/// Поворот на угол. в градусах
		/// </summary>
		/// <param name="angle"></param>
		public override void Rotate(int angle)
		{
			_angle += angle;
		}

		public override void RotateReset()
		{
			_angle = 0;
		}

		private FontBase _currentFont;
		private Dictionary<string, FontBase> _fonts = new Dictionary<string, FontBase>();

		public override void SetFont(string fontCodeName)
		{
			if (!_fonts.ContainsKey(fontCodeName)) return;
			_currentFont = _fonts[fontCodeName];
			FontHeight = _currentFont.FontHeight;
			_currentFont.ActivateFont();
		}


		// создание текстуры в памяти openGL
		private static uint MakeGlTexture(int format, IntPtr pixels, int w, int h)
		{
			// идентификатор текстурного объекта 
			uint texObject;
			// генерируем текстурный объект 
			gl.GenTextures(1, out texObject);
			// устанавливаем режим упаковки пикселей 
			gl.PixelStorei(GL.UNPACK_ALIGNMENT, 1);
			// создаем привязку к только что созданной текстуре 
			gl.BindTexture(GL.TEXTURE_2D, texObject);
			// устанавливаем режим фильтрации и повторения текстуры 
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, GL.REPEAT);// .CLAMP);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, GL.REPEAT);// .CLAMP);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, GL.NEAREST);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, GL.NEAREST);
			gl.TexEnvf(GL.TEXTURE_ENV, GL.TEXTURE_ENV_MODE, GL.REPLACE);
			// создаем RGB или RGBA текстуру 
			switch (format) {
				case GL.RGB:
					gl.TexImage2D(GL.TEXTURE_2D, 0, GL.RGB, w, h, 0, GL.RGB, GL.UNSIGNED_BYTE, pixels);
					break;
				case GL.RGBA:
					gl.TexImage2D(GL.TEXTURE_2D, 0, GL.RGBA, w, h, 0, GL.RGBA, GL.UNSIGNED_BYTE, pixels);
					break;
			}
			// возвращаем идентификатор текстурного объекта 
			return texObject;
		}

		protected override void _CopyToTexture(string textureName)
		{
			var texInfo = _atlasManager.GetTextureInfo(textureName);
			if (texInfo == null) return;
			base._CopyToTexture(textureName);
			// До копирования экрана в текстуру нужно указать её вызовом glBindTexture()
			gl.BindTexture(GL.TEXTURE_2D, texInfo.TextureCode);

			// Настал момент, которого мы ждали - мы рендерим экран в текстуру.
			// Передаем тип текстуры, детализацию, формат пиксела, x и y позицию старта,
			// ширину и высоту для захвата, и границу. Если вы хотите сохранить только часть
			// экрана, это легко сделать изменением передаваемых параметров.
			gl.CopyTexImage2D(GL.TEXTURE_2D, 0, GL.RGB, 0, 0, texInfo.AtlasWidth, texInfo.AtlasWidth, 0);
		}

		protected override void _SetClipPlane(int x1, int y1, int x2, int y2)
		{
			gl.ClipPlane(GL.CLIP_PLANE0, new double[] { 0, 1, 0, -y1 });
			gl.Enable(GL.CLIP_PLANE0);
			gl.ClipPlane(GL.CLIP_PLANE1, new double[] { 0, -1, 0, y2 });
			gl.Enable(GL.CLIP_PLANE1);

			gl.ClipPlane(GL.CLIP_PLANE2, new double[] { 1, 0, 0, -x1 });
			gl.Enable(GL.CLIP_PLANE2);
			gl.ClipPlane(GL.CLIP_PLANE3, new double[] { -1, 0, 0, x2 });
			gl.Enable(GL.CLIP_PLANE3);
		}

		protected override void _ClipPlaneOff()
		{
			gl.Disable(GL.CLIP_PLANE0);
			gl.Disable(GL.CLIP_PLANE1);
			gl.Disable(GL.CLIP_PLANE2);
			gl.Disable(GL.CLIP_PLANE3);
		}

		protected override void _SetStencilArea(int x1, int y1, int x2, int y2)
		{
			gl.Clear(GL.STENCIL_BUFFER_BIT);
			gl.Enable(GL.STENCIL_TEST);
			gl.StencilFunc(GL.NEVER, 1, 0);
			gl.StencilOp(GL.REPLACE, GL.KEEP, GL.KEEP);
			gl.Disable(GL.TEXTURE_2D);

			gl.Color4f(0, 0, 0, 1);
			gl.Begin(GL.QUADS);
			gl.Vertex2f(x1, y1);
			gl.Vertex2f(x2, y1);
			gl.Vertex2f(x2, y2);
			gl.Vertex2f(x1, y2);
			gl.End();

			gl.StencilFunc(GL.EQUAL, 1, 1);
			gl.StencilOp(GL.KEEP, GL.KEEP, GL.KEEP);
		}

		protected override void _StencilAreaOff()
		{
			gl.Disable(GL.STENCIL_TEST);
		}

		public override Size GetTextureSize(string textureName)
		{
			var texInfo = _atlasManager.GetTextureInfo(textureName);
			if (texInfo == null) return Size.Empty;
			return new Size(texInfo.Width, texInfo.Height);
		}

		public static string vertexShader2Source = @"
#version 440 core
void main(void)
{
 //gl_Position = vec4( 0.25, -0.25,  0.5,  1.0);
}";
		public static string fragmentShader2Source = @"
#version 440 core
out vec4 color;
void main(void)
{
 color = vec4(1.0, 0.0, 0.0, 0.3);
}";
		private uint _program;

		public override void InitShader()
		{

			var vertexShader = gl.CreateShader(GL.VERTEX_SHADER);
			gl.ShaderSource(vertexShader,
			//File.ReadAllText(@"Shaders\vertexShader.vert")
			vertexShader2Source
			);
			gl.CompileShader(vertexShader);

			var fragmentShader = gl.CreateShader(GL.FRAGMENT_SHADER);
			gl.ShaderSource(fragmentShader,
			//File.ReadAllText(@"Shaders\fragmentShader.frag")
			fragmentShader2Source
			);
			gl.CompileShader(fragmentShader);

			var program = gl.CreateProgram();
			//gl.AttachShader(program, vertexShader);
			gl.AttachShader(program, fragmentShader);
			gl.LinkProgram(program);

			gl.DetachShader(program, vertexShader);
			gl.DetachShader(program, fragmentShader);
			gl.DeleteShader(vertexShader);
			gl.DeleteShader(fragmentShader);
			_program = program;
		}

		public override void UseShader()
		{
			gl.UseProgram(_program);
			// получаем имя переменной
			//int uniformLoopDuration = glGetUniformLocation(theProgram, "loopDuration");
			// устанавливаем её значение 
			//glUniform1f(uniformLoopDuration, 5.0f);
			//mMVPMatrixHandle = GLES20.glGetUniformLocation(programHandle, "u_MVPMatrix");
			//mPositionHandle = GLES20.glGetAttribLocation(programHandle, "a_Position");
			// вон сколько их всяких
			//gl.GetAttribLocation
			//gl.GetFragDataLocation
			//gl.GetProgramResourceLocation
			//gl.GetUniformLocation
			//gl.GetVaryingLocation
		}
		public override void StopShader()
		{
			gl.UseProgram(0);
		}

		protected override int _printTexture(string textureName, string fontName)
		{
			if (string.IsNullOrEmpty(textureName)) return 0;

			var texInfo = _atlasManager.GetTextureInfo(textureName);
			if (texInfo == null) return 0;
			int dx, dy, placeWidth, placeHeight;
			_getTextureSizeForFont(textureName, fontName, out dx, out dy, out placeWidth, out placeHeight);
			DrawTexturePart(CurTxtX + dx, CurTxtY + dy, textureName, placeWidth, placeHeight);
			//Line(CurTxtX - 5, CurTxtY - 5, CurTxtX + 5, CurTxtY + 5);
			//Line(CurTxtX + 5, CurTxtY - 5, CurTxtX - 5, CurTxtY + 5);

			//Line(CurTxtX - 5 + dx, CurTxtY - 5 + dy, CurTxtX + 5 + dx, CurTxtY + 5 + dy);
			//Line(CurTxtX + 5 + dx, CurTxtY - 5 + dy, CurTxtX - 5 + dx, CurTxtY + 5 + dy);

			return placeWidth/*placeHeight*/;
		}

		public void _getTextureSizeForFont(string textureName, string fontName, out int dx, out int dy, out int placeWidth, out int placeHeight)
		{
			var texInfo = _atlasManager.GetTextureInfo(textureName);
			float coeff = 0;
			if (texInfo.Width < texInfo.Height)
				coeff = (float)texInfo.Width / texInfo.Height;
			else
				coeff = (float)texInfo.Height / texInfo.Width;

			placeWidth = FontHeight;// текущий размер шрифта
			if (!string.IsNullOrEmpty(fontName) && _fonts.ContainsKey(fontName))
				placeWidth = _fonts[fontName].FontHeight;// размер шрифта для которого выводится текстура
			placeHeight = (int)(placeWidth * coeff + 0.5);
			dx = 0;
			dy = (int)(placeHeight * 1 / 7);
		}

		// https://habr.com/ru/post/347354/
		// создание буфера кадра 
		// придётся делать всю инфраструктуру
		// у VisualizationProvider появится метод для обработки рендера в текстуру (передаётся имя текстуры и там уже всё рендерится в неё)
		// и дальше обычным способом - выводим текстуру (полученную уже рендером в текстуру) на экран как обычно
		// в начале создаём текстуру для рендера
		// когда надо подключаемся к очереди рисования в текстуру
		// там очищаем экран, настраиваем операции (автоматический шаг)
		// рисуем на экран что нужно
		// после этого сохраняем текстуру, отключаем рисование в текстуру (автоматический шаг)
		private uint _fbuffer;
		private uint _fbTexture;

		public int CreateFrameBufferTexture()
		{
			uint framebuffer;
			gl.GenFramebuffers(1, out framebuffer);
			gl.BindFramebuffer(GL.FRAMEBUFFER, framebuffer);


			uint texture;
			gl.GenTextures(1, out texture);
			gl.BindTexture(GL.TEXTURE_2D, texture);

			gl.TexImage2D(GL.TEXTURE_2D, 0, GL.RGB, 800, 600, 0, GL.RGB, GL.UNSIGNED_BYTE, null);

			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, GL.LINEAR);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, GL.LINEAR);

			gl.FramebufferTexture2D(GL.FRAMEBUFFER, GL.COLOR_ATTACHMENT0, GL.TEXTURE_2D, texture, 0);

			_fbTexture = texture;
			return (int)texture;
		}

		public int CreateFrameBuffer()
		{
			int num = (int) gl.GenFramebuffer();
			return num;
		}

		public void StartFrameBuffer(int num)
		{
			gl.BindFramebuffer(GL.FRAMEBUFFER, (uint) _fbuffer);
		}

		public void EndFrameBuffer()
		{
			gl.BindFramebuffer(GL.FRAMEBUFFER, 0);
		}

	}
}