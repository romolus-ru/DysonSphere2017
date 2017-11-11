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
	/// Управление атласами. в основном для VisualizationProvider for OpenGL (из-за того что нужен UInt)
	/// </summary>
	public class AtlasManager
	{
		private DataSupportBase _data;
		private LogSystem _log;
		private string LogTag = "AtlasManager";

		private Dictionary<string, Texture> _atlasTextures = new Dictionary<string, Texture>();

		public AtlasManager(DataSupportBase data, LogSystem log)
		{
			_data = data;
			_log = log;
		}

		public AtlasFiles GetAtlasFile(string atlasName)
		{
			var atlasInfo = _data.GetAtlasFile(atlasName);
			if (atlasInfo == null) _log.AddLog(LogTag, "Атлас не найден " + atlasName);
			return atlasInfo;
		}
		/// <summary>
		/// Загружаем атлас из базы и возвращаем имя файла, который надо загрузить
		/// </summary>
		/// <param name="atlasName"></param>
		/// <returns></returns>
		public void InitAtlasTextures(AtlasFiles atlas, uint textureId)
		{
			// загружаем атлас и сохраняем его имя чтоб вернуть. загружаем данные о текстурах и сохраняем текстуру под кодом "AtlasName.TextureName"
			var list = _data.GetAtlasTextures(atlas.IdAtlasFile);
			if (list == null) return;
			foreach (var t in list) {
				var tn = new Texture
				{
					TextureCode = textureId,
					AtlasWidth = (int)atlas.Width,
					AtlasHeight = (int)atlas.Height,
					P1X = (int)t.P1X,
					P1Y = (int)t.P1Y,
					P2X = (int)t.P2X,
					P2Y = (int)t.P2Y,
					AtlasName = atlas.AtlasName,
					TextureName = t.Name,
					Description = t.Description
				};
				var name = atlas.AtlasName + "." + t.Name;
				if (_atlasTextures.ContainsKey(name)) {
					_log.AddLog(LogTag, "Текстура " + t.Name + " уже проинициализирована для атласа " + atlas.AtlasName);
					continue;
				}
				_atlasTextures.Add(name, tn);
			}

			return;
		}

		public Texture GetTextureInfo(string textureName)
		{
			if (_atlasTextures.ContainsKey(textureName)) {
				return _atlasTextures[textureName];
			}
			_log.AddLog(LogTag, "текстура не обнаружена " + textureName);
			return null;
		}
	}
}
