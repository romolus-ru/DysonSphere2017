// TODO сделать добавление текстуры
// TODO сделать на форме редактирования текстуры вывод атласа и координат текстуры
// TODO сделать визуализацию положения текстур на атласе
// TODO сделать каскадное удаление атласа средствами DataSupport

using AtlasViewer.Model.Entities;
using DataSupportEF;
using Engine.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AtlasViewer.ViewModel
{
	public class ViewModelMainWindow : ViewModelBase
	{

		private Random _rnd = new Random();

		/// <summary>
		/// Соединение с БД
		/// </summary>
		private DataSupportEF6 _db = null;

		#region AtlasFiles
		private List<AtlasFiles> _atlasFiles;// для хранения всех атласов из БД
		private ObservableCollection<ModelAtlasFile> _viewAtlasFiles;// хранение атласов для вывода на экран
		public ObservableCollection<ModelAtlasFile> ViewAtlasFiles {
			get { return _viewAtlasFiles; }
			set { _viewAtlasFiles = value; OnPropertyChanged("ViewAtlasFiles"); }
		}

		private ModelAtlasFile _selectedAtlasFile;
		public ModelAtlasFile SelectedAtlasFile {
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
			ViewAtlasFiles = new ObservableCollection<ModelAtlasFile>();
			_atlasFiles = _db.AtlasFilesGetAll();
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
			foreach (var cl1 in _atlasFiles) {
				var maf = new ModelAtlasFile(cl1);
				ViewAtlasFiles.Add(maf);
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
			_atlasFiles.Add(newAtlasFile);
			RefreshViewAtlasFiles();
		}

		public void AtlasFilesEdit()
		{
			if (SelectedAtlasFile == null) return;
			var vmAtlasFileEdit = new ViewModelAtlasFileEdit(SelectedAtlasFile.AtlasFileData);
			var dr = ViewService.RunAtlasFileEdit(vmAtlasFileEdit);
			if (!dr) return;// иначе сохраняем
			_db.AddAtlasFile(SelectedAtlasFile.AtlasFileData);
			RefreshViewAtlasFiles();
		}

		public void AtlasFilesDelete()
		{
			if (SelectedAtlasFile == null) return;
			_db.DeleteAtlasFile(SelectedAtlasFile.AtlasFileData);
			ViewAtlasFiles.Remove(SelectedAtlasFile);
			SelectedAtlasFile = null;
			RefreshViewAtlasFiles();
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
			var atlasTextures = _db.GetAtlasTextures(SelectedAtlasFile.AtlasFileData.IdAtlasFile);
			foreach (var od1 in atlasTextures) {
				ViewAtlasTextures.Add(od1);
			}
			SelectedAtlasTexture = null;
		}

		public void AtlasTexturesAddNew()
		{
			/*if (SelectedAtlasFile == null) return;
			var newAtlasTexture = new AtlasTextures();
			var vmAtlasTextureEdit = new ViewModelAtlasTextureEdit(newAtlasTexture, SelectedAtlasFile);
			var dr = ViewService.RunAtlasTextureEdit(vmAtlasTextureEdit);
			if (!dr) return;// иначе сохраняем
			newAtlasTexture.AtlasFileId = SelectedAtlasFile.IdAtlasFile;
			_db.AddAtlasTexture(newAtlasTexture);
			ViewAtlasTextures.Add(newAtlasTexture);*/
		}

		public void AtlasTextureEdit()
		{
			/*if (SelectedAtlasTexture == null) return;
			if (SelectedAtlasFile == null) return;
			var vmAtlasTextureEdit = new ViewModelAtlasTextureEdit(SelectedAtlasTexture, SelectedAtlasFile);
			var dr = ViewService.RunAtlasTextureEdit(vmAtlasTextureEdit);
			if (!dr) return;// иначе сохраняем
			_db.AddAtlasTexture(SelectedAtlasTexture);*/
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
