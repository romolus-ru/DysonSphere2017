using System.Windows.Forms;

namespace Engine.Helpers
{
	/// <summary>
	/// Формирование двойного клика и другие операции связанные с классом Input
	/// </summary>
	public static class InputHelper
	{
		public static string KeyCombinationToString(params Keys[] keys)
		{
			if (keys == null) return null;
			return string.Join("+", keys);
		}
	}
}
