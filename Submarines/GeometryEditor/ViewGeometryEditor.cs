using System;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Helpers;
using Engine.Visualization;
using Submarines.Geometry;
using Submarines.Items;

namespace Submarines.GeometryEditor
{
	internal class ViewGeometryEditor : ViewComponent
	{
		public Action OnCloseEditor;
		private SelectGeometryWindow _selectGeometryWindow = null;
		private GeometryBase _geometry = null;
		private ViewManager _viewManager = null;

		тут. добавить масштаб и создание новой геометрии

		private int _mapX = 0;
		private int _mapY = 0;
		private int _dragNum = -1;// номер в списке - что бы заменить
		private int _dragCode = -1;// 0 или 1 (From or To)
		private LineInfo _dragLine;// линия которая будет изменена
		private Vector _dragCurrent;// текущая перемещаемая точка
		private Vector _dragStatic;// текущая точка которая остаётся неподвижной
		private int _dragMode = -1;// 0 - map 1 - point

		private const int MouseMinimalDistance = 10;

		public ViewGeometryEditor(ViewManager viewManager)
		{
			_viewManager = viewManager;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewGeometryEditor");

			var buttonCloseEditor = new ViewButton();
			AddComponent(buttonCloseEditor);
			buttonCloseEditor.InitButton(CloseEditor, "CloseEditor", "Закрыть редактор", Keys.X);
			buttonCloseEditor.SetParams(110, 15, 140, 25, "CloseEditor");
			buttonCloseEditor.InitTexture("textRB", "textRB");

			var selectGeometry = new ViewButton();
			AddComponent(selectGeometry);
			selectGeometry.InitButton(SelectGeometry, "SelectGeometry", "Выбрать геометрию", Keys.G);
			selectGeometry.SetParams(110, 35, 140, 25, "SelectGeometry");
			selectGeometry.InitTexture("textRB", "textRB");

			var buttonSaveGeometries = new ViewButton();
			AddComponent(buttonSaveGeometries);
			buttonSaveGeometries.InitButton(SaveGeometries, "Save", "Save", Keys.X);
			buttonSaveGeometries.SetParams(310, 15, 140, 25, "Save");
			buttonSaveGeometries.InitTexture("textRB", "textRB");

			var buttonAddLine = new ViewButton();
			AddComponent(buttonAddLine);
			buttonAddLine.InitButton(AddLine, "Add line", "Добавить линию", Keys.G);
			buttonAddLine.SetParams(310, 35, 140, 25, "AddLine");
			buttonAddLine.InitTexture("textRB", "textRB");

			GUIHelper.DraggableDefaultColor = Color.Transparent;
			GUIHelper.DraggableCursorOverColor = Color.Transparent;
			GUIHelper.DraggableDragModeColor = Color.White;
			var mover = new ViewDraggable();
			this.AddComponent(mover);
			mover.SetParams(0, 150, visualizationProvider.CanvasWidth - 10, visualizationProvider.CanvasHeight - 200, "mover");
			mover.OnMoveObjectRelative += DragMove;
			mover.OnDragModeStart += DragStart;
			mover.OnDragModeEnd += DragModeEnd;
		}

		private void DragStart()
		{
			_dragMode = _dragNum == -1 ? 0 : 1;
		}

		private void DragModeEnd()
		{
			_dragMode = -1;
			if (_dragNum == -1)
				return;

			// применяем координаты
			var minDist = _dragCurrent.DistanceTo(_geometry.Lines[0].From); // первоначальное значение, чтоб не с потолка брать
			var foundNum = -1;
			Vector nearPoint = new Vector(0, 0, 0);
			float curDist;
			for (int i = 0; i < _geometry.Lines.Count; i++) {
				if (i == _dragNum) continue;

				LineInfo line = _geometry.Lines[i];

				curDist = _dragCurrent.DistanceTo(line.From);
				if (curDist <= MouseMinimalDistance && curDist <= minDist) {
					minDist = curDist;
					foundNum = i;
					nearPoint = line.From;
				}
				curDist = _dragCurrent.DistanceTo(line.To);
				if (curDist <= MouseMinimalDistance && curDist <= minDist) {
					minDist = curDist;
					foundNum = i;
					nearPoint = line.To;
				}
			}

			if (foundNum != -1) {// меняем координаты на найденные ближайшие
				_dragCurrent = nearPoint;
			}

			// заменяем линию новой
			var newLine = new LineInfo(_dragCurrent, _dragStatic);
			_geometry.Lines[_dragNum] = newLine;
			_dragNum = -1;
		}

		private void DragMove(int dx, int dy)
		{
			if (_dragMode == 0) {
				_mapX += dx;
				_mapY += dy;
			}
			else {
				_dragCurrent = _dragCurrent.MoveRelative(dx, dy);
			}
		}

		private void AddLine()
		{

		}

		protected override void Cursor(int cursorX, int cursorY)
		{
			if (_dragMode != -1)
				return;
			if (_geometry == null || _geometry.Lines.Count == 0)
				return;

			var p = new Vector(cursorX - _mapX, cursorY - _mapY, 0);
			var minDist = p.DistanceTo(_geometry.Lines[0].From); // первоначальное значение, чтоб не с потолка брать
			var dragCode = 0;
			var dragNum = -1;
			Vector dragCurrent = new Vector(0, 0, 0);
			Vector dragStatic = new Vector(0, 0, 0);
			float curDist;
			for (int i = 0; i < _geometry.Lines.Count; i++) {
				LineInfo line = _geometry.Lines[i];

				curDist = p.DistanceTo(line.From);
				if (curDist <= MouseMinimalDistance && curDist <= minDist) {
					minDist = curDist;
					dragCode = 0;
					dragNum = i;
					dragStatic = line.To;
					dragCurrent = line.From;
				}
				curDist = p.DistanceTo(line.To);
				if (curDist <= MouseMinimalDistance && curDist <= minDist) {
					minDist = curDist;
					dragCode = 1;
					dragNum = i;
					dragStatic = line.From;
					dragCurrent = line.To;
				}
			}

			if (dragNum == -1) {
				_dragNum = -1;
				_dragCode = -1;
			}
			else {
				_dragNum = dragNum;
				_dragCode = dragCode;
				_dragLine = _geometry.Lines[_dragNum];
				_dragStatic = dragStatic;
				_dragCurrent = dragCurrent;
			}
		}

		private void SaveGeometries()
		{
			ItemsManager.SaveGeometries();
		}

		private void SelectGeometry()
		{
			_selectGeometryWindow = new SelectGeometryWindow();
			_selectGeometryWindow.InitWindow(_viewManager, SetGeometry, null);
		}

		public void SetGeometry(GeometryBase geometry)
		{
			_geometry = geometry;
			CorrectPosAndScale();
		}

		private void CorrectPosAndScale()
		{
			_mapX = 0;
			_mapY = 0;
			if (_geometry == null)
				return;

			float x = 0;
			float y = 0;
			foreach (var line in _geometry.Lines) {
				x += line.From.X;
				y += line.From.Y;
				x += line.To.X;
				y += line.To.Y;
			}

			x /= (_geometry.Lines.Count * 2);
			y /= (_geometry.Lines.Count * 2);
			_mapX = (int) (-x + VisualizationProvider.CanvasWidth / 2f);
			_mapY = (int) (-y + VisualizationProvider.CanvasHeight / 2f);
		}

		private void CloseEditor()
		{
			OnCloseEditor?.Invoke();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			if (_geometry == null)
				return;

			visualizationProvider.OffsetAdd(_mapX, _mapY);
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Circle(0, 0, 10);
			foreach (var line in _geometry.Lines) {
				DrawLine(visualizationProvider, line.From, line.To);
			}

			if (_dragNum != -1) {
				visualizationProvider.SetColor(Color.Red);
				DrawLine(visualizationProvider, _dragLine.From, _dragLine.To);

				if (_dragMode == 1) {
					visualizationProvider.SetColor(Color.GreenYellow);
					DrawLine(visualizationProvider, _dragCurrent, _dragStatic);
				}
			}

			visualizationProvider.OffsetRemove();
		}

		private void DrawLine(VisualizationProvider visualizationProvider, Vector from, Vector to)
		{
			visualizationProvider.Line((int) from.X, (int) from.Y, (int) to.X, (int) to.Y);
		}

	}
}