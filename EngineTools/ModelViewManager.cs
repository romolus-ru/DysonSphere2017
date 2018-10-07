using Engine;
using Engine.Visualization;

namespace EngineTools
{
	/// <summary>
	/// Для инструментов, запуск основных систем
	/// </summary>
	class ModelViewManager
	{
		private ViewManager _viewManager;
		private ModelMain _modelMain;
		
		private ViewTools _vt;
		private ModelTools _mt;
		
		public void Start(ModelMain modelMain, ViewManager viewManager, DataSupportBase dataSupport)
		{
			_modelMain = modelMain;
			_viewManager = viewManager;

			_mt = new ModelTools();
			_modelMain.AddModel(_mt);

			_vt = new ViewTools();
			_vt.InitTools(_viewManager, dataSupport);
			_viewManager.AddView(_vt);
			_vt.OnExitPressed += Close;
		}

		private void Close()
		{
			StateClient.SaveState();
			_viewManager.Provider.Exit();
		}
	}
}