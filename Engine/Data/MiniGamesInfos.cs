using Engine.DataPlus;
using Engine.EventSystem.Event;

namespace Engine.Data
{
	public class MiniGamesInfos : EventBase
	{
		public long IdMiniGamesInfos { get; set; }
		public long MiniGameId { get; set; }
		[MemberSpecialEditor(EditorType = "SelectClassInFile")]
		public string ClassFile { get; set; }
		[SkipEditEditor]
		public string ClassName { get; set; }
		public string Section { get; set; }
		public string Values { get; set; }
	}
}