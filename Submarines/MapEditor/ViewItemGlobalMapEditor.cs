﻿using Engine;
using Engine.Helpers;
using Engine.Visualization;
using Submarines.Editors;
using Submarines.Geometry;
using Submarines.Items;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Submarines.MapEditor
{
    /// <summary>
    /// Редактирование глобального представления карты - локации и переходы между ними
    /// </summary>
    internal class ViewItemGlobalMapEditor : ViewComponent
    {
        private enum DragSelectionType { None, Map, MapPoint, MapRelation, }
        private enum SelectionMode { None, SelectPoint1, SelectPoint2, }
        public Action OnCloseEditor;
        private ItemGlobalMap _globalMap = null;

        private DragSelectionType _dragMode1 = DragSelectionType.None;
        private ItemMapPoint _dragMapPoint;// передвигать можно
        private ItemMapRelation _selectedRelation;// только для редактирования

        private SelectionMode _currentSelectionMode = SelectionMode.None;
        private ItemMapPoint _selectedPoint1;
        private ItemMapPoint _selectedPoint2;






        private SelectItemMapWindow _selectItemMapWindow = null;
        private ItemMap _map = null;
        private ItemMap.ItemMapSpawnPoint _mapSpawnSelected = null;
        private GeometryBase _mapGeometry;
        private ViewManager _viewManager = null;
        private float _currentZoom = 1;
        private int _currentZoomIndex = 0;

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

        public ViewItemGlobalMapEditor(ViewManager viewManager) {
            _viewManager = viewManager;
        }

        protected override void InitObject(VisualizationProvider visualizationProvider, Input input) {
            SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewItemGlobalMapEditor");

            _globalMap = ItemsManager.LoadGlobalMap();

            var buttonCloseEditor = new ViewButton();
            AddComponent(buttonCloseEditor);
            buttonCloseEditor.InitButton(CloseEditor, "CloseEditor", "Закрыть редактор", Keys.X);
            buttonCloseEditor.SetParams(110, 15, 140, 25, "CloseEditor");
            buttonCloseEditor.InitTexture("textRB", "textRB");

            var newItemMapPoint = new ViewButton();
            AddComponent(newItemMapPoint);
            newItemMapPoint.InitButton(NewItemMapPoint, "NewItemMapPoint", "Создать новую точку на карте", Keys.N);
            newItemMapPoint.SetParams(110, 90, 140, 25, "NewItemMapPoint");
            newItemMapPoint.InitTexture("textRB", "textRB");

            var newItemMapRelationt = new ViewButton();
            AddComponent(newItemMapRelationt);
            newItemMapRelationt.InitButton(NewItemMapRelation, "NewItemMapRelation", "Создать новую связь между точками на карте", Keys.N);
            newItemMapRelationt.SetParams(110, 90, 140, 25, "NewItemMapRelation");
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
        }

        private void NewItemMapRelation() {
            _currentSelectionMode = SelectionMode.SelectPoint1;
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
            if (_dragMode1 != DragSelectionType.None)
                return;
            if (_globalMap == null || _globalMap.MapPoints.Count == 0)
                return;

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
                    _dragMode1 = DragSelectionType.MapPoint;
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
                    _dragMode1 = DragSelectionType.MapRelation;
                }
            }
            тут
            if (dragNum == -1) {
                _dragNum = -1;
            } else {
                _dragNum = dragNum;
                _mapSpawnSelected = _map.MapSpawns[_dragNum];
                _dragStatic = dragStatic;
                _dragCurrent = dragCurrent;
            }
        }

        private void SaveGlobalMap() {
            ItemsManager.SaveGlobalMap(_globalMap);
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
                if (_map.MapSpawns.Count > 0) {
                    visualizationProvider.SetColor(Color.DarkOrchid);
                    foreach (var spawn in _map.MapSpawns) {
                        DrawSpawn(visualizationProvider, spawn.Point);
                    }
                }
            }

            if (_dragNum != -1) {
                visualizationProvider.SetColor(Color.Red);
                DrawSpawn(visualizationProvider, _mapSpawnSelected.Point);

                if (_dragMode == 1) {
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

    }
}