using Engine.Enums;
using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.DataPlus
{
	/// <summary>
	/// Результат выполнения операции
	/// </summary>
	public class ResultOperation : EventBase
	{
		public TCPOperations OperationType;
		public ErrorType Result;
		public string Message;
	}
}