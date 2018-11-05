using System.Collections.Generic;
using System.Linq;

namespace SpaceConstruction.Game.States
{
	/// <summary>
	/// Управление состояниями корабля
	/// общее управление всеми кораблями. это общий для всех объeкт, а не состояние внутри корабля
	/// каждый корабль хранит внутри себя текущее состояние, что бы, например 
	/// при получении последовательности разгрузки он сразу мог сказать - у меня нету груза, и перешёл к следущей команде
	/// </summary>
	public class ShipStates
	{
		/// <summary>
		/// Состояния и команды для них, основные последовательности команд
		/// </summary>
		Dictionary<ShipCommandsEnum, List<ShipStatesEnum>> _states = new Dictionary<ShipCommandsEnum, List<ShipStatesEnum>>()
		{
			{
				ShipCommandsEnum.NoCommand,
				null
			},
			{
				ShipCommandsEnum.CargoDelivery,
				new List<ShipStatesEnum>() {
					ShipStatesEnum.MoveCargo,
					ShipStatesEnum.Landing,
					ShipStatesEnum.Unloading,
					ShipStatesEnum.Takeoff
				}
			},
			{
				ShipCommandsEnum.GetCargo,
				new List<ShipStatesEnum>() {
					ShipStatesEnum.MoveToCargo,
					ShipStatesEnum.Landing,
					ShipStatesEnum.Loading,
					ShipStatesEnum.Takeoff
				}
			},
			{
				ShipCommandsEnum.CancelAndMoveToSpace,
				new List<ShipStatesEnum>() {
					ShipStatesEnum.Unloading,
					ShipStatesEnum.Takeoff
				}
			},
			{
				ShipCommandsEnum.MoveToBase,
				new List<ShipStatesEnum>() {
					ShipStatesEnum.MoveToBase,
					ShipStatesEnum.Landing,
				}
			}
		};

		/// <summary>
		/// Подготовка к выполнению команды - отмена всех предыдущих команд и переход к начальному состоянию для получения дальнейших команд
		/// например для перемещения к базе надо разгрузиться и взлететь с планеты
		/// </summary>
		List<KeyValuePair<ShipCommandsEnum, ShipCommandsEnum>> _prepareStates = new List<KeyValuePair<ShipCommandsEnum, ShipCommandsEnum>>()
		{
			new KeyValuePair<ShipCommandsEnum, ShipCommandsEnum>(
				ShipCommandsEnum.CargoDelivery, ShipCommandsEnum.CancelAndMoveToSpace ),
			new KeyValuePair<ShipCommandsEnum, ShipCommandsEnum>(
				ShipCommandsEnum.GetCargo, ShipCommandsEnum.CancelAndMoveToSpace),
			new KeyValuePair<ShipCommandsEnum, ShipCommandsEnum>(
				ShipCommandsEnum.MoveToBase, ShipCommandsEnum.CancelAndMoveToSpace)
		};

		/// <summary>
		/// Цепочка команд - после завершения команды (исчерпания списка) тут запрашивается следуюшая команда
		/// </summary>
		List<KeyValuePair<ShipCommandsEnum, ShipCommandsEnum>> _chainCommands = new List<KeyValuePair<ShipCommandsEnum, ShipCommandsEnum>>()
		{
			new KeyValuePair<ShipCommandsEnum, ShipCommandsEnum>(
				ShipCommandsEnum.CargoDelivery, ShipCommandsEnum.GetCargo),
			new KeyValuePair<ShipCommandsEnum, ShipCommandsEnum>(
				ShipCommandsEnum.GetCargo, ShipCommandsEnum.CargoDelivery),
			new KeyValuePair<ShipCommandsEnum, ShipCommandsEnum>(
				ShipCommandsEnum.MoveToBase, ShipCommandsEnum.NoCommand)
		};

		/// <summary>
		/// Пришёл сигнал извне или корабль завершил выполнение команды - переключаем на другую команду
		/// </summary>
		public void SwitchCommandTo(ShipCommandsEnum newCommand, List<ShipStatesEnum> listStates, bool cargoLoaded, bool shipOnPlanet)
		{
			listStates.Clear();
			var prepareCommand = _prepareStates.Where(pair => pair.Key == newCommand);// добавляем подготовочные команды
			if (prepareCommand != null) {
				var listPrep = _states[prepareCommand.First().Value];
				if (!cargoLoaded)
					listPrep.Remove(ShipStatesEnum.Unloading);
				if (!shipOnPlanet)
					listPrep.Remove(ShipStatesEnum.Takeoff);
				if (listPrep != null && listPrep.Count > 0)
					listStates.AddRange(listPrep);
			}
			var listCommand = _states[newCommand];// добавляем команды самой команды
			if (listCommand != null)
				listStates.AddRange(listCommand);
		}

		/// <summary>
		/// Корабль запросил следующую команду - находим и формируем список команд. если нету - то nocommand
		/// </summary>
		public void SetChainCommand(out ShipCommandsEnum newCommand, ShipCommandsEnum currCommand, List<ShipStatesEnum> listStates)
		{
			newCommand = ShipCommandsEnum.NoCommand;
			listStates.Clear();
			var chain = _chainCommands.Where(pair => pair.Key == currCommand);
			if (chain != null) {
				newCommand = chain.First().Value;
				var list = _states[newCommand];
				if (list != null) {
					listStates.AddRange(list);
				}
			}
		}
	}
}