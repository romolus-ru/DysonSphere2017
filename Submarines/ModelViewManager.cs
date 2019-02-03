using Engine;
using Engine.Visualization;
using System;
using Submarines.Submarines;

namespace Submarines
{
	/// <summary>
	/// Заставка, загрузка, начальное меню и запуск игр
	/// </summary>
	public class ModelViewManager
	{
		private ViewManager _viewManager;
		private ModelMainClient _modelMainClient;

		private ViewGame _vtg;
		private ModelGame _mtg;

		public Action OnExit;

		public void Start(ModelMainClient modelMainClient, ViewManager viewManager)
		{
			_modelMainClient = modelMainClient;
			_viewManager = viewManager;

			Submarine submarine = new Submarine(null, null);
			ShipController shipController = new ShipController(submarine);

			_mtg = new ModelGame(submarine, shipController);
			_modelMainClient.AddModel(_mtg);

			_vtg = new ViewGame();
			_vtg.InitGame(_viewManager, submarine, shipController);
			_viewManager.AddView(_vtg);
			_vtg.OnExitPressed += Close;

		}

		private void Close()
		{
			StateClient.SaveState();
			_viewManager.Provider.Exit();
		}

	}
}