using Engine;
using Engine.Visualization;
using System;

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

			Ship ship = new Ship();
			ShipController shipController = new ShipController(ship);

			_mtg = new ModelGame(ship, shipController);
			_modelMainClient.AddModel(_mtg);

			_vtg = new ViewGame();
			_vtg.InitGame(_viewManager, ship, shipController);
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