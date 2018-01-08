﻿using Engine.Data;
using Engine.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AtlasViewer.ViewModel
{
	public class ViewModelAtlasFileEdit : ViewModelBase
	{
		public string DialogResult = "None";
		private AtlasFiles _editingAtlasFile;

		/// <summary>
		/// Для закрытия вспомогательных окон
		/// </summary>
		public event EventHandler RequestClose;

		/// <summary>
		/// Для обновления вспомогательных окон
		/// </summary>
		public event EventHandler RequestRefreshWindow;

		public string AtlasName { get; set; }
		private string _atlasFile;
		public string AtlasFile {
			get { return _atlasFile; }
			set {
				if (_atlasFile == value) return;
				_atlasFile = value;
				OnPropertyChanged("AtlasFile");
				OnPropertyChanged("AtlasFileToView");
				GetAtlasSize();
				OnPropertyChanged("AtlasSize");
			}
		}

		private void GetAtlasSize()
		{
			try {
				var file = AtlasUtils.GetAtlasFileFullPath(AtlasFile);
				var a = new BitmapImage(new Uri(file));
				if (a != null) {
					_atlasWidth = a.PixelWidth;
					_atlasHeight = a.PixelHeight;
				}
			}
			catch (Exception) {
				
			}
		}

		public string AtlasFileToView { get {
				var f = AtlasUtils.GetAtlasFileFullPath(AtlasFile);
				if (!f.EndsWith(".png")) return "";
				return f;
			}
		}

		private int _atlasWidth { get; set; }
		private int _atlasHeight { get; set; }
		public string AtlasSize {
			get { return "размер " + _atlasWidth + ":" + _atlasHeight; }
		}

		public string Size { get; set; }

		public ICommand StoreChangesCommand { get; set; }
		public ICommand CancelChangesCommand { get; set; }
		public ICommand SelectFileCommand { get; set; }
		public ViewModelAtlasFileEdit()
		{
		}

		public ViewModelAtlasFileEdit(AtlasFiles atlasFile)
		{
			_editingAtlasFile = atlasFile;
			AtlasName = atlasFile.AtlasName;
			AtlasFile = atlasFile.AtlasFile;
			StoreChangesCommand = new RelayCommand(arg => StoreChanges());
			SelectFileCommand = new RelayCommand(arg => SelectFile());
		}

		private void SelectFile()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = false;
			openFileDialog.InitialDirectory = AtlasUtils.GetAtlasFilePath();
			openFileDialog.Filter = "All files (*.*)|*.*";
			var file = "";
			if (openFileDialog.ShowDialog() == true) {
				var fl= openFileDialog.FileName;
				file = AtlasUtils.GetAtlasFileShortPath(fl);
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
