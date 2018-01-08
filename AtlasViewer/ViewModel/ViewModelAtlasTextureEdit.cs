using Engine.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AtlasViewer.ViewModel
{
	public class ViewModelAtlasTextureEdit : ViewModelBase
	{
		public string DialogResult = "None";
		private AtlasFiles _viewAtlasFile;
		private AtlasTextures _editingAtlasTexture;

		/// <summary>
		/// Для закрытия вспомогательных окон
		/// </summary>
		public event EventHandler RequestClose;

		/// <summary>
		/// Для обновления вспомогательных окон
		/// </summary>
		public event EventHandler RequestRefreshWindow;

		public string TextureName { get; set; }
		public string TextureDesctiption { get; set; }
		public long P1X { get; set; }
		public long P1Y { get; set; }
		public long P2X { get; set; }
		public long P2Y { get; set; }
		public string AtlasFileToView {
			get {
				var f = GetAtlasFileFullPath(_viewAtlasFile.AtlasFile);
				Debug.WriteLine("ff = " + f);
				if (!f.EndsWith(".png")) return "";
				return f;
			}
		}

		public ICommand StoreChangesCommand { get; set; }
		public ICommand CancelChangesCommand { get; set; }
		public ViewModelAtlasTextureEdit()
		{
		}

		public ViewModelAtlasTextureEdit(AtlasFiles atlasFile, AtlasTextures atlasTexture)
		{
			_viewAtlasFile = atlasFile;
			_editingAtlasTexture = atlasTexture;
			TextureName = _editingAtlasTexture.Name;
			TextureDesctiption = _editingAtlasTexture.Description;
			P1X = _editingAtlasTexture.P1X;
			P1Y = _editingAtlasTexture.P1Y;
			P2X = _editingAtlasTexture.P2X;
			P2Y = _editingAtlasTexture.P2Y;

			StoreChangesCommand = new RelayCommand(arg => StoreChanges());
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

		public static string GetAtlasFileFullPath(string file)
		{
			return Path.Combine(GetAtlasFilePath(), file);
		}

		public void StoreChanges()
		{
			var changes =
				_editingAtlasTexture.Name != TextureName ||
				_editingAtlasTexture.Description != TextureDesctiption ||
				_editingAtlasTexture.P1X != P1X ||
				_editingAtlasTexture.P1Y != P1Y ||
				_editingAtlasTexture.P2X != P2X ||
				_editingAtlasTexture.P2Y != P2Y;
			if (changes) {
				_editingAtlasTexture.Name = TextureName;
				_editingAtlasTexture.Description = TextureDesctiption;
				_editingAtlasTexture.P1X = P1X;
				_editingAtlasTexture.P1Y = P1Y;
				_editingAtlasTexture.P2X = P2X;
				_editingAtlasTexture.P2Y = P2Y;
				DialogResult = "Changed";
			}
			RequestClose(this, new EventArgs());
		}

	}
}
