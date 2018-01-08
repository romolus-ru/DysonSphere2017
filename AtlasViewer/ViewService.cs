

using AtlasViewer.ViewModel;
using Engine.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AtlasViewer
{
	/// <summary>
	/// Создание и запуск окон
	/// </summary>
	public static class ViewService
	{
		/// <summary>
		/// Запускаем форму редактирования файла атласа
		/// </summary>
		/// <param name="viewModelAtlasFileEdit">ВидМодель для редактирования файла атласа</param>
		/// <returns>Поменялось ли что-нибудь</returns>
		public static bool RunAtlasFileEdit(ViewModelAtlasFileEdit viewModelAtlasFileEdit)
		{
			var f = new AtlasFileEdit();
			viewModelAtlasFileEdit.RequestClose += (s, e) => f.Close();
			viewModelAtlasFileEdit.RequestRefreshWindow += (s, e) => f.InvalidateVisual();
			f.DataContext = viewModelAtlasFileEdit;
			var dr = f.ShowDialog();
			return viewModelAtlasFileEdit.DialogResult == "Changed";
		}

		/// <summary>
		/// Запускаем форму редактирования текстуры атласа
		/// </summary>
		/// <param name="viewModelAtlasTextureEdit">ВидМодель для редактирования текстуры атласа</param>
		/// <returns>Поменялось ли что-нибудь</returns>
		public static bool RunAtlasTextureEdit(ViewModelAtlasTextureEdit viewModelAtlasTextureEdit)
		{
			var f = new AtlasTextureEdit();
			viewModelAtlasTextureEdit.RequestClose += (s, e) => f.Close();
			f.DataContext = viewModelAtlasTextureEdit;
			var dr = f.ShowDialog();
			return viewModelAtlasTextureEdit.DialogResult == "Changed";
		}

		public static Window mainWindow;

		public static void ShowAtlasTextures(List<AtlasTextures> textures, AtlasTextures selected=null)
		{
			var canvas = mainWindow.FindName("myCanvas") as Canvas;
			if (canvas == null) return;
			canvas.Children.Clear();
			foreach (var texture in textures) {
				Rectangle box = new Rectangle();
				box.Width = (texture.P2X - texture.P1X) / Utils.PixelSize;
				box.Height = (texture.P2Y - texture.P1Y) / Utils.PixelSize;
				box.Stroke = new SolidColorBrush(Colors.Red);
				if (texture == selected) box.Stroke = new SolidColorBrush(Colors.Blue);
				Canvas.SetLeft(box, texture.P1X / Utils.PixelSize);
				Canvas.SetTop(box, texture.P1Y / Utils.PixelSize);
				canvas.Children.Add(box);
			}
		}

		public static Window editTextureWindow;

		public static void ShowAtlasTextureInEditor(List<AtlasTextures> textures, AtlasTextures selected = null)
		{
			/*var canvas = mainWindow.FindName("myCanvas") as Canvas;
			if (canvas == null) return;
			canvas.Children.Clear();
			foreach (var texture in textures) {
				Rectangle box = new Rectangle();
				box.Width = texture.P2X - texture.P1X;
				box.Height = texture.P2Y - texture.P1Y;
				box.Stroke = new SolidColorBrush(Colors.Chocolate);
				if (texture == selected) box.Stroke = new SolidColorBrush(Colors.LightSeaGreen);
				Canvas.SetLeft(box, texture.P1X);
				Canvas.SetTop(box, texture.P1Y);
				canvas.Children.Add(box);
			}*/
		}
	}
}
