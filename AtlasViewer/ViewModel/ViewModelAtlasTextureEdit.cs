﻿using Engine.Data;
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
	//тут. и форму для текстур тоже - там всё должно остаться (вывод атласа точно) и поверх отображаются координаты редактируемой текстуры
	class ViewModelAtlasTextureEdit : ViewModelBase
	{
		public string DialogResult = "None";
		private AtlasFiles _viewAtlasFile;
		private AtlasTextures _editingAtlasFile;

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
		public int P1X { get; set; }
		public int P1Y { get; set; }
		public int P2X { get; set; }
		public int P2Y { get; set; }
		тут добавить события
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
		public ICommand SelectFileCommand { get; set; }
		public ViewModelAtlasTextureEdit()
		{
		}

		public ViewModelAtlasTextureEdit(AtlasFiles atlasFile)
		{
			_editingAtlasFile = atlasFile;
			AtlasName = atlasFile.AtlasName;
			this.AtlasFile = atlasFile.AtlasFile;
			StoreChangesCommand = new RelayCommand(arg => StoreChanges());
			SelectFileCommand = new RelayCommand(arg => SelectFile());
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
			var pathLenght = path.Length;
			if (pathLenght >= filePath.Length) return "";
			return filePath.Substring(pathLenght + 1);
		}

		public static string GetAtlasFileFullPath(string file)
		{
			return Path.Combine(GetAtlasFilePath(), file);
		}

		private void SelectFile()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = false;
			openFileDialog.InitialDirectory = GetAtlasFilePath();
			openFileDialog.Filter = "All files (*.*)|*.*";
			var file = "";
			if (openFileDialog.ShowDialog() == true) {
				var fl = openFileDialog.FileName;
				file = GetAtlasFileShortPath(fl);
			}
			openFileDialog = null;
			if (!string.IsNullOrEmpty(file)) {
				this.AtlasFile = file;
				RequestRefreshWindow(this, new EventArgs());
			}

		}

		public void StoreChanges()
		{
			var changes =
				_editingAtlasFile.AtlasName != AtlasName ||
				_editingAtlasFile.Width != _atlasWidth ||
				_editingAtlasFile.Height != _atlasHeight ||
				_editingAtlasFile.AtlasFile != AtlasFile;
			if (changes) {
				_editingAtlasFile.AtlasName = AtlasName;
				_editingAtlasFile.Width = _atlasWidth;
				_editingAtlasFile.Height = _atlasHeight;
				_editingAtlasFile.AtlasFile = AtlasFile;
				DialogResult = "Changed";
			}
			RequestClose(this, new EventArgs());
		}

	}
}
