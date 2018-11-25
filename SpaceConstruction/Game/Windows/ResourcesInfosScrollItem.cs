using Engine.Visualization;
using Engine.Visualization.Scroll;
using SpaceConstruction.Game.Resources;
using System.Drawing;

namespace SpaceConstruction.Game.Windows
{
	internal class ResourcesInfosScrollItem : ScrollItem
	{
		private ResourceInfo _resourceInfo;

		public ResourcesInfosScrollItem(ResourceInfo resourceInfo)
		{
			_resourceInfo = resourceInfo;
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Print(X + 10, Y, _resourceInfo.Name + " " + _resourceInfo.ResourceGroup + " " + _resourceInfo.ResourceType);
			visualizationProvider.Print(X + 10, Y+20, "vol="+_resourceInfo.VolumeCoefficient + " weght=" + _resourceInfo.DencityCoefficient);
			if (!string.IsNullOrEmpty(_resourceInfo.Texture))
				visualizationProvider.DrawTexture(X + 40, Y + 40, _resourceInfo.Texture);
			visualizationProvider.SetColor(Color.GreenYellow);
			visualizationProvider.Rectangle(X, Y, Width, Height);
		}

		//public override bool Filtrate(string filter = null)
		//{
		//	if (string.IsNullOrEmpty(filter))
		//		return true;
		//	return _value.IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) >= 0;
		//}
	}
}