using System;
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
		protected Dictionary<ResourcesEnum, int> _resources = new Dictionary<ResourcesEnum, int>();
		public Resources() { Clear(); }
		public void Clear()
		{
			_resources.Clear();
			foreach (ResourcesEnum item in Enum.GetValues(typeof(ResourcesEnum)))
				_resources.Add(item, 0);
		}

		public bool IsEmpty() => _resources.Sum(v => v.Value) == 0;
		public void Add(ResourcesEnum resource, int value) => _resources[resource] += value;

		public void Add(Resources resources)
		{
			foreach (var value in resources._resources) {
				_resources[value.Key] += value.Value;
			}
		}

		public int Value(ResourcesEnum resource) {
			if (!_resources.ContainsKey(resource)) return 0;
			return _resources[resource];
		}

		/// <summary>
		/// Проверяем что ресурсов больше чем передали для проверки
		/// </summary>
		/// <returns></returns>
		public bool GreaterThen(Resources toVerify)
		{
			var res = true;
			foreach (var value in _resources) {
				if (value.Value < toVerify._resources[value.Key]) {
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
			foreach (var res1 in _resources) {
				res.Add(res1.Key, res1.Value);
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
				Add(res.Key, increase);
			}
		}
	}
}