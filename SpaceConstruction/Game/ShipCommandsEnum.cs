namespace SpaceConstruction.Game
{
	public enum ShipCommandsEnum
	{
		/// <summary>
		/// на базе без команд
		/// </summary>
		NoCommand,
		/// <summary>
		/// вернуться на базу. в том числе после полного выполнения заказа
		/// </summary>
		MoveToBase,
		/// <summary>
		/// отменяем любую команду и выходим в околопланетное пространство для дальнейших инструкций
		/// </summary>
		CancelAndMoveToSpace,
		/// <summary>
		/// забрать груз
		/// </summary>
		GetCargo,
		/// <summary>
		/// перевезти груз
		/// </summary>
		CargoDelivery,
	}
}