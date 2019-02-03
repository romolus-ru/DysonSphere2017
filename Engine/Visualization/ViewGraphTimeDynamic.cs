using System;
using System.Collections.Generic;
using Engine.DataPlus;

namespace Engine.Visualization
{
	/// <summary>
	/// Вывод графиков с постоянно обновляемыми данными
	/// </summary>
	public class ViewGraphTimeDynamic : ViewGraph
	{
		private List<TimePoint<float>> _points = new List<TimePoint<float>>();
		private int _maxCount = 100;
		private DateTime _minX;
		private float _minY;
		private DateTime _maxX;
		private float _maxY;
		private float _scaleX;
		private float _scaleY;

		public void AddPoint(float x)
		{
			if (_points.Count >= _maxCount)
				_points.RemoveAt(0);

			_points.Add(new TimePoint<float>() {
				Time = DateTime.Now,
				Value = x
			});
		}

		public override void CalculateBorders()
		{
			_minX = DateTime.Now;
			_minY = 0;
			_maxX = DateTime.Now;
			_maxY = 0;
			if (_points.Count <= 0)
				return;
			_minX = _points[0].Time;
			_minY = _points[0].Value;
			_maxX = _points[0].Time;
			_maxY = _points[0].Value;
			foreach (TimePoint<float> timePoint in _points) {
				if (_minX < timePoint.Time)
					_minX = timePoint.Time;
				if (_minY < timePoint.Value)
					_minY = timePoint.Value;
				if (_maxX > timePoint.Time)
					_maxX = timePoint.Time;
				if (_maxY > timePoint.Value)
					_maxY = timePoint.Value;
			}

			_scaleX = 1f * (_maxX - _minX).Milliseconds / _points.Count;
			_scaleY = 1f * (_maxY - _minY) / _points.Count;

		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			
		}
	}
}