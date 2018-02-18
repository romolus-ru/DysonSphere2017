using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Enums
{
	public enum TCPOperations
	{
		/// <summary>
		/// Установление соединения
		/// </summary>
		ConnectEstablishing,
		/// <summary>
		/// Сообщения об ошибках, например неправильный пароль
		/// </summary>
		ErrorMessage,
		/// <summary>
		/// Отмена всех операций
		/// </summary>
		AbortOperations,
		/// <summary>
		/// Регистрация
		/// </summary>
		Registration,
		/// <summary>
		/// Логин
		/// </summary>
		Login,
		/// <summary>
		/// Тестовый чат, временно для проверки
		/// </summary>
		TestChat,
		
	}
}
