﻿using Engine;
using Engine.Visualization;
using System;
using Submarines.Items;
using Submarines.Submarines;

namespace Submarines
{
	/// <summary>
	/// Взаимодействие между моделью и визуализацией
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

			ItemSubmarine itemSubmarine = (ItemSubmarine)ItemsManager.GetItemBase("SubmarineDefault");
			Submarine submarine = (Submarine)SubmarinesBuilder.Create(itemSubmarine);
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