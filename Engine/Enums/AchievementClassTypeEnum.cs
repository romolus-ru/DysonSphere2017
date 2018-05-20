using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Enums
{
	/// <summary>
	/// Для возможности централизованного создания классов для ачивок
	/// </summary>
	public enum AchievementClassTypeEnum
	{
		/// <summary>
		/// Неопределено
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// Отдельный класс для 
		/// </summary>
		Custom = 1,
		/// <summary>
		/// Подсчет количества вызовов
		/// </summary>
		EventCounter = 20,
		/// <summary>
		/// Суммирование значений
		/// </summary>
		AddCounter = 30,
		/// <summary>
		/// Сравнение с заданным пределом
		/// </summary>
		LimitCounter = 40,
	}
}