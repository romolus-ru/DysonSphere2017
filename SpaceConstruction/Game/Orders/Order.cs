using Engine.Helpers;
using SpaceConstruction.Game.Resources;
using System;
using System.Collections.Generic;

namespace SpaceConstruction.Game.Orders
{
	public class Order
	{
		/// <summary>
		/// Информация о заказе
		/// </summary>
		public OrderInfo OrderInfo;
		/// <summary>
		/// Оставшееся количество ресурсов для перевозки
		/// </summary>
		public ResourcesHolder AmountResources;
		/// <summary>
		/// Перевозимое в текущий момент количество ресурсов (что бы лишнего не навозить)
		/// </summary>
		public ResourcesHolder AmountResourcesInProgress;
		/// <summary>
		/// Уже перевезенное количество ресурсов (чтоб показывать прогресс перевозки)
		/// </summary>
		public ResourcesHolder AmountResourcesDelivered;
		/// <summary>
		/// Уровень заказа, для различных стадий игры
		/// </summary>
		public int Level;
		public string OrderName;
		public string OrderDescription;
		/// <summary>
		/// Планета которая сделала заказ
		/// </summary>
		public Planet Destination;
		/// <summary>
		/// Планета откуда надо взять ресурсы
		/// </summary>
		public Planet Source;

		public float ProgressMax;// всего
		public float ProgressMoved;// перемещено
		public float ProgressInMove;// перемещается

		public Order(OrderInfo orderInfo, int hardness = 1)
		{
			//order.AmountResources = copyOrder.AmountResources.GetCopy();
			OrderInfo = orderInfo;
			OrderName = orderInfo.Name;
			OrderDescription = orderInfo.Description;
			Level = orderInfo.Level;
			// генерируем сколько нужно для заказа

			if (hardness > 1) {
				var multiplier = RandomHelper.Random(hardness) / hardness;
				AmountResources.Increase(multiplier);
			}
		}

		public void InitProgress()
		{
			ProgressMax = AmountResources.Volume();
			ProgressMoved = 0;
			ProgressInMove = 0;
		}

		public List<string> GetInfo()
		{
			var ret = new List<string>();
			var rows = AmountResources.GetInfo();
			if (rows.Count > 0) {
				ret.Add("Требуется перевезти (" + Level + ")");
				ret.AddRange(rows);
			} else
				ret.Add("Ожидается завершение заказа (" + Level + ")");
			return ret;
		}

		/// <summary>
		/// Получить со склада товар для перевозки
		/// </summary>
		/// <returns></returns>
		public void LoadToShipStore(ResourcesHolder fillCargo, int totalVolume, int totalWeight)
		{
			fillCargo.Clear();
			int freeVolume = totalVolume;
			int freeWeight = totalWeight;
			// проходим по ресурсам которые надо перевезти и добавляем их в зависимости от объема трюма
			foreach (var resValue in AmountResources) {
				if (resValue.Value <= 0)
					continue;
				float rVolume = resValue.ResInfo.VolumeCoefficient;
				float rWeight = resValue.ResInfo.DencityCoefficient;
				var cVolume = freeVolume / rVolume;
				var cWeight = freeWeight / rWeight;
				if (cVolume < 1 || cWeight < 1) continue;// чтоб хоть 1 товар умещался

				int countResCargo = (int)Math.Min(resValue.Value, Math.Floor(Math.Min(cVolume, cWeight)));
				
				fillCargo.Add(resValue.ResType, countResCargo);
				freeVolume -= (int)Math.Ceiling(countResCargo * rVolume);
				freeWeight -= (int)Math.Ceiling(countResCargo * rWeight);
			}

			AmountResources -= fillCargo;
			AmountResourcesInProgress += fillCargo;
			ProgressInMove += fillCargo.Volume();
			//StateEngine.Log.AddLog("++ cargo = " + fillCargo.Volume() + " A=" + AmountResources.Volume() 
			//	+ " I=" + AmountResourcesInProgress.Volume() + " M=" + ProgressInMove);
		}

		/// <summary>
		/// Сгрузить всё с корабля на склад заказчика
		/// </summary>
		/// <param name="cargoCurrent"></param>
		public void UnloadToPlanetStore(ResourcesHolder cargoCurrent)
		{
			AmountResourcesInProgress -= cargoCurrent;
			AmountResourcesDelivered += cargoCurrent;
			ProgressInMove -= cargoCurrent.Volume();
			ProgressMoved += cargoCurrent.Volume();
			//StateEngine.Log.AddLog("-- cargo = " + _cargoCurrent.Volume() + " A=" + AmountResources.Volume()
			//	+ " I=" + AmountResourcesInProgress.Volume() + " M=" + ProgressInMove);
		}

		/// <summary>
		/// Отменить перевозку
		/// </summary>
		/// <param name="cargoCurrent"></param>
		public void CancelShipDelivery(ResourcesHolder cargoCurrent)
		{
			AmountResourcesInProgress -= cargoCurrent;
			AmountResources += cargoCurrent;
			ProgressInMove -= cargoCurrent.Volume();
		}
	}
}