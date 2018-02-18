using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Enums
{
	/// <summary>
	/// Всевозможные ошибки
	/// </summary>
	public enum ErrorType
	{
		NoError,
		RegistrationUserIsNull,
		UserAlreadyRegistered,
		LoginFailed,
	}
}
