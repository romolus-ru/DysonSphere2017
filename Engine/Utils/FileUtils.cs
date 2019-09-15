using System.IO;
using System.IO.Compression;

namespace Engine.Utils
{
	public static class FileUtils
	{
		public static void LoadString(string archiveName, string fileName, out string value)
		{
			value = null;
			if (!File.Exists(archiveName)) return;
			using (var zipToOpen = new FileStream(archiveName, FileMode.OpenOrCreate))
			using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read)) {
				foreach (ZipArchiveEntry entry in archive.Entries) {
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
			using (var zipToOpen = new FileStream(archiveName, FileMode.OpenOrCreate))
			using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update)) {
				ZipArchiveEntry entry = null;
				foreach (var searchEntry in archive.Entries) {
					if (searchEntry.FullName == fileName) { entry = searchEntry;break; }
				}
				if (entry == null) { entry = archive.CreateEntry(fileName, CompressionLevel.Optimal); }
				using (var stream = entry.Open())
				using (var writer = new StreamWriter(stream)) {
					writer.Write(value);
				}
			}
		}

		/// <summary>
		/// Считываем напрямую содержимое файла как строку
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string LoadStringFromFile(string fileName)
		{
            if (!File.Exists(fileName))
                return null;
			return File.ReadAllText(fileName);
		}

	}
}
