using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils
{
	public static class AtlasUtils
	{
		public static string GetAtlasFileFullPath(string file)
		{
			return Path.Combine(GetAtlasFilePath(), file);
		}

		public static string GetAtlasFilePath()
		{
			var path = Directory.GetCurrentDirectory();
			path = Path.GetDirectoryName(path);
			path = Path.Combine(path, "_files");
			return path;
		}

		public static string GetAtlasFileShortPath(string filePath)
		{
			var path = GetAtlasFilePath().ToUpper();
			if (!filePath.ToUpper().StartsWith(path)) return "";
			if (filePath.Contains("..")) return "";
			var pathLength = path.Length;
			if (pathLength >= filePath.Length) return "";
			return filePath.Substring(pathLength + 1);
		}
	}
}
