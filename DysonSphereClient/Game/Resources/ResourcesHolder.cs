﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game.Resource
{
	/// <summary>
	/// Описание ресурсов
	/// </summary>
	public class ResourcesHolder
	{
		protected List<ResourceValue> _resources = new List<ResourceValue>();
		public ResourcesHolder() { Clear(); }
		public void Clear()
		{
			_resources.Clear();
		}

		public List<ResourceValue>.Enumerator GetEnumerator()
		{
			return _resources.GetEnumerator();
		}

		public bool IsEmpty() => _resources.Sum(v => v.Value) == 0;
		private ResourceValue GetResource(ResourcesEnum resource)
			=> _resources.Where(v => v.Res == resource).FirstOrDefault();
		private int GetResourceValue(ResourcesEnum resource)
		{
			var r = GetResource(resource);
			return r != null ? r.Value : 0;
		}
		public void Add(ResourcesEnum resource, int value)
		{
			var r = GetResource(resource);
			if (r == null) {
				r = new ResourceValue() { Res = resource, Value = 0 };
				_resources.Add(r);
			}
			r.Value += value;
		}

		public void Add(ResourcesHolder resources)
		{
			foreach (var value in resources._resources) {
				var r = GetResource(value.Res);
				r.Value += value.Value;
			}
		}

		public int Value(ResourcesEnum resource)
		{
			var r = GetResource(resource);
			return r != null ? r.Value : 0;
		}

		/// <summary>
		/// Проверяем что ресурсов больше чем передали для проверки
		/// </summary>
		/// <returns></returns>
		public bool GreaterThen(ResourcesHolder toVerify)
		{
			var res = true;
			foreach (var value in _resources) {
				if (value.Value < toVerify.GetResourceValue(value.Res)) {
					res = false;
					break;
				}
			}
			return res;
		}

		public string GetInfo()
		{
			var s = "";
			foreach (var res in _resources) {
				if (res.Value == 0) continue;
				s += "(" + res.Res.ToString() + "," + res.Value.ToString() + ")";
			}
			return s;
		}

		/// <summary>
		/// Получить копию ресурсов
		/// </summary>
		/// <returns></returns>
		public ResourcesHolder GetCopy()
		{
			var res = new ResourcesHolder();
			foreach (var res1 in _resources) {
				res._resources.Add(res1.GetCopy());
			}
			return res;
		}

		/// <summary>
		/// Умножить количество ресурсов
		/// </summary>
		public void Increase(float multiplier)
		{
			foreach (var res in _resources) {
				int increase = (int)(res.Value * multiplier);
				if (increase == 0) continue;
				res.Value += increase;
			}
		}
	}
}