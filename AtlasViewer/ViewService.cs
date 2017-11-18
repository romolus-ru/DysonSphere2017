

using AtlasViewer.ViewModel;

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

		/*/// <summary>
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
		}*/
	}
}
