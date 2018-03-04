using Engine;
using Engine.Models;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient
{
	/// <summary>
	/// Заставка, загрузка, начальное меню и запуск игр
	/// </summary>
	public class ModelClientManager
	{
		private Stopwatch _stopwatch;

		private ViewManager _viewManager;
		private ModelMainClient _modelMainClient;

		private GameView _gv;
		private ViewProgressBar _vp;

		public Action OnExit;

		public void Start(ModelMainClient modelMainClient, ViewManager viewManager)
		{
			_stopwatch = Stopwatch.StartNew();
			_modelMainClient = modelMainClient;
			_viewManager = viewManager;

			ModelIntro mi = new ModelIntro();
			mi.OnComplete += IntroEnded;
			_modelMainClient.AddModel(mi);

			_gv = new GameView();
			_viewManager.AddView(_gv);
			_gv.SetParams(0, 0, _viewManager.Provider.CanvasWidth, _viewManager.Provider.CanvasHeight, "Заставка");
		}

		private void IntroEnded(Model introModel)
		{
			_modelMainClient.DelModel(introModel);
			_viewManager.RemoveView(_gv);

			ModelLoadResources ml = new ModelLoadResources();
			ml.OnComplete += ResourcesLoaded;
			ml.OnProgress += ResourcesProgress;
			_modelMainClient.AddModel(ml);

			_vp = new ViewProgressBar();
			_viewManager.AddView(_vp);
			_vp.SetParams(0, 0, _viewManager.Provider.CanvasWidth, _viewManager.Provider.CanvasHeight, "Загрузка");
		}

		private void ResourcesProgress(int progress)
		{
			_vp.Percent = progress;
		}

		private void ResourcesLoaded(Model loadModel)
		{
			_modelMainClient.DelModel(loadModel);
			_viewManager.RemoveView(_vp);

			var mmenu = new ModelMenu(_stopwatch, _modelMainClient, _viewManager);
			mmenu.OnExitPressed += Close;
		}

		private void Close()
		{
			StateClient.SaveState();
			_viewManager.Provider.Exit();
		}
	}
}
