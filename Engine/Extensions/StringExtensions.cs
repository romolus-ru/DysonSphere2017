namespace Engine.Extensions
{
	public static class StringExtensions
	{
		public static bool IsNullOrEmpty(this string value)
		{
			if (value != null)
				return value.Length == 0;
			return true;
		}
	}
}