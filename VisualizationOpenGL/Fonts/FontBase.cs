namespace VisualizationOpenGL.Fonts
{
	/// <summary>
	/// Шрифт
	/// </summary>
	internal class FontBase
	{
		public string FontCodeName { get; }
		public int FontHeight { get; }

		public FontBase(string fontCodeName, int fontHeight)
		{
			FontCodeName = fontCodeName;
			FontHeight = fontHeight;
		}

		public virtual int TextLength(string text)
		{
			return 0;
		}

		public virtual void PrintOnly(int x, int y, string text)
		{
		}

		public virtual void ActivateFont()
		{

		}
	}
}