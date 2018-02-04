// TODO сделать добавление текстуры
// TODO сделать на форме редактирования текстуры вывод атласа и координат текстуры
// TODO сделать визуализацию положения текстур на атласе
// TODO сделать каскадное удаление атласа средствами DataSupport

using AtlasViewer.Model.Entities;
using DataSupportEF;
using Engine.Data;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AtlasViewer.ViewModel
{
	public class ViewModelMainWindow : ViewModelBase
	{
			//попробовать взять отсюда пример - получение координат с формы
			//https://stackoverflow.com/questions/6059894/how-draw-rectangle-in-wpf

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
				OnPropertyChanged("AtlasFileToView");
				OnPropertyChanged("AtlasFileToViewWidth");
				OnPropertyChanged("AtlasFileToViewHeight");
			}
		}

		public string AtlasFileToView {
			get {
				var f = AtlasUtils.GetAtlasFileFullPath(_selectedAtlasFile.AtlasFileData.AtlasFile);
				if (!f.EndsWith(".png")) return "";
				return f;
			}
		}

		public long AtlasFileToViewWidth {
			get {
				if (_selectedAtlasFile == null) return 0;
				return (long)(_selectedAtlasFile.AtlasFileData.Width / Utils.PixelSize);
			}
		}
		public long AtlasFileToViewHeight {
			get {
				if (_selectedAtlasFile == null) return 0;
				return (long)(_selectedAtlasFile.AtlasFileData.Height / Utils.PixelSize);
			}
		}

		public ICommand AtlasFileAddNewCommand { get; set; }
		public ICommand AtlasFileEditCommand { get; set; }
		public ICommand AtlasFileSplitCommand { get; set; }
		public ICommand AtlasFileDeleteCommand { get; set; }
		public ICommand AtlasFileSelectionChangedCommand { get; set; }
		#endregion

		#region AtlasTextures
		private List<AtlasTextures> _atlasTextures;
		private ObservableCollection<ModelAtlasTexture> _viewAtlasTextures;
		public ObservableCollection<ModelAtlasTexture> ViewAtlasTextures {
			get { return _viewAtlasTextures; }
			set { _viewAtlasTextures = value; OnPropertyChanged("ViewAtlasTextures"); }
		}

		private ModelAtlasTexture _selectedAtlasTexture;
		public ModelAtlasTexture SelectedAtlasTexture {
			get { return _selectedAtlasTexture; }
			set {
				_selectedAtlasTexture = value;
				ViewService.ShowAtlasTextures(ViewAtlasTextures.ToList(), _selectedAtlasTexture);
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
			AtlasFileSplitCommand= new RelayCommand(arg => AtlasFilesSplit());
			AtlasFileDeleteCommand = new RelayCommand(arg => AtlasFilesDelete());
			AtlasFileSelectionChangedCommand = new RelayCommand(arg => AtlasFilesSelectionChanged());
			ViewAtlasFiles = new ObservableCollection<ModelAtlasFile>();
			_atlasFiles = _db.AtlasFilesGetAll();
			RefreshViewAtlasFiles();

			AtlasTextureAddNewCommand = new RelayCommand(arg => AtlasTexturesAddNew());
			AtlasTextureEditCommand = new RelayCommand(arg => AtlasTextureEdit());
			AtlasTextureDeleteCommand = new RelayCommand(arg => AtlasTextureDelete());
			ViewAtlasTextures = new ObservableCollection<ModelAtlasTexture>();
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

		public void AtlasFilesSplit()
		{
			if (SelectedAtlasFile == null) return;
			var values = new Dictionary<string, int> { { "Height", 0 }, { "Width", 0 } };
			var vmSplitCount = new ViewModelSplitCount(values);
			var dr = ViewService.RunSplitCount(vmSplitCount);
			var w = values["Width"];
			var h = values["Height"];
			if (w == 0 || h == 0) return;
			var stepW = _selectedAtlasFile.AtlasFileData.Width / w;
			var stepH = _selectedAtlasFile.AtlasFileData.Height / h;
			for (int i = 0; i < w; i++) {
				for (int j = 0; j < h; j++) {
					var x1 = j * stepW;
					var y1 = i * stepH;
					var newAtlasTexture = new AtlasTextures();
					newAtlasTexture.AtlasFileId = SelectedAtlasFile.AtlasFileData.IdAtlasFile;
					newAtlasTexture.Name = "t" + (w * i + j + 1);
					newAtlasTexture.Description = newAtlasTexture.Name + " auto";
					newAtlasTexture.P1X = x1;
					newAtlasTexture.P1Y = y1;
					newAtlasTexture.P2X = x1 + stepW;
					newAtlasTexture.P2Y = y1 + stepH;
					_db.AddAtlasTexture(newAtlasTexture);
					ViewAtlasTextures.Add(new ModelAtlasTexture(newAtlasTexture));
				}
			}
			//_db.AddAtlasFile(SelectedAtlasFile.AtlasFileData);
			//RefreshViewAtlasTextures();
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
			_atlasTextures = _db.GetAtlasTextures(SelectedAtlasFile.AtlasFileData.IdAtlasFile);
			foreach (var od1 in _atlasTextures) {
				ViewAtlasTextures.Add(new ModelAtlasTexture(od1));
			}
			SelectedAtlasTexture = null;
			ViewService.ShowAtlasTextures(ViewAtlasTextures.ToList());
		}

		public void AtlasTexturesAddNew()
		{
			if (SelectedAtlasFile == null) return;
			var newAtlasTexture = new AtlasTextures();
			var vmAtlasTextureEdit = new ViewModelAtlasTextureEdit(SelectedAtlasFile.AtlasFileData, newAtlasTexture);
			var dr = ViewService.RunAtlasTextureEdit(vmAtlasTextureEdit);
			if (!dr) return;// иначе сохраняем
			newAtlasTexture.AtlasFileId = SelectedAtlasFile.AtlasFileData.IdAtlasFile;
			_db.AddAtlasTexture(newAtlasTexture);
			_atlasTextures.Add(newAtlasTexture);
			ViewAtlasTextures.Add(new ModelAtlasTexture(newAtlasTexture));
		}

		public void AtlasTextureEdit()
		{
			if (SelectedAtlasTexture == null) return;
			if (SelectedAtlasFile == null) return;
			var vmAtlasTextureEdit = new ViewModelAtlasTextureEdit(SelectedAtlasFile.AtlasFileData, SelectedAtlasTexture.AtlasTextureData);
			var dr = ViewService.RunAtlasTextureEdit(vmAtlasTextureEdit);
			if (!dr) return;// иначе сохраняем
			_db.AddAtlasTexture(SelectedAtlasTexture.AtlasTextureData);
			SelectedAtlasTexture.OnPropertyChanged("");
			ViewService.ShowAtlasTextures(ViewAtlasTextures.ToList(), SelectedAtlasTexture);
		}

		public void AtlasTextureDelete()
		{
			if (SelectedAtlasTexture == null) return;

			_db.DeleteAtlasTexture(SelectedAtlasTexture.AtlasTextureData);
			_atlasTextures.Remove(SelectedAtlasTexture.AtlasTextureData);
			ViewAtlasTextures.Remove(SelectedAtlasTexture);
			SelectedAtlasTexture = null;
		}
		#endregion

	}
}
