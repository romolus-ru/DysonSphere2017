using Engine.Enums;
using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
	public partial class UserRegistration : EventBase
	{
		public long UserId { get; set; }
		/// <summary>
		/// Создаётся автоматически для пользователя при первом запуске игры
		/// </summary>
		public string UserGUID { get; set; }
		/// <summary>
		/// Имя игрока видное в игре
		/// </summary>
		public string NickName { get; set; }
		/// <summary>
		/// Официальное имя, которое будет использоваться администрацией
		/// </summary>
		public string OfficialName { get; set; }
		/// <summary>
		/// Почта для подтверждения и восстановления
		/// </summary>
		public string Mail { get; set; }
		/// <summary>
		/// Пароль
		/// </summary>
		/// <remarks>
		/// Пользователь вводит пароль, пароль меняется и отправляется на сервер. там он преобразуется и записывается/проверяется с тем что есть
		/// Первая игровая сессия может происходить вообще с отдельным сервером
		/// </remarks>
		public string HSPassword { get; set; }
		/// <summary>
		/// Роль пользователя
		/// </summary>
		public Role UserRole { get; set; }
	}
}
