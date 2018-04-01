﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Описание ресурсов
	/// </summary>
	public class Resources
	{
		protected Dictionary<ResourcesEnum, int> _values = new Dictionary<ResourcesEnum, int>();
		public Resources() { Clear(); }
		public void Clear()
		{
			_values.Clear();
			foreach (ResourcesEnum item in Enum.GetValues(typeof(ResourcesEnum)))
				_values.Add(item, 0);
		}

		public bool IsEmpty() => _values.Sum(v => v.Value) == 0;
		public void Add(ResourcesEnum resource, int value) => _values[resource] += value;

		public void Add(Resources resources)
		{
			foreach (var value in resources._values) {
				_values[value.Key] += value.Value;
			}
		}

		public int Value(ResourcesEnum resource) {
			if (!_values.ContainsKey(resource)) return 0;
			return _values[resource];
		}

		/// <summary>
		/// Проверяем что ресурсов больше чем передали для проверки
		/// </summary>
		/// <returns></returns>
		public bool GreaterThen(Resources toVerify)
		{
			var res = true;
			foreach (var value in _values) {
				if (value.Value < toVerify._values[value.Key]) {
					res = false;
					break;
				}
			}
			return res;
		}

		public string GetInfo()
		{
			var s = "";
			foreach (var res in _values) {
				if (res.Value == 0) continue;
				s += "(" + res.Key.ToString() + "," + res.Value.ToString() + ")";
			}
			return s;
		}

		/// <summary>
		/// Получить копию ресурсов
		/// </summary>
		/// <returns></returns>
		public Resources GetCopy()
		{
			var res = new Resources();
			foreach (var res1 in _values) {
				res.Add(res1.Key, res1.Value);
			}
			return res;
		}

	}
}