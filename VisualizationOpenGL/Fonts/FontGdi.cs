using System;
using System.Linq;
using System.Text;
using OpenGL4NET;
using Tao.Platform.Windows;

namespace VisualizationOpenGL.Fonts
{
	internal class FontGdi : FontBase
	{
		public string FontName { get; }
		private int _fontOpenGLList = -1;
		private Gdi.GLYPHMETRICSFLOAT[] _glyphMetrics = new Gdi.GLYPHMETRICSFLOAT[256];
		private int _windowHeight;
		private IntPtr _font;

		public FontGdi(string fontCodeName, string fontName, int fontHeight, int windowHeight) : base(fontCodeName, fontHeight)
		{
			FontName = fontName;
			_windowHeight = windowHeight;
			BuildFont(fontName, fontHeight);
		}


		private void BuildFont(string fontName, int fontHeight)
		{
			IntPtr font;
			IntPtr oldfont;

			_fontOpenGLList = (int)gl.GenLists(256);
			font = Gdi.CreateFont(-fontHeight,
				0,
				0,
				0,
				Gdi.FF_DONTCARE,//Gdi.FW_BOLD
				false,
				false,
				false,
				Gdi.DEFAULT_CHARSET,
				Gdi.OUT_TT_PRECIS,
				Gdi.CLIP_DEFAULT_PRECIS,
				Gdi.ANTIALIASED_QUALITY,
				0,
				fontName);

			IntPtr dc = Wgl.wglGetCurrentDC();
			oldfont = Gdi.SelectObject(dc, font);
			Wgl.wglUseFontOutlinesA(dc, 0, 256, _fontOpenGLList, 0.1f, 0.2f, Wgl.WGL_FONT_POLYGONS, _glyphMetrics);
			Wgl.wglUseFontBitmapsA(dc, 0, 256, _fontOpenGLList);

			Gdi.SelectObject(dc, oldfont);
			_font = font;

			//if (_fontInfos.ContainsKey(fontCodeName)) {
			//	Gdi.DeleteObject(_fontInfos[fontCodeName].Font);
			//	// TODO логировать удаление шрифта
			//} else {
			//	_fontInfos.Add(fontCodeName, new FontInfo());
			//}
			//var fi = _fontInfos[fontCodeName];
			//fi.FontType = FontType.GdiFont;
			//fi.Font = font;
			//fi.FontOpenGLList = _fontOpenGLList;
			//fi.GlyphMetrics = _glyphMetrics;
			//fi.FontHeight = fontHeight;
			//fi.Counter = 0;
		}

		public override int TextLength(string text)
		{
			var w1251Bytes = ConvertEncoding(text);
			return TextLength(w1251Bytes);
		}

		private int TextLength(byte[] text)
		{
			//int len = text.Sum(с => ((int)(_glyphMetrics[с].gmfCellIncX * FontHeight * 1.22f + 0.5f)));
			//double len = text.Sum(c => Math.Ceiling(_glyphMetrics[c].gmfCellIncX * FontHeight + FontHeight / 4));
			double len = text.Sum(c =>
				{
					var glc = _glyphMetrics[c];
					return Math.Ceiling((glc.gmfCellIncX + glc.gmfBlackBoxX / 2 - glc.gmfptGlyphOrigin.X * 2) * FontHeight);
				}
			);
			return (int)Math.Ceiling(len);


			//var w1251Bytes = ConvertEncoding(text);
			//var glm = _glyphMetrics;// default
			//var fontHeight = FontHeight;
			//if (!string.IsNullOrEmpty(font) && _fonts.ContainsKey(font)) {
			//	var fi = _fonts[font];
			//	glm = fi.GlyphMetrics;
			//}

			//// https://gamedev.ru/code/forum/?id=32665
			//double len = w1251Bytes.Sum(c =>
			//	             {
			//		             var glc = glm[c];
			//		             return (glc.gmfCellIncX);
			//	             }
			//             ) * fontHeight;
			//return (int)len;

			//// https://doxygen.reactos.org/d1/dbc/3dtext_8c.html // nope
			//double len = w1251Bytes.Sum(c =>
			//{
			//	var glc = glm[c];
			//	return ((glc.gmfCellIncX + glc.gmfBlackBoxX / 2 - glc.gmfptGlyphOrigin.X));
			//}
			//) * fontHeight;
			//return (int)len;// Math.Ceiling(len);

		}

		/// <summary>
		/// Вспомогательная функция для конвертирования юникода в другую кодировку и преобразование в массив байтов
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private byte[] ConvertEncoding(string text)
		{
			Encoding w1251 = Encoding.GetEncoding("windows-1251");
			Encoding unicode = Encoding.Unicode;
			byte[] unicodeBytes = unicode.GetBytes(text);
			byte[] w1251Bytes = Encoding.Convert(unicode, w1251, unicodeBytes);
			return w1251Bytes;
		}
		
		public override void PrintOnly(int x, int y, string text)
		{
			byte[] w1251Bytes = ConvertEncoding(text);

			// вывод текста
			gl.PushAttrib(GL.DEPTH_TEST);       // Save the current Depth test settings (Used for blending )
			gl.PushAttrib(GL.TEXTURE_2D);       // Save the current GL_TEXTURE_2D
			gl.PushAttrib(GL.ALPHA_TEST);       // Save the current GL_ALPHA_TEST
			gl.PushAttrib(GL.BLEND);            // Save the current GL_BLEND
			gl.Disable(GL.DEPTH_TEST);          // Turn off depth testing (otherwise we get no FPS)
			gl.Disable(GL.TEXTURE_2D);          // Выключаем текстурирование, текстурный текст
			gl.Enable(GL.ALPHA_TEST);
			gl.Enable(GL.BLEND);

			gl.WindowPos2iARB(x, _windowHeight - y - FontHeight);
			gl.PushAttrib(GL.LIST_BIT);     // Save's the current base list
			gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
			gl.ListBase((uint)_fontOpenGLList);            // Set the base list to our character list
			gl.CallLists(w1251Bytes.Length, GL.UNSIGNED_BYTE, w1251Bytes); // Display the text
			gl.PopAttrib();                     // Restore the old base list

			gl.PopAttrib(); // Restore GL_BLEND
			gl.PopAttrib(); // Restore GL_ALPHA_TEST
			gl.PopAttrib(); // Restore GL_TEXTURE_2D
			gl.PopAttrib(); // Restore GL_DEPTH_TEST
		}

		public override void ActivateFont()
		{
			IntPtr dc = Wgl.wglGetCurrentDC();
			IntPtr oldfont = Gdi.SelectObject(dc, _font);
			//Wgl.wglUseFontOutlinesA(dc, 0, 256, _fontOpenGLList, 0.1f, 0.2f, Wgl.WGL_FONT_POLYGONS, _glyphMetrics);
			Wgl.wglUseFontBitmapsA(dc, 0, 256, _fontOpenGLList);
			Gdi.SelectObject(dc, oldfont);
		}
	}
}