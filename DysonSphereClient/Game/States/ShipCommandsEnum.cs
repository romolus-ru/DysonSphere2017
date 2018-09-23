using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game.States
{
	public enum ShipCommandsEnum
	{
		NoCommand,// на базе без команд
		MoveToBase,// вернуться на базу. в том числе после полного выполнения заказа
		CancelAndMoveToSpace,// отменяем любую команду и выходим в околопланетное пространство для дальнейших инструкций
		GetCargo,// забрать груз
		CargoDelivery,// перевезти груз
	}
}