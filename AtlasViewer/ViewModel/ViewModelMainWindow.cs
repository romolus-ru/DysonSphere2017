using DataSupportEF;
using Engine.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

// экран разделен на 2 части - слева дерево файлов, справа сам файл картинкой
// при нажатии или раскрытии подгружаются список текстур и каждая из них выводится на атласе прямоугольником

namespace AtlasViewer.ViewModel
{
	public class ViewModelMainWindow : ViewModelBase
	{

		private Random _rnd = new Random();

		/// <summary>
		/// Соединение с БД
		/// </summary>
		// TODO переделать на DataSupportEF
		// TODO сделать каскадное удаление атласа средствами DataSupport
		private DataSupportEF6 _db = null;

		#region AtlasFiles
		private ObservableCollection<AtlasFiles> _viewAtlasFiles;
		public ObservableCollection<AtlasFiles> ViewAtlasFiles {
			get { return _viewAtlasFiles; }
			set { _viewAtlasFiles = value; OnPropertyChanged("ViewAtlasFiles"); }
		}

		private AtlasFiles _selectedAtlasFile;
		public AtlasFiles SelectedAtlasFile {
			get { return _selectedAtlasFile; }
			set {
				_selectedAtlasFile = value;
				OnPropertyChanged("SelectedAtlasFile");
				OnPropertyChanged("EnableAtlasTextureButtons");
				OnPropertyChanged("EnableAtlasTextureMenuButtons");
			}
		}

		public ICommand AtlasFileAddNewCommand { get; set; }
		public ICommand AtlasFileEditCommand { get; set; }
		public ICommand AtlasFileDeleteCommand { get; set; }
		public ICommand AtlasFileSelectionChangedCommand { get; set; }
		#endregion

		#region AtlasTextures
		private ObservableCollection<AtlasTextures> _viewAtlasTextures;
		public ObservableCollection<AtlasTextures> ViewAtlasTextures {
			get { return _viewAtlasTextures; }
			set { _viewAtlasTextures = value; OnPropertyChanged("ViewAtlasTextures"); }
		}

		private AtlasTextures _selectedAtlasTexture;
		public AtlasTextures SelectedAtlasTexture {
			get { return _selectedAtlasTexture; }
			set {
				_selectedAtlasTexture = value;
				OnPropertyChanged("SelectedAtlasTexture");
				OnPropertyChanged("EnableAtlasTextureMenuButtons");
			}
		}

		public ICommand AtlasTextureAddNewCommand { get; set; }
		public ICommand AtlasTextureEditCommand { get; set; }
		public ICommand AtlasTextureDeleteCommand { get; set; }

		/// <summary>
		/// Разрешение операций
		/// </summary>
		public bool EnableAtlasTextureButtons { get { return SelectedAtlasFile != null; } }

		/// <summary>
		/// Разрешение пунктов всплывающего меню
		/// </summary>
		public bool EnableAtlasTextureMenuButtons { get { return SelectedAtlasTexture != null; } }


		#endregion


		public ViewModelMainWindow()
		{
			// создаём соединение с базой
			_db = new DataSupportEF6();

			AtlasFileAddNewCommand = new RelayCommand(arg => AtlasFilesAddNew());
			AtlasFileEditCommand = new RelayCommand(arg => AtlasFilesEdit());
			AtlasFileDeleteCommand = new RelayCommand(arg => AtlasFilesDelete());
			AtlasFileSelectionChangedCommand = new RelayCommand(arg => AtlasFilesSelectionChanged());
			ViewAtlasFiles = new ObservableCollection<AtlasFiles>();
			RefreshViewAtlasFiles();

			AtlasTextureAddNewCommand = new RelayCommand(arg => AtlasTexturesAddNew());
			AtlasTextureEditCommand = new RelayCommand(arg => AtlasTextureEdit());
			AtlasTextureDeleteCommand = new RelayCommand(arg => AtlasTextureDelete());
			ViewAtlasTextures = new ObservableCollection<AtlasTextures>();
			RefreshViewAtlasTextures();
		}

		#region AtlasFiles
		/// <summary>
		/// Обновляем список файлов
		/// </summary>
		private void RefreshViewAtlasFiles()
		{
			ViewAtlasFiles.Clear();
			var atlasFiles = _db.AtlasFilesGetAll();
			foreach (var cl1 in atlasFiles) {
				ViewAtlasFiles.Add(cl1);
			}
			SelectedAtlasFile = null;
		}

		public void AtlasFilesAddNew()
		{
			var newAtlasFile = new AtlasFiles();
			var vmAtlasFilesEdit = new ViewModelAtlasFileEdit(newAtlasFile);
			var dr = ViewService.RunAtlasFileEdit(vmAtlasFilesEdit);
			if (!dr) return;// иначе сохраняем
			_db.AddAtlasFile(newAtlasFile);
			ViewAtlasFiles.Add(newAtlasFile);
		}

		public void AtlasFilesEdit()
		{
			if (SelectedAtlasFile == null) return;
			var vmAtlasFileEdit = new ViewModelAtlasFileEdit(SelectedAtlasFile);
			var dr = ViewService.RunAtlasFilesEdit(vmAtlasFileEdit);
			if (!dr) return;// иначе сохраняем
			_db.AddAtlasFile(SelectedAtlasFile);
		}

		public void AtlasFilesDelete()
		{
			if (SelectedAtlasFile == null) return;
			_db.DeleteAtlasFile(SelectedAtlasFile);
			ViewAtlasFiles.Remove(SelectedAtlasFile);
			SelectedAtlasFile = null;
		}

		/// <summary>
		/// Обновляем список текстур если поменялся атлас
		/// </summary>
		private void AtlasFilesSelectionChanged()
		{
			RefreshViewAtlasTextures();
		}

		#endregion

		#region AtlasTextures
		/// <summary>
		/// Обновляем список
		/// </summary>
		private void RefreshViewAtlasTextures()
		{
			ViewAtlasTextures.Clear();
			if (SelectedAtlasFile == null) return;
			var atlasTextures = _db.GetAtlasTextures(SelectedAtlasFile.IdAtlasFile);
			foreach (var od1 in atlasTextures) {
				ViewAtlasTextures.Add(od1);
			}
			SelectedAtlasTexture = null;
		}

		public void AtlasTexturesAddNew()
		{
			if (SelectedAtlasFile == null) return;
			var newAtlasTexture = new AtlasTextures();
			var vmAtlasTextureEdit = new ViewModelAtlasTextureEdit(newAtlasTexture, SelectedAtlasFile);
			var dr = ViewService.RunAtlasTextureEdit(vmAtlasTextureEdit);
			if (!dr) return;// иначе сохраняем
			newAtlasTexture.AtlasFileId = SelectedAtlasFile.IdAtlasFile;
			_db.AddAtlasTexture(newAtlasTexture);
			ViewAtlasTextures.Add(newAtlasTexture);
		}

		public void AtlasTextureEdit()
		{
			if (SelectedAtlasTexture == null) return;
			if (SelectedAtlasFile == null) return;
			var vmAtlasTextureEdit = new ViewModelAtlasTextureEdit(SelectedAtlasTexture, SelectedAtlasFile);
			var dr = ViewService.RunAtlasTextureEdit(vmAtlasTextureEdit);
			if (!dr) return;// иначе сохраняем
			_db.AddAtlasTexture(SelectedAtlasTexture);
		}

		public void AtlasTextureDelete()
		{
			if (SelectedAtlasTexture == null) return;

			_db.DeleteAtlasTexture(SelectedAtlasTexture);
			ViewAtlasTextures.Remove(SelectedAtlasTexture);
			SelectedAtlasTexture = null;
		}
		#endregion

	}
}
