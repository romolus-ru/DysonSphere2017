using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Data
{
	/// <summary>
	/// Соединение таблиц MiniGames и MiniGamesGenres
	/// </summary>
	public class MiniGamesMiniGamesGenres
	{
		public long IdMiniGamesMiniGamesGenres { get; set; }
		public long MiniGameId { get; set; }
		public long MiniGamesGenres { get; set; }
	}
}