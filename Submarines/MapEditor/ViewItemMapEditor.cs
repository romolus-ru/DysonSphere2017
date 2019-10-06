using Engine;
using Engine.Helpers;
using Engine.Visualization;
using Submarines.Items;
using System;
using System.Drawing;
using System.Windows.Forms;
using Submarines.Editors;
using Submarines.Geometry;
using Engine.Visualization.Scroll;
using System.Collections.Generic;

namespace Submarines.MapEditor
{
    // для редактора mapgeometry сделать переопределение и выбирать из геометрий для карт (SelectGeometryWindow)
    //переделать в редактор карты. выбор/сохранение карты, расстановка точек и задание им типов (респаун (+радиус) город, специальная точка, телепорт и т.п.)

    /// <summary>
    /// Просмотр карты и добавление,установка м редактирование респаунов
    /// </summary>
    internal class ViewItemMapEditor : ViewComponent
    {
        public Action OnCloseEditor;
        private SelectItemMapWindow _selectItemMapWindow = null;
        private ItemMap _map = null;
        private ItemMap.ItemMapSpawnPoint _mapSpawnSelected = null;
        private GeometryBase _mapGeometry;
        private ViewManager _viewManager = null;
        private float _currentZoom = 1;
        private int _currentZoomIndex = 0;
        private SpawnGeometryCache _cacheSpawnGeometry;

        /// <summary>
        /// После редактирования если код карты изменен надо обязательно поменять их у всех spawnPoints
        /// </summary>
        private string _oldMapCode = null;

        private int _mapX = 0;
        private int _mapY = 0;
        private int _dragNum = -1;// номер в списке - что бы заменить
        private Vector _dragCurrent;// текущая перемещаемая точка
        private Vector _dragStatic;// текущая точка которая остаётся неподвижной
        private int _dragMode = -1;// 0 - map 1 - point
        private float _dxZoomed;
        private float _dyZoomed;

        private const int MouseMinimalDistance = 10;

        public ViewItemMapEditor(ViewManager viewManager) {
            _viewManager = viewManager;
        }

        protected override void InitObject(VisualizationProvider visualizationProvider, Input input) {
            SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewItemMapEditor");

            var buttonCloseEditor = new ViewButton();
            AddComponent(buttonCloseEditor);
            buttonCloseEditor.InitButton(CloseEditor, "CloseEditor", "Закрыть редактор", Keys.X);
            buttonCloseEditor.SetParams(110, 15, 140, 25, "CloseEditor");
            buttonCloseEditor.InitTexture("textRB", "textRB");

            var selectGeometry = new ViewButton();
            AddComponent(selectGeometry);
            selectGeometry.InitButton(SelectItemMap, "SelectItemMap", "Выбрать карту", Keys.I);
            selectGeometry.SetParams(110, 35, 140, 25, "SelectItemMap");
            selectGeometry.InitTexture("textRB", "textRB");

            var editItemMap = new ViewButton();
            AddComponent(editItemMap);
            editItemMap.InitButton(EditCurrentItemMap, "EditItemMap", "Редактировать информацию о карте", Keys.E);
            editItemMap.SetParams(110, 60, 140, 25, "EditItemMap");
            editItemMap.InitTexture("textRB", "textRB");

            var newMapItem = new ViewButton();
            AddComponent(newMapItem);
            newMapItem.InitButton(NewItemMap, "NewItemMap", "Создать новую Карту", Keys.N);
            newMapItem.SetParams(110, 90, 140, 25, "NewItemMap");
            newMapItem.InitTexture("textRB", "textRB");

            var newSpawnItem = new ViewButton();
            AddComponent(newSpawnItem);
            newSpawnItem.InitButton(NewSpawnItem, "NewSpawnItem", "Создать новую точку на карте", Keys.M);
            newSpawnItem.SetParams(110, 120, 140, 25, "NewSpawnItem");
            newSpawnItem.InitTexture("textRB", "textRB");

            var buttonSaveMaps = new ViewButton();
            AddComponent(buttonSaveMaps);
            buttonSaveMaps.InitButton(SaveMaps, "Save", "Save", Keys.X);
            buttonSaveMaps.SetParams(310, 15, 140, 25, "Save");
            buttonSaveMaps.InitTexture("textRB", "textRB");

            var buttonZoomPlus = new ViewButton();
            AddComponent(buttonZoomPlus);
            buttonZoomPlus.InitButton(ZoomPlus, "Zoom+", "Увеличить", Keys.Oemplus);
            buttonZoomPlus.SetParams(510, 15, 140, 25, "AddLine");
            buttonZoomPlus.InitTexture("textRB", "textRB");

            var buttonZoomMinus = new ViewButton();
            AddComponent(buttonZoomMinus);
            buttonZoomMinus.InitButton(ZoomMinus, "Zoom-", "Уменьшить", Keys.OemMinus);
            buttonZoomMinus.SetParams(510, 35, 140, 25, "AddLine");
            buttonZoomMinus.InitTexture("textRB", "textRB");

            GUIHelper.DraggableDefaultColor = Color.Transparent;
            GUIHelper.DraggableCursorOverColor = Color.Transparent;
            GUIHelper.DraggableDragModeColor = Color.White;

            var mover = new ViewDraggable();
            AddComponent(mover);
            mover.SetParams(0, 150, visualizationProvider.CanvasWidth - 10, visualizationProvider.CanvasHeight - 200, "mover");
            mover.OnMoveObjectRelative += DragMove;
            mover.OnDragModeStart += DragStart;
            mover.OnDragModeEnd += DragModeEnd;

            Input.AddKeyActionSticked(DeleteSelectedSpawn, Keys.Delete);
            _cacheSpawnGeometry = new SpawnGeometryCache();
        }

        protected override void ClearObject() {
            Input.RemoveKeyActionSticked(DeleteSelectedSpawn, Keys.Delete);
             base.ClearObject();
        }

        private void NewSpawnItem() {
            if (_map == null)
                return;
            var newSpawnPoint = _map.GenerateNewSpawn(0, 0);
            RunDataEditorSpawn(newSpawnPoint, update: AddNewSpawnPoint);
        }

        private void AddNewSpawnPoint(ItemMap.ItemMapSpawnPoint remove) {
            if (_map == null)
                return;
            _map.AddNewSpawn(remove);
        }

        private void DeleteSelectedSpawn() {
            if (_map?.MapSpawns == null && _mapSpawnSelected != null)
                _map.MapSpawns.Remove(_mapSpawnSelected);
        }

        private void EditCurrentItemMap() {
            if (_map == null)
                return;
            RunDataEditorMap(_map, null);
        }

        private void NewItemMap() {
            var itemMap = new ItemMap { MapName = "NewMap", MapCode = "NewMapCode" };
            RunDataEditorMap(itemMap, SaveNewMap);
        }
               
        private void SaveNewMap(ItemMap newMap) {
            ItemsManager.AddMap(newMap);
            SetItemMap(newMap);
        }

        private void RunDataEditorMap(ItemMap item, Action<ItemMap> update) {
            new DataEditor<ItemMap>()
                .AddEditor("SelectMapGeometry", typeof(GeometryScrollItem<>), InitMapFilter)
                .InitWindow(_viewManager, item, update: update);
        }

        private void InitMapFilter(ScrollItem scroll) {
            var s = scroll as GeometryScrollItem<ItemMap>;
            if (s != null) {
                var list = new List<GeometryType>();
                list.Add(GeometryType.Map);
                s.Filter = list;
            }
        }

        private void ZoomPlus() {
            _currentZoomIndex--;
            if (_currentZoomIndex < 0)
                _currentZoomIndex = 0;
            _currentZoom = GameConstants.ZoomValue[_currentZoomIndex];
        }

        private void ZoomMinus() {
            _currentZoomIndex++;
            if (_currentZoomIndex >= GameConstants.ZoomValue.Count)
                _currentZoomIndex = GameConstants.ZoomValue.Count - 1;
            _currentZoom = GameConstants.ZoomValue[_currentZoomIndex];
        }

        private void DragStart() {
            _dragMode = _dragNum == -1 ? 0 : 1;
        }

        private void DragModeEnd() {
            _dragMode = -1;
            if (_dragNum == -1)
                return;

            var total = _dragCurrent - _map.MapSpawns[_dragNum].Point;
            if (Math.Abs(total.X) < 3 && Math.Abs(total.Y) < 3)
                StartEditPoint(_dragNum);

            // применяем координаты
            var minDist = _dragCurrent.DistanceTo(_map.MapSpawns[0].Point);
            var foundNum = -1;
            Vector nearPoint = new Vector(0, 0, 0);
            var mmDist = MouseMinimalDistance;
            float curDist;
            for (int i = 0; i < _map.MapSpawns.Count; i++) {
                if (i == _dragNum)
                    continue;

                var point = _map.MapSpawns[i].Point;

                curDist = _dragCurrent.DistanceTo(point);
                if (curDist <= mmDist && curDist <= minDist) {
                    minDist = curDist;
                    foundNum = i;
                    nearPoint = point;
                }
            }

            if (foundNum != -1) {// меняем координаты на найденные ближайшие
                _dragCurrent = nearPoint;
            }

            // заменяем точку новой
            //var newLine = new LineInfo(_dragCurrent, _dragStatic);
            _map.MapSpawns[_dragNum].Point = _dragCurrent;
            _dragNum = -1;
            _dxZoomed = 0;
            _dyZoomed = 0;
        }

        private void DragMove(int dx, int dy) {
            _dxZoomed += (dx / _currentZoom);
            _dyZoomed += (dy / _currentZoom);
            int deltaX = (int)_dxZoomed;
            int deltaY = (int)_dyZoomed;
            _dxZoomed -= deltaX;// оставляем остаток - а то набежит ошибка и будет не круто
            _dyZoomed -= deltaY;// с другой стороны можно упростить - при делении получается умножение на целое число (на данный момент) и остатков нету
            if (_dragMode == 0) {
                _mapX += deltaX;
                _mapY += deltaY;
            } else {
                _dragCurrent = _dragCurrent.MoveRelative(deltaX, deltaY);
            }
        }

        protected override void Cursor(int cursorX, int cursorY) {
            if (_dragMode != -1)
                return;
            if (_map == null || _map.MapSpawns==null || _map.MapSpawns.Count == 0)
                return;

            var p = new Vector((cursorX - _mapX) / _currentZoom, (cursorY - _mapY) / _currentZoom, 0);
            var minDist = p.DistanceTo(_map.MapSpawns[0].Point);
            var dragNum = -1;
            Vector dragCurrent = new Vector(0, 0, 0);
            Vector dragStatic = new Vector(0, 0, 0);
            var mmDist = MouseMinimalDistance;
            float curDist;
            for (int i = 0; i < _map.MapSpawns.Count; i++) {
                var point = _map.MapSpawns[i].Point;

                curDist = p.DistanceTo(point);
                if (curDist <= mmDist && curDist <= minDist) {
                    minDist = curDist;
                    dragNum = i;
                    dragStatic = point;
                    dragCurrent = point;
                }
            }

            if (dragNum == -1) {
                _dragNum = -1;
            } else {
                _dragNum = dragNum;
                _mapSpawnSelected = _map.MapSpawns[_dragNum];
                _dragStatic = dragStatic;
                _dragCurrent = dragCurrent;
            }
        }

        private void SaveMaps() {
            ItemsManager.SaveMaps();
        }

        private void StartEditPoint(int dragNum) {
            RunDataEditorSpawn(_map.MapSpawns[dragNum], update: null);
        }

        private void RunDataEditorSpawn(ItemMap.ItemMapSpawnPoint item, Action<ItemMap.ItemMapSpawnPoint> update) {
            new DataEditor<ItemMap.ItemMapSpawnPoint>()
                .AddEditor("SelectSpawnGeometry", typeof(GeometryScrollItem<>), InitSpawnFilter)
                .InitWindow(_viewManager, item, update: update);
        }

        private void InitSpawnFilter(ScrollItem scroll) {
            var s = scroll as GeometryScrollItem<ItemMap.ItemMapSpawnPoint>;
            if (s != null) {
                var list = new List<GeometryType>();
                list.Add(GeometryType.Place);
                list.Add(GeometryType.Symbol);
                s.Filter = list;
            }
        }

        private void SelectItemMap() {
            _selectItemMapWindow = new SelectItemMapWindow();
            _selectItemMapWindow.InitWindow(_viewManager, SetItemMap, onClose: null, filter: null);
        }

        public void SetItemMap(ItemMap itemMap) {
            _map = itemMap;
            _mapGeometry = ItemsManager.GetGeometry(_map.MapGeometryName);
            CorrectPosAndScale();
        }

        private void CorrectPosAndScale() {
            _mapX = 0;
            _mapY = 0;
            if (_mapGeometry == null)
                return;

            float x = 0;
            float y = 0;
            foreach (var line in _mapGeometry.Lines) {
                x += line.From.X;
                y += line.From.Y;
                x += line.To.X;
                y += line.To.Y;
            }

            if (_mapGeometry.Lines.Count > 0) {
                x /= (_mapGeometry.Lines.Count * 2);
                y /= (_mapGeometry.Lines.Count * 2);
            }

            _mapX = (int)(-x + VisualizationProvider.CanvasWidth / 2f);
            _mapY = (int)(-y + VisualizationProvider.CanvasHeight / 2f);
        }

        private void CloseEditor() {
            OnCloseEditor?.Invoke();
        }

        public override void DrawObject(VisualizationProvider visualizationProvider) {
            if (_map == null)
                return;

            visualizationProvider.SetColor(Color.BlanchedAlmond);
            visualizationProvider.Print(510, 0, "Zoom 1:" + (1 / _currentZoom) + "x "
                                                + _mapX + ":" + _mapY);

            visualizationProvider.OffsetAdd(_mapX, _mapY);
            visualizationProvider.SetColor(Color.White);
            visualizationProvider.Circle(0, 0, 10);
            if (_mapGeometry != null) {
                foreach (var line in _mapGeometry.Lines) {
                    DrawLine(visualizationProvider, line.From, line.To);
                }
                if (_map.MapSpawns?.Count > 0) {
                    visualizationProvider.SetColor(Color.DarkOrchid);
                    foreach (var spawn in _map.MapSpawns) {
                        DrawSpawn(visualizationProvider, spawn);
                    }
                }
            }

            if (_dragNum != -1) {
                visualizationProvider.SetColor(Color.Red);
                DrawSpawn(visualizationProvider, _mapSpawnSelected);

                if (_dragMode == 1) {
                    visualizationProvider.SetColor(Color.GreenYellow);
                    DrawSpawnPoint(visualizationProvider, _dragCurrent);
                }
            }

            visualizationProvider.OffsetRemove();
        }

        private void DrawSpawn(VisualizationProvider visualizationProvider, ItemMap.ItemMapSpawnPoint spawn) {
            DrawSpawnPoint(visualizationProvider, spawn.Point);
            visualizationProvider.Print(
                (int)(spawn.Point.X * _currentZoom - 20), (int)(spawn.Point.Y * _currentZoom + 5),
                spawn.Id + " " + spawn.Name + " " + spawn.SpawnType + " " + spawn.Description);

            var geometry = _cacheSpawnGeometry.GetValue(spawn);
            if (geometry != null) {
                visualizationProvider.OffsetAdd((int)spawn.Point.X, (int)spawn.Point.Y);
                foreach (var line in geometry.Lines) {
                    DrawLine(visualizationProvider, line.From, line.To);
                }
                visualizationProvider.OffsetRemove();
            }
        }

        private void DrawSpawnPoint(VisualizationProvider visualizationProvider, Vector spawnPoint) {
            visualizationProvider.Circle((int)spawnPoint.X, (int)spawnPoint.Y, 8);
        }

        private void DrawLine(VisualizationProvider visualizationProvider, Vector from, Vector to) {
            visualizationProvider.Line(
                (int)(from.X * _currentZoom), (int)(from.Y * _currentZoom),
                (int)(to.X * _currentZoom), (int)(to.Y * _currentZoom));
        }

    }
}