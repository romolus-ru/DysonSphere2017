using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Exceptions
{
	/// <summary>
	/// Для передачи информации об отсутствующем соединении
	/// </summary>
	public class NoConnectionException : Exception
	{
		public Exception OriginalException;
		public string Msg;
		public NoConnectionException(Exception exception, string msg)
		{
			OriginalException = exception;
			Msg = msg;
		}
	}
}
