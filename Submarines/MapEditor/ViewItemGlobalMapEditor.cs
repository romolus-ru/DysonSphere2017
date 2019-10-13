using Engine;
using Engine.Extensions;
using Engine.Helpers;
using Engine.Visualization;
using Engine.Visualization.Scroll;
using Submarines.Editors;
using Submarines.Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Submarines.MapEditor
{
    /// <summary>
    /// Редактирование глобального представления карты - локации и переходы между ними
    /// </summary>
    internal class ViewItemGlobalMapEditor : ViewComponent
    {
        private enum SelectionType { None, Map, MapPoint, MapRelation, }
        private enum RelationSelectionMode { None, SelectPoint1, SelectPoint2, }
        public Action OnCloseEditor;
        private ItemGlobalMap _globalMap = null;

        private SelectionType _selectionType = SelectionType.None;
        private ItemMapPoint _dragMapPoint;// передвигать можно
        private bool _dragMode;
        private ItemMapRelation _selectedRelation;// только для редактирования

        private RelationSelectionMode _currentRelationSelectionMode = RelationSelectionMode.None;
        private ItemMapPoint _selectedPoint1;
        private ItemMapPoint _selectedPoint2;

        private SelectItemMapWindow _selectItemMapWindow = null;
        private ViewManager _viewManager = null;
        private ViewDraggable _mover = null;
        private float _currentZoom = 1;
        private int _currentZoomIndex = 0;

        private int _mapX = 0;
        private int _mapY = 0;
        private Vector _dragCurrent;// текущая перемещаемая точка
        private float _dxZoomed;
        private float _dyZoomed;

        private const int MouseMinimalDistance = 10;
        private ItemMapPointCache _pcache = new ItemMapPointCache();

        public ViewItemGlobalMapEditor(ViewManager viewManager) {
            _viewManager = viewManager;
        }

        protected override void InitObject(VisualizationProvider visualizationProvider, Input input) {
            SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewItemGlobalMapEditor");

            _globalMap = ItemsManager.LoadGlobalMap();

            var buttonCloseEditor = new ViewButton();
            AddComponent(buttonCloseEditor);
            buttonCloseEditor.InitButton(CloseEditor, "CloseEditor", "Закрыть редактор", Keys.X);
            buttonCloseEditor.SetParams(110, 10, 140, 25, "CloseEditor");
            buttonCloseEditor.InitTexture("textRB", "textRB");

            var newItemMapPoint = new ViewButton();
            AddComponent(newItemMapPoint);
            newItemMapPoint.InitButton(NewItemMapPoint, "NewItemMapPoint", "Создать новую точку на карте", Keys.N);
            newItemMapPoint.SetParams(110, 30, 140, 25, "NewItemMapPoint");
            newItemMapPoint.InitTexture("textRB", "textRB");

            var newItemMapRelationt = new ViewButton();
            AddComponent(newItemMapRelationt);
            newItemMapRelationt.InitButton(NewItemMapRelation, "NewItemMapRelation", "Создать новую связь между точками на карте", Keys.N);
            newItemMapRelationt.SetParams(110, 50, 140, 25, "NewItemMapRelation");
            newItemMapRelationt.InitTexture("textRB", "textRB");

            var buttonSaveGlobalMap = new ViewButton();
            AddComponent(buttonSaveGlobalMap);
            buttonSaveGlobalMap.InitButton(SaveGlobalMap, "Save", "Save", Keys.X);
            buttonSaveGlobalMap.SetParams(310, 15, 140, 25, "Save");
            buttonSaveGlobalMap.InitTexture("textRB", "textRB");

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

            _mover = new ViewDraggable();
            AddComponent(_mover);
            _mover.SetParams(0, 150, visualizationProvider.CanvasWidth - 10, visualizationProvider.CanvasHeight - 200, "mover");
            _mover.OnMoveObjectRelative += DragMove;
            _mover.OnDragModeStart += DragStart;
            _mover.OnDragModeEnd += DragModeEnd;

            Input.AddKeyActionSticked(ClearModes, Keys.Escape);

            CorrectPosAndScale();
            BuildCache();
        }

        protected override void ClearObject() {
            Input.RemoveKeyActionSticked(ClearModes, Keys.Delete);
            base.ClearObject();
        }

        private void ClearModes() {
            _currentRelationSelectionMode = RelationSelectionMode.None;
            _selectedPoint1 = null;
            _selectedPoint2 = null;
        }

        private void NewItemMapPoint() {
            _selectItemMapWindow = new SelectItemMapWindow();
            _selectItemMapWindow.InitWindow(_viewManager, onSelect: GetItemMap, onClose: null, filter: GetExistedMapCodes());
        }

        private List<string> GetExistedMapCodes() {
            if (_globalMap.MapPoints == null || _globalMap.MapPoints.Count <= 0)
                return null;
            var list = new List<string>();
            foreach (var mapPoint in _globalMap.MapPoints) {
                list.Add(mapPoint.MapCode);
            }
            return list;
        }
        private void GetItemMap(ItemMap itemMap) {
            var mapPoint = new ItemMapPoint();
            mapPoint.Point = new Vector(0, 0, 0);
            mapPoint.PointId = _globalMap.GetNewId();
            mapPoint.PointName = "NewPoint" + mapPoint.PointId + " " + itemMap.MapCode;
            mapPoint.MapCode = itemMap.MapCode;

            RunDataEditor(mapPoint, AddItemMap);
        }
        
        private void AddItemMap(ItemMapPoint newItemMapPoint) {
            _globalMap.MapPoints.Add(newItemMapPoint);
            CorrectPosAndScale();
        }

        private void StartEditPoint(ItemMapPoint point) {
            RunDataEditor(point, null);
        }

        private void RunDataEditor(ItemMapPoint point, Action<ItemMapPoint> update) {
            new DataEditor<ItemMapPoint>()
                .AddEditor("SelectMap", typeof(MapCodeScrollItem<>), InitCodeMapFilter)
                .InitWindow(_viewManager, point, update);
        }

        private void InitCodeMapFilter(ScrollItem scroll) {
            var s = scroll as MapCodeScrollItem<ItemMapPoint>;
            if (s != null) {
                s.Filter = GetExistedMapCodes();
            }
        }

        private void NewItemMapRelation() {
            _currentRelationSelectionMode = RelationSelectionMode.SelectPoint1;
        }

        private void CreateItemMapRelation(ItemMapPoint point1, ItemMapPoint point2) {
            var item = new ItemMapRelation();
            item.MapPointId1 = point1.PointId;
            item.MapPointId2 = point2.PointId;
            _selectedRelation = item;
            StartItemMapRelationEditor(item, AddItemMapRelation);
        }

        private void StartItemMapRelationEditor(ItemMapRelation relation, Action<ItemMapRelation> update) {
            new DataEditor<ItemMapRelation>()
                .AddEditor("SelectSpawnId1", typeof(MapSpawnIdScrollItem<>), InitMapSpawnValues1)
                .AddEditor("SelectSpawnId2", typeof(MapSpawnIdScrollItem<>), InitMapSpawnValues2)
                .InitWindow(_viewManager, relation, update: update);
        }

        private void InitMapSpawnValues1(ScrollItem scroll) {
            var s = scroll as MapSpawnIdScrollItem<ItemMapRelation>;
            if (s != null) {
                var mapCode1 = _globalMap.MapPoints.FirstOrDefault(m => m.PointId == _selectedRelation.MapPointId1)?.MapCode;
                if (!mapCode1.IsNullOrEmpty()) {
                    var map = ItemsManager.GetMap(mapCode1);
                    s.Map = map;
                }
            }
        }

        private void InitMapSpawnValues2(ScrollItem scroll) {
            var s = scroll as MapSpawnIdScrollItem<ItemMapRelation>;
            if (s != null) {
                var mapCode2 = _globalMap.MapPoints.FirstOrDefault(m => m.PointId == _selectedRelation.MapPointId2)?.MapCode;
                if (!mapCode2.IsNullOrEmpty()) {
                    var map = ItemsManager.GetMap(mapCode2);
                    s.Map = map;
                }
            }
        }

        private void AddItemMapRelation(ItemMapRelation relation) {
            _globalMap.MapRelations.Add(relation);
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
            _dragMode = true;
        }

        private void DragModeEnd() {
            _dragMode = false;

            if (_selectionType == SelectionType.MapRelation && _selectedRelation != null) {
                StartItemMapRelationEditor(_selectedRelation, null);
                return;
            }

            if (_currentRelationSelectionMode == RelationSelectionMode.SelectPoint1) {
                _selectedPoint1 = _dragMapPoint;
                _currentRelationSelectionMode = RelationSelectionMode.SelectPoint2;
                return;
            }

            if (_currentRelationSelectionMode == RelationSelectionMode.SelectPoint2) {
                _currentRelationSelectionMode = RelationSelectionMode.None;
                CreateItemMapRelation(_selectedPoint1, _dragMapPoint);
                return;
            }

            if (_selectionType == SelectionType.MapPoint && _dragMapPoint != null) {
                var total = _dragCurrent - _dragMapPoint.Point;
                if (Math.Abs(total.X) < 3 && Math.Abs(total.Y) < 3)
                    StartEditPoint(_dragMapPoint);
                else { 
                    _dragMapPoint.Point = _dragCurrent;
                    _pcache.Clear();
                }
            }
            _dragMapPoint = null;
            _dxZoomed = 0;
            _dyZoomed = 0;
        }

        private void DragMove(int dx, int dy) {
            _dxZoomed += (dx / _currentZoom);
            _dyZoomed += (dy / _currentZoom);
            int deltaX = (int)_dxZoomed;
            int deltaY = (int)_dyZoomed;
            _dxZoomed -= deltaX;
            _dyZoomed -= deltaY;
            switch (_selectionType) {
                case SelectionType.Map:
                    _mapX += deltaX;
                    _mapY += deltaY;
                    break;
                case SelectionType.MapPoint:
                    _dragCurrent = _dragCurrent.MoveRelative(deltaX, deltaY);
                    break;
            }
        }

        protected override void Cursor(int cursorX, int cursorY) {
            if (_dragMode)
                return;
            if (_globalMap == null || _globalMap.MapPoints.Count == 0)
                return;

            _selectionType = SelectionType.Map;
            _dragMapPoint = null;
            _selectedRelation = null;

            var p = new Vector((cursorX - _mapX) / _currentZoom, (cursorY - _mapY) / _currentZoom, 0);
            var minDist = p.DistanceTo(_globalMap.MapPoints[0].Point);
            var mmDist = MouseMinimalDistance;
            float curDist;
            for (int i = 0; i < _globalMap.MapPoints.Count; i++) {
                var mapPoint = _globalMap.MapPoints[i];
                var point = mapPoint.Point;

                curDist = p.DistanceTo(point);
                if (curDist <= mmDist && curDist <= minDist) {
                    minDist = curDist;
                    _dragMapPoint = mapPoint;
                    _dragCurrent = mapPoint.Point;
                    _selectionType = SelectionType.MapPoint;
                }
            }

            for (int i = 0; i < _globalMap.MapRelations.Count; i++) {
                var mapRelation = _globalMap.MapRelations[i];
                var point1 = _pcache.GetValue(mapRelation.MapPointId1);
                var point2 = _pcache.GetValue(mapRelation.MapPointId2);
                var point = (point1 + point2) / 2;

                curDist = p.DistanceTo(point);
                if (curDist <= mmDist && curDist <= minDist) {
                    minDist = curDist;
                    _dragMapPoint = null;
                    _selectedRelation = mapRelation;
                    _selectionType = SelectionType.MapRelation;
                }
            }

        }

        private void SaveGlobalMap() {
            ItemsManager.SaveGlobalMap(_globalMap);
        }

        private void CorrectPosAndScale() {
            _mapX = 0;
            _mapY = 0;

            float x = 0;
            float y = 0;
            foreach (var point in _globalMap.MapPoints) {
                x += point.Point.X;
                y += point.Point.Y;
            }

            if (_globalMap.MapPoints.Count > 0) {
                x /= (_globalMap.MapPoints.Count * 2);
                y /= (_globalMap.MapPoints.Count * 2);
            }

            _mapX = (int)(-x + VisualizationProvider.CanvasWidth / 2f);
            _mapY = (int)(-y + VisualizationProvider.CanvasHeight / 2f);
        }

        private void CloseEditor() {
            OnCloseEditor?.Invoke();
        }

        public override void DrawObject(VisualizationProvider visualizationProvider) {
            visualizationProvider.SetColor(Color.BlanchedAlmond);
            visualizationProvider.Print(510, 0, "Zoom 1:" + (1 / _currentZoom) + "x "
                                                + _mapX + ":" + _mapY);

            visualizationProvider.OffsetAdd(_mapX, _mapY);
            visualizationProvider.SetColor(Color.White);
            visualizationProvider.Circle(0, 0, 10);
            visualizationProvider.SetColor(Color.Coral);
            foreach (var point in _globalMap.MapPoints) {
                visualizationProvider.Circle((int)point.Point.X, (int)point.Point.Y, 11);
            }
            foreach (var line in _globalMap.MapRelations) {
                DrawLine2(visualizationProvider, line.MapPointId1, line.MapPointId2, _globalMap.MapPoints);
            }

            if (_dragMapPoint != null) {
                visualizationProvider.SetColor(Color.Red);
                DrawSpawn(visualizationProvider, _dragMapPoint.Point);

                if (_dragMode) {
                    visualizationProvider.SetColor(Color.GreenYellow);
                    DrawSpawn(visualizationProvider, _dragCurrent);
                }
            }

            if (_selectedRelation != null) {
                visualizationProvider.SetColor(Color.Red);
                DrawLine2(visualizationProvider, _selectedRelation.MapPointId1, _selectedRelation.MapPointId2, _globalMap.MapPoints);
            }

            visualizationProvider.OffsetRemove();
        }

        private void DrawSpawn(VisualizationProvider visualizationProvider, Vector spawnPoint) {
            visualizationProvider.Circle((int)spawnPoint.X, (int)spawnPoint.Y, 8);
            //visualizationProvider.Line(
            //    (int)(spawnPoint.X * _currentZoom), (int)(spawnPoint.Y * _currentZoom),
            //    (int)(spawnPoint.X * _currentZoom + 10), (int)(spawnPoint.Y * _currentZoom + 10));
        }

        private void DrawLine(VisualizationProvider visualizationProvider, Vector from, Vector to) {
            visualizationProvider.Line(
                (int)(from.X * _currentZoom), (int)(from.Y * _currentZoom),
                (int)(to.X * _currentZoom), (int)(to.Y * _currentZoom));
        }

        private void BuildCache() {
            _pcache.InitCache(_globalMap.MapPoints);
        }

        private void DrawLine2(VisualizationProvider visualizationProvider, int id1, int id2, List<ItemMapPoint> points) {
            var p1 = _pcache.GetValue(id1);
            var p2 = _pcache.GetValue(id2);
            visualizationProvider.Line(
                (int)(p1.X * _currentZoom), (int)(p1.Y * _currentZoom),
                (int)(p2.X * _currentZoom), (int)(p2.Y * _currentZoom));
        }

    }
}