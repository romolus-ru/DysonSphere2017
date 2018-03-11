using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Описание заданий которые игрок должен будет делать
	/// </summary>
	public enum QuestEnum
	{
		/// <summary>
		/// Постройка фазенды - нужны материалы, немного инструментов и пара рабочих
		/// </summary>
		BuildFazenda,
		/// <summary>
		/// Научный эксперимент, нужны ученые
		/// </summary>
		ScienceExperiment,
		/// <summary>
		/// Постройка небоскреба - много материалов, немного инструментов и немного рабочих
		/// </summary>
		BuildingTower,

	}
}