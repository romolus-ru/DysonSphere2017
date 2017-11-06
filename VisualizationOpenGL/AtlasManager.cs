using Engine;
using Engine.Data;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizationProviderOpenGL
{
	/// <summary>
	/// Управление атласами. в основном для VisualizationProvider
	/// </summary>
	public class AtlasManager
	{
		private DataSupportBase _data;
		private LogSystem _log;
		private string LogTag = "AtlasManager";

		private Dictionary<string, List<Texture>> _atlasTextures = new Dictionary<string, List<Texture>>();

		public AtlasManager(DataSupportBase data, LogSystem log)
		{
			_data = data;
			_log = log;
		}

		/// <summary>
		/// Загружаем атлас из базы и возвращаем имя файла который надо загрузить
		/// </summary>
		/// <param name="atlasName"></param>
		/// <returns></returns>
		public string LoadAtlas(string atlasName)
		{
			// TODO тут
			// загружаем атлас и сохраняем её имя чтоб вернуть. загружаем данные о текстурах и сохраняем текстуру под кодом "AtlasName.TextureName"
			// и логируем случаи когда обращаемся к несуществующей текстуре
			return null;
		}
	}
}
