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
		NoCommand,
		MoveToOrder,
		Ordered,
		ToBase,
	}
}
