namespace SpaceConstruction.Game.States
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