using Engine;
using Engine.Visualization;
using System;
using System.Windows.Forms;

namespace Submarines.MapEditor
{

	переделать в редактор карты. выбор/сохранение карты, расстановка точек и задание им типов (респаун (+радиус) город, специальная точка, телепорт и т.п.)

	class ViewItemMapEditor : ViewComponent
	{
		public Action OnCloseEditor;
		private SelectGeometryWindow _selectGeometryWindow = null;
		private GeometryBase _geometry = null;
		private ViewManager _viewManager = null;
		private float _currentZoom = 1;
		private int _currentZoomIndex = 0;

		private int _mapX = 0;
		private int _mapY = 0;
		private int _dragNum = -1;// номер в списке - что бы заменить
		private int _dragCode = -1;// 0 или 1 (From or To)
		private LineInfo _dragLine;// линия которая будет изменена
		private Vector _dragCurrent;// текущая перемещаемая точка
		private Vector _dragStatic;// текущая точка которая остаётся неподвижной
		private int _dragMode = -1;// 0 - map 1 - point
		private float _dxZoomed;
		private float _dyZoomed;

		private const int MouseMinimalDistance = 10;

		public ViewItemMapEditor(ViewManager viewManager)
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

			var editGeometry = new ViewButton();
			AddComponent(editGeometry);
			editGeometry.InitButton(EditCurrentGeometry, "EditGeometry", "Редактировать информацию о геометрии", Keys.E);
			editGeometry.SetParams(110, 60, 140, 25, "EditGeometry");
			editGeometry.InitTexture("textRB", "textRB");

			var newGeometry = new ViewButton();
			AddComponent(newGeometry);
			newGeometry.InitButton(NewGeometry, "NewGeometry", "Создать новую геометрию", Keys.N);
			newGeometry.SetParams(110, 90, 140, 25, "NewGeometry");
			newGeometry.InitTexture("textRB", "textRB");

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

			Input.AddKeyActionSticked(DeleteLine, Keys.Delete);
		}

		protected override void ClearObject()
		{
			base.ClearObject();
			Input.RemoveKeyActionSticked(DeleteLine, Keys.Delete);
		}

		private void DeleteLine()
		{
			if (_dragNum == -1)
				return;
			_geometry.Lines.RemoveAt(_dragNum);
			_dragNum = -1;
			_dragCode = -1;
		}

		private void EditCurrentGeometry()
		{
			if (_geometry == null)
				return;
			new DataEditor<GeometryBase>().InitWindow(_viewManager, _geometry, null);
		}

		private void NewGeometry()
		{
			var newGeometry = new GeometryBase { Name = "NewGeometry", Color = Color.Red };
			new DataEditor<GeometryBase>().InitWindow(_viewManager, newGeometry, NewGeometryAdd);
		}

		private void NewGeometryAdd(GeometryBase newGeometry)
		{
			_geometry = newGeometry;
			ItemsManager.AddGeometry(newGeometry);
		}

		private void ZoomPlus()
		{
			_currentZoomIndex--;
			if (_currentZoomIndex < 0)
				_currentZoomIndex = 0;
			_currentZoom = GameConstants.ZoomValue[_currentZoomIndex];
		}

		private void ZoomMinus()
		{
			_currentZoomIndex++;
			if (_currentZoomIndex >= GameConstants.ZoomValue.Count)
				_currentZoomIndex = GameConstants.ZoomValue.Count - 1;
			_currentZoom = GameConstants.ZoomValue[_currentZoomIndex];
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
			var mmDist = MouseMinimalDistance;
			float curDist;
			for (int i = 0; i < _geometry.Lines.Count; i++) {
				if (i == _dragNum) continue;

				LineInfo line = _geometry.Lines[i];

				curDist = _dragCurrent.DistanceTo(line.From);
				if (curDist <= mmDist && curDist <= minDist) {
					minDist = curDist;
					foundNum = i;
					nearPoint = line.From;
				}
				curDist = _dragCurrent.DistanceTo(line.To);
				if (curDist <= mmDist && curDist <= minDist) {
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
			_dxZoomed = 0;
			_dyZoomed = 0;
		}

		private void DragMove(int dx, int dy)
		{
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

		private void AddLine()
		{
			LineInfo newLineInfo;
			newLineInfo.From = new Vector(0, 100 / _currentZoom, 0);
			newLineInfo.To = new Vector(0, -100 / _currentZoom, 0);
			_geometry.Lines.Add(newLineInfo);
		}

		protected override void Cursor(int cursorX, int cursorY)
		{
			if (_dragMode != -1)
				return;
			if (_geometry == null || _geometry.Lines.Count == 0)
				return;

			var p = new Vector((cursorX - _mapX) / _currentZoom, (cursorY - _mapY) / _currentZoom, 0);
			var minDist = p.DistanceTo(_geometry.Lines[0].From); // первоначальное значение, чтоб не с потолка брать
			var dragCode = 0;
			var dragNum = -1;
			Vector dragCurrent = new Vector(0, 0, 0);
			Vector dragStatic = new Vector(0, 0, 0);
			var mmDist = MouseMinimalDistance;
			float curDist;
			for (int i = 0; i < _geometry.Lines.Count; i++) {
				LineInfo line = _geometry.Lines[i];

				curDist = p.DistanceTo(line.From);
				if (curDist <= mmDist && curDist <= minDist) {
					minDist = curDist;
					dragCode = 0;
					dragNum = i;
					dragStatic = line.To;
					dragCurrent = line.From;
				}
				curDist = p.DistanceTo(line.To);
				if (curDist <= mmDist && curDist <= minDist) {
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
			} else {
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

			if (_geometry.Lines.Count > 0) {
				x /= (_geometry.Lines.Count * 2);
				y /= (_geometry.Lines.Count * 2);
			}

			_mapX = (int)(-x + VisualizationProvider.CanvasWidth / 2f);
			_mapY = (int)(-y + VisualizationProvider.CanvasHeight / 2f);
		}

		private void CloseEditor()
		{
			OnCloseEditor?.Invoke();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			if (_geometry == null)
				return;

			visualizationProvider.SetColor(Color.BlanchedAlmond);
			visualizationProvider.Print(510, 0, "Zoom 1:" + (1 / _currentZoom) + "x "
												+ _mapX + ":" + _mapY);

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
			visualizationProvider.Line(
				(int)(from.X * _currentZoom), (int)(from.Y * _currentZoom),
				(int)(to.X * _currentZoom), (int)(to.Y * _currentZoom));
		}

	}
}