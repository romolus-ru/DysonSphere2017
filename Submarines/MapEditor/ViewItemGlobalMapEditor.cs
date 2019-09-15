using Engine;
using Engine.Helpers;
using Engine.Visualization;
using Submarines.Editors;
using Submarines.Geometry;
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
        private float _currentZoom = 1;
        private int _currentZoomIndex = 0;

        private int _mapX = 0;
        private int _mapY = 0;
        private Vector _dragCurrent;// текущая перемещаемая точка
        private float _dxZoomed;
        private float _dyZoomed;

        private const int MouseMinimalDistance = 10;

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

            var mover = new ViewDraggable();
            AddComponent(mover);
            mover.SetParams(0, 150, visualizationProvider.CanvasWidth - 10, visualizationProvider.CanvasHeight - 200, "mover");
            mover.OnMoveObjectRelative += DragMove;
            mover.OnDragModeStart += DragStart;
            mover.OnDragModeEnd += DragModeEnd;
            CorrectPosAndScale();
        }

        private void NewItemMapPoint() {
            _selectItemMapWindow = new SelectItemMapWindow();
            _selectItemMapWindow.InitWindow(_viewManager, GetItemMap, null);
        }

        private void GetItemMap(ItemMap itemMap) {
            var mapPoint = new ItemMapPoint();
            mapPoint.Point = new Vector(0, 0, 0);
            mapPoint.PointId = _globalMap.GetNewId();
            mapPoint.PointName = "NewPoint" + mapPoint.PointId + " " + itemMap.MapCode;
            mapPoint.MapCode = itemMap.MapCode;
            new DataEditor<ItemMapPoint>().InitWindow(_viewManager, mapPoint, AddItemMap);
        }

        private void AddItemMap(ItemMapPoint newItemMapPoint) {
            _globalMap.MapPoints.Add(newItemMapPoint);
            CorrectPosAndScale();
        }

        private void NewItemMapRelation() {
            _currentRelationSelectionMode = RelationSelectionMode.SelectPoint1;
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
            if (_selectionType == SelectionType.MapPoint && _dragMapPoint != null) {
                _dragMapPoint.Point = _dragCurrent;
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
                var point1 = _globalMap.GetPointById(mapRelation.MapPointId1).Point;
                var point2 = _globalMap.GetPointById(mapRelation.MapPointId2).Point;
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

        сделать класс для кэширования данных на основе словаря
        private Dictionary<int, Vector> _idPoints = new Dictionary<int, Vector>();
        private void DrawLine2(VisualizationProvider visualizationProvider, int id1, int id2, List<ItemMapPoint> points) {
            var p1 = GetPoint(id1, points);
            var p2 = GetPoint(id2, points);
            visualizationProvider.Line(
                (int)(p1.X * _currentZoom), (int)(p1.Y * _currentZoom),
                (int)(p2.X * _currentZoom), (int)(p2.Y * _currentZoom));
        }

        private Vector GetPoint(int id, List<ItemMapPoint> points) {
            Vector point;
            if (!_idPoints.TryGetValue(id, out point)){
                point = points.FirstOrDefault(p => p.PointId == id).Point;
                _idPoints.Add(id, point);
            }
            return point;
        }

    }
}