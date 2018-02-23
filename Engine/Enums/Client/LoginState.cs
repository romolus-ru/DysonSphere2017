using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Enums.Client
{
	/// <summary>
	/// Состояние залогинивания
	/// </summary>
	public enum LoginState
	{
		Unknown,
		NotLogedIn,
		LogInRequest,
		LogIn,
	}
}
