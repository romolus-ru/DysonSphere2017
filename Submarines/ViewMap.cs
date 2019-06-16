using System;
using Engine.Visualization;
using Submarines.Submarines;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Submarines.AI.Commands.Move;
using Submarines.Maps;

namespace Submarines
{
	internal class ViewMap : ViewComponent
	{
		public delegate void PlayerFireDelegate(float x, float y);

		public PlayerFireDelegate PlayerFire;

		private Submarine _submarine;
		private MapBase _map;
		private MoveCommand _moveCommand;
		private int _startX;
		private int _startY;
		private int _endX;
		private int _endY;

		internal void SetMap(MapBase map)
		{
			_map = map;
			_submarine = (Submarine)_map.PlayerShip;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);
			input.AddKeyActionSticked(EndClick, Keys.LButton);
		}

		private void EndClick()
		{
			_startX = Input.CursorX;
			_startY = Input.CursorY;
			//_moveCommand = MoveCommandCreator.Create(null, _submarine, 100,
			//	new Vector(
			//		_startX + _submarine.Position.X - 700,
			//		_startY + _submarine.Position.Y - 500,
			//		0));
			PlayerFire?.Invoke(
				_startX + _submarine.Position.X - 700,
				_startY + _submarine.Position.Y - 500);
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			if (_submarine == null)
				return;

			visualizationProvider.SetColor(Color.LightSalmon);
			visualizationProvider.Rectangle(500, 500, 20, 20);

			visualizationProvider.OffsetAdd(700, 500);

			visualizationProvider.Rotate((int)_submarine.CurrentAngle + 90);
			visualizationProvider.DrawTexture(0, 0, "Submarines01map.pl01");
			visualizationProvider.RotateReset();

			if (_submarine.Geometry != null) {
				visualizationProvider.SetColor(_submarine.Geometry.Color);
				foreach (var line in _submarine.GeometryRotatedLines) {
					DrawLine(visualizationProvider, line.From, line.To);
				}
			}

			visualizationProvider.OffsetRemove();

			visualizationProvider.OffsetAdd(700 - (int)_submarine.Position.X, 500 - (int)_submarine.Position.Y);

			visualizationProvider.SetColor(_map.Geometry.Color);
			foreach (var line in _map.Geometry.Lines) {
				DrawLine(visualizationProvider, line.From, line.To);
			}

			_moveCommand = _map._mapAIController._commands.Count > 0
				? _moveCommand = _map._mapAIController._commands[0] as MoveCommand
				: null;

			if (_moveCommand?.BezierPoints != null) {
				var submarine = _map.Submarines[_map.Submarines.Count - 1];

				visualizationProvider.Rotate((int)submarine.CurrentAngle + 90);
				visualizationProvider.DrawTexture(
					(int) (submarine.Position.X),
					(int) (submarine.Position.Y),
					"Submarines01map.pl02");
				visualizationProvider.RotateReset();

				//_moveCommand.Execute();
				//var segment = _moveCommand.GetCommand();
				//if (segment != null) {
				//	_submarine.SetSpeed(segment.Speed);
				//	_submarine.AddSteering(-segment.Angle);
				//}

				visualizationProvider.SetColor(Color.LawnGreen);
				var count = _moveCommand.BezierPoints.Count;
				for (int i = 1; i < count; i++) {
					var p1 = _moveCommand.BezierPoints[i-1];
					var p2 = _moveCommand.BezierPoints[i];
					visualizationProvider.Line(p1.X, p1.Y, p2.X, p2.Y);
				}

				visualizationProvider.SetColor(Color.CornflowerBlue);
				var count2 = _moveCommand.BasePoints.Count;
				for (int i = 1; i < count2; i++) {
					var p1 = _moveCommand.BasePoints[i - 1];
					var p2 = _moveCommand.BasePoints[i];
					visualizationProvider.Line(p1.X, p1.Y, p2.X, p2.Y);
				}

				visualizationProvider.SetColor(Color.Gold);
				var count3 = _moveCommand.Simplified.Count;
				for (int i = 1; i < count3; i++) {
					var p1 = _moveCommand.Simplified[i - 1];
					var p2 = _moveCommand.Simplified[i];
					visualizationProvider.Line((int) p1.X, (int) p1.Y, (int) p2.X, (int) p2.Y);
				}

				visualizationProvider.SetColor(Color.Cyan);
				var count4 = _moveCommand.Segments.Count;
				float angle = -90;
				float x = 0;
				float y = 0;
				for (int i = 0; i < count4; i++) {
					var p = _moveCommand.Segments[i];
					angle -= p.Angle;
					var radians = angle * (Math.PI / 180);
					float px = x + (float) (Constants.TimerInterval * p.Speed * Math.Cos(radians));
					float py = y + (float) (Constants.TimerInterval * p.Speed * Math.Sin(radians));

					visualizationProvider.Line((int) x + 10, (int) y + 10, (int) px, (int) py);
					x = px;
					y = py;
				}


			}

			visualizationProvider.OffsetRemove();

		}

		private void DrawLine(VisualizationProvider visualizationProvider, Vector from, Vector to)
		{
			visualizationProvider.Line((int)from.X, (int)from.Y, (int)to.X, (int)to.Y);
		}
	}
}