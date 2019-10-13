using Engine;
using Engine.Visualization;
using System;
using Submarines.GeometryEditor;
using Submarines.Items;
using Submarines.Maps;
using Submarines.Submarines;
using Submarines.MapEditor;
using Submarines.Maps.Spawns;
using System.Linq;

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
        private ViewItemMapEditor _vItemMapEditor;
        private ViewItemGlobalMapEditor _vItemMapRelationEditor;

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
			_vMenu.OnStartGame = StartGamePrepare;
			_vMenu.OnStartGeometryEditor = StartGeometryEditor;
            _vMenu.OnStartItemMapEditor = StartItemMapEditor;
            _vMenu.OnStartItemMapRelationEditor = StartItemMapRelationEditor;
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

        private void StartItemMapEditor()
        {
            _viewManager.RemoveView(_vMenu);
            _vMenu = null;

            _vItemMapEditor = new ViewItemMapEditor(_viewManager);
            _viewManager.AddView(_vItemMapEditor);
            _vItemMapEditor.OnCloseEditor = CloseItemMapEditor;
        }

        private void CloseItemMapEditor()
        {
            _viewManager.RemoveView(_vItemMapEditor);
            _vItemMapEditor = null;
            Start();
        }

        private void StartItemMapRelationEditor() {
            _viewManager.RemoveView(_vMenu);
            _vMenu = null;

            _vItemMapRelationEditor = new ViewItemGlobalMapEditor(_viewManager);
            _viewManager.AddView(_vItemMapRelationEditor);
            _vItemMapRelationEditor.OnCloseEditor = CloseItemMapRelationEditor;
        }

        private void CloseItemMapRelationEditor() {
            _viewManager.RemoveView(_vItemMapRelationEditor);
            _vItemMapRelationEditor = null;
            Start();
        }

        private string _mapName = "TM01";
        private int _mapSpawnId = -1;
        private ItemGlobalMap _globalMap;

        private void StartGamePrepare() {
            _viewManager.RemoveView(_vMenu);
            _vMenu = null;

            _globalMap = ItemsManager.LoadGlobalMap();
            StartGame();
        }

        private void StartGame()
		{
            ItemSubmarine itemSubmarine = (ItemSubmarine) ItemsManager.GetItemBase("SubmarineDefault");
            Submarine submarine = (Submarine)SubmarinesBuilder.Create(itemSubmarine);
            
            if (_mapSpawnId != -1) {// перемещаем корабль к точке
                var map1 = ItemsManager.GetMap(_mapName);
                var spawn = map1.MapSpawns.FirstOrDefault(p => p.Id == _mapSpawnId);
                submarine.SetStartValues(spawn.Point, spawn.StartAngle);
            }


            ShipController shipController = new ShipController(submarine);
            var map = MapsBuilder.CreateMap(_mapName, submarine, _globalMap);
            map.OnTeleport += Teleport;
			shipController.OnFire += map.PlayerShoot;

			_mtg = new ModelGame(map);
			_modelMainClient.AddModel(_mtg);
			_vtg = new ViewGame();
			_vtg.InitGame(_viewManager, map, shipController);
			_viewManager.AddView(_vtg);
			_vtg.OnExitPressed += Close;

		}

        private void Teleport(MapSpawnTeleport spawn) {
            _mapName = spawn.TargetMapCode;
            _mapSpawnId = spawn.TargetMapSpawnId;

            _modelMainClient.DelModel(_mtg);
            _mtg.Stop();
            _mtg = null;

            _viewManager.RemoveView(_vtg);
            _vtg.Clear();
            _vtg = null;

            StartGame();
        }

        private void Close()
		{
			StateClient.SaveState();
			_viewManager.Provider.Exit();
		}

	}
}