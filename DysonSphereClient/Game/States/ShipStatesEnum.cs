using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game.States
{
	/// <summary>
	/// Перечисление для состояний корабля
	/// </summary>
	public enum ShipStatesEnum
	{
		NoCommand,// нету команд
		MoveToBase,// возвращение на базу
		MoveCargo,// перемещение груза
		MoveToCargo,// перемещение за грузом
		Landing,// посадка
		Takeoff,// взлёт
		Loading,// загрузка
		Unloading,// разгрузка
	}
}
