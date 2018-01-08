using System.Reflection;
using System.Windows;

namespace AtlasViewer.ViewModel
{
	public class Utils
	{
		static Utils()
		{
			var flags = BindingFlags.NonPublic | BindingFlags.Static;
			var dpiProperty = typeof(SystemParameters).GetProperty("Dpi", flags);

			Dpi = (int)dpiProperty.GetValue(null, null);
			PixelSize = 72.0 / Dpi;
		}

		//Размер физического пикселя в виртуальных единицах
		public static double PixelSize { get; private set; }

		//Текущее разрешение
		public static int Dpi { get; private set; }

	}
}
