using System;
using System.Collections.Generic;
using Submarines.AI.Commands;
using Submarines.AI.Commands.Move;
using Submarines.Items;
using Submarines.Submarines;

namespace Submarines.Maps
{
	/// <summary>
	/// Управление действиями автоматики
	/// </summary>
	internal class MapAiController
	{
		public List<Command> _commands = new List<Command>();
		private List<Command> _commandsToAdd = new List<Command>();
		private List<Command> _commandsToDelete = new List<Command>();

		public MapBase Map;

		/// <summary>
		/// Выстрелить по координатам
		/// </summary>
		/// <param name="submarine"></param>
		/// <param name="weapon"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void ShootToCoordinatesCommand(SubmarineBase submarine, Weapon weapon, float x, float y)
		{
			if (!weapon.ReadyToShoot)
				return;
			// Проверяет есть ли возможность выстрела
			// Проверяет наличие ресурсов для выстрела
			// Забирает ресурсы на выстрел, создаёт подлодку-снаряд
			// Задаёт ей траекторию движения (команду движения)
			
			// создаём выстрел
			ItemSubmarine itemSubmarine = (ItemSubmarine)ItemsManager.GetItemBase("RocketDefault");
			SubmarineBase shoot = SubmarinesBuilder.Create(itemSubmarine);
			shoot.Engine.SetSpeedPercent(100);
			Map.AddShoot(shoot);
			// создаём команду для управления выстрелом
			MoveCommand moveCommand = MoveCommandCreator.Create(RemoveCommand, shoot, 100, new Vector(x, y, 0));
			_commandsToAdd.Add(moveCommand);
		}

		private void RemoveCommand(Command command)
		{
			_commandsToDelete.Add(command);
		}

		public void ProcessCommands(TimeSpan elapsedTime)
		{
			if (_commandsToAdd.Count > 0) {
				foreach (var command in _commandsToAdd) {
					_commands.Add(command);
				}

				_commandsToAdd.Clear();
			}

			foreach (var command in _commands) {
				if (!command.IsActive)
					continue;
				command.Execute(elapsedTime);
			}

			if (_commandsToDelete.Count > 0) {
				foreach (var command in _commandsToDelete) {
					_commands.Remove(command);
				}

				_commandsToDelete.Clear();
			}
		}
	}
}