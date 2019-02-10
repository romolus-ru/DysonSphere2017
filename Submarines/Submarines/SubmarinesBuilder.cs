using System;
using Submarines.Items;

namespace Submarines.Submarines
{
	/// <summary>
	/// Создать подлодку в зависимости от переданных параметров
	/// </summary>
	internal class SubmarinesBuilder
	{
		/// <summary>
		/// Создать подлодку или боеприпас и оснастить его начинкой
		/// </summary>
		/// <returns></returns>
		public SubmarineBase Build()
		{
			return null;
		}

		/// <summary>
		/// Создать двигатель с заданными характеристиками
		/// </summary>
		/// <returns></returns>
		private Engine CreateEngine()
		{
			return null;
		}

		/// <summary>
		/// Создать устройство изменения курса
		/// </summary>
		/// <returns></returns>
		private ManeuverDevice CreateManeuverDevice()
		{
			return null;
		}

		internal static SubmarineBase Create(ItemSubmarine itemSubmarine)
		{
			тут
			throw new NotImplementedException();
		}
	}
}
