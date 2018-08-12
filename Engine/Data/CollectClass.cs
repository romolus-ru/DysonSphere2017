using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Data
{
	/// <summary>
	/// Загружаемый класс
	/// </summary>
	public class CollectClass : EventBase
	{
		public int Id { get; set; }
		public string FileName { get; set; }
		public string ClassName { get; set; }

		public CollectClass()
		{ }

		public CollectClass(int Id, string FileName, string ClassName)
		{
			this.Id = Id;
			this.FileName = FileName;
			this.ClassName = ClassName;
		}
	}
}