using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Enums
{
	public enum Role
	{
		/// <summary>
		/// Взломщик
		/// </summary>
		Intruder = 0,
		PlayerUnregistered = 1,
		/// <summary>
		//// Обычный игрок
		/// </summary>
		Player = 2,
		/// <summary>
		/// Платящий
		/// </summary>
		PlayerVip = 3,
		/// <summary>
		/// Организатор и ведущий эвентов, помощь
		/// </summary>
		GameMaster = 4,
		/// <summary>
		/// Поддержка, наблюдение, помощь
		/// </summary>
		Support = 5,
		/// <summary>
		/// Администратор, общие настройки сервера
		/// </summary>
		Admin = 6,
		/// <summary>
		/// всё вместе и сразу
		/// </summary>
		MetaUser = 7,
	}
}
