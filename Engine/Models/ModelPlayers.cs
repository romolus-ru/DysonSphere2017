using Engine.Data;
using Engine.Enums;
using Engine.Helpers;
using Engine.TCPNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
	/// <summary>
	/// Данные об игроках
	/// </summary>
	public class ModelPlayers:Model
	{		
		/// <summary>
		/// База данных
		/// </summary>
		protected DataSupportBase DB { get; private set; }

		public ModelPlayers(DataSupportBase db)
		{
			DB = db;
		}
	}
}
