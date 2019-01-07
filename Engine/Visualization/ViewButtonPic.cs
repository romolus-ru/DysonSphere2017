using Engine.Helpers;

namespace Engine.Visualization
{
	public class ViewButtonPic : ViewButton
	{
		private float _scale = 1;
		public void SetPic(string texture, float scale=1)
		{
			_btnTexture = texture;
			_scale = scale;
		}
		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.DrawTexture(X, Y, _btnTexture, _scale);

			if (CursorOver)
				ViewHelper.ShowHint(this, Hint, HintKeys);
		}

	}
}