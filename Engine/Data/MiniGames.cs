using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Data
{
	public class MiniGames
	{
		public long Id { get; set; }
		/// <summary>
		/// Кодовое наименование игры
		/// </summary>
		public string CodeName { get; set; }
		/// <summary>
		/// Официальное имя игры
		/// </summary>
		public string Name { get; set; }
		public string Description { get; set; }
		/// <summary>
		/// Версия игры
		/// </summary>
		public long VersionCode { get; set; }
	}
}
