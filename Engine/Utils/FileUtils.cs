using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils
{
	public static class FileUtils
	{

		public static void LoadString(string archiveName, string fileName, out string value)
		{
			value = null;
			if (!File.Exists(archiveName)) return;
			using (var _zipToOpen = new FileStream(archiveName, FileMode.OpenOrCreate))
			using (var _archive = new ZipArchive(_zipToOpen, ZipArchiveMode.Read)) {
				foreach (ZipArchiveEntry entry in _archive.Entries) {
					if (entry.FullName == fileName) {
						using (var stream = entry.Open())
						using (var reader = new StreamReader(stream)) {
							value = reader.ReadToEnd();
							return;
						}
					}
				}
			}
		}

		public static void SaveString(string archiveName, string fileName, string value)
		{
			var path = Directory.GetParent(archiveName).FullName;
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			using (var _zipToOpen = new FileStream(archiveName, FileMode.OpenOrCreate))
			using (var _archive = new ZipArchive(_zipToOpen, ZipArchiveMode.Update)) {
				ZipArchiveEntry entry = null;
				foreach (var searchEntry in _archive.Entries) {
					if (searchEntry.FullName == fileName) { entry = searchEntry;break; }
				}
				if (entry == null) { entry = _archive.CreateEntry(fileName, CompressionLevel.Optimal); }
				using (var stream = entry.Open())
				using (var writer = new StreamWriter(stream)) {
					writer.Write(value);
				}
			}
		}




	}
}
