using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Команды передвижения корабля
	/// </summary>
	public enum ShipCommandEnum
	{
		/// <summary>
		/// Нету команд
		/// </summary>
		NoCommand,
		/// <summary>
		/// Двигаться к заказу
		/// </summary>
		MoveToOrder,
		/// <summary>
		/// Выполнить заказ
		/// </summary>
		Ordered,
		/// <summary>
		/// Вернуться на базу
		/// </summary>
		ToBase,
		/// <summary>
		/// Дополнение - загрузка
		/// </summary>
		CargoLoad,
		/// <summary>
		/// Дополнение - разгрузка
		/// </summary>
		CargoUnload,
	}
}
