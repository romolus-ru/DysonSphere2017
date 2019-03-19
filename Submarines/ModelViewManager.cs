using Engine;
using Engine.Visualization;
using System;
using Submarines.GeometryEditor;
using Submarines.Items;
using Submarines.Maps;
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

		private ViewMainMenu _vMenu;

		private ViewGeometryEditor _vGeometryEditor;

		public Action OnExit;

		public void Start(ModelMainClient modelMainClient, ViewManager viewManager)
		{
			_modelMainClient = modelMainClient;
			_viewManager = viewManager;
			Start();
		}

		private void Start()
		{
			_vMenu = new ViewMainMenu();
			_viewManager.AddView(_vMenu);
			_vMenu.OnStartGame = StartGame;
			_vMenu.OnStartGeometryEditor = StartGeometryEditor;
		}

		private void StartGeometryEditor()
		{
			_viewManager.RemoveView(_vMenu);
			_vMenu = null;

			_vGeometryEditor = new ViewGeometryEditor(_viewManager);
			_viewManager.AddView(_vGeometryEditor);
			_vGeometryEditor.OnCloseEditor = CloseGeometryEditor;
		}

		private void CloseGeometryEditor()
		{
			_viewManager.RemoveView(_vGeometryEditor);
			_vGeometryEditor = null;
			Start();
		}

		private void StartGame()
		{
			_viewManager.RemoveView(_vMenu);
			_vMenu = null;

			ItemSubmarine itemSubmarine = (ItemSubmarine) ItemsManager.GetItemBase("SubmarineDefault");
			Submarine submarine = (Submarine) SubmarinesBuilder.Create(itemSubmarine);
			ShipController shipController = new ShipController(submarine);
			var mapInfo = ItemsManager.GetMap("Test");
			var map = MapsBuilder.CreateMap(mapInfo, submarine);

			_mtg = new ModelGame(map);
			_modelMainClient.AddModel(_mtg);
			_vtg = new ViewGame();
			_vtg.InitGame(_viewManager, map, shipController);
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