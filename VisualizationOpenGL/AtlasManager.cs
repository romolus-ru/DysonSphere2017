﻿using Engine;
using Engine.Data;
using Engine.Utils;
using System.Collections.Generic;

namespace VisualizationOpenGL
{
	/// <summary>
	/// Управление атласами. в основном для VisualizationProvider for OpenGL (из-за того что нужен UInt)
	/// </summary>
	internal class AtlasManager
	{
		private DataSupportBase _data;
		private LogSystem _log;
		private string LogTag = "AtlasManager";

		private List<string> _atlasLoaded = new List<string>();
		private Dictionary<string, Texture> _atlasTextures = new Dictionary<string, Texture>();

		public AtlasManager(DataSupportBase data, LogSystem log)
		{
			_data = data;
			_log = log;
		}

		public AtlasFiles GetAtlasFile(string atlasName)
		{
			if (_atlasLoaded.Contains(atlasName)) {
				//_log.AddLog(LogTag, "Атлас уже загружен " + atlasName);
				return null;
			}

			AtlasFiles atlasInfo = _data.GetAtlasFile(atlasName);

			if (atlasInfo == null) {
				_log.AddLog(LogTag, "Атлас не найден " + atlasName);
				return null;
			}

			return atlasInfo;
		}

		/// <summary>
		/// Загружаем атлас из базы и возвращаем имя файла, который надо загрузить
		/// </summary>
		/// <returns></returns>
		public void InitAtlasTextures(AtlasFiles atlas, uint textureId, int blendParam)
		{
			// сохраняем что атлас загрузили. загружаем данные о текстурах и сохраняем текстуру под кодом "AtlasName.TextureName"
			_atlasLoaded.Add(atlas.AtlasName);
			var list = _data.GetAtlasTextures(atlas.IdAtlasFile);
			if (list == null) return;
			foreach (var t in list) {
				var tn = new Texture
				{
					TextureCode = textureId,
					BlendParam=blendParam,
					AtlasWidth = (int)atlas.Width,
					AtlasHeight = (int)atlas.Height,
					X = (int)t.P1X,
					Y = (int)t.P1Y,
					Width = (int)t.P2X- (int)t.P1X,
					Height = (int)t.P2Y- (int)t.P1Y,
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
