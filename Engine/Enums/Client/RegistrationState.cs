using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Enums.Client
{
	/// <summary>
	/// Состояние регистрации
	/// </summary>
	public enum RegistrationState : byte
	{
		Unknown,
		NotRegistered,
		RegistrationRequest,
		RegistrationRejected,
		Registered,
	}
}
